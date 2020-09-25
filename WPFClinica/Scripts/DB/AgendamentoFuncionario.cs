using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class AgendamentoFuncionario {

        public const string Name = nameof(AgendamentoFuncionario);

        public int Agendamento { get; set; }
        public string Funcionario { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar associação de agendamento-medico.",
                $"create table if not exists {Name} (" +
                $"  {nameof(Agendamento)} int not null," +
                $"  {nameof(Funcionario)} varchar(15) not null," +
                $"  primary key ({nameof(Agendamento)})," +
                $"  foreign key ({nameof(Agendamento)}) references {Agendamentos.Name} ({nameof(Agendamentos.ID)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Funcionario)}) references {Funcionarios.Name} ({nameof(Funcionarios.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(int agendamento, string medico) {
            NonQuery("Erro ao inserir associação agendamento-medico.", (c) => {
                c.CommandText = $"insert into {Name} values (@agendamento, @medico);";
                c.Parameters.AddWithValue("@agendamento", agendamento);
                c.Parameters.AddWithValue("@medico", medico);
                return c;
            });
        }

        public static AgendamentoFuncionario Select(int agendamento) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(Agendamento)} = @id;";
            c.Parameters.AddWithValue("@id", agendamento);
            AgendamentoFuncionario af = null;
            QueryR("Erro ao obter associação de médico e agendamento.", c, (r) => {
                if (r.Read()) {
                    af = new AgendamentoFuncionario() {
                        Agendamento = r.GetInt32(0),
                        Funcionario = r.GetString(1)
                    };
                }
            });
            return af;
        }

    }

}
