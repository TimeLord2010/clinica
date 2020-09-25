using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.EnumStrings;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class PacienteProcedimentos {

        public const string Name = nameof(PacienteProcedimentos);

        public int Senha { get; set; }
        public int Procedimento { get; set; }
        public int Convenio { get; set; }
        public ProcedimentoStatus _Status { get; set; }

        public static void CreateTable() {
            var le = new ListaEspera();
            var pc = new ProcedimentoConvenio();
            NonQuery($"Erro ao criar a associação de paciente e procedimentos.", $"create table if not exists {nameof(PacienteProcedimentos)} (" +
                $"  {nameof(Senha)} int," +
                $"  {nameof(Procedimento)} int," +
                $"  {nameof(Convenio)} int," +
                $"  {nameof(_Status)} enum('Pendente', 'Em andamento', 'Pronto')," +
                $"  primary key ({nameof(Senha)}, {nameof(Procedimento)})," +
                $"  foreign key ({nameof(Senha)}) references {ListaEspera.Name} ({nameof(le.Senha)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Procedimento)}, {nameof(Convenio)}) references {ProcedimentoConvenio.Name} ({nameof(pc.Procedimento)}, {nameof(pc.Convenio)}) on delete cascade on update cascade" +
                $");");
        }

        public static void DropTable() {
            NonQuery("Erro ao deletar associações de paciente e procedimentos", $"drop table {nameof(PacienteProcedimentos)};");
        }

        public static void RecreateTable() {
            DropTable();
            CreateTable();
        }

        private static void Create (Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void NonQuery (string title, string q) {
            NonQuery(title, (c) => {
                c.CommandText = q;
                return c;
            });
        }

        private static void NonQuery (string title, Func<MySqlCommand, MySqlCommand> func) {
            SQLOperations.NonQuery(title, func, Create, true);
        }

        public static void Delete_Senha(string senha) {
            Delete_Senha(ToInt32(senha));
        }

        public static void Delete_Senha(int senha) {
            NonQuery($"Erro ao deletar procedimentos de paciente. ({ErrorCodes.DB0003})", (c) => {
                c.CommandText = $"delete from {nameof(PacienteProcedimentos)} where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                return c;
            });
        }

        public static void Delete(int senha, int procedimento) {
            NonQuery($"Erro ao deletar procedimento do paciente. ({ErrorCodes.DB0004})", (c) => {
                c.CommandText = $"delete from {nameof(PacienteProcedimentos)} " +
                $"where {nameof(Senha)} = @senha and {nameof(Procedimento)} = @procedimento;";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@procedimento", procedimento);
                return c;
            });
        }

        public static void Delete(string senha, string procedimento) {
            Delete(ToInt32(senha), ToInt32(procedimento));
        }

        public static void Insert(int senha, int procedimento, int convenio, ProcedimentoStatus status) {
            NonQuery($"Erro ao inserir associação de paciente e procedimento.", (c) => {
                c.CommandText = $"insert into {nameof(PacienteProcedimentos)} values (@senha, @procedimento, @convenio, @status);";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@procedimento", procedimento);
                c.Parameters.AddWithValue("@convenio", convenio);
                c.Parameters.AddWithValue("@status", Get(status));
                return c;
            });
        }

        public static void Update(int senha, int procedimento, ProcedimentoStatus status) {
            NonQuery("Erro ao atualizar o status do procedimento.", (c) => {
                c.CommandText = $"update {nameof(PacienteProcedimentos)} set {nameof(_Status)} = @status where {nameof(Senha)} = @senha and {nameof(Procedimento)} = @proc;";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@proc", procedimento);
                c.Parameters.AddWithValue("@status", Get(status));
                return c;
            });
        }

        public static void Update(int senha, int procedimento, string convenio) {
            NonQuery("Erro ao atualizar associação de paciente e procedimento.", (c) => {
                c.CommandText = $"update {nameof(PacienteProcedimentos)} set {nameof(Convenio)} = @c " +
                $"where {nameof(Senha)} = @s and {nameof(Procedimento)} = @p;";
                c.Parameters.AddWithValue("@s", senha);
                c.Parameters.AddWithValue("@p", procedimento);
                c.Parameters.AddWithValue("@c", convenio);
                return c;
            });
        }

        public static ProcedimentoStatus Select(int senha, string procedimento) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(Senha)} = @senha and {nameof(procedimento)} = @procedimento;";
            c.Parameters.AddWithValue("@senha", senha);
            c.Parameters.AddWithValue("@procedimento", procedimento);
            ProcedimentoStatus ps = ProcedimentoStatus.Pendente;
            QueryRLoop("Erro ao obter associação de paciente-procedimento.", c, (r) => {
                SetProcedimento(r.GetString(2));
            });
            return ps;
        }

        public static List<_PacienteProcedimento> Select(int senha) {
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  b.{nameof(ProcedimentosLab.ID)}, " +
            $"  b.{nameof(ProcedimentosLab.Procedimento)}, " +
            $"  a.{nameof(Convenio)}, " +
            $"  a.{nameof(_Status)}, " +
            $"  c.{nameof(ProcedimentoConvenio.Valor)}," +
            $"  d.{nameof(Convenios.Convenio)} " +
            $"from {Name} as a " +
            $"inner join {ProcedimentosLab.Name} as b on a.{nameof(Procedimento)} = b.{nameof(ProcedimentosLab.ID)} " +
            $"inner join {ProcedimentoConvenio.Name} as c on a.{nameof(Procedimento)} = c.{nameof(ProcedimentoConvenio.Procedimento)} and a.{nameof(Convenio)} = c.{nameof(ProcedimentoConvenio.Convenio)} " +
            $"inner join {Convenios.Name} as d on a.{nameof(Convenio)} = d.{nameof(Convenios.ID)} " +
            $"where {nameof(Senha)} = @senha;";
            c.Parameters.AddWithValue("@senha", senha);
            var lista = new List<_PacienteProcedimento>();
            QueryRLoop("Erro ao obter associação de paciente-procedimento.", c, (r) => {
                lista.Add(new _PacienteProcedimento() {
                    Proc_ID = r.GetInt32(0),
                    Procedimento = r.GetString(1),
                    Con_ID = r.GetInt32(2),
                    Status = r.GetString(3),
                    Valor = r.GetDouble(4),
                    Convenio = r.GetString(5)
                });
            });
            return lista;
        }

        public static List<_PacienteProcedimento> SelectLike_NomePaciente(string nome) {
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  a.{nameof(ListaEspera.Senha)}, " +
            $"  {nameof(Pessoas.Nome)}, " +
            $"  {nameof(ListaEspera._DateTime)}," +
            $"  c.{nameof(Pessoas.CPF)} " +
            $"from {Name} as a " +
            $"inner join {ListaEspera.Name} as b on a.{nameof(Senha)} = b.{nameof(ListaEspera.Senha)} " +
            $"inner join {Pessoas.p.TableName} as c on c.{nameof(Pessoas.CPF)} = b.{nameof(ListaEspera.Paciente)} " +
            $"where {nameof(Pessoas.Nome)} like concat(@nome, '%') and " +
            $"  {nameof(ListaEspera.Fila)} = @fila and " +
            $"  (b.{nameof(ListaEspera._Status)} <> @status) " +
            $"group by {nameof(ListaEspera.Senha)} " +
            $"limit 200;";
            c.Parameters.AddWithValue("@nome", nome);
            c.Parameters.AddWithValue("@fila", ES.f[Fila.Laboratorio]);
            c.Parameters.AddWithValue("@status", Get(PStatus.Atendido));
            var lista = new List<_PacienteProcedimento>();
            QueryRLoop("Erro ao obter associação de paciente-procedimentos.", c, (r) => {
                lista.Add(new _PacienteProcedimento() {
                    Senha = r.GetInt32(0),
                    Nome_Paciente = r.GetString(1),
                    Atualização = r.GetMySqlDateTime(2).GetDateTime(),
                    CFP_Paciente = r.GetString(3)
                });
            });
            return lista;
        }
    }

}
