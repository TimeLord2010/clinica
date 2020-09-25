using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.Entities;

namespace WPFClinica.Scripts.DB {
    class ProcedimentosLab_FormaPagamento : Template {

        public int Historico { get; set; }
        public int Pagamento { get; set; }
        public double Valor { get; set; }

        static ProcedimentosLab_FormaPagamento plf = new ProcedimentosLab_FormaPagamento();

        public override string TableName {
            get => nameof(ProcedimentosLab_FormaPagamento);
        }

        public override Action GetCT() {
            return () => {
                ProcedimentosLab.CreateTable();
                FormaDePagamento.CreateTable();
                SQLOperations.NonQuery($"Erro ao criar tabela. ({ErrorCodes.DB0048})",
                    $"create table if not exists {TableName} (" +
                    $"  {nameof(Historico)} int," +
                    $"  {nameof(Pagamento)} int," +
                    $"  {nameof(Valor)} double," +
                    $"  primary key ({nameof(Historico)}, {nameof(Pagamento)})," +
                    $"  foreign key ({nameof(Historico)}) references {Historico_ProcedimentosLab.Name} ({nameof(Historico_ProcedimentosLab.ID)}) on delete cascade on update cascade," +
                    $"  foreign key ({nameof(Pagamento)}) references {FormaDePagamento.Name} ({nameof(FormaDePagamento.ID)}) on delete cascade on update cascade " +
                    $");");
            };
        }

        public static void Insert(int proc, int pag, double valor) {
            using (var c = new MySqlCommand($"insert into {plf.TableName} values ({proc}, {pag}, @valor);")) {
                c.Parameters.AddWithValue("@valor", valor);
                plf.NonQuery($"Erro em Proc_Lab-Pagamento. ({ErrorCodes.DB0049})", c);
            }
        }

        public static void Delete(int proc, int pag) {
            using (var c = new MySqlCommand($"delete from {plf.TableName} where {nameof(Historico)} = {proc} and {nameof(Pagamento)} = {pag};")) {
                plf.NonQuery($"Erro em ProcLab-Pagamento. ({ErrorCodes.DB0051})", c);
            }
        }

        public static List<_ProcedimentosLab_FormaDePagamento> Select(int proc) {
            var a = new ProcedimentosLab_FormaPagamento();
            var lista = new List<_ProcedimentosLab_FormaDePagamento>();
            using (var c = new MySqlCommand()) {
                c.CommandText =
                    $"select " +
                    $"  a.*, b.{nameof(FormaDePagamento.Descricao)} " +
                    $"from {plf.TableName} as a " +
                    $"inner join {FormaDePagamento.Name} as b on a.{nameof(Pagamento)} = b.{nameof(FormaDePagamento.ID)} " +
                    $"where {nameof(Historico)} = {proc};";
                a.QueryRLoop($"Erro em ProcLab-Pagamento. ({ErrorCodes.DB0050})", c, (r) => {
                    var pl = new ProcedimentosLab() {
                        ID = r.GetInt32(0)
                    };
                    var fp = new FormaDePagamento() {
                        ID = r.GetInt32(1),
                        Descricao = r.GetString(3)
                    };
                    lista.Add(new _ProcedimentosLab_FormaDePagamento() {
                        ProcedimentoLab = pl,
                        FormaDePagamento = fp,
                        Valor = r.GetDouble(2)
                    });
                });
            }
            return lista;
        }

    }
}
