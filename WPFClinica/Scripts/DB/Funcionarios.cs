using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {
    public class Funcionarios {

        public const string Name = "funcionarios";
        static CultureInfo Brasil = new CultureInfo("pt-BR");

        public string CPF { get; set; }
        public double Salario { get; set; }
        public string Senha { get; set; }
        public string Observacao { get; set; }
        public bool Ativo { get; set; }
        public byte[] PicData { get; set; }
        public string PicName { get; set; }
        public int PicSize { get; set; }

        public static void CreateTable() {
            //Pessoas.CreateTable();
            var p = new Pessoas();
            var title = $"Erro ao criar tabela '{Name}'.";
            var q = $"create table if not exists {Name} (" +
                $"  {nameof(CPF)} varchar(15) not null," +
                $"  {nameof(PicData)} longblob, " +
                $"  {nameof(PicName)} nvarchar(50), " +
                $"  {nameof(PicSize)} int, " +
                $"  {nameof(Salario)} double not null, " +
                $"  {nameof(Senha)} nvarchar(20) not null, " +
                $"  {nameof(Observacao)} mediumtext not null, " +
                $"  {nameof(Ativo)} bool not null, " +
                $"  primary key ({nameof(CPF)}), " +
                $"  foreign key ({nameof(CPF)}) references {Pessoas.p.TableName} ({nameof(p.CPF)}) ON DELETE CASCADE ON UPDATE CASCADE" +
                $");";
            NonQuery(title, q);
            InsertOrUpdate("033.380.952-10", 0, "1025", "", true, null);
        }

        public static void SelectPic(string cpf, out string name, out byte[] data) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select {nameof(PicData)}, {nameof(PicName)}, {nameof(PicSize)} " +
                $"from {Name} " +
                $"where {nameof(CPF)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", cpf);
            string fn = null;
            byte[] fd = null;
            QueryR("Erro ao obter imagem de perfil do funcinário.", c, (r) => {
                if (r.Read()) {
                    fn = r.IsDBNull(1) ? null : r.GetString(1);
                    var fl = r.IsDBNull(2) ? -1 : r.GetInt32(2);
                    fd = fl < 0 ? null : new byte[fl];
                    if (!r.IsDBNull(0)) {
                        r.GetBytes(0, 0, fd, 0, fl);
                    }
                }
            });
            name = fn;
            data = fd;
        }

        /// <summary>
        /// CPF, Nome, Sexo, Nascimento, Ativo, Administrador, Recepcionista, Tecnico_Enfermagem, Medico
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static List<string[]> Select(string cpf, string nome) {
            var p = new Pessoas();
            var aa = new Administradores();
            var bb = new Recepcionista();
            var cc = new Tecnico_Enfermagem();
            var d = new Medicos();
            var c = new MySqlCommand();
            c.CommandText = $"select a.{nameof(CPF)}, {nameof(p.Nome)}, {nameof(p.Sexo)}, {nameof(p.Nascimento)}, {nameof(Ativo)}, c.cpf, d.{nameof(bb.CPF)}, e.{nameof(cc.CPF)}, f.{nameof(d.CPF)} " +
            $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(CPF)} = b.{nameof(p.CPF)} " +
            $"left join {aa.TableName} as c on a.{nameof(CPF)} = c.cpf " +
            $"left join {Recepcionista.Name} as d on a.{nameof(CPF)} = d.{nameof(bb.CPF)} " +
            $"left join {Tecnico_Enfermagem.Name} as e on a.{nameof(CPF)} = e.{nameof(cc.CPF)} " +
            $"left join {Medicos.Name} as f on a.{nameof(CPF)} = f.{nameof(d.CPF)} " +
            $"where a.{nameof(CPF)} like concat(@cpf, '%') and {nameof(p.Nome)} like concat(@nome, '%');";
            c.Parameters.AddWithValue("@cpf", cpf);
            c.Parameters.AddWithValue("@nome", nome);
            List<string[]> a = new List<string[]>();
            QueryR($"Erro ao obter registros da tabela {Name}.", c, (r) => {
                while (r.Read()) {
                    var b = new string[r.FieldCount];
                    int i = 0;
                    b[i++] = r.GetString(0);
                    b[i++] = r.GetString(1);
                    b[i++] = r.GetBoolean(2) ? "M" : "F";
                    var dt = r.GetMySqlDateTime(3);
                    b[i++] = $"{dt.Day}/{dt.Month}/{dt.Year}";
                    b[i++] = r.GetBoolean(4) ? "Ativo" : "Inativo";
                    b[i++] = r.IsDBNull(5) ? "✗" : "✓";
                    b[i++] = r.IsDBNull(6) ? "✗" : "✓";
                    b[i++] = r.IsDBNull(7) ? "✗" : "✓";
                    b[i++] = r.IsDBNull(8) ? "✗" : "✓";
                    a.Add(b);
                }
            });
            return a;
        }

        public static Funcionarios Select(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(CPF)}, {nameof(Salario)}, {nameof(Senha)}, {nameof(Observacao)}, {nameof(Ativo)} " +
            $"from {Name} where {nameof(CPF)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", cpf);
            Funcionarios f = null;
            QueryRLoop("Erro ao obter funcionário.", c, (r) => {
                f = new Funcionarios() {
                    CPF = r.GetString(0),
                    Salario = r.GetDouble(1),
                    Senha = r.GetString(2),
                    Observacao = r.GetString(3),
                    Ativo = r.GetBoolean(4)
                };
            });
            return f;
        }

        public static void Insert(string cpf, dynamic salario, string senha, string observacao, bool ativo, FileInfo file) {
            NonQuery("Erro ao inserir funcionário.", (c) => {
                c.CommandText = $"insert into {Name} values (@cpf, @pic, @picName, @picSize, @salario, @senha, @observacao, @ativo);";
                c.Parameters.AddWithValue("@cpf", cpf);
                c.Parameters.AddWithValue("@pic", file == null ? null : File.ReadAllBytes(file.FullName));
                c.Parameters.AddWithValue("@picName", file?.Name);
                c.Parameters.AddWithValue("@picSize", file?.Length);
                c.Parameters.AddWithValue("@salario", ToDouble(salario, Brasil));
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@observacao", observacao);
                c.Parameters.AddWithValue("@ativo", ativo);
                return c;
            });
        }

        public static void InsertOrUpdate(string cpf, double salario, string senha, string observacao, bool ativo, FileInfo file) {
            var r = Select(cpf);
            if (r != null) {
                Update(cpf, salario, senha, observacao, ativo, file);
            } else {
                Insert(cpf, salario, senha, observacao, ativo, file);
            }
        }

        public static void Update(string cpf, double salario, string senha, string observacao, bool ativo, FileInfo file) {
            NonQuery("Erro ao atualizar funcionário", (c) => {
                c.CommandText = $"update {Name} set " +
                $"{nameof(Salario)} = @salario, {nameof(Senha)} = @senha, {nameof(Observacao)} = @observacao, {nameof(Ativo)} = @ativo, " +
                $"{nameof(PicName)} = @picName, {nameof(PicData)} = @picData, {nameof(PicSize)} = @picSize " +
                $"where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@salario", salario);
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@observacao", observacao);
                c.Parameters.AddWithValue("@ativo", ativo);
                c.Parameters.AddWithValue("@picName", file?.Name);
                c.Parameters.AddWithValue("@picData", file == null ? null : File.ReadAllBytes(file.FullName));
                c.Parameters.AddWithValue("@picSize", file?.Length);
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static bool Authenticate(string cpf, string senha) {
            NonQuery("Erro ao criar banco de dados", $"create database if not exists `{SQLOperations.Database}`;");
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where cpf = @cpf and senha = @senha;";
            c.Parameters.AddWithValue("@cpf", cpf);
            c.Parameters.AddWithValue("@senha", senha);
            bool a = false;
            QueryRLoop("Erro ao verificar se funcionário existe.", c, (r) => {
                a = true;
            });
            return a;
        }

        public static bool IsFuncionario(string cpf) {
            return Select(cpf) != null;
        }

    }
}
