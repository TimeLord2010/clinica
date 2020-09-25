using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.Entities;

namespace WPFClinica.Scripts.DB {

    class Procedimento_FormaPagamentos : Template {

        public int Procedimento { get; set; }
        public int FormaPagamento { get; set; }
        public double Valor { get; set; }

        static Procedimento_FormaPagamentos pf = new Procedimento_FormaPagamentos();

        public Procedimento_FormaPagamentos() { }

        public Procedimento_FormaPagamentos(int proc, int pag, double valor) {
            Procedimento = proc;
            FormaPagamento = pag;
            Valor = valor;
        }

        public override string TableName {
            get => nameof(Procedimento_FormaPagamentos);
        }

        public override Action GetCT() {
            return () => {
                Procedimentos.CreateTable();
                FormaDePagamento.CreateTable();
                SQLOperations.NonQuery($"Erro em Procedimento_FormaPagamento. ({ErrorCodes.DB0052})",
                    $"create table if not exists {TableName} (" +
                    $"  {nameof(Procedimento)} int," +
                    $"  pagamento int," +
                    $"  valor double," +
                    $"  primary key ({nameof(Procedimento)}, pagamento)," +
                    $"  foreign key ({nameof(Procedimento)}) references {Historico_Procedimento.Name} ({nameof(Historico_Procedimento.ID)}) on delete cascade," +
                    $"  foreign key (pagamento) references {FormaDePagamento.Name} ({nameof(FormaDePagamento.ID)}) on delete cascade" +
                    $");");
            };
        }

        public void Insert() {
            Insert(Procedimento, FormaPagamento, Valor);
        }

        public void Insert(int historico, int pagamento, double valor) {
            using (var c = new MySqlCommand($"insert into {pf.TableName} values (@proc, @pag, @val)")) {
                c.Parameters.AddWithValue("@proc", historico);
                c.Parameters.AddWithValue("@pag", pagamento);
                c.Parameters.AddWithValue("@val", valor);
                pf.NonQuery($"Erro em Procedimento-Pagamento. ({ErrorCodes.DB0053})", c);
            }
        }

        public List<_Procedimento_FormaDePagamentos> Select(int procedimento) {
            var lista = new List<_Procedimento_FormaDePagamentos>();
            using (var c = new MySqlCommand(
                $"select " +
                $"  a.*, b.{nameof(FormaDePagamento.Descricao)} " +
                $"from {pf.TableName} as a " +
                $"inner join {FormaDePagamento.Name} as b on a.pagamento = b.{nameof(FormaDePagamento.ID)} " +
                $"where procedimento = @proc;")) {
                c.Parameters.AddWithValue("@proc", procedimento);
                QueryRLoop($"Erro em Procedimento-Pagamento. ({ErrorCodes.DB0054})", c, (r) => {
                    var proc = new Procedimentos() {
                        ID = r.GetInt32(0)
                    };
                    var pag = new FormaDePagamento() {
                        ID = r.GetInt32(1),
                        Descricao = r.GetString(3)
                    };
                    lista.Add(new _Procedimento_FormaDePagamentos() {
                        Procedimento = proc,
                        Pagamento = pag,
                        Valor = r.GetDouble(2)
                    });
                });
                return lista;
            }
        }

    }
}