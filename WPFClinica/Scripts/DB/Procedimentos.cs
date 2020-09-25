using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class Procedimentos {

        public const string Name = nameof(Procedimentos);

        public int ID { get; set; }
        public string Descricao { get; set; }
        public int Tipo { get; set; }
        public double Valor { get; set; }

        private static string QueryBegin = $"select a.*, b.{nameof(TipoProcedimento.Tipo)} " +
                $"from {Name} as a " +
                $"inner join {TipoProcedimento.Name} as b on a.{nameof(Tipo)} = b.{nameof(TipoProcedimento.ID)} " +
                $"where ";

        public static void CreateTable () {
            TipoProcedimento.CreateTable();
            NonQuery("Erro ao criar tabela de procedimentos médicos.", 
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(Descricao)} nvarchar(200)," +
                $"  {nameof(Tipo)} int," +
                $"  {nameof(Valor)} double," +
                $"  primary key ({nameof(ID)})," +
                $"  foreign key ({nameof(Tipo)}) references {TipoProcedimento.Name} ({nameof(TipoProcedimento.ID)})" +
                $");");
        }

        public static void Insert (string descricao, int tipo, double valor) {
            NonQuery("Erro ao inserir procedimento médico.", (c) => {
                c.CommandText = 
                $"insert into {Name} values (null, @descricao, @tipo, @valor);";
                c.Parameters.AddWithValue("@descricao", descricao);
                c.Parameters.AddWithValue("@tipo", tipo);
                c.Parameters.AddWithValue("@valor", valor);
                return c;
            });
        }

        public static void Update (int id, string descricao, int tipo, double valor) {
            NonQuery("Erro ao atualizar procedimento médico.", (c) => {
                c.CommandText = 
                $"update {Name} set " +
                $"  {nameof(Descricao)} = @descricao, " +
                $"  {nameof(Tipo)} = @tipo, " +
                $"  {nameof(Valor)} = @valor " +
                $"where" +
                $"  {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@descricao", descricao);
                c.Parameters.AddWithValue("@tipo", tipo);
                c.Parameters.AddWithValue("@valor", valor);
                return c;
            });
        }

        public static void Update_Descricao (int id, string descrição) {
            NonQuery("Erro ao atualizar  descrição de procedimento médico.", (c) => {
                c.CommandText = 
                $"update {Name} set " +
                $"  {nameof(Descricao)} = @descricao " +
                $"where " +
                $"  {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@descricao", descrição);
                return c;
            });
        }

        public static void Update_Tipo (int id, int tipo) {
            NonQuery("Erro ao atualizar tipo de procedimento médico.", (c) => {
                c.CommandText = 
                $"update {Name} set " +
                $"  {nameof(Tipo)} = @tipo " +
                $"where " +
                $"  {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@tipo", tipo);
                return c;
            });
        }

        public static void Update_Valor (int id, double valor) {
            NonQuery("Erro ao atualizar tipo de procedimento médico.", (c) => {
                c.CommandText =
                $"update {Name} set " +
                $"  {nameof(Valor)} = @valor " +
                $"where " +
                $"  {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@valor", valor);
                return c;
            });
        }

        public static void Delete (int id) {
            NonQuery("Erro ao deletar procedimento médico.", (c) => {
                c.CommandText = 
                $"delete from {Name} where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                return c;
            });
        }

        public static _Procedimento Select_ID (int id) {
            var c = new MySqlCommand();
            c.CommandText = 
                QueryBegin +
                $"a.{nameof(ID)} = @id;";
            c.Parameters.AddWithValue("@id", id);
            return GetProcedimentos(c)[0];
        }

        public static List<_Procedimento> Select_Tipo (int tipo) {
            var c = new MySqlCommand();
            c.CommandText = QueryBegin +
                $"a.{nameof(Tipo)} = @tipo " +
                $"limit 500;";
            c.Parameters.AddWithValue("@tipo", tipo);
            return GetProcedimentos(c);
        }

        public static List<_Procedimento> SelectLike (int tipo, string descricao) {
            MySqlCommand c = new MySqlCommand();
            c.CommandText =
                QueryBegin + 
                $"a.{nameof(Tipo)} = @tipo and {nameof(Descricao)} like concat(@descricao, '%') " +
                $"limit 500;";
            c.Parameters.AddWithValue("@tipo", tipo);
            c.Parameters.AddWithValue("@descricao", descricao);
            return GetProcedimentos(c);
        }

        private static List<_Procedimento> GetProcedimentos (MySqlCommand c) {
            var lista = new List<_Procedimento>();
            QueryR("Erro ao obter procedimentos médicos.", c, (r) => {
                while (r.Read()) {
                    lista.Add(new _Procedimento() {
                        ID = r.GetInt32(0),
                        Descricao = r.GetString(1),
                        Tipo_ID = r.GetInt32(2),
                        Valor = r.GetDouble(3),
                        Tipo = r.GetString(4)
                    });
                }
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
            }, true);
            return lista;
        }

    }

}
