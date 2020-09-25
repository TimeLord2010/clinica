using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Recepcionista {

        public const string Name = "recepcionistas";

        public string CPF { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar a tabela de recepcionista.",
                $"create table if not exists {Name} (" +
                "   cpf varchar(15)," +
                "   primary key(cpf)," +
                "   foreign key(cpf) references funcionarios(cpf) ON DELETE CASCADE ON UPDATE CASCADE" +
                ");");
        }

        public static void Insert(string cpf) {
            NonQuery("Erro ao inserir recepcionista.", (c) => {
                c.CommandText = $"insert into {Name} values (@cpf);";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void TryInsert(string cpf) {
            if (!IsRecepcionista(cpf)) {
                Insert(cpf);
            }
        }

        public static void Delete(string cpf) {
            NonQuery("Erro ao deletar recepcionista.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static bool IsRecepcionista(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(CPF)} = @cpf";
            c.Parameters.AddWithValue("@cpf", cpf);
            bool a = false;
            QueryRLoop("Erro ao obter recepcionista.", c, (r) => {
                a = true;
            });
            return a;
        }

    }

}
