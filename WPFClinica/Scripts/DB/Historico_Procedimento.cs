using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class Historico_Procedimento {

        public const string Name = nameof(Historico_Procedimento);

        public int ID { get; set; }
        public int Proc_ID { get; set; }
        public string Paciente { get; set; }
        public DateTime RealizadoEm { get; set; }
        public DateTime? PagoEm { get; set; }
        public double Valor { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar tabela de histórico de procedimento.",
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(Proc_ID)} int," +
                $"  {nameof(Paciente)} varchar(15)," +
                $"  {nameof(RealizadoEm)} datetime," +
                $"  {nameof(PagoEm)} datetime null," +
                $"  {nameof(Valor)} double," +
                $"  primary key ({nameof(ID)})," +
                $"  foreign key ({nameof(Paciente)}) references {Pacientes.Name} ({nameof(Pacientes.CPF)}) on delete no action on update cascade," +
                $"  foreign key ({nameof(Proc_ID)}) references {Procedimentos.Name} ({nameof(Procedimentos.ID)}) on delete no action on update cascade" +
                $");");
        }

        public static void Insert(int proc, string cpf, DateTime realizacao, DateTime? pago, double valor) {
            NonQuery($"Erro ao inserir histórico de procedimentos. ({ErrorCodes.DB0034})", (c) => {
                c.CommandText = $"insert into {Name} (Proc_ID, Paciente, RealizadoEm, PagoEm, Valor) values (@proc, @pac, @rea, {(pago.HasValue ? "@pag" : "null")}, @val);";
                c.Parameters.AddWithValue("@proc", proc);
                c.Parameters.AddWithValue("@pac", cpf);
                c.Parameters.AddWithValue("@rea", realizacao.ToString("yyyy-MM-dd HH:mm:ss"));
                if (pago != null) c.Parameters.AddWithValue("@pag", pago.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@val", valor);
                return c;
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
            }, true);
        }

        public static void Update_Realizacao(int id, DateTime realizacao) {
            NonQuery("Erro ao atualizar data de realizacao de procedimento médico", (c) => {
                c.CommandText = $"update {Name} set {nameof(RealizadoEm)} = @rea where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@rea", realizacao.ToString("yyyy-MM-dd HH:mm:ss"));
                return c;
            });
        }

        public static void Delete(int id) {
            NonQuery("Erro ao deletar histórico procedimento médico.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                return c;
            });
        }

        public static List<Historico_Procedimento> Select(int proc, string paciente, int limit = 1) {
            var c = new MySqlCommand(
                $"select" +
                $"  a.{nameof(ID)}, " +
                $"  a.{nameof(Proc_ID)}, " +
                $"  a.{nameof(Paciente)}, " +
                $"  a.{nameof(RealizadoEm)}," +
                $"  a.{nameof(PagoEm)}," +
                $"  a.{nameof(Valor)} " +
                $"from {Name} as a " +
                $"where a.{nameof(Paciente)} = @cpf and a.{nameof(Proc_ID)} = {proc} " +
                $"order by {nameof(ID)} desc " +
                $"limit {limit};");
            c.Parameters.AddWithValue("@cpf", paciente);
            var lista = new List<Historico_Procedimento>();
            QueryRLoop($"Erro em historico-procedimento. ({ErrorCodes.DB0055})", c, (r) => {
                fillList(lista, r);
            });
            return lista;
        }

        public static List<_HistoricoProcedimento> Select_Realizado(string procedimento, string tipo, DateTime begin, DateTime end) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select " +
                $"  a.{nameof(ID)}, " +
                $"  a.{nameof(Proc_ID)}, " +
                $"  a.{nameof(Paciente)}, " +
                $"  a.{nameof(RealizadoEm)}," +
                $"  a.{nameof(PagoEm)}," +
                $"  a.{nameof(Valor)}," +
                $"  b.{nameof(Pessoas.Nome)}," +
                $"  c.{nameof(Procedimentos.Tipo)}," +
                $"  c.{nameof(Procedimentos.Descricao)}," +
                $"  d.{nameof(TipoProcedimento.Tipo)} " +
                $"from {Name} as a " +
                $"inner join {Pessoas.p.TableName} as b on a.{nameof(Paciente)} = b.{nameof(Pessoas.CPF)} " +
                $"inner join {Procedimentos.Name} as c on a.{nameof(Proc_ID)} = c.{nameof(Procedimentos.ID)} " +
                $"inner join {TipoProcedimento.Name} as d on c.{nameof(Procedimentos.Tipo)} = d.{nameof(TipoProcedimento.ID)} " +
                $"where " +
                $"  c.{nameof(Procedimentos.Descricao)} like concat(@proc, '%') and " +
                $"  d.{nameof(TipoProcedimento.Tipo)} like concat(@tipo, '%') and " +
                $"  ({nameof(RealizadoEm)} between @begin and @end) " +
                $"limit 1000;";
            c.Parameters.AddWithValue("@proc", procedimento);
            c.Parameters.AddWithValue("@tipo", tipo);
            c.Parameters.AddWithValue("@begin", begin.ToString("yyyy-MM-dd HH:mm:ss"));
            c.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));
            var lista = new List<_HistoricoProcedimento>();
            QueryRLoop($"Erro ao obter histórico de procedimento. ({ErrorCodes.DB0039})", c, (r) => {
                lista.Add(new _HistoricoProcedimento() {
                    ID = r.GetInt32(0),
                    Proc_ID = r.GetInt32(1),
                    CPF = r.GetString(2),
                    RealizadoEm = r.GetMySqlDateTime(3).GetDateTime(),
                    PagoEm = r.IsDBNull(4) ? (DateTime?)null : r.GetMySqlDateTime(4).GetDateTime(),
                    Valor = r.GetDouble(5),
                    Nome = r.GetString(6),
                    Tipo_ID = r.GetInt32(7),
                    Procedimento = r.GetString(8),
                    Tipo = r.GetString(9)
                });
            });
            return lista;
        }

        public static void fillList(List<Historico_Procedimento> lista, MySqlDataReader r) {
            lista.Add(new Historico_Procedimento() { 
                ID = r.GetInt32(0),
                Proc_ID = r.GetInt32(1),
                Paciente = r.GetString(2),
                RealizadoEm = r.GetMySqlDateTime(3).GetDateTime(),
                PagoEm = r.IsDBNull(4) ? (DateTime?)null : r.GetMySqlDateTime(4).GetDateTime(),
                Valor = r.GetDouble(5)
            });
        }

    }
}
