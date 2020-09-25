using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using static WPFClinica.Scripts.DB.SQLOperations;


namespace WPFClinica.Scripts.DB {

    class FuncionarioTriagem {

        public string CPF_Funcionario { get; set; }

        public static void CreateTable () {
            var f = new Funcionarios();
            NonQuery("Erro ao criar funcinario de triagem.", 
                $"create table if not exists {nameof(FuncionarioTriagem)} (" +
                $"  {nameof(CPF_Funcionario)} varchar(15)," +
                $"  primary key ({nameof(CPF_Funcionario)})," +
                $"  foreign key ({nameof(CPF_Funcionario)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert (string cpf) {
            NonQuery("Erro ao obter funcionario de triagem.", (c) => {
                c.CommandText = $"insert into {nameof(FuncionarioTriagem)} values (@cpf);";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void Delete (string cpf) {
            NonQuery("Erro ao deletar funcionario de triagem.", (c) => {
                c.CommandText = $"delete from {nameof(FuncionarioTriagem)} where {nameof(CPF_Funcionario)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static bool IsFuncionarioTriagem (string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {nameof(FuncionarioTriagem)} where {nameof(CPF_Funcionario)} = @cpf;";
            c.Parameters.AddWithValue("@cpf", cpf);
            bool a = false;
            QueryR("Erro ao obter registro de funcionário de triagem.", c, (r) => {
                if (r.Read()) {
                    a = true;
                }
            });
            return a;
        }

    }
}
