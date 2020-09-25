using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;
using MySql.Data.MySqlClient;
using System.Windows.Controls;

namespace WPFClinica.Scripts.DB {

    class TipoProcedimento {

        public const string Name = nameof(TipoProcedimento);

        public int ID { get; set; }
        public string Tipo { get; set; }

        public static void CreateTable() {
            SQLOperations.NonQuery("Erro ao criar tabela de tipo de procedimentos.",
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(Tipo)} nvarchar(100) unique not null," +
                $"  primary key ({nameof(ID)})" +
                $");");
        }

        private static void Create(Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void NonQuery(string title, Func<MySqlCommand, MySqlCommand> func) {
            SQLOperations.NonQuery(title, func, Create, true);
        }

        private static void QueryRLoop(string title, MySqlCommand c, Action<MySqlDataReader> action) {
            SQLOperations.QueryRLoop(title, c, action, Create, true);
        }

        public static void Insert(string nome) {
            NonQuery("Erro ao inserir tipo de procedimento.", (c) => {
                c.CommandText = $"insert into {Name} values (null, @nome);";
                c.Parameters.AddWithValue("@nome", nome);
                return c;
            });
        }

        public static void Update(int id, string nome) {
            NonQuery("Erro ao atualizar nome de tipo de procedimento médico.", (c) => {
                c.CommandText =
                $"update {Name} set {nameof(Tipo)} = @nome " +
                $"where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@nome", nome);
                return c;
            });
        }

        public static void Delete(int id) {
            NonQuery("Erro ao deletar tipo de procedimento.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                return c;
            });
        }

        public static int? Select(string tipo) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select {nameof(ID)} " +
                $"from {Name} " +
                $"where {nameof(Tipo)} = @tipo " +
                $"limit 1;";
            c.Parameters.AddWithValue("@tipo", tipo);
            int? a = null;
            QueryRLoop("Erro ao obter um id com string de tipo fornecida.", c, (r) => {
                a = r.GetInt32(0);
            });
            return a;
        }

        public static int? Select(ComboBox cb) {
            return Select((cb.SelectedItem as ComboBoxItem));
        }

        public static int? Select(ComboBoxItem cbi) {
            return Select(cbi.Content.ToString());
        }

        public static string Select(int id) {
            var c = new MySqlCommand();
            c.CommandText = $"select Tipo from {Name} where {nameof(ID)} = @id;";
            c.Parameters.AddWithValue("@id", id);
            string a = null;
            QueryRLoop("Erro ao obter nome do tipo de procedimento.", c, (r) => {
                a = r.GetString(0);
            });
            return a;
        }

        public static List<TipoProcedimento> SelectLike(string tipo) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select * " +
                $"from {Name} " +
                $"where {nameof(Tipo)} like concat(@tipo, '%') " +
                $"limit 500;";
            c.Parameters.AddWithValue("@tipo", tipo);
            var lista = new List<TipoProcedimento>();
            QueryRLoop("Erro ao obter tipo de procedimento.", c, (r) => {
                lista.Add(new TipoProcedimento() {
                    ID = r.GetInt32(0),
                    Tipo = r.GetString(1)
                });
            });
            return lista;
        }

        public static void Fill(ComboBox cb) {
            cb.Items.Clear();
            var tipos = TipoProcedimento.SelectLike("");
            foreach (var item in tipos) {
                cb.Items.Add(new ComboBoxItem() {
                    Content = item.Tipo
                });
            }
        }

        public static void SetItem(ComboBox cb, int id) {
            var tipo = Select(id);
            cb.SelectedIndex = -1;
            for (int i = 0; i < cb.Items.Count; i++) {
                var item = cb.Items[i] as ComboBoxItem;
                if (item.Content.ToString() == tipo) {
                    cb.SelectedIndex = i;
                    break;
                }
            }
        }

    }
}
