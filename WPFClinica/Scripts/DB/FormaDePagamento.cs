using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Scripts.DB {

    public class FormaDePagamento {

        public const string Name = nameof(FormaDePagamento);

        public int ID { get; set; }
        public string Descricao { get; set; }

        public static void CreateTable() {
            SQLOperations.NonQuery($"Erro ao criar formas de pagamento",
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(Descricao)} nvarchar(150) unique," +
                $"  primary key ({nameof(ID)})" +
                $");");
            var descrição = Select(1);
            if (descrição != null) {
                return;
            }
            using (var c = new MySqlCommand($"insert into {Name} (Descricao) values ('Dinheiro'), ('Crédito'), ('Débito')")) {
                NonQuery($"Erro ao inserir registro inicial de forma de pagamento.", c);
            }
        }

        private static void Create(Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void NonQuery(string title, MySqlCommand c) {
            SQLOperations.NonQuery(title, c, Create, true);
        }

        private static void QueryRLoop(string title, MySqlCommand c, Action<MySqlDataReader> action) {
            SQLOperations.QueryRLoop(title, c, action, Create, true);
        }

        public static void Insert(string descricao) {
            var c = new MySqlCommand();
            c.CommandText = $"insert into {Name} values (null, @des);";
            c.Parameters.AddWithValue("@des", descricao);
            NonQuery($"Erro ao inserir forma de pagamento. ({ErrorCodes.DB0029})", c);
        }

        public static void Update(int id, string descricao) {
            var c = new MySqlCommand();
            c.CommandText = $"update {Name} set {nameof(Descricao)} = @des where {nameof(ID)} = @id;";
            c.Parameters.AddWithValue("@id", id);
            c.Parameters.AddWithValue("@des", descricao);
            NonQuery($"Erro ao inserir forma de pagamento. ({ErrorCodes.DB0029})", c);
        }

        public static void Delete(int id) {
            var c = new MySqlCommand();
            c.CommandText = $"delete from {Name} where {nameof(ID)} = @id";
            c.Parameters.AddWithValue("@id", id);
            NonQuery($"Erro ao deletar forma de pagamento. ({ErrorCodes.DB0030})", c);
        }

        public static string Select(int id) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Descricao)} from {Name} where {nameof(ID)} = @id";
            c.Parameters.AddWithValue("@id", id);
            string des = null;
            QueryRLoop($"Erro ao obter forma de pagamento. ({ErrorCodes.DB0031})", c, (r) => {
                des = r.GetString(0);
            });
            return des;
        }

        public static FormaDePagamento Select(string descricao) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(Descricao)} = @des;";
            c.Parameters.AddWithValue("@des", descricao);
            FormaDePagamento a = null;
            QueryRLoop($"Erro ao obter formas de pagamento. ({ErrorCodes.DB0032})", c, (r) => {
                a = new FormaDePagamento() {
                    ID = r.GetInt32(0),
                    Descricao = r.GetString(1)
                };
            });
            return a;
        }

        public static List<FormaDePagamento> SelectLike(string descricao) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(Descricao)} like concat(@des, '%');";
            c.Parameters.AddWithValue("@des", descricao);
            var lista = new List<FormaDePagamento>();
            QueryRLoop($"Erro ao obter formas de pagamento. ({ErrorCodes.DB0032})", c, (r) => {
                lista.Add(new FormaDePagamento() {
                    ID = r.GetInt32(0),
                    Descricao = r.GetString(1)
                });
            });
            return lista;
        }

    }
}
