using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class ProcedimentosLab {

        public const string Name = nameof(ProcedimentosLab);

        public int ID { get; set; }
        public string Procedimento { get; set; }

        public static void CreateTable() {
            NonQuery($"Erro ao criar tabela {Name}",
                $"create table if not exists {Name} (" +
                $"  {nameof(ID)} int auto_increment," +
                $"  {nameof(Procedimento)} nvarchar(150)," +
                $"  primary key({nameof(ID)})" +
                $");");
        }

        public static void Insert(string procedimento) {
            NonQuery("Erro ao inserir procedimento.", (c) => {
                c.CommandText = $"insert into {Name} values (null, @procedimento);";
                c.Parameters.AddWithValue("@procedimento", procedimento);
                return c;
            });
        }

        public static void Insert(int id, string procedimento) {
            NonQuery("Erro ao inserir procedimento.", (c) => {
                c.CommandText = $"insert into {Name} values (@id, @procedimento);";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@procedimento", procedimento);
                return c;
            });
        }

        public static void TryInsert(string procedimento) {
            try {
                Insert(procedimento);
            } catch (Exception) { }
        }

        public static void Delete(int id) {
            NonQuery("Erro ao deletar procedimento.", $"delete from {Name} where {nameof(ID)} = {id};");
        }

        public static void Update(int id, string procedimento) {
            NonQuery("Erro ao atualizar procedimento.", (c) => {
                c.CommandText = $"update {Name} set {nameof(Procedimento)} = @proc where {nameof(ID)} = @id;";
                c.Parameters.AddWithValue("@id", id);
                c.Parameters.AddWithValue("@proc", procedimento);
                return c;
            });
        }

        /// <summary>
        /// id, procedimento, Convênio, valor
        /// </summary>
        /// <param name="procedimento"></param>
        /// <returns></returns>
        public static List<_ProcedimentoConvenio> SelectLike(string procedimento) {
            var pc = new ProcedimentoConvenio();
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  a.{nameof(ID)}, " +
            $"  a.{nameof(Procedimento)}, " +
            $"  b.{nameof(pc.Convenio)}, " +
            $"  b.{nameof(pc.Valor)}," +
            $"  c.{nameof(Convenios.Convenio)} " +
            $"from {Name} as a " +
            $"inner join {ProcedimentoConvenio.Name} as b on a.{nameof(ID)} = b.{nameof(pc.Procedimento)} " +
            $"inner join {Convenios.Name} as c on b.{nameof(pc.Convenio)} = c.{nameof(Convenios.ID)} " +
            $"where a.{nameof(Procedimento)} like concat(@p, '%') " +
            $"limit 500;";
            c.Parameters.AddWithValue("@p", procedimento);
            var lista = new List<_ProcedimentoConvenio>();
            QueryRLoop($"Erro ao pesquisar na tabela {Name}. ({ErrorCodes.DB0008})", c, (r) => {
                lista.Add(new _ProcedimentoConvenio() {
                    Proc_ID = r.GetInt32(0),
                    Procedimento = r.IsDBNull(1)? null : r.GetString(1),
                    Con_ID = r.IsDBNull(2)? -1 : r.GetInt32(2),
                    Convenio = r.IsDBNull(4) ? null : r.GetString(4),
                    Valor = r.IsDBNull(3)? double.NaN : r.GetDouble(3)
                });
            });
            return lista;
        }

        public static List<_ProcedimentoConvenio> SelectLike(string convenio, string procedimento) {
            var pc = new ProcedimentoConvenio();
            var c = new MySqlCommand();
            c.CommandText =
            $"select " +
            $"  a.{nameof(ID)}, " +
            $"  a.{nameof(Procedimento)}, " +
            $"  b.{nameof(pc.Convenio)}, " +
            $"  b.{nameof(pc.Valor)}," +
            $"  c.{nameof(Convenios.Convenio)} " +
            $"from {Name} as a " +
            $"left join {ProcedimentoConvenio.Name} as b on a.{nameof(ID)} = b.{nameof(pc.Procedimento)} " +
            $"left join {Convenios.Name} as c on b.{nameof(pc.Convenio)} = c.{nameof(Convenios.ID)} " +
            $"where a.{nameof(Procedimento)} like concat(@p, '%') and c.{nameof(Convenios.Convenio)} = @c " +
            $"limit 500;";
            c.Parameters.AddWithValue("@p", procedimento);
            c.Parameters.AddWithValue("@c", convenio);
            var lista = new List<_ProcedimentoConvenio>();
            QueryRLoop($"Erro ao pesquisar na tabela {Name}. ({ErrorCodes.DB0007})", c, (r) => {
                lista.Add(new _ProcedimentoConvenio() {
                    Proc_ID = r.GetInt32(0),
                    Procedimento = r.GetString(1),
                    Con_ID = r.GetInt32(2),
                    Valor = r.GetDouble(3),
                    Convenio = r.GetString(4)
                });
            });
            return lista;
        }

        /// <summary>
        /// ID, Procedimento
        /// </summary>
        /// <param name="procedimento"></param>
        /// <returns></returns>
        public static List<ProcedimentosLab> SelectLike2(string procedimento) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select * from {Name} " +
                $"where {nameof(Procedimento)} like concat(@p, '%') limit 500;";
            c.Parameters.AddWithValue("@p", procedimento);
            var lista = new List<ProcedimentosLab>();
            QueryR($"Erro ao obter procedimentos laboratoriais. ({nameof(ErrorCodes.DB0009)})", c, (r) => {
                while (r.Read()) {
                    lista.Add(new ProcedimentosLab() {
                        ID = r.GetInt32(0),
                        Procedimento = r.GetString(1)
                    });
                }
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
            }, true);
            return lista;
        }

        /// <summary>
        /// Nome do procedimento.
        /// </summary>
        /// <returns></returns>
        public static string Select(int id) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Procedimento)} from {Name} where {nameof(ID)} = @id;";
            c.Parameters.AddWithValue("@id", id);
            string a = null;
            QueryRLoop($"Erro ao obter o nome do procedimento laboratorial. ({nameof(ErrorCodes.DB0010)})", c, (r) => {
                a = r.GetString(0);
            });
            return a;
        }

    }

}
