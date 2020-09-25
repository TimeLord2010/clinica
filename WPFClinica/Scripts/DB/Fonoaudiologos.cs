using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.AbstractClasses;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Fonoaudiologos : Profissoes {

        /*public string CPF { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            NonQuery($"Erro ao criar tabela {nameof(Fonoaudiologos)}", $"create table if not exists {nameof(Fonoaudiologos)} (" +
                $"  {nameof(CPF)} varchar(15)," +
                $"  primary key ({nameof(CPF)})," +
                $"  foreign key ({nameof(CPF)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(string cpf) {
            NonQuery("Erro ao inserir fonoaudiologo.", (c) => {
                c.CommandText = $"insert into {nameof(Fonoaudiologos)} values (@cpf);";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void Delete(string cpf) {
            NonQuery("Erro ao deletar fonoaudiologo.", (c) => {
                c.CommandText = $"delete from {nameof(Fonoaudiologos)} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static bool IsFono(string cpf) {
            var c = new MySqlCommand();
                c.CommandText = $"select * from {nameof(Fonoaudiologos)} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
            bool a = false;
            QueryR("Erro ao verificar se é fonoaudiologo.", c, (r) => {
                a = true;
            });
            return a;
        }

        public static void TryInsert (string cpf) {
            SQLOperations.MessageExceptions = false;
            Insert(cpf);
            SQLOperations.MessageExceptions = true;
        }*/
        public override string TableName => nameof(Fonoaudiologos);
    }

}
