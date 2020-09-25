using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Paciente_Sala {

        public const string Name = "paciente_sala";

        public string Funcionario { get; set; }
        public string Paciente { get; set; }

        public static void CreateTable() {
            var s = new Salas();
            var p = new Pacientes();
            NonQuery("Erro ao criar associação paciente-sala.",
                $"create table if not exists {Name} (" +
                $"  {nameof(Funcionario)} varchar(15)," +
                $"  {nameof(Paciente)} varchar(15) unique," +
                $"  primary key ({nameof(Funcionario)})," +
                $"  foreign key ({nameof(Funcionario)}) references {Salas.Name} ({nameof(s.Funcionario)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Paciente)}) references {Pacientes.Name} ({nameof(p.CPF)}) on delete cascade on update cascade " +
                $");");
        }

        private static void Create (Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void QueryRLoop (string title, MySqlCommand c, Action<MySqlDataReader> r) {
            SQLOperations.QueryRLoop(title, c, r, Create, true);
        }

        public static void Insert(string funcionario, string paciente) {
            NonQuery("Erro ao inserir associação paciente-sala.", (c) => {
                c.CommandText = $"insert into {Name} values (@funcionario, @paciente);";
                c.Parameters.AddWithValue("@funcionario", funcionario);
                c.Parameters.AddWithValue("@paciente", paciente);
                return c;
            });
        }

        public static Paciente_Sala Select_Funcionario(string funcionario) {
            var c = new MySqlCommand($"select * from {Name} where {nameof(Funcionario)} = @funcionario;");
            c.Parameters.AddWithValue("@funcionario", funcionario);
            Paciente_Sala ps = null;
            QueryRLoop("Erro ao obter associação paciente-sala", c, (r) => {
                ps = new Paciente_Sala() {
                    Funcionario = r.GetString(0),
                    Paciente = r.GetString(1)
                };
            });
            return ps;
        }

        public static Paciente_Sala Select_Paciente(string paciente) {
            var c = new MySqlCommand($"select * from {Name} where {nameof(Paciente)} = @paciente;");
            c.Parameters.AddWithValue("@paciente", paciente);
            Paciente_Sala ps = null;
            QueryRLoop("Erro ao obter sala do paciente.", c, (r) => {
                ps = new Paciente_Sala() {
                    Funcionario = r.GetString(0),
                    Paciente = r.GetString(1)
                };
            });
            return ps;
        }

        public static void Delete(string funcionario) {
            NonQuery("Erro ao deletar associação paciente-sala", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(Funcionario)} = @funcionario;";
                c.Parameters.AddWithValue("@funcionario", funcionario);
                return c;
            });
        }

        public static void DeletePaciente(string paciente) {
            NonQuery("Erro ao deletar associação de paciente e sala.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(Paciente)} = @paciente;";
                c.Parameters.AddWithValue("@paciente", paciente);
                return c;
            });
        }

        public static void Update(string funcionario, string paciente) {
            NonQuery("Erro ao atualizar associação paciente-sala", (c) => {
                c.CommandText = $"update {Name} set {nameof(Paciente)} = @paciente where {nameof(Funcionario)} = @funcionario;";
                c.Parameters.AddWithValue("@paciente", paciente);
                c.Parameters.AddWithValue("@funcionario", funcionario);
                return c;
            });
        }

        public static void InsertOrUpdate(string funcionario, string paciente) {
            var r = Select_Funcionario(funcionario);
            if (r == null) {
                Insert(funcionario, paciente);
            } else {
                Update(funcionario, paciente);
            }
        }

    }

}
