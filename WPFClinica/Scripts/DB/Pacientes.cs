using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Pacientes {

        public const string Name = "pacientes";

        public string CPF { get; set; }
        public bool AC { get; set; }
        public bool NC { get; set; }
        public bool AE { get; set; }
        public bool NE { get; set; }

        public static void CreateTable() {
            var p = new Pessoas();
            NonQuery($"Erro ao criar a tabela {Name}",
                $"create table if not exists {Name} (" +
                $"  {nameof(CPF)} varchar(15)," +
                $"  {nameof(AC)} bool," +
                $"  {nameof(NC)} bool," +
                $"  {nameof(AE)} bool," +
                $"  {nameof(NE)} bool," +
                $"  primary key ({nameof(CPF)})," +
                $"  foreign key ({nameof(CPF)}) references {Pessoas.p.TableName} ({nameof(p.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(string cpf, bool ac, bool nc, bool ae, bool ne) {
            NonQuery("Erro ao criar paciente.", (c) => {
                c.CommandText = $"insert into {Name} values (@cpf, @ac, @nc, @ae, @ne);";
                c.Parameters.AddWithValue("@cpf", cpf);
                c.Parameters.AddWithValue("@ac", ac);
                c.Parameters.AddWithValue("@nc", nc);
                c.Parameters.AddWithValue("@ae", ae);
                c.Parameters.AddWithValue("@ne", ne);
                return c;
            });
        }

        public static void InsertOrUpdate (string cpf, bool ac, bool nc, bool ae, bool ne) {
            var r = Select(cpf);
            if (r == null) {
                Insert(cpf, ac, nc, ae, ne);
            } else {
                Update(cpf, ac, nc, ae, ne);
            }
        }

        public static void Update(string cpf, bool ac, bool nc, bool ae, bool ne) {
            NonQuery("Erro ao atualizar paciente.", (c) => {
                c.CommandText = $"update {Name} set " +
                $"{nameof(AC)} = @ac, {nameof(NC)} = @nc, {nameof(AE)} = @ae, {nameof(NE)} = @ne " +
                $"where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                c.Parameters.AddWithValue("@ac", ac);
                c.Parameters.AddWithValue("@nc", nc);
                c.Parameters.AddWithValue("@ae", ae);
                c.Parameters.AddWithValue("@ne", ne);
                return c;
            });
        }

        public static Pacientes Select(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = 
                $"select " +
                $"  {nameof(AC)}, " +
                $"  {nameof(AE)}," +
                $"  {nameof(NC)}," +
                $"  {nameof(NE)} " +
                $"from {Name} " +
                $"where " +
                $"  {nameof(CPF)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", cpf);
            Pacientes p = null;
            QueryRLoop("Erro ao obter paciente.", c, (r) => {
                p = new Pacientes() {
                    CPF = cpf,
                    AC = r.GetBoolean(0),
                    AE = r.GetBoolean(1),
                    NC = r.GetBoolean(2),
                    NE = r.GetBoolean(3)
                };
            });
            return p;
        }

        /// <summary>
        /// CPF, Nome, Nascimento, Sexo, Email, Contato1, Contato2
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static List<Pessoas> SelectLike(string cpf, string nome) {
            var ps = new List<Pessoas>();
            var c = new MySqlCommand();
            c.CommandText =
            $"select b.{nameof(Pessoas.CPF)}, b.{nameof(Pessoas.Nome)}, b.{nameof(Pessoas.Nascimento)}, " +
            $"  b.{nameof(Pessoas.Sexo)}, b.{nameof(Pessoas.Email)}, b.{nameof(Pessoas.Contato1)}, b.{nameof(Pessoas.Contato2)} " +
            $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(CPF)} = b.{nameof(Pessoas.CPF)} " +
            $"where a.{nameof(CPF)} like concat(@cpf, '%') and b.{nameof(Pessoas.Nome)} like concat(@nome, '%') " +
            $"limit 200;";
            c.Parameters.AddWithValue("@cpf", cpf);
            c.Parameters.AddWithValue("@nome", nome);
            QueryR("Erro ao obter pacientes.", c, (r) => {
                while (r.Read()) {
                    ps.Add(new Pessoas() {
                        CPF = r.GetString(0),
                        Nome = r.GetString(1),
                        Nascimento = r.GetMySqlDateTime(2).GetDateTime(),
                        Sexo = r.GetBoolean(3),
                        Email = r.GetString(4),
                        Contato1 = r.GetString(5),
                        Contato2 = r.GetString(6)
                    });
                }
            });
            return ps;
        }

    }

}
