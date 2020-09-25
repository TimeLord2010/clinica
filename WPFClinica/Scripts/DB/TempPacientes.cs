using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class TempPacientes {

        public const string Name = "temp_pacientes";

        public string Nome { get; set; }
        public string Contato { get; set; }

        public static void CreateTable() {
            var title = $"Erro ao criar a tabela {Name}.";
                var q = 
                $"create table if not exists {Name} (" +
                $"  {nameof(Nome)} nvarchar(100), " +
                $"  {nameof(Contato)} varchar(25) unique," +
                $"  primary key({nameof(Nome)})" +
                $");";
            NonQuery(title, q);
        }

        /// <summary>
        /// Contato
        /// </summary>
        /// <param name="nome"></param>
        /// <returns></returns>
        public static string Select(string nome) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Contato)} from {Name} where {nameof(Nome)} = @nome;";
            c.Parameters.AddWithValue("@nome", nome);
            string contato = null;
            QueryRLoop("Erro ao obter paciente temporário.", c, (r) => {
                contato = r.GetString(0);
            });
            return contato;
        }

        public static List<TempPacientes> Select(string nome, string contato) {
            var lista = new List<TempPacientes>();
            var c = new MySqlCommand();
            c.CommandText = 
                $"select * from {Name} " +
                $"where {nameof(Nome)} like concat(@nome, '%') and {nameof(Contato)} like concat(@contato, '%');";
            c.Parameters.AddWithValue("@nome", nome);
            c.Parameters.AddWithValue("@contato", contato);
            QueryRLoop("Erro ao obter cadastros de paciente temporário.", c, (r) => {
                lista.Add(new TempPacientes() {
                    Nome = r.GetString(0),
                    Contato = r.GetString(1)
                });
            });
            return lista;
        }

        public static void Delete(string nome) {
            NonQuery("Erro ao deletar paciente temporário.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(Nome)} = @nome;";
                c.Parameters.AddWithValue("@nome", nome);
                return c;
            });
        }

        public static void Insert(string nome, string contato) {
            NonQuery("Erro ao inserir cadastro parcial de paciente.", (c) => {
                c.CommandText = $"insert into {Name} values (@nome, @contato);";
                c.Parameters.AddWithValue("@nome", nome);
                c.Parameters.AddWithValue("@contato", contato);
                return c;
            });
        }

    }

}
