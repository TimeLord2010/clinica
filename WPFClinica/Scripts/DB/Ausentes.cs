using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.EnumStrings;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class Ausentes {

        public string Paciente { get; set; }
        public Fila Fila { get; set; }
        public DateTime _DateTime { get; set; }
        public string Observacao { get; set; }

        public static void CreateTable () {
            NonQuery("Erro ao criar lista de ausentes.", 
                $"create table if not exists {nameof(Ausentes)} (" +
                $"  {nameof(Paciente)} varchar(15)," +
                $"  {nameof(Fila)} enum('Triagem','Consultorio','Laboratorio') not null," +
                $"  {nameof(_DateTime)} datetime," +
                $"  {nameof(Observacao)} mediumtext," +
                $"  primary key ({nameof(Paciente)})," +
                $"  foreign key ({nameof(Paciente)}) references {Pacientes.Name} ({nameof(Pacientes.CPF)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert (string paciente, Fila fila, DateTime dt, string observacao) {
            NonQuery("Erro ao obter os pacientes ausentes.", (c) => {
                c.CommandText = $"insert into {nameof(Ausentes)} values (@paciente, @fila, @dt, @obs);";
                c.Parameters.AddWithValue("@paciente", paciente);
                c.Parameters.AddWithValue("@fila", ES.f[fila]);
                c.Parameters.AddWithValue("@dt", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@obs", observacao);
                return c;
            });
        }

        public static void Delete (string paciente) {
            NonQuery("Erro ao deletar paciente ausente.", (c) => {
                c.CommandText = $"delete from {nameof(Ausentes)} where {nameof(Paciente)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", paciente);
                return c;
            });
        }

    }

}