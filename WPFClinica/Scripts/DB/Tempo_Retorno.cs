using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class Tempo_Retorno {

        public const string Name = nameof(Tempo_Retorno);

        public double Dias { get; set; }

        public static void CreateTable () {
            NonQuery("Erro ao criar tempo para aceito de retorno.", 
                $"create table if not exists {Name} (" +
                $"  id int," +
                $"  dias int," +
                $"  primary key (id)" +
                $");");
        }

        private static void Insert (int dias) {
            NonQuery("Erro ao inserir tempo de retorno.", 
                $"insert into {Name} values(1, {dias});");
        }

        private static void Update (int dias) {
            NonQuery("Erro ao inserir tempo de retorno.", 
                $"update {Name} set dias = {dias} where id = 1;");
        }

        public static void InsertOrUpdate (int dias) {
            if (Select() == null) {
                Insert(dias);
            } else {
                Update(dias);
            }
        }

        public static int? Select () {
            var c = new MySqlCommand($"select dias from {Name};");
            int? a = null;
            QueryRLoop("Erro ao obter tempo de retorno.", c, (r) => {
                a = r.GetInt32(0);
            });
            return a;
        }

    }

}
