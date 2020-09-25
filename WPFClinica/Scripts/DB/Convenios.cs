using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.DB;
using WPFClinica.Scripts;
using static WPFClinica.Scripts.DB.SQLOperations;
using MySql.Data.MySqlClient;
using System.Windows;

namespace WPFClinica.Scripts.DB {
    class Convenios {

        public const string Name = nameof(Convenios);

        public int ID { get; set; }
        public string Convenio { get; set; }


        public static void CreateTable() {
            NonQuery("Erro ao tabela de convenios",
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(Convenio)} nvarchar(150) unique not null," +
                $"  primary key ({nameof(ID)})" +
                $");");
        }

        public static Convenios Select(int id) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select {nameof(Convenio)} " +
                $"from {Name} " +
                $"where {nameof(ID)} = @id;";
            c.Parameters.AddWithValue("@id", id);
            Convenios convenio = null;
            QueryR("Erro ao obter convênio.", c, (r) => {
                if (r.Read()) {
                    convenio = new Convenios() {
                        ID = id,
                        Convenio = r.GetString(0)
                    };
                }
            });
            return convenio;
        }

        public static int Select_ID(string convenio) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select {nameof(ID)} " +
                $"from {Name} " +
                $"where " +
                $"  {nameof(Convenio)} = @convenio " +
                $"order by {nameof(ID)} desc " +
                $"limit 1";
            c.Parameters.AddWithValue("@convenio", convenio);
            int id = -1;
            QueryR("Erro ao obter identificador de convênio.", c, (r) => {
                if (r.Read()) {
                    id = r.GetInt32(0);
                }
            });
            return id;
        }

        public static List<Convenios> SelectLike(string convenio) {
            int a = 0;
            var c = new MySqlCommand();
            c.CommandText =
                $"select * " +
                $"from {Name} " +
                $"where {nameof(Convenio)} like concat(@convenio, '%');";
            c.Parameters.AddWithValue("@convenio", convenio);
        A:
            var lista = new List<Convenios>();
            QueryR("Erro ao obter registros de convenios.", c, (r) => {
                while (r.Read()) {
                    lista.Add(new Convenios() {
                        ID = r.GetInt32(0),
                        Convenio = r.GetString(1)
                    });
                }
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
                a += 1;
            });
            if (a == 1) goto A;
            return lista;
        }

        public static void Insert(string convenio) {
            NonQuery("Erro ao inserir convênio.", (c) => {
                c.CommandText = $"insert into {Name} values (null, @convenio);";
                c.Parameters.AddWithValue("@convenio", convenio);
                return c;
            });
        }

        public static void Update(string oldConvenio, string newConvenio) {
            NonQuery("Erro ao atualizar convênio.", (c) => {
                c.CommandText =
                $"update {Name} set {nameof(Convenio)} = @new " +
                $"where {nameof(Convenio)} @old;";
                c.Parameters.AddWithValue("@old", oldConvenio);
                c.Parameters.AddWithValue("@new", newConvenio);
                return c;
            });
        }

        public static void Update(int id, string convenio) {
            NonQuery("Erro ao atualizar nome do convênio.", (c) => {
                c.CommandText =
                $"update {Name} set {nameof(Convenio)} = @convenio " +
                $"where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@convenio", convenio);
                return c;
            });
        }

        public static void Delete(int id) {
            NonQuery("Erro ao deletar convênio", (c) => {
                c.CommandText =
                $"delete from {Name} " +
                $"where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                return c;
            });
        }

        public static void Delete(string convenio) {
            NonQuery("Erro ao deletar covênio.", (c) => {
                c.CommandText =
                $"delete from {Name} " +
                $"where {nameof(Convenio)} = @convenio;";
                c.Parameters.AddWithValue("@convenio", convenio);
                return c;
            });
        }

    }
}
