using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class Psicologos {

        public const string Name = nameof(Psicologos);

        public string CPF { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            NonQuery("Erro ao criar tabela de psicologos.",
                $"create table if not exists {nameof(Psicologos)} (" +
                $"  {nameof(CPF)} varchar(15)," +
                $"  primary key ({nameof(CPF)})," +
                $"  foreign key ({nameof(CPF)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(string cpf) {
            NonQuery("Erro ao inserir psicologo.", (c) => {
                c.CommandText = $"insert into {nameof(Psicologos)} values (@cpf);";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void Delete(string cpf) {
            NonQuery("Erro ao deletar psicólogo.", (c) => {
                c.CommandText = $"delete from {nameof(Psicologos)} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void TryInsert(string cpf) {
            if (!IsPsicologo(cpf)) Insert(cpf);
        }

        public static bool IsPsicologo(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(CPF)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", cpf);
            bool a = false;
            QueryRLoop("Erro ao verificar se é psicologo.", c, (r) => {
                a = true;
            });
            return a;
        }

    }
}
