using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.AbstractClasses;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Administradores : Profissoes {

        public override string TableName => "administradores";

        public new Action GetCT() {
            return () => {
                base.GetCT();
                Insert("033.380.952-10");
            };
        }

        /*public const string Name = "administradores";

        public string CPF { get; set; }

        public static void CreateTable() {
            var q =
                $"create table if not exists {Name} (" +
                "   cpf varchar(15)," +
                "   primary key(cpf)," +
                "   foreign key(cpf) references funcionarios(cpf) ON DELETE CASCADE ON UPDATE CASCADE" +
                ");";
            NonQuery("Erro ao criar a tabela de administradores.", q);
        }

        public static void Insert(string cpf) {
            NonQuery($"Erro ao inserir administrador.", (c) => {
                c.CommandText = $"insert into {Name} values (@cpf);";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static void TryInsert (string cpf) {
            SQLOperations.MessageExceptions = false;
            Insert(cpf);
            SQLOperations.MessageExceptions = true;
        }

        public static void Delete(string cpf) {
            NonQuery("Erro ao deletar administrador.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        public static bool IsAdministrador(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(CPF)} = @cpf";
            c.Parameters.AddWithValue("@cpf", cpf);
            bool a = false;
            QueryR("Erro ao obter administrador", c, (r) => {
                if (r.Read()) {
                    a = true;
                }
            });
            return a;
        }*/
    }

}
