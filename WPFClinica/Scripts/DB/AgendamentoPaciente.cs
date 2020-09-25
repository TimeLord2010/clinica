using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class AgendamentoPaciente {

        public const string Name = "agendamento_paciente";

        public int Agendamento { get; set; }
        public string Paciente { get; set; }

        public static void CreateTable() {
            var a = new Agendamentos();
            var p = new Pacientes();
            NonQuery("Erro ao criar associação de agendamento-paciente",
                $"create table if not exists {Name} (" +
                $"  {nameof(Agendamento)} int," +
                $"  {nameof(Paciente)} varchar(15)," +
                $"  primary key ({nameof(Agendamento)})," +
                $"  foreign key ({nameof(Agendamento)}) references {Agendamentos.Name} ({nameof(a.ID)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Paciente)}) references {Pacientes.Name} ({nameof(p.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static AgendamentoPaciente Select(int agendamento) {
            AgendamentoPaciente am = null;
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Paciente)} " +
            $"from {Name} as a " +
            $"where {nameof(Agendamento)} = @agen;";
            c.Parameters.AddWithValue("@agen", agendamento);
            QueryRLoop("Erro ao obter associação agendamento-paciente.", c, (r) => {
                am = new AgendamentoPaciente() {
                    Agendamento = agendamento,
                    Paciente = r.GetString(0)
                };
            });
            return am;
        }

        public static AgendamentoPaciente Select(int agendamento, string medico) {
            AgendamentoPaciente ap = null;
            var c = new MySqlCommand();
                c.CommandText = $"select {nameof(Paciente)} " +
                $"from {Name} as a " +
                $"left join {AgendamentoFuncionario.Name} as b on a.{nameof(Agendamento)} = b.{nameof(AgendamentoFuncionario.Agendamento)} " +
                $"where a.{nameof(Agendamento)} = @agen and b.{nameof(AgendamentoFuncionario.Funcionario)} {(medico == null ? "is null" : "= @medico")} " +
                $"limit 1;";
                c.Parameters.AddWithValue("@agen", agendamento);
                c.Parameters.AddWithValue("@medico", medico);
            QueryRLoop("Erro ao obter associação agendamento-paciente.", c, (r) => {
                ap = new AgendamentoPaciente() {
                    Agendamento = agendamento,
                    Paciente = r.GetString(0)
                };
            });
            return ap;
        }

        public static void Insert(int agendamento, string paciente) {
            NonQuery("Erro ao inserir associação agendamento-paciente.", (c) => {
                c.CommandText = $"insert into {Name} values (@agendamento, @paciente);";
                c.Parameters.AddWithValue("@agendamento", agendamento);
                c.Parameters.AddWithValue("@paciente", paciente);
                return c;
            });
        }
    }

}
