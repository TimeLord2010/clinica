using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Tecnico_Enfermagem {

        public const string Name = nameof(Tecnico_Enfermagem);

        public string CPF { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            var title = $"Erro ao criar a tabela {Name}.";
            var q =
                $"create table if not exists {Name} (" +
                $"  {nameof(CPF)} varchar(15)," +
                $"  primary key ({nameof(CPF)})," +
                $"  foreign key ({nameof(CPF)}) references {Funcionarios.Name} ({nameof(f.CPF)}) ON DELETE CASCADE ON UPDATE CASCADE" +
                $");";
            SQLOperations.NonQuery(title, q);
        }

        private static void Create(Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void NonQuery(string title, Func<MySqlCommand, MySqlCommand> func) {
            SQLOperations.NonQuery(title, func, Create, true);
        }

        private static void QueryRLoop(string title, MySqlCommand c, Action<MySqlDataReader> r) {
            SQLOperations.QueryRLoop(title, c, r, Create, true);
        }

        public static void Insert(string cpf) {
            NonQuery("Erro ao criar tecnico em enfermagem.", (c) => {
                c.CommandText = $"insert into {Name} values (@cpf);";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void TryInsert(string cpf) {
            if (!IsTecnico(cpf)) {
                Insert(cpf);
            }
        }

        public static void Delete(string cpf) {
            NonQuery("Erro ao deletar tecnico em enfermagem.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static bool IsTecnico(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(CPF)} = @cpf";
            c.Parameters.AddWithValue("@cpf", cpf);
            bool a = false;
            QueryRLoop("Erro ao obter técnico em enfermagem.", c, (r) => {
                a = true;
            });
            return a;
        }

    }

}
