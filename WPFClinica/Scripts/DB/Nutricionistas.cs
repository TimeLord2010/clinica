using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class Nutricionistas {

        public const string Name = nameof(Nutricionistas);

        public string CPF { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            NonQuery("Erro ao criar tabela de nutricionistas.", (c) => {
                c.CommandText = $"create table if not exists {nameof(Nutricionistas)} (" +
                $"  {nameof(CPF)} varchar(15)," +
                $"  foreign key ({nameof(CPF)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");";
                return c;
            });
        }

        public static void Insert(string cpf) {
            NonQuery("Erro ao inserir nutricionista.", (c) => {
                c.CommandText = $"insert into {nameof(Nutricionistas)} values (@cpf);";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void Delete(string cpf) {
            NonQuery("Erro ao deletar nutricionista.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void TryInsert(string cpf) {
            if (!IsNutricionista(cpf)) Insert(cpf);
        }

        public static bool IsNutricionista(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(CPF)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", cpf);
            bool a = false;
            QueryRLoop("Erro ao verificar se é nutricionista.", c, (r) => {
                a = true;
            });
            return a;
        }

    }
}
