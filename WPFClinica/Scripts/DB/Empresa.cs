using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.Entities;

namespace WPFClinica.Scripts.DB {

    class Empresa : Template {

        public override string TableName => nameof(Empresa);

        static Empresa e = new Empresa();

        public static void Insert(string nome, string cnpj, string observação, DateTime dt) {
            using (var c = new MySqlCommand($"insert into {e.TableName} values (null, @nome, @cnpj, @dt, @obs);")) {
                c.Parameters.AddWithValue("@nome", nome);
                c.Parameters.AddWithValue("@cnpj", cnpj);
                c.Parameters.AddWithValue("@dt", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@obs", observação);
                e.NonQuery("Erro ao inserir empresa.", c);
            }
        }

        public static void Insert(string nome, string cnpj, string obs) {
            Insert(nome, cnpj, obs, DateTime.Now);
        }

        public static _Empresa Select(int id) {
            using (var c = new MySqlCommand($"select * from {e.TableName} where id = {id};")) {
                var emp = _Empresa.DefaultValues;
                e.QueryRLoop($"Erro ao obter empresa.", c, (r) => {
                    emp = _Empresa.Get(r);
                });
                return emp;
            }
        }

        public static List<_Empresa> Select(string nome, string cnpj, int limit = 1000) {
            using (var c = new MySqlCommand($"select * from {e.TableName} where nome like concat(@nome,'%') and cnpj like concat(@cnpj, '%') limit {limit};")) {
                c.Parameters.AddWithValue("@nome", nome);
                c.Parameters.AddWithValue("@cnpj", cnpj);
                var lista = new List<_Empresa>();
                e.QueryRLoop($"Erro ao obter empresas.", c, (r) => {
                    var emp = _Empresa.Get(r);
                    if (emp.ID > 1) {
                        lista.Add(emp);
                    }
                });
                return lista;
            }
        }

        public static void Update(int id, string nome, string cnpj, string observação) {
            using (var c = new MySqlCommand($"update {e.TableName} set nome = @nome, cnpj = @cnpj, observacao = @obs where id = {id};")) {
                c.Parameters.AddWithValue("@nome", nome);
                c.Parameters.AddWithValue("@cnpj", cnpj);
                c.Parameters.AddWithValue("@obs", observação);
                e.NonQuery("Erro ao atualizar dados da empresa.", c);
            }
        }

        public override Action GetCT() {
            return () => {
                string a = $"create table if not exists {TableName} (" +
                "   id int auto_increment," +
                "   nome nvarchar(200)," +
                "   cnpj varchar(30)," +
                "   registro datetime," +
                "   observacao mediumtext, " +
                "   primary key (id)" +
                ");";
                SQLOperations.NonQuery("Erro ao criar tabela de empresa.", a);
                if (Select(1).ID == -1) {
                    Insert("", "", "", DateTime.MinValue);
                }
            };
        }

    }
}
