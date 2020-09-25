using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static System.Convert;
//using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class ListaEspera_Funcionario {

        public const string Name = nameof(ListaEspera_Funcionario);

        public int Senha { get; set; }
        public string Funcionario { get; set; }

        public static void CreateTable() {
            var le = new ListaEspera();
            var f = new Funcionarios();
            SQLOperations.NonQuery($"Erro ao criar associação de {Name}",
                $"create table if not exists {Name} (" +
                $"  {nameof(Senha)} int, " +
                $"  {nameof(Funcionario)} varchar(15)," +
                $"  primary key ({nameof(Senha)})," +
                $"  foreign key ({nameof(Senha)}) references {ListaEspera.Name} ({nameof(le.Senha)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Funcionario)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        private static void Create (Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                MessageBox.Show("Creating");
                CreateTable();
            }
        }

        private static void NonQuery (string title, Func<MySqlCommand, MySqlCommand> action) {
            SQLOperations.NonQuery(title, action, Create, true);
        }

        private static void QueryRLoop (string title, MySqlCommand c, Action<MySqlDataReader> action) {
            SQLOperations.QueryRLoop(title, c, action, Create, true);
        }

        public static void Insert(int senha, string funcionario) {
            NonQuery($"Erro ao inserir associação {Name}.", (c) => {
                c.CommandText = $"insert into {Name} values (@senha, @f);";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@f", funcionario);
                return c;
            });
        }

        public static void Update(int senha, string funcionario) {
            NonQuery($"Erro ao inserir associação {Name}.", (c) => {
                c.CommandText = $"update {Name} set {nameof(Funcionario)} = @f where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@f", funcionario);
                return c;
            });
        }

        public static ListaEspera_Funcionario Select(int senha) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(Senha)} = @senha";
            c.Parameters.AddWithValue("@senha", senha);
            ListaEspera_Funcionario f = null;
            QueryRLoop($"Erro ao obter associação {Name}. ({nameof(ErrorCodes.DB0006)})", c, (r) => {
                f = new ListaEspera_Funcionario() {
                    Senha = r.GetInt32(0),
                    Funcionario = r.GetString(1)
                };
            });
            return f;
        }

        public static ListaEspera_Funcionario Select(int senha, string funcionario) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} " +
            $"where {nameof(Senha)} = @senha and {nameof(Funcionario)} = @f;";
            c.Parameters.AddWithValue("@senha", senha);
            c.Parameters.AddWithValue("@f", funcionario);
            ListaEspera_Funcionario f = null;
            QueryRLoop($"Erro ao obter {Name}. ({ErrorCodes.DB0005})", c, (r) => {
                f = new ListaEspera_Funcionario() {
                    Senha = r.GetInt32(0),
                    Funcionario = r.GetString(1)
                };
            });
            return f;
        }

        public static void Delete(int senha) {
            NonQuery($"Erro ao deletar associação entre {Name}.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                return c;
            });
        }

        public static void InsertOrUpdate(int senha, string funcionario) {
            var r = Select(senha, funcionario);
            if (r == null) {
                Insert(senha, funcionario);
            } else {
                Update(senha, funcionario);
            }
        }

    }

}
