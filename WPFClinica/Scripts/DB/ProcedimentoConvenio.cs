using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class ProcedimentoConvenio {

        public const string Name = "procedimento_convenios";

        public int Procedimento { get; set; }
        public int Convenio { get; set; }
        public double Valor { get; set; }

        public static void CreateTable() {
            var procedimentos = new ProcedimentosLab();
            NonQuery("Erro ao criar associação de convêncio e procedimento.",
                $"create table if not exists {Name} (" +
                $"  {nameof(Procedimento)} int," +
                $"  {nameof(Convenio)} int," +
                $"  {nameof(Valor)} float(10,2)," +
                $"  primary key ({nameof(Procedimento)}, {nameof(Convenio)})," +
                $"  foreign key ({nameof(Convenio)}) references {Convenios.Name} ({nameof(Convenios.ID)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Procedimento)}) references {ProcedimentosLab.Name} ({nameof(procedimentos.ID)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(int procedimento_id, int con_id, double val) {
            NonQuery("Erro ao inserir associação de procedimento e convênio.", (c) => {
                c.CommandText = $"insert into {Name} values (@pro, @con, @val);";
                c.Parameters.AddWithValue("@pro", procedimento_id);
                c.Parameters.AddWithValue("@con", con_id);
                c.Parameters.AddWithValue("@val", val);
                return c;
            });
        }

        public static void Update_Convênio(int procedimento, int oldC, int newC) {
            NonQuery("Erro ao atualizar o convênio.", (c) => {
                c.CommandText = $"update {Name} set {nameof(Convenio)} = @new where {nameof(Procedimento)} = @id and {nameof(Convenio)} = @old;";
                c.Parameters.AddWithValue("@id", procedimento);
                c.Parameters.AddWithValue("@old", oldC);
                c.Parameters.AddWithValue("@new", newC);
                return c;
            });
        }

        public static void Update_Valor(int procedimento, int convenio, double valor) {
            NonQuery("Erro ao atualizar valor de procedimento.", (c) => {
                c.CommandText = $"update {Name} set {nameof(Valor)} = @val where {nameof(Procedimento)} = @proc and {nameof(Convenio)} = @con;";
                c.Parameters.AddWithValue("@val", valor);
                c.Parameters.AddWithValue("@proc", procedimento);
                c.Parameters.AddWithValue("@con", convenio);
                return c;
            });
        }

        public static void Delete(int procedimento, int convenio) {
            NonQuery("Erro ao deletar convênio de procedimento.", (c) => {
                c.CommandText =
                $"delete from {Name} where {nameof(Procedimento)} = @proc and {nameof(Convenio)} = @con;";
                c.Parameters.AddWithValue("@proc", procedimento);
                c.Parameters.AddWithValue("@con", convenio);
                return c;
            });
        }

        public static List<ProcedimentoConvenio> Select(int procedimento_id) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Convenio)}, {nameof(Valor)} from {Name} where {nameof(Procedimento)} = @id;";
            c.Parameters.AddWithValue("@id", procedimento_id);
            var lista = new List<ProcedimentoConvenio>();
            QueryRLoop("Erro ao obter associação entra convênio e procedimento.", c, (r) => {
                lista.Add(new ProcedimentoConvenio() {
                    Procedimento = procedimento_id,
                    Convenio = r.GetInt32(0),
                    Valor = r.GetDouble(1)
                });
            });
            return lista;
        }

        /// <summary>
        /// Procedimento_ID, Convenio, Valor, Nome_Procedimento
        /// </summary>
        /// <param name="nome_procedimento"></param>
        /// <param name="convenio"></param>
        /// <returns></returns>
        public static List<_ProcedimentoConvenio> SelectLike(string nome_procedimento, string convenio) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select a.*, b.{nameof(ProcedimentosLab.Procedimento)}, c.{nameof(Convenios.Convenio)} from {Name} as a " +
                $"inner join {ProcedimentosLab.Name} as b on b.{nameof(ProcedimentosLab.ID)} = a.{nameof(Procedimento)} " +
                $"inner join {Convenios.Name} as c on a.{nameof(Convenio)} = c.{nameof(Convenios.ID)} " +
                $"where c.{nameof(Convenios.Convenio)} like concat(@c, '%') and b.{nameof(ProcedimentosLab.Procedimento)} like concat(@np, '%') " +
                $"limit 500;";
            c.Parameters.AddWithValue("@np", nome_procedimento);
            c.Parameters.AddWithValue("@c", convenio);
            var lista = new List<_ProcedimentoConvenio>();
            QueryR("Erro ao obter procedimentos e convenios.", c, (r) => {
                while (r.Read()) {
                    var a = new _ProcedimentoConvenio {
                        Proc_ID = r.GetInt32(0),
                        Con_ID = r.GetInt32(1),
                        Valor = r.GetDouble(2),
                        Procedimento = r.GetString(3),
                        Convenio = r.GetString(4)
                    };
                    lista.Add(a);
                }
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
            }, true);
            return lista;
        }

        /// <summary>
        /// Valor
        /// </summary>
        /// <returns></returns>
        public static double Select(int procedimento, int convenio) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Valor)} from {Name} where {nameof(Procedimento)} = @p and {nameof(Convenio)} = @c;";
            c.Parameters.AddWithValue("@p", procedimento);
            c.Parameters.AddWithValue("@c", convenio);
            double a = double.NaN;
            QueryRLoop("Erro ao obter o valor do procedimento.", c, (r) => {
                a = r.GetDouble(0);
            });
            return a;
        }

    }

}
