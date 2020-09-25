using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Historico_ProcedimentosLab {

        public const string Name = nameof(Historico_ProcedimentosLab);

        public int ID { get; set; }
        public string Paciente { get; set; }
        public int Procedimento { get; set; }
        public int Convenio { get; set; }
        public DateTime? RealizadoEm { get; set; }
        public DateTime? PagoEm { get; set; }
        public double Pago { get; set; }
        public bool Is_enabled { get; set; }
        public bool was_uploaded { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar tabela de histórico de procedimentos.", (c) => {
                c.CommandText =
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment, " +
                $"  {nameof(Paciente)} varchar(15) not null," +
                $"  {nameof(Procedimento)} int null," +
                $"  {nameof(Convenio)} int null," +
                $"  {nameof(RealizadoEm)} datetime null," +
                $"  {nameof(PagoEm)} datetime null," +
                $"  {nameof(Pago)} float," +
                $"  {nameof(Is_enabled)} bit default 1," +
                $"  primary key ({nameof(ID)})," +
                $"  foreign key ({nameof(Paciente)}) references {Pacientes.Name} ({nameof(Pacientes.CPF)}) on delete no action on update cascade," +
                $"  foreign key ({nameof(Convenio)}) references {Convenios.Name} ({nameof(Convenios.ID)}) on delete set null on update cascade," +
                $"  foreign key ({nameof(Procedimento)}) references {ProcedimentosLab.Name} ({nameof(ProcedimentosLab.ID)}) on delete set null on update cascade" +
                $");";
                return c;
            });
        }

        private static void Create (Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void QueryRLoop (string title, MySqlCommand c, Action<MySqlDataReader> action) {
            SQLOperations.QueryRLoop(title, c, action, Create, true);
        }

        private static void NonQuery (string title, Func<MySqlCommand, MySqlCommand> func) {
            SQLOperations.NonQuery(title, func, Create, true);
        }

        public static void Insert(string paciente, int procedimento, int convenio, DateTime? realizadoEm, DateTime? pagoEm, double pago) {
            //var properties = typeof(Historico_ProcedimentosLab).GetProperties().Where(x => x.Name != nameof(Is_enabled)).Select(x => $"`{x.Name}`");
            NonQuery($"Erro ao inserir {Name}. ({ErrorCodes.DB0013})", (c) => {
                // ({string.Join(", ", properties)})
                c.CommandText = $"insert into {Name} (Paciente, Procedimento, Convenio, RealizadoEm, PagoEm, Pago, Is_enabled) values  (@pac, @proc, @con, {(realizadoEm.HasValue? "@rea" : "null")},{(pagoEm.HasValue? "@pagoEm" : "null")}, @pago, 1);";
                c.Parameters.AddWithValue("@pac", paciente);
                c.Parameters.AddWithValue("@proc", procedimento);
                c.Parameters.AddWithValue("@con", convenio);
                if (realizadoEm.HasValue) c.Parameters.AddWithValue("@rea", realizadoEm.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                if (pagoEm.HasValue) c.Parameters.AddWithValue("@pagoEm", pagoEm.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@pago", pago);
                return c;
            });
        }

        public static List<_Historico_ProcedimentosLab> Select(string paciente, int procedimento, int convenio, DateTime begin, DateTime end) {
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  a.{nameof(ID)}, " + // 0
            $"  a.{nameof(Paciente)}, " + // 1
            $"  b.{nameof(Pessoas.Nome)}, " + // 2
            $"  a.{nameof(Procedimento)}, " + // 3
            $"  c.{nameof(ProcedimentosLab.Procedimento)}, " + // 4
            $"  a.{nameof(Pago)}, " + // 5
            $"  a.{nameof(RealizadoEm)}," + // 6
            $"  a.{nameof(PagoEm)}," + // 7
            $"  a.{nameof(Convenio)} " + // 8
            $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(Paciente)} = b.{nameof(Pessoas.CPF)} " +
            $"inner join {ProcedimentosLab.Name} as c on a.{nameof(Procedimento)} = c.{nameof(ProcedimentosLab.ID)} " +
            $"where " +
            $"  a.{nameof(Paciente)} = @paciente and " +
            $"  a.{nameof(Procedimento)} = {procedimento} and " +
            $"  (a.{nameof(PagoEm)} between @begin and @end) and " +
            $"  a.{nameof(Convenio)} = {convenio} and" +
            $"  {nameof(Is_enabled)} = 1 " +
            $"limit 500;";
            c.Parameters.AddWithValue("@paciente", paciente);
            c.Parameters.AddWithValue("@begin", begin.ToString("yyyy-MM-dd HH:mm:ss"));
            c.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));
            var a = new List<_Historico_ProcedimentosLab>();
            QueryRLoop($"Erro ao obter {Name}. ({ErrorCodes.DB0011})", c, (r) => {
                a.Add(new _Historico_ProcedimentosLab() {
                    ID = r.GetInt32(0),
                    CPF = r.GetString(1),
                    Nome = r.GetString(2),
                    Procedimento_ID = r.GetInt32(3),
                    ProcedimentoLab = r.GetString(4),
                    Pago = r.GetDouble(5),
                    RealizadoEm = r.IsDBNull(6) ? (DateTime?)null : r.GetMySqlDateTime(6).GetDateTime(),
                    PagoEm = r.GetMySqlDateTime(7).GetDateTime(),
                    Con_ID = r.GetInt32(8)
                });
            });
            return a;
        }

        public static List<_Historico_ProcedimentosLab> SelectLike (string procedimento, string convenio, DateTime inicio, DateTime fim, string paciente, bool includeAll) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select " +
                $"  a.{nameof(ID)}," + //0
                $"  a.{nameof(Procedimento)}," + //1
                $"  a.{nameof(Convenio)}," + //2
                $"  a.{nameof(Paciente)}," + //3
                $"  a.{nameof(RealizadoEm)}," + //4
                $"  a.{nameof(PagoEm)}," + //5
                $"  a.{nameof(Pago)}," + //6
                $"  b.{nameof(ProcedimentosLab.Procedimento)}," + //7
                $"  c.{nameof(Convenios.Convenio)}," + //8
                $"  d.{nameof(Pessoas.Nome)} " + //9
                $"from {Name} as a " +
                $"left join {ProcedimentosLab.Name} as b on a.{nameof(Procedimento)} = b.{nameof(ProcedimentosLab.ID)} " +
                $"left join {Convenios.Name} as c on a.{nameof(Convenio)} = c.{nameof(Convenios.ID)} " +
                $"left join {Pessoas.p.TableName} as d on a.{nameof(Paciente)} = d.{nameof(Pessoas.CPF)} " +
                $"where " +
                $"  b.{nameof(ProcedimentosLab.Procedimento)} like concat(@proc, '%') and " +
                $"  c.{nameof(Convenios.Convenio)} like concat(@con, '%') and " +
                $"  (a.{nameof(PagoEm)} between @ini and @fim) and " +
                $"  d.{nameof(Pessoas.Nome)} like concat('%' , @nome_paciente, '%') " +
                $"  {(includeAll? "" : $"and a.{nameof(PagoEm)} is not null")} and " +
                $"  {nameof(Is_enabled)} = 1 " +
                $"order by a.{nameof(PagoEm)} desc " +
                $"limit 10000;";
            c.Parameters.AddWithValue("@proc", procedimento);
            c.Parameters.AddWithValue("@con", convenio);
            c.Parameters.AddWithValue("@ini", inicio.ToString("yyyy-MM-dd HH:mm:ss"));
            c.Parameters.AddWithValue("@fim", fim.ToString("yyyy-MM-dd HH:mm:ss"));
            c.Parameters.AddWithValue("@nome_paciente", paciente);
            var lista = new List<_Historico_ProcedimentosLab>();
            QueryRLoop($"Erro ao obter {Name}. ({ErrorCodes.DB0014})", c, (r) => {
                lista.Add(new _Historico_ProcedimentosLab() {
                    ID = r.GetInt32(0),
                    Procedimento_ID = r.GetInt32(1),
                    Con_ID = r.GetInt32(2),
                    CPF = r.GetString(3),
                    RealizadoEm = r.IsDBNull(4) ? (DateTime?)null : r.GetMySqlDateTime(4).GetDateTime(),
                    PagoEm = r.IsDBNull(5)? (DateTime?)null : r.GetMySqlDateTime(5).GetDateTime(),
                    Pago = r.GetDouble(6),
                    ProcedimentoLab = r.GetString(7),
                    Convenio = r.GetString(8),
                    Nome = r.GetString(9)
                });
            });
            return lista;
        }

        public static void Update_RealizadoEm(string paciente, int procedimento, int convenio, DateTime? realizadoEm) {
            int id = GetLastID(paciente, procedimento, convenio);
            NonQuery("Erro ao atualizar data de realização.", (c) => {
                c.CommandText = $"update {Name} set " +
                $"  {nameof(RealizadoEm)} = {(realizadoEm.HasValue? "@r" : "null")} " +
                $"where " +
                $"  {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                if (realizadoEm.HasValue) c.Parameters.AddWithValue("@r", realizadoEm.Value.ToString("yyyy-MM-dd HH:mm:ss"));
                return c;
            });
        }

        public static void Update_PagoEm(string paciente, int procedimento, int convenio, DateTime pagoEm, double valor) {
            int id = GetLastID(paciente, procedimento, convenio);
            NonQuery("Erro ao atualizar pagamento de historico de procedimento laboratorial", (c) => {
                c.CommandText =
                $"update {Name} set " +
                $"  {nameof(PagoEm)} = @pe," +
                $"  {nameof(Pago)} = @v " +
                $"where" +
                $"  {nameof(ID)} = @id";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@pe", pagoEm.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@v", valor);
                return c;
            });
        }

        public static void Update_IsEnabled (int id, bool is_enabled) {
            MySql_Handler.NonQuery($"update {Name} set {nameof(Is_enabled)} = {(is_enabled? 1 : 0)} where {nameof(ID)} = {id};");
        }

        private static int GetLastID(string paciente, int procedimento, int convenio) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select {nameof(ID)} " +
                $"from {Name} " +
                $"where " +
                $"  {nameof(Paciente)} = @pac and" +
                $"  {nameof(Procedimento)} = @proc and" +
                $"  {nameof(Convenio)} = @con and" +
                $"  {nameof(Is_enabled)} = 1 " +
                $"order by {nameof(ID)} desc " +
                $"limit 1;";
            c.Parameters.AddWithValue("@pac", paciente);
            c.Parameters.AddWithValue("@proc", procedimento);
            c.Parameters.AddWithValue("@con", convenio);
            int id = -1;
            QueryRLoop($"Erro ao obter {Name}. ({ErrorCodes.DB0012})", c, (r) => {
                id = r.GetInt32(0);
            });
            if (id == -1) {
                throw new Exception("Identificador não encontrado.");
            }
            return id;
        }


    }

}
