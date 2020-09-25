using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Anexos {

        public const string Name = "anexos";

        public string CPF { get; set; }
        public string Nome { get; set; }
        public int Tamanho { get; set; }
        public byte[] _Data { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            NonQuery($"Erro ao criar a tabela de anexos",
                $"create table if not exists {Name} (" +
                $"  {nameof(CPF)} varchar(15) not null," +
                $"  {nameof(Nome)} varchar(50) not null," +
                $"  {nameof(Tamanho)} int not null," +
                $"  {nameof(_Data)} longblob not null," +
                $"  primary key ({nameof(CPF)}, {nameof(Nome)})," +
                $"  foreign key ({nameof(CPF)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(string funcionario, string file) {
            Insert(funcionario, new FileInfo(file));
        }

        public static void InsertOrUpdate (string funcionario, string file) {
            var r = SelectNames(funcionario);
            if (!r.Contains(file)) {
                Insert(funcionario, file);
            } else {
                Update(funcionario, file);
            }
        }

        public static void Update (string funcionario, string file) {
            Delete(funcionario, new FileInfo(file).Name);
            Insert(funcionario, file);
        }

        public static void Insert(string funcionario, FileInfo info) {
            NonQuery("Erro ao inserir anexo.", (c) => {
                c.CommandText = $"insert into {Name} values (@funcionario, @nome, @tamanho, @data);";
                c.Parameters.AddWithValue("@funcionario", funcionario);
                c.Parameters.AddWithValue("@nome", info.Name);
                c.Parameters.AddWithValue("@tamanho", info.Length);
                c.Parameters.AddWithValue("@data", File.ReadAllBytes(info.FullName));
                return c;
            });
        }

        public static void Delete(string funcinario, string fileName) {
            NonQuery("Erro ao deletar anexo.", (c) => {
                c.CommandText = $"delete from {Name} where cpf = @funcionario and nome = @nome;";
                c.Parameters.AddWithValue("@funcionario", funcinario);
                c.Parameters.AddWithValue("@nome", fileName);
                return c;
            });
        }

        public static List<Anexos> Select (string funcionario) {
            var lista = new List<Anexos>();
            var c = new MySqlCommand();
            c.CommandText = 
                $"select {nameof(Nome)}, {nameof(Tamanho)}, {nameof(_Data)} " +
                $"from {Name} " +
                $"where {nameof(CPF)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", funcionario);
            QueryR("Erro ao obter anexos.", c, (r) => {
                while (r.Read()) {
                    var data = new byte[r.GetInt32(1)];
                    r.GetBytes(2, 0, data, 0, data.Length);
                    lista.Add(new Anexos() {
                        Nome = r.GetString(0),
                        Tamanho = r.GetInt32(1),
                        _Data = data
                    });
                }
            });
            return lista;
        }

        public static List<string> SelectNames (string funcionario) {
            var lista = new List<string>();
            var c = new MySqlCommand();
            c.CommandText =
                $"select {nameof(Nome)} " +
                $"from {Name} " +
                $"where {nameof(CPF)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", funcionario);
            QueryR("Erro ao obter anexos.", c, (r) => {
                while (r.Read()) {
                    lista.Add(r.GetString(0));
                }
            });
            return lista;
        }

        public static Anexos Select (string funcionario, string fileName) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(CPF)} = @cpf and {nameof(Nome)} = @fn;";
            c.Parameters.AddWithValue("@cpf", funcionario);
            c.Parameters.AddWithValue("@fn", fileName);
            Anexos anexo = null;
            QueryR("Erro ao obter anexo.", c, (r) => {
                if (r.Read()) {
                    anexo = new Anexos() {
                        CPF = funcionario,
                        Nome = fileName,
                        Tamanho = r.GetInt32(2)
                    };
                    anexo._Data = new byte[anexo.Tamanho];
                    r.GetBytes(3, 0, anexo._Data, 0, anexo.Tamanho);
                }
            });
            return anexo;
        }

    }

}
