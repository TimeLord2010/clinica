using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;
using static WPFClinica.Scripts.DB.EnumStrings;
using MySql.Data.MySqlClient;

namespace WPFClinica.Scripts.DB {
    public class Salas {

        public const string Name = nameof(Salas);

        public string Funcionario { get; set; }
        public string Nome { get; set; }
        public string Funcao { get; set; }
        public string _Status { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            var title = $"Erro ao criar a tabela {nameof(Salas)}.";
            var q =
                $"create table if not exists {nameof(Salas)} (" +
                $"  {nameof(Funcionario)} varchar(15)," +
                $"  {nameof(Nome)} nvarchar(50) unique," +
                $"  {nameof(Funcao)} enum('Triagem', 'Consultorio', 'Laboratorio')," +
                $"  {nameof(_Status)} enum('Em atendimento', 'Ocupado', 'Livre', 'Aguardando')," +
                $"   primary key({nameof(Funcionario)})," +
                $"   foreign key({nameof(Funcionario)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");";
            NonQuery(title, q);
        }

        private static void Create (Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void QueryRLoop (string title, MySqlCommand c, Action<MySqlDataReader> r) {
            SQLOperations.QueryRLoop(title, c, r, Create, true);
        }

        public static void Insert(string funcionario, string sala, SStatus status, Fila fila) {
            NonQuery($"Erro ao inserir registro em {nameof(Salas)}.", (c) => {
                c.CommandText = $"insert into {Name} values (@medico, @sala, @fila, @status);";
                c.Parameters.AddWithValue("@medico", funcionario);
                c.Parameters.AddWithValue("@sala", sala);
                c.Parameters.AddWithValue("@fila", ES.f[fila]);
                c.Parameters.AddWithValue("@status", Get(status));
                return c;
            });
        }

        public static void Update(string funcionario, string sala, SStatus status, Fila fila) {
            NonQuery($"Erro ao atualizar sala.", (c) => {
                c.CommandText = $"update {Name} " +
                $"set {nameof(Nome)} = @sala, {nameof(_Status)} = @status, {nameof(Funcao)} = @fila " +
                $"where {nameof(Funcionario)} = @funcionario;";
                c.Parameters.AddWithValue("@funcionario", funcionario);
                c.Parameters.AddWithValue("@sala", sala);
                c.Parameters.AddWithValue("@fila", ES.f[fila]);
                c.Parameters.AddWithValue("@status", Get(status));
                return c;
            });
        }

        public static void InsertOrUpdate(string funcionario, string sala, SStatus status, Fila fila) {
            var r = Select(funcionario);
            if (r == null) {
                Insert(funcionario, sala, status, fila);
            } else {
                Update(funcionario, sala, status, fila);
            }
        }

        /// <summary>
        /// CPF_Funcionario, Nome_Funcionario, Nome, Funcao, Status
        /// </summary>
        /// <param name="funcionario"></param>
        /// <returns></returns>
        public static _Sala? Select(string funcionario) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select " +
                $"  a.{nameof(Funcionario)}, " +
                $"  b.{nameof(Pessoas.Nome)}, " +
                $"  a.{nameof(Nome)}," +
                $"  a.{nameof(Funcao)}, " +
                $"  a.{nameof(_Status)} " +
                $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(Funcionario)} = b.{nameof(Pessoas.CPF)} " +
            $"where {nameof(Funcionario)} = @f;";
            c.Parameters.AddWithValue("@f", funcionario);
            _Sala? sala = null;
            QueryRLoop("Erro ao obter sala.", c, (r) => {
                sala = new _Sala {
                    CPF_Funcionario = r.GetString(0),
                    Nome_Funcionario = r.GetString(1),
                    Nome = r.GetString(2),
                    Funcao = r.GetString(3),
                    Status = r.GetString(4)
                };
            });
            return sala;
        }

        public static Salas Select_Sala(string nome) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(Nome)} = @nome;";
            c.Parameters.AddWithValue("@nome", nome);
            Salas salas = null;
            QueryRLoop("Erro ao obter sala através do nome da sala.", c, (r) => {
                salas = new Salas();
                salas.Funcionario = r.GetString(0);
                salas.Nome = r.GetString(1);
                salas.Funcao = r.GetString(2);
                salas._Status = r.GetString(3);
            });
            return salas;
        }

        /// <summary>
        /// NomeFuncionario, NomeSala, FuncaoSala, StatusSala
        /// </summary>
        /// <param name="funcionario">Nome do funcionário</param>
        /// <param name="sala">Nome da sala</param>
        /// <param name="fila">Função da sala</param>
        /// <param name="status">Status da sala</param>
        /// <returns></returns>
        public static List<_Sala> SelectLike(string funcionario, string sala, Fila fila, SStatus status) {
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  b.{nameof(Pessoas.Nome)}, " +
            $"  a.{nameof(Nome)}, " +
            $"  {nameof(Funcao)}, " +
            $"  {nameof(_Status)} " +
            $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(Funcionario)} = b.{nameof(Pessoas.CPF)} " +
            $"where " +
            $"  b.{nameof(Pessoas.Nome)} like concat(@nomeF, '%') and " +
            $"  a.{nameof(Nome)} like concat(@nomeS, '%') and " +
            $"  a.{nameof(Funcao)} = @funcao and " +
            $"  a.{nameof(_Status)} = @status;";
            c.Parameters.AddWithValue("@nomeF", funcionario);
            c.Parameters.AddWithValue("@nomeS", sala);
            c.Parameters.AddWithValue("@funcao", ES.f[fila]);
            c.Parameters.AddWithValue("@status", Get(status));
            var lista = new List<_Sala>();
            QueryRLoop("Erro ao obter sala.", c, (r) => {
                lista.Add(new _Sala() {
                    CPF_Funcionario = funcionario,
                    Nome_Funcionario = r.GetString(0),
                    Nome = r.GetString(1),
                    Funcao = r.GetString(2),
                    Status = r.GetString(3)
                });
            });
            return lista;
        }

    }
}
