using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Agendamentos {

        public const string Name = "agendamentos";

        public int ID { get; set; }
        public DateTime _Data { get; set; }
        public TimeSpan Inicio { get; set; }
        public TimeSpan Fim { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar tabela de agendamentos",
                $"create table if not exists {Name}(" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(_Data)} date not null," +
                $"  {nameof(Inicio)} time not null," +
                $"  {nameof(Fim)} time not null," +
                $"  primary key ({nameof(ID)})" +
                $");");
        }

        public static void Insert(DateTime date, TimeSpan inicio, TimeSpan fim) {
            NonQuery("Erro ao inserir agendamento.", (c) => {
                c.CommandText = $"insert into {Name} values (null, @date, @begin, @end);";
                c.Parameters.AddWithValue("@date", date.ToString("yyyy-MM-dd"));
                c.Parameters.AddWithValue("@begin", $"{inicio.Hours}:{inicio.Minutes}:{inicio.Seconds}");
                c.Parameters.AddWithValue("@end", $"{fim.Hours}:{fim.Minutes}:{fim.Seconds}");
                return c;
            });
        }

        public static void Delete(int id) {
            NonQuery("Erro ao deletar agendamento",
                $"delete from {Name} where {nameof(ID)} = {id};");
        }

        public static void Delete(string id) {
            Delete(ToInt32(id));
        }

        public static Agendamentos Select(int id) {
            var a = new Agendamentos();
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(_Data)}, {nameof(Inicio)}, {nameof(Fim)} from {Name} where {nameof(ID)} = @id";
            c.Parameters.AddWithValue("@id", id);
            QueryR("Erro ao busca em agendamentos", c, (r) => {
                if (r.Read()) {
                    var dt = r.GetMySqlDateTime(0);
                    a._Data = dt.GetDateTime();
                    a.Inicio = Database.GetTimeSpan(r.GetString(1));
                    a.Fim = Database.GetTimeSpan(r.GetString(2));
                } else {
                    a = null;
                }
            });
            return a;
        }

        /// <summary>
        /// Id
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int? Select(DateTime dt, TimeSpan begin, TimeSpan end) {
            var c = new MySqlCommand();
            c.CommandText = $"select id from {Name} where {nameof(_Data)} = @data and {nameof(Inicio)} = @inicio and {nameof(Fim)} = @fim";
            c.Parameters.AddWithValue("@data", dt.ToString("yyyy-MM-dd"));
            c.Parameters.AddWithValue("@inicio", $"{begin.Hours}:{begin.Minutes}:{begin.Seconds}");
            c.Parameters.AddWithValue("@fim", $"{end.Hours}:{end.Minutes}:{end.Seconds}");
            int? a = null;
            QueryRLoop("Erro ao obter identificador do agendamento", c, (r) => {
                a = r.GetInt32(0);
            });
            return a;
        }

        public static List<Agendamentos> Select(DateTime begin, DateTime end, string medico = null) {
            var agendamento = new AgendamentoFuncionario();
            var c = new MySqlCommand();
            var a = "";
            var b = "";
            if (medico != null) {
                a = $"inner join {AgendamentoFuncionario.Name} as b on a.{nameof(ID)} = b.{nameof(agendamento.Agendamento)}";
                b = $" and b.{nameof(agendamento.Funcionario)} = @medico";
            }
            c.CommandText = $"select * " +
                $"from {Name} as a " +
                $"{a} " +
                $"where ({nameof(_Data)} between @begin and @end) {b};";
            c.Parameters.AddWithValue("@begin", begin.ToString("yyyy-MM-dd"));
            c.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd"));
            if (medico != null) c.Parameters.AddWithValue("@medico", medico);
            var result = new List<Agendamentos>();
            QueryR("Erro ao pesquisar agendamentos.", c, (r) => {
                while (r.Read()) {
                    var agen = new Agendamentos {
                        ID = r.GetInt32(0),
                        _Data = r.GetMySqlDateTime(1).GetDateTime(),
                        Inicio = Database.GetTimeSpan(r.GetString(2)),
                        Fim = Database.GetTimeSpan(r.GetString(3))
                    };
                    result.Add(agen);
                }
            });
            return result;
        }

    }

}
