using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.Entities;

namespace WPFClinica.Scripts.DB {

    class Consultas_FormaPagamento : Template {

        private const string a = "dos argumentos é inválido para inserção de associação de histórico de consulta e forma de pagamento.";
        private readonly ArgumentException InvalidArgNum = new ArgumentException($"O número {a}");
        private readonly ArgumentException InvalidArgType = new ArgumentException($"O tipo {a}");

        public void Delete(params object[] para) {
            var c = new MySqlCommand();
            if (para.Length == 2) {
                if (para[0] is int consulta && para[1] is int pagamento) {
                    c.CommandText = $"delete from {TableName} where consulta = {consulta} and pagamento = {pagamento};";
                } else {
                    throw new ArgumentException($"{InvalidArgType.Message} ({ErrorCodes.AG0001})");
                }
            } else {
                throw new ArgumentException($"{InvalidArgNum.Message} ({ErrorCodes.AG0002})");
            }
            NonQuery($"Erro ({ErrorCodes.DB0046}).", c);
        }

        public override string TableName {
            get => nameof(Consultas_FormaPagamento);
        }

        public void Insert(params object[] para) {
            var c = new MySqlCommand();
            if (para.Length == 3) {
                if (para[0] is int consulta && para[1] is int pagamento && para[2] is double valor) {
                    c.CommandText = $"insert into {TableName} values ({consulta}, {pagamento}, {valor});";
                } else {
                    throw new ArgumentException($"{InvalidArgType.Message} ({ErrorCodes.AG0003})");
                }
            } else {
                throw new ArgumentException($"{InvalidArgNum.Message} ({ErrorCodes.AG0004})");
            }
            NonQuery($"Erro ({ErrorCodes.DB0046}).", c);
        }

        /// <summary>
        /// Int Consulta, Int FormaPagamento
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public List<_Consulta_FormaPagamento> Select(params object[] para) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select " +
                $"  a.consulta, " + // 0
                $"  a.pagamento," + // 1
                $"  b.Paciente," + // 2
                $"  b.Funcionario," + // 3
                $"  b.Profissao," + // 4
                $"  b.Especializacao," + // 5
                $"  b.Pago," + // 6
                $"  b.PagoEm," + // 7
                $"  b.Retorno," + // 8
                $"  b.RealizadoEm," + // 9
                $"  c.Descricao," + // 10
                $"  a.valor " + // 11
                $"from {TableName} as a " +
                $"inner join {Historico_Consultas.Name} as b on a.consulta = b.ID " +
                $"left join {FormaDePagamento.Name} as c on a.pagamento = c.ID " +
                $"where ";
            if (para.Length == 2) {
                if (para[0] is int consulta1 && para[1] is int pagamento1) {
                    c.CommandText += $"a.consulta = {consulta1} and b.pagamento = {pagamento1}";
                } else if (para[0] is int consulta) {
                    c.CommandText += $"a.consulta = {consulta}";
                } else if (para[1] is int pagamento) {
                    c.CommandText += $"b.pagamento = {pagamento}";
                } else {
                    throw new ArgumentException($"{InvalidArgType} ({ErrorCodes.AG0005})");
                }
            } else {
                throw new ArgumentException($"{InvalidArgNum} ({ErrorCodes.AG0006})");
            }
            c.CommandText += " limit 5000";
            var lista = new List<_Consulta_FormaPagamento>();
            QueryRLoop($"Erro ({ErrorCodes.DB0047})", c, (r) => {
                var h = new Historico_Consultas() { 
                    ID = r.GetInt32(0),
                    Paciente = r.IsDBNull(2)? "[Nulo]" : r.GetString(2),
                    Funcionario = r.IsDBNull(3)? "[Nulo]" : r.GetString(3),
                    Profissao = r.IsDBNull(4)? "[Nulo]" : r.GetString(4),
                    Especializacao = r.IsDBNull(5)? "[Nulo]" : r.GetString(5),
                    Pago = r.IsDBNull(6)? double.NaN : r.GetDouble(6),
                    PagoEm = r.IsDBNull(7) ? (DateTime?)null : r.GetMySqlDateTime(7).GetDateTime(),
                    Retorno = r.IsDBNull(8)? false : r.GetBoolean(8),
                    RealizadoEm = r.IsDBNull(9)? DateTime.MinValue : r.GetMySqlDateTime(9).GetDateTime()
                };
                var p = new FormaDePagamento() { 
                    ID = r.IsDBNull(1) ? -1 : r.GetInt32(1),
                    Descricao = r.IsDBNull(10)? "[Nulo]" : r.GetString(10)
                };
                var cfp = new _Consulta_FormaPagamento() {
                    Consulta = h,
                    FormaDePagamento = p,
                    Valor = r.IsDBNull(11)? double.NaN : r.GetDouble(11)
                };
                lista.Add(cfp);
            });
            return lista;
        }

        public override Action GetCT() {
            return () => {
                Historico_Consultas.CreateTable();
                FormaDePagamento.CreateTable();
                SQLOperations.NonQuery("Erro ao criar associação entre consulta e pagamento.",
                    $"create table if not exists {TableName} (" +
                    $"  consulta int," +
                    $"  pagamento int," +
                    $"  valor double," +
                    $"  primary key (consulta, pagamento)," +
                    $"  foreign key (consulta) references {Historico_Consultas.Name} ({nameof(Historico_Consultas.ID)}) on delete cascade on update cascade," +
                    $"  foreign key (pagamento) references {FormaDePagamento.Name} ({nameof(FormaDePagamento.ID)}) on delete no action on update cascade" +
                    $");");
            };
        }
    }

}
