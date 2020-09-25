using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFClinica.Scripts.Entities;

namespace WPFClinica.Scripts.DB {

    class HistoricoVersoes : Template {

        public int ID { get; set; }
        public string Versao { get; set; }
        public DateTime _DateTime { get; set; }

        static HistoricoVersoes hv = new HistoricoVersoes();

        public override string TableName {
            get => nameof(HistoricoVersoes);
        }

        public static void Insert (string versao) {
            Insert(versao, DateTime.Now);
        }

        public static void Insert(string versao, DateTime dt) {
            using (var c = new MySqlCommand($"insert into {hv.TableName} values (null, @versao, @dt);")) {
                c.Parameters.AddWithValue("@versao", versao);
                c.Parameters.AddWithValue("@dt", dt.ToString("yyyy-MM-dd HH:mm:ss"));
                hv.NonQuery($"Erro ao inserir versão. ({ErrorCodes.DB0042})", c);
            }
        }

        public static List<_HistoricoVersoes> Select (int id) {
            using (var c = new MySqlCommand($"select * from {hv.TableName} where {nameof(ID)} = {id};")) {
                var lista = new List<_HistoricoVersoes>();
                hv.QueryRLoop($"Erro ao obter versões. ({ErrorCodes.DB0043})", c, (r) => {
                    lista.Add(_HistoricoVersoes.GetValues(r));
                });
                return lista;
            }
        }

        public static List<_HistoricoVersoes> Select (string beginV) {
            using (var c = new MySqlCommand($"select * from {hv.TableName} where {nameof(Versao)} like concat(@begin, '%');")) {
                c.Parameters.AddWithValue("@begin", beginV);
                var lista = new List<_HistoricoVersoes>();
                hv.QueryRLoop($"Erro ao obter versões. ({ErrorCodes.DB0043})", c, (r) => {
                    lista.Add(_HistoricoVersoes.GetValues(r));
                });
                return lista;
            }
        }

        public static _HistoricoVersoes Select() {
            using (var c = new MySqlCommand($"select * from {hv.TableName} order by {nameof(ID)} desc limit 1;")) {
                var h = _HistoricoVersoes.GetDefaultValues;
                hv.QueryRLoop("Erro ao obter ultima versão.", c, (r) => {
                    h = _HistoricoVersoes.GetValues(r);
                });
                return h;
            }
        }

        public override Action GetCT() {
            return () => {
                SQLOperations.NonQuery($"Erro ao criar historico de versões. ({ErrorCodes.DB0041})",
                    $"create table if not exists {TableName} (" +
                    $"  {nameof(ID)} int auto_increment," +
                    $"  {nameof(Versao)} varchar(30)," +
                    $"  {nameof(_DateTime)} datetime," +
                    $"  primary key ({nameof(ID)})" +
                    $");");
            };
        }

    }

}
