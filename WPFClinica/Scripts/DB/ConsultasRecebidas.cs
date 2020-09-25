using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static WPFClinica.Scripts.DB.SQLOperations;
using static System.Convert;
using MySql.Data.MySqlClient;

namespace WPFClinica.Scripts.DB {

    class ConsultasRecebidas {

        public const string Name = nameof(ConsultasRecebidas);

        public int Consulta { get; set; }
        public double Valor { get; set; }
        public DateTime RecebidaEm { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar tabela de consultas reebidas.",
                $"create table if not exists {Name} (" +
                $"  {nameof(Consulta)} int," +
                $"  {nameof(Valor)} double," +
                $"  {nameof(RecebidaEm)} datetime," +
                $"  primary key ({nameof(Consulta)})," +
                $"  foreign key ({nameof(Consulta)}) references {Historico_Consultas.Name} ({nameof(Historico_Consultas.ID)}) on delete cascade" +
                $");");
        }

        public static void Insert(int id, double valor, DateTime dt) {
            NonQuery("Erro ao inserir em consultas recebidas.", (c) => {
                c.CommandText = $"insert into {Name} values (@id, @valor, @dt);";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@valor", valor);
                c.Parameters.AddWithValue("@dt", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                return c;
            });
        }

        public static void Delete(int id) {
            NonQuery("Erro ao deeltar de consultas recebidas.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(Consulta)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                return c;
            });
        }

        public static void Update_Valor(int id, double valor) {
            NonQuery("Erro ao atualizar o valor em consultas recebidas.", (c) => {
                c.CommandText = $"update {Name} set {nameof(Valor)} = @valor where {nameof(Consulta)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@valor", valor);
                return c;
            });
        }

        public static void Update_RecebidaEm (int id, DateTime dt) {
            NonQuery("Erro ao atualizar o data de recebimento em consultas recebidas.", (c) => {
                c.CommandText = $"update {Name} set {nameof(RecebidaEm)} = @rec where {nameof(Consulta)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@rec", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                return c;
            });
        }

        public static void Update(int id, double valor, DateTime recebida) {
            NonQuery("Erro ao atualizar consulta recebida.", (c) => {
                c.CommandText = $"update {Name} set " +
                $"  {nameof(Valor)} = @valor, " +
                $"  {nameof(RecebidaEm)} = @recebida " +
                $"where " +
                $"  {nameof(Consulta)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@valor", valor);
                c.Parameters.AddWithValue("@recebida", recebida.ToString("yyyy-MM-dd HH:mm:ss"));
                return c;
            });
        }

        public static ConsultasRecebidas Select(int id) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Consulta)}, {nameof(Valor)} from {Name} where {nameof(Consulta)} = @id;";
            c.Parameters.AddWithValue("@id", id);
            ConsultasRecebidas cr = null;
            QueryRLoop("Erro ao obter consulta recebida.", c, (r) => {
                cr = new ConsultasRecebidas();
                cr.Consulta = id;
                cr.Valor = r.GetDouble(1);
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
            }, true);
            return cr;
        }

    }
}
