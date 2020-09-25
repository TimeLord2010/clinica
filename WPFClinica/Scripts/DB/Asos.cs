using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFClinica.Scripts.Entities;

namespace WPFClinica.Scripts.DB {
    class Asos : Template {

        static Asos asos = new Asos();
        static Empresa emp = new Empresa();

        public static void Insert(string cpf, int empresa, DateTime realizadoEm, DateTime notificarEm) {
            using (var c = new MySqlCommand($"insert into {asos.TableName} values (@pac, @emp, @re, 0, @no) " +
                $"on duplicate key update empresa = {empresa}, realizado = @re, notificado = 0, notificarEm = @no;")) {
                c.Parameters.AddWithValue("@pac", cpf);
                c.Parameters.AddWithValue("@emp", empresa);
                c.Parameters.AddWithValue("@re", realizadoEm.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@no", notificarEm.ToString("yyyy-MM-dd HH:mm:ss"));
                asos.NonQuery("Erro ao inserir asos.", c);
            }
        }

        public static void Insert(string cpf, int empresa, DateTime notificarEm) {
            Insert(cpf, empresa, DateTime.Now, notificarEm);
        }

        public static void Update(string cpf, DateTime realizadoEm, DateTime notificarEm) {
            using (var c = new MySqlCommand($"update {asos.TableName} set realizado = @dt, notificado = 0, notificarEm = @no where paciente = @pac;")) {
                c.Parameters.AddWithValue("@pac", cpf);
                c.Parameters.AddWithValue("@dt", realizadoEm.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@no", notificarEm.ToString("yyyy-MM-dd HH:mm:ss"));
                asos.NonQuery("Erro ao atualizar asos.", c);
            }
        }

        public static void Update (string cpf, bool notificado) {
            using (var c = new MySqlCommand($"update {asos.TableName} set notificado = {(notificado? "true" : "false")} where paciente = @cpf")) {
                c.Parameters.AddWithValue("@cpf", cpf);
                asos.NonQuery("Erro ao atualizar aso.", c);
            }
        }

        public static _Asos Select(string cpf) {
            using (var c = new MySqlCommand($"select realizado, empresa, notificado, notificarEm from {asos.TableName} where paciente = @pac;")) {
                c.Parameters.AddWithValue("@pac", cpf);
                var aso = _Asos.DefaultValues;
                asos.QueryRLoop("Erro ao obter data de aso.", c, (r) => {
                    aso.Paciente = new Pessoas() { 
                        CPF = cpf
                    }; 
                    aso.RealizadoEm = r.GetMySqlDateTime(0).GetDateTime();
                    aso.Empresa = new _Empresa(r.GetInt32(1));
                    aso.Notificado = r.GetBoolean(2);
                    aso.NotificadoEm = r.GetMySqlDateTime(3).GetDateTime();
                });
                return aso;
            }
        }

        public static List<(string PacienteCPF, _Empresa Empresa, DateTime Realizado)> Select(DateTime begin, DateTime end) {
            var e = new Empresa();
            using (var c = new MySqlCommand($"select paciente, realizado, a.empresa, b.nome, b.cnpj, b.registo, b.observacao from {asos.TableName} as a " +
                $"left join {e.TableName} as b on a.empresa = b.id " +
                $"where realizado between @begin and @end;")) {
                c.Parameters.AddWithValue("@begin", begin.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd HH:mm:ss"));
                var list = new List<(string, _Empresa, DateTime)>();
                asos.QueryRLoop("Erro ao obter asos.", c, (r) => {
                    list.Add((r.GetString(0), new _Empresa() {
                        ID = r.IsDBNull(2) ? -1 : r.GetInt32(2),
                        Nome = r.IsDBNull(3) ? "" : r.GetString(3),
                        CNPJ = r.IsDBNull(4) ? "" : r.GetString(4),
                        Registro = r.IsDBNull(2) ? DateTime.MinValue : r.GetMySqlDateTime(5).GetDateTime(),
                        Observação = r.IsDBNull(6) ? "" : r.GetString(6)
                    }, r.GetMySqlDateTime(1).GetDateTime()));
                });
                return list;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="np"></param>
        /// <param name="ne"></param>
        /// <param name="cnpj"></param>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="realizado">
        /// If this parameter is true, then the date range will reflect the realization date. Otherwise, it will notified date.
        /// </param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static List<_Asos> Select(string np, string ne, string cnpj, DateTime b, DateTime e, bool realizado = true, int limit = 500) {
            var column = realizado ? "realizado" : "notificarEm";
            var aux = realizado ? "" : "and a.notificado = 0";
            var list = new List<_Asos>();
            using (var c = new MySqlCommand(
                $"select " +
                $"  a.realizado," +  // 0
                $"  b.cpf," + // 1
                $"  b.nome," + // 2
                $"  b.nascimento," + // 3
                $"  c.id," + // 4
                $"  c.nome," + // 5
                $"  c.cnpj, " + // 6
                $"  a.notificado," + // 7
                $"  a.notificarEm " + // 8
                $"from {asos.TableName} as a " +
                $"inner join {Pessoas.p.TableName} as b on a.paciente = b.cpf " +
                $"inner join {emp.TableName} as c on a.empresa = c.id " +
                $"where " +
                $"b.Nome like concat(@np,'%') and " +
                $"c.nome like concat(@ne, '%') and " +
                $"c.cnpj like concat(@cnpj, '%') and " +
                $"(a.{column} between @b and @e) " +
                $"{aux} " +
                $"limit {limit};")) {
                c.Parameters.AddWithValue("@np", np);
                c.Parameters.AddWithValue("@ne", ne);
                c.Parameters.AddWithValue("@cnpj", cnpj);
                c.Parameters.AddWithValue("@b", b.ToString("yyyy-MM-dd HH:mm:ss"));
                c.Parameters.AddWithValue("@e", e.ToString("yyyy-MM-dd HH:mm:ss"));
                asos.QueryRLoop($"Erro ao obter ASOs.", c, (r) => {
                    list.Add(new _Asos() {
                        RealizadoEm = r.GetMySqlDateTime(0).GetDateTime(),
                        Paciente = new Pessoas() {
                            CPF = r.GetString(1),
                            Nome = r.GetString(2),
                            Nascimento = r.GetMySqlDateTime(3).GetDateTime()
                        },
                        Empresa = new _Empresa() { 
                            ID = r.GetInt32(4),
                            Nome = r.GetString(5),
                            CNPJ = r.GetString(6)
                        },
                        Notificado = r.GetBoolean(7),
                        NotificadoEm = r.GetMySqlDateTime(8).GetDateTime()
                    });
                });
            }
            return list;
        }


        public override string TableName {
            get => nameof(Asos);
        }

        public override Action GetCT() {
            return () => {
                var emp = new Empresa();
                emp.CreateTable();
                SQLOperations.NonQuery($"Erro ao criar asos. ({ErrorCodes.DB0058})", $"create table if not exists {TableName} (" +
                "   paciente varchar(15)," +
                "   empresa int, " +
                "   realizado datetime," +
                "   notificado bool," +
                "   notificarEm datetime," +
                "   primary key (paciente)," +
                $"  foreign key (paciente) references {Pacientes.Name} (cpf)" +
                $");");
            };
        }
    }
}
