using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.EnumStrings;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Pagamentos {

        public const string Name = "pagamentos";

        public int ID { get; set; }
        public string Funcionario { get; set; }
        public string Funcao { get; set; }
        public DateTime _DateTime { get; set; }
        public string Observacao { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            NonQuery("Erro ao criar tabela de pagamentos.",
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(Funcionario)} varchar(15)," +
                $"  {nameof(Funcao)} enum('Medico','Administrador', 'Recepcionista', 'Tecnico em Enfermagem')," +
                $"  {nameof(_DateTime)} datetime," +
                $"  {nameof(Observacao)} nvarchar(255)," +
                $"  primary key ({nameof(ID)})," +
                $"  foreign key ({nameof(Funcionario)}) references {Funcionarios.Name} ({nameof(f.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(string funcionario, Funcao funcao, string observacao, DateTime dateTime) {
            NonQuery("Erro ao gravar pagamento.", (c) => {
                c.CommandText = $"insert into {Name} values (null, @funcio, @funcao, @dateTime, @obs);";
                c.Parameters.AddWithValue("@funcio", funcionario);
                c.Parameters.AddWithValue("@funcao", Get(funcao));
                c.Parameters.AddWithValue("@dateTime", dateTime.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@obs", observacao);
                return c;
            });
        }

        public static void Insert(string funcionario, Funcao funcao, string observacao) {
            Insert(funcionario, funcao, observacao, DateTime.Now);
        }

    }

}
