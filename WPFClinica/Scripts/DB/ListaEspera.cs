using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static System.Convert;
using static WPFClinica.Scripts.DB.EnumStrings;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class ListaEspera {

        public const string Name = "lista_espera";

        public int Senha { get; set; }
        public string Paciente { get; set; }
        public bool Prioridade { get; set; }
        public DateTime _DateTime { get; set; }
        public Fila Fila { get; set; }
        public PStatus _Status { get; set; }

        public static void CreateTable() {
            var p = new Pacientes();
                var q = 
                $"create table if not exists {Name} (" +
                $"  {nameof(Senha)} int auto_increment, " +
                $"  {nameof(Paciente)} varchar(15) unique not null, " +
                $"  {nameof(_DateTime)} datetime not null, " +
                $"  {nameof(_Status)} nvarchar(20) not null, " +
                $"  {nameof(Fila)} nvarchar(20) not null, " +
                $"  {nameof(Prioridade)} bool not null, " +
                $"  primary key({nameof(Senha)}), " +
                $"  foreign key({nameof(Paciente)}) references {Pacientes.Name} ({nameof(p.CPF)}) on delete cascade on update cascade" +
                $");";
            NonQuery($"Erro ao criar a tabela '{Name}'", q);
        }

        private static void Create (Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
                ListaEspera_Especializacao.CreateTable();
                ListaEspera_Agendamento.CreateTable();
                ListaEspera_Funcionario.CreateTable();
            }
        }

        private static void NonQuery (string title, string q) {
            NonQuery(title, (c) => {
                c.CommandText = q;
                return c;
            });
        }

        private static void NonQuery (string title, Func<MySqlCommand, MySqlCommand> c) {
            SQLOperations.NonQuery(title, c, Create, true);
        }

        private static void QueryRLoop (string title, MySqlCommand c, Action<MySqlDataReader> action) {
            SQLOperations.QueryRLoop(title, c, action, Create, true);
        }

        public static void Insert(string paciente, PStatus status, Fila encaminhado, bool prioridade) {
            Insert(paciente, DateTime.Now, status, encaminhado, prioridade);
        }

        public static void Insert(string paciente, DateTime dt, PStatus status, Fila encaminhado, bool prioridade) {
            NonQuery($"Erro ao inserir em lista de espera. ({ErrorCodes.DB0033})", (c) => {
                c.CommandText = $"insert into {Name} values (null, @paciente, @time, @status, @encaminhado, @prioridade);";
                c.Parameters.AddWithValue("@paciente", paciente);
                c.Parameters.AddWithValue("@time", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@status", Get(status));
                c.Parameters.AddWithValue("@encaminhado", ES.f[encaminhado]);
                c.Parameters.AddWithValue("@prioridade", prioridade);
                return c;
            });
        }

        public static void Insert(string paciente, DateTime dt, string status, string encaminhado, bool prioridade) {
            NonQuery("Erro ao inserir em lista de espera", (c) => {
                c.CommandText = $"insert into {Name} values (null, @paciente, @time, @status, @encaminhado, @prioridade);";
                c.Parameters.AddWithValue("@paciente", paciente);
                c.Parameters.AddWithValue("@time", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@status", status);
                c.Parameters.AddWithValue("@encaminhado", encaminhado);
                c.Parameters.AddWithValue("@prioridade", prioridade);
                return c;
            });
        }

        public static ListaEspera Select_Paciente(string cpfPaciente) {
            ListaEspera lista = null;
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name}  " +
            $"where {nameof(Paciente)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", cpfPaciente);
            QueryR("Erro ao obter lista de espera a partir de cpf do paciente.", c, (r) => {
                lista = GetLista(r);
            });
            return lista;
        }

        /// <summary>
        /// Senha, DateTime, Fila, Status, CPF_Paciente, Nome_Paciente, Prioridade
        /// </summary>
        public static List<_ListaEspera> SelectLike(string cpfPaciente, string nomePaciente, Fila? fila, PStatus? status) {
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  a.{nameof(Senha)}, " +
            $"  a.{nameof(_DateTime)}, " +
            $"  a.{nameof(Fila)}, " +
            $"  a.{nameof(_Status)}, " +
            $"  a.{nameof(Paciente)}, " +
            $"  b.{nameof(Pessoas.Nome)}, " +
            $"  a.{nameof(Prioridade)}  " +
            $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(Paciente)} = b.{nameof(Pessoas.CPF)} " +
            $"where " +
            $"  a.{nameof(Paciente)} like concat(@cpf, '%') and " +
            $"  b.{nameof(Pessoas.Nome)} like concat(@nome, '%') and " +
            $"  a.{nameof(Fila)} like concat(@fila, '%') and " +
            $"  a.{nameof(_Status)} like concat(@status, '%') " +
            $"order by {nameof(_DateTime)} desc " +
            $"limit 200;";
            c.Parameters.AddWithValue("@cpf", cpfPaciente);
            c.Parameters.AddWithValue("@nome", nomePaciente);
            c.Parameters.AddWithValue("@fila", fila == null ? "" : ES.f[fila.Value]);
            c.Parameters.AddWithValue("@status", status == null ? "" : Get(status ?? PStatus.Atendido));
            var lista = new List<_ListaEspera>();
            QueryRLoop("Erro ao obter registros de lista de espera.", c, (r) => {
                lista.Add(new _ListaEspera() {
                    Senha = r.GetInt32(0),
                    Atualizacao = r.GetMySqlDateTime(1).GetDateTime(),
                    Fila = r.GetString(2),
                    Status = r.GetString(3),
                    CPF = r.GetString(4),
                    Nome = r.GetString(5),
                    Prioridade = r.GetBoolean(6)
                });
            });
            return lista;
        }

        public static List<ListaEspera> SelectLike(string cpfPaciente, string nomePaciente, bool? prioridade, Fila? fila, PStatus? status) {
            var list = new List<ListaEspera>();
            var a = "";
            if (prioridade != null) a += "and prioridade = @prioridade ";
            var c = new MySqlCommand {
                CommandText =
                $"select * " +
            $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(Paciente)} = b.{nameof(Pessoas.CPF)} " +
            $"where " +
            $"  b.{nameof(Pessoas.CPF)} like concat(@cpf, '%') and" +
            $"  b.{nameof(Pessoas.Nome)} like concat(@nome, '%') and " +
            $"  a.{nameof(Fila)} like concat(@fila, '%') and " +
            $"  a.{nameof(_Status)} like concat(@status, '%') {a} " +
            $"limit 200;"
            };
            c.Parameters.AddWithValue("@cpf", cpfPaciente);
            c.Parameters.AddWithValue("@nome", nomePaciente);
            c.Parameters.AddWithValue("@fila", fila == null ? "" : ES.f[fila.Value]);
            c.Parameters.AddWithValue("@status", status == null ? "" : Get(status ?? PStatus.Esperando));
            if (prioridade != null) c.Parameters.AddWithValue("@prioridade", prioridade ?? false);
            QueryR("Erro ao pesquisar em lista de espera.", c, (r) => {
                list = GetListas(r);
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
            }, true);
            return list;
        }

        public static List<_ListaEspera> Select(Fila fila, PStatus status) {
            var lista = new List<_ListaEspera>();
            var c = new MySqlCommand();
            c.CommandText =
                $"select " +
                $"  a.{nameof(Senha)}," + // 0
                $"  a.{nameof(Paciente)}," + // 1
                $"  a.{nameof(_DateTime)}," + // 2
                $"  a.{nameof(_Status)}," + // 3
                $"  a.{nameof(Prioridade)}," + // 4
                $"  a.{nameof(Fila)}," + // 5
                $"  b.{nameof(Pessoas.Nome)}," + // 6
                $"  b.{nameof(Pessoas.Nascimento)} " + // 7
                $"from {Name} as a " +
                $"inner join {Pessoas.p.TableName} as b on a.{nameof(Paciente)} = b.{nameof(Pessoas.CPF)} " +
                $"where {nameof(Fila)} = @fila and {nameof(_Status)} = @status " +
                $"order by {nameof(_DateTime)} desc " +
                $"limit 500;";
            c.Parameters.AddWithValue("@fila", ES.f[fila]);
            c.Parameters.AddWithValue("@status", Get(status));
            QueryRLoop("Erro ao obter pesquisa em lista de espera.", c, (r) => {
                lista.Add(new _ListaEspera() {
                    Senha = r.GetInt32(0),
                    CPF = r.GetString(1),
                    Atualizacao = r.GetMySqlDateTime(2).GetDateTime(),
                    Status = r.GetString(3),
                    Prioridade = r.GetBoolean(4),
                    Fila = r.GetString(5),
                    Nome = r.GetString(6),
                    Nascimento_Paciente = r.GetMySqlDateTime(7).GetDateTime()
                });
            });
            return lista;
        }

        public static ListaEspera Select(string senha) {
            return Select(ToInt32(senha));
        }

        public static ListaEspera Select(int senha) {
            ListaEspera lista = null;
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(Senha)} = @senha;";
            c.Parameters.AddWithValue("@senha", senha);
            QueryR("Erro ao obter linha de lista de espera", c, (r) => {
                try {
                    lista = GetLista(r);
                } catch (Exception) { }
            });
            return lista;
        }

        public static List<ListaEspera> SelectM(string cpf_medico, string especializacao) {
            var c = new MySqlCommand();
            c.CommandText = $"select * " +
            $"from {Name} as a " +
            $"left join {nameof(ListaEspera_Funcionario)} as b on a.{nameof(Senha)} = b.{nameof(ListaEspera_Funcionario.Senha)} " +
            $"left join {nameof(ListaEspera_Especializacao)} as c on a.{nameof(Senha)} = c.{nameof(ListaEspera_Especializacao.Senha)} " +
            $"where b.{nameof(ListaEspera_Funcionario.Funcionario)} = @cpf or c.{nameof(ListaEspera_Especializacao.Especializacao)} = @esp " +
            $"limit 500;";
            c.Parameters.AddWithValue("@cpf", cpf_medico);
            c.Parameters.AddWithValue("@esp", especializacao);
            var lista = new List<ListaEspera>();
            QueryRLoop("Erro ao obter lista de espera para o médico.", c, (r) => {
                lista.Add(new ListaEspera() {
                    Senha = r.GetInt32(0),
                    Paciente = r.GetString(1),
                    _DateTime = r.GetMySqlDateTime(2).GetDateTime(),
                    _Status = ES.GetPStatus(r.GetString(3)),
                    Fila = ES.GetFila(r.GetString(4)),
                    Prioridade = r.GetBoolean(5)
                });
            });
            return lista;
        }

        public static List<_ListaEspera> SelectM(string cpf_medico) {
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  a.{nameof(Senha)}, " +
            $"  e.{nameof(Pessoas.Nome)}, " +
            $"  e.{nameof(Pessoas.Nascimento)}, " +
            $"  a.{nameof(_DateTime)}, " +
            $"  a.{nameof(_Status)}, " +
            $"  a.{nameof(Fila)}, " +
            $"  a.{nameof(Prioridade)}, " +
            $"  b.{nameof(ListaEspera_Funcionario.Funcionario)}, " +
            $"  c.{nameof(ListaEspera_Especializacao.Especializacao)} " +
            $"from {Name} as a " +
            $"left join {nameof(ListaEspera_Funcionario)} as b on a.{nameof(Senha)} = b.{nameof(ListaEspera_Funcionario.Senha)} " +
            $"left join {nameof(ListaEspera_Especializacao)} as c on a.{nameof(Senha)} = c.{nameof(ListaEspera_Especializacao.Senha)} " +
            $"inner join {Pessoas.p.TableName} as e on a.{nameof(Paciente)} = e.{nameof(Pessoas.CPF)} " +
            $"where " +
            $"  (b.{nameof(ListaEspera_Funcionario.Funcionario)} = @cpf or b.{nameof(ListaEspera_Funcionario.Funcionario)} is null) and " +
            $"  a.{nameof(_Status)} = @status and " +
            $"  a.{nameof(Fila)} = @fila " +
            $"limit 500;";
            c.Parameters.AddWithValue("@cpf", cpf_medico);
            c.Parameters.AddWithValue("@fila", ES.f[Fila.Consultorio]);
            c.Parameters.AddWithValue("@status", Get(PStatus.Esperando));
            var lista = new List<_ListaEspera>();
            QueryRLoop("Erro ao obter lista de espera para o médico.", c, (r) => {
                lista.Add(new _ListaEspera() {
                    Senha = r.GetInt32(0),
                    Nome = r.GetString(1),
                    Nascimento_Paciente = r.GetMySqlDateTime(2).GetDateTime(),
                    Atualizacao = r.GetMySqlDateTime(3).GetDateTime(),
                    Status = r.GetString(4),
                    Fila = r.GetString(5),
                    Prioridade = r.GetBoolean(6),
                    CPF_Funcionario = r.IsDBNull(7) ? null : r.GetString(7),
                    Especializacao = r.IsDBNull(8) ? null : r.GetString(8)
                });
            });
            return lista;
        }

        /// <summary>
        /// Senha, Nome, Nascimento, DataTime, Status, Fila, Prioridade, Funcionario, Observacao, CPF_Paciente
        /// </summary>
        /// <param name="cpf_medico"></param>
        /// <returns></returns>
        public static List<_ListaEspera> Select2(string cpf_medico) {
            var lista = new List<_ListaEspera>();
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  a.{nameof(Senha)}, " +
            $"  e.{nameof(Pessoas.Nome)}, " +
            $"  e.{nameof(Pessoas.Nascimento)}, " +
            $"  a.{nameof(_DateTime)}, " +
            $"  a.{nameof(_Status)}, " +
            $"  a.{nameof(Fila)}, " +
            $"  a.{nameof(Prioridade)}, " +
            $"  b.{nameof(ListaEspera_Funcionario.Funcionario)}, " +
            $"  a.{nameof(Paciente)} " +
            $"from {Name} as a " +
            $"inner join {nameof(ListaEspera_Funcionario)} as b on a.{nameof(Senha)} = b.{nameof(ListaEspera_Funcionario.Senha)} " +
            $"inner join {Pessoas.p.TableName} as e on a.{nameof(Paciente)} = e.{nameof(Pessoas.CPF)} " +
            $"where " +
            $"  b.{nameof(ListaEspera_Funcionario.Funcionario)} = @cpf and" +
            $"  a.{nameof(_Status)} = '{ES.ps[PStatus.Esperando]}' and " +
            $"  a.{nameof(Fila)} = '{ES.f[Fila.Consultorio]}' " +
            $"limit 500;";
            c.Parameters.AddWithValue("@cpf", cpf_medico);
            QueryRLoop("Erro ao obter lista de espera para o médico.", c, (r) => {
                lista.Add(new _ListaEspera() {
                    Senha = r.GetInt32(0),
                    Nome = r.GetString(1),
                    Nascimento_Paciente = r.GetMySqlDateTime(2).GetDateTime(),
                    Atualizacao = r.GetMySqlDateTime(3).GetDateTime(),
                    Status = r.GetString(4),
                    Fila = r.GetString(5),
                    Prioridade = r.GetBoolean(6),
                    CPF_Funcionario = r.GetString(7),
                    CPF = r.GetString(8)
                });
            });
            return lista;
        }

        public static void InsertOrUpdate(string paciente, bool prioridade, Fila fila, PStatus status) {
            var lista = Select_Paciente(paciente);
            if (lista == null) {
                Insert(paciente, status, fila, prioridade);
            } else {
                Update(paciente, prioridade, fila, status);
                Update_DT_Paciente(paciente);
            }
        }

        public static void Update(string paciente, bool prioridade, Fila fila, PStatus status) {
            NonQuery($"", (c) => {
                c.CommandText = $"update {Name} set " +
                $"  {nameof(Prioridade)} = @prioridade, " +
                $"  {nameof(Fila)} = @fila, " +
                $"  {nameof(_Status)} = @status " +
                $"where {nameof(Paciente)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", paciente);
                c.Parameters.AddWithValue("@prioridade", prioridade);
                c.Parameters.AddWithValue("@fila", ES.f[fila]);
                c.Parameters.AddWithValue("@status", Get(status));
                return c;
            });
        }

        public static void Update(int senha, string paciente, bool prioridade, DateTime dt, Fila fila, PStatus status) {
            NonQuery("Erro ao atualizar lista de espera.", (c) => {
                c.CommandText = $"update {Name} set " +
                $"  {nameof(Paciente)} = @pac, " +
                $"  {nameof(Prioridade)} = @prio, " +
                $"  {nameof(_DateTime)} = @time, " +
                $"  {nameof(Fila)} = @fila, " +
                $"  {nameof(_Status)} = @status " +
                $"where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@pac", paciente);
                c.Parameters.AddWithValue("@prio", prioridade);
                c.Parameters.AddWithValue("@time", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@fila", ES.f[fila]);
                c.Parameters.AddWithValue("@status", Get(status));
                c.Parameters.AddWithValue("@senha", senha);
                return c;
            });
        }

        public static void Update(int senha, Fila fila, PStatus status) {
            NonQuery("Erro ao atualizar lista de espera.", (c) => {
                c.CommandText = $"update {Name} set {nameof(Fila)} = @fila, {nameof(_Status)} = @status " +
                $"where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@fila", ES.f[fila]);
                c.Parameters.AddWithValue("@status", Get(status));
                return c;
            });
        }

        public static void Update (int senha, PStatus status) {
            NonQuery($"Erro ao atualizar status de lista de espera. ({ErrorCodes.DB0040})", (c) => {
                c.CommandText = $"update {Name} set {nameof(_Status)} = @status where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@status", ES.ps[status]);
                return c;
            });
        }

        public static void Update_DT_Senha(int senha) {
            NonQuery("Erro ao atualizar a atualização de lista de espera.", (c) => {
                c.CommandText = $"update {Name} set {nameof(_DateTime)} = @datetime where {nameof(Senha)} = @senha";
                c.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@senha", senha);
                return c;
            });
        }

        public static void Update_DT_Paciente(string paciente) {
            NonQuery("Erro ao atualizar a atualização de lista de espera.", (c) => {
                c.CommandText = $"update {Name} set {nameof(_DateTime)} = @datetime where {nameof(Paciente)} = @cpf;";
                c.Parameters.AddWithValue("@datetime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@cpf", paciente);
                return c;
            });
        }

        public static void Delete(int senha) {
            NonQuery("Erro ao deletar registro de lista de espera.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                return c;
            });
        }

        public static void Delete (string cpf_paciente) {
            if (cpf_paciente == null) {
                throw new ArgumentNullException("cpf nao pode ser nulo para deletar da lista de espera.");
            }
            var le = Select_Paciente(cpf_paciente);
            if (le != null) {
                Delete(le.Senha);
            }
        }

        private static ListaEspera GetLista(MySqlDataReader r) {
            ListaEspera lista = null;
            if (r.Read()) {
                lista = new ListaEspera() {
                    Senha = r.GetInt32(0),
                    Paciente = r.GetString(1),
                    _DateTime = r.IsDBNull(2) ? DateTime.MinValue : r.GetMySqlDateTime(2).GetDateTime(),
                    _Status = r.IsDBNull(3)? PStatus.Atendido : SetPStatus(r.GetString(3)),
                    Fila = r.IsDBNull(4)? Fila.Nulo : ES.GetFila(r.GetString(4)),
                    Prioridade = r.IsDBNull(5)? false : r.GetBoolean(5)
                };
            }
            return lista;
        }

        private static List<ListaEspera> GetListas(MySqlDataReader r) {
            var lista = new List<ListaEspera>();
            while (r.Read()) {
                lista.Add(new ListaEspera() {
                    Senha = r.GetInt32(0),
                    Paciente = r.GetString(1),
                    _DateTime = r.GetMySqlDateTime(2).GetDateTime(),
                    _Status = SetPStatus(r.GetString(3)),
                    Fila = ES.GetFila(r.GetString(4)),
                    Prioridade = r.GetBoolean(5)
                });
            }
            return lista;
        }

    }
}
