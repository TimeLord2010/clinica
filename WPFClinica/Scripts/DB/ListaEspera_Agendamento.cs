using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class ListaEspera_Agendamento {

        public const string Name = nameof(ListaEspera_Agendamento);

        public int Senha { get; set; }
        public int Agendamento { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar associação entre lista de espera e agendamento.",
                $"create table if not exists {Name} (" +
                $"  {nameof(Senha)} int," +
                $"  {nameof(Agendamento)} int," +
                $"  primary key ({nameof(Senha)})," +
                $"  foreign key ({nameof(Senha)}) references {ListaEspera.Name} ({nameof(ListaEspera.Senha)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Agendamento)}) references {Historico_Consultas.Name} ({nameof(Historico_Consultas.ID)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(int senha, int agendamento) {
            var c = new MySqlCommand($"insert into {Name} values ({senha}, {agendamento});");
            NonQuery("Erro ao inserir associação entre lista de espera e agendamento.",
                c, 
                (ex) => {
                    if (ex.Message.Contains("doesn't exist")) {
                        CreateTable();
                    }
                }, 
                true);
        }

        public static void Update_Senha(int agendamento, int senha) {
            NonQuery("Erro ao atualizar associação entre lista de espera e agendamento.",
                $"update listaespera_agendamento set senha = {senha} where agendamento = {agendamento};");
        }

        public static void Delete(int agendamento) {
            NonQuery("Erro ao deletar associação entre lista de espera e agendamento.",
                $"delete from {Name} where agendamento = {agendamento};");
        }

        public static int? Select_Senha(int senha) {
            var c = new MySqlCommand();
            c.CommandText = $"select agendamento from {Name} where senha = @senha;";
            c.Parameters.AddWithValue("@senha", senha);
            int? a = null;
            QueryRLoop("Erro ao buscar associação entre lista de espera e agendamento.", c, (r) => {
                a = r.GetInt32(0);
            });
            return a;
        }

    }

}