using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class AgendamentoTempPaciente {

        public const string Name = "agendamento_temp_paciente";

        public int Agendamento { get; set; }
        public string TempPaciente { get; set; }

        public static void CreateTable() {
            var a = new Agendamentos();
            var tp = new TempPacientes();
            NonQuery("Erro ao criar associação de agendamento-tempPaciente",
                $"create table if not exists {Name} (" +
                $"  {nameof(Agendamento)} int," +
                $"  {nameof(TempPaciente)} nvarchar(50)," +
                $"  primary key ({nameof(Agendamento)})," +
                $"  foreign key ({nameof(Agendamento)}) references {Agendamentos.Name} ({nameof(a.ID)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(TempPaciente)}) references {TempPacientes.Name} ({nameof(tp.Nome)}) on delete cascade on update cascade" +
                $");");
        }

        public static AgendamentoTempPaciente Select(int agendamento) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(TempPaciente)} " +
            $"from {Name} " +
            $"where {nameof(Agendamento)} = @id;";
            c.Parameters.AddWithValue("@id", agendamento);
            AgendamentoTempPaciente atp = null;
            QueryRLoop("Erro ao obter associação agendamento-tempPaciente.", c, (r) => {
                atp = new AgendamentoTempPaciente() {
                    Agendamento = agendamento,
                    TempPaciente = r.GetString(0)
                };
            });
            return atp;
        }

        public static AgendamentoTempPaciente Select(int agendamento, string funcionario) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(TempPaciente)} " +
            $"from {Name} as a " +
            $"left join {AgendamentoFuncionario.Name} as b on a.{nameof(Agendamento)} = b.{nameof(AgendamentoFuncionario.Agendamento)} " +
            $"where a.{nameof(Agendamento)} = @id and b.{nameof(AgendamentoFuncionario.Funcionario)} {(funcionario == null ? "is null" : "= @medico")} " +
            $"limit 100;";
            c.Parameters.AddWithValue("@id", agendamento);
            c.Parameters.AddWithValue("@medico", funcionario);
            AgendamentoTempPaciente atp = null;
            QueryRLoop("Erro ao obter associação agendamento-tempPaciente.", c, (r) => {
                atp = new AgendamentoTempPaciente() {
                    Agendamento = agendamento,
                    TempPaciente = r.GetString(0)
                };
            });
            return atp;
        }

        public static void Insert(int agendamento, string tempPaciente) {
            NonQuery("Erro ao inserir associação agendamento-tempPaciente.", (c) => {
                c.CommandText = $"insert into {Name} values (@agendamento, @tempPaciente);";
                c.Parameters.AddWithValue("@agendamento", agendamento);
                c.Parameters.AddWithValue("@tempPaciente", tempPaciente);
                return c;
            });
        }

    }

}
