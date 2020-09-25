using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClinica.Scripts.DB {

    class ListaEspera_Exames {

        public const string Name = nameof(ListaEspera_Exames);

        public int Senha { get; set; }
        public int Exame { get; set; }

        public static void CreateTable() {
            Procedimentos.CreateTable();
            SQLOperations.NonQuery("Erro ao criar exames para lista de espera.",
                $"create table if not exists {Name} (" +
                $"  {nameof(Senha)} int," +
                $"  {nameof(Exame)} int," +
                $"  primary key ({nameof(Senha)}, {nameof(Exame)})," +
                $"  foreign key ({nameof(Senha)}) references {ListaEspera.Name} ({nameof(ListaEspera.Senha)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Exame)}) references {Procedimentos.Name} ({nameof(Procedimentos.ID)}) on delete cascade on update cascade" +
                $");");
        }

        private static void Create(Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                CreateTable();
            }
        }

        private static void NonQuery(string title, MySqlCommand c) {
            SQLOperations.NonQuery(title, c, Create, true);
        }

        private static void QueryRLoop(string title, MySqlCommand c, Action<MySqlDataReader> action) {
            SQLOperations.QueryRLoop(title, c, action, Create, true);
        }

        public static void Insert(int senha, int exame) {
            var c = new MySqlCommand();
            c.CommandText = $"insert into {Name} values (@senha, @exame);";
            c.Parameters.AddWithValue("@senha", senha);
            c.Parameters.AddWithValue("@exame", exame);
            NonQuery($"Erro ao inserir exame em lista. ({ErrorCodes.DB0035})", c);
        }

        public static void Delete(int senha, int exame) {
            var c = new MySqlCommand();
            c.CommandText = $"delete from {Name} where {nameof(Senha)} = @senha and {nameof(Exame)} = @exame;";
            c.Parameters.AddWithValue("@senha", senha);
            c.Parameters.AddWithValue("@exame", exame);
            NonQuery($"Erro ao deletar exame de lista. ({ErrorCodes.DB0036})", c);
        }

        public static void Delete(int senha) {
            var c = new MySqlCommand();
            c.CommandText = $"delete from {Name} where {nameof(Senha)} = @senha;";
            c.Parameters.AddWithValue("@senha", senha);
            NonQuery($"Erro ao deletar exames de lista. ({ErrorCodes.DB0037})", c);
        }

        public static _Lista_Procedimentos Select(int senha) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select " +
                $"  a.{nameof(Senha)}," + // 0
                $"  a.{nameof(Exame)}," + // 1
                $"  b.{nameof(ListaEspera.Paciente)}," + // 2
                $"  c.{nameof(Pessoas.Nome)}," + // 3
                $"  e.{nameof(Procedimentos.Descricao)}," + // 4
                $"  e.{nameof(Procedimentos.Tipo)}," + // 5
                $"  e.{nameof(Procedimentos.Valor)}," + // 6
                $"  f.{nameof(TipoProcedimento.Tipo)}," + // 7
                $"  b.{nameof(ListaEspera.Fila)} " + // 8
                $"from {Name} as a " +
                $"inner join {ListaEspera.Name} as b on a.{nameof(Senha)} = b.{nameof(ListaEspera.Senha)} " +
                $"inner join {Pessoas.p.TableName} as c on b.{nameof(ListaEspera.Paciente)} = c.{nameof(Pessoas.CPF)} " +
                $"inner join {Procedimentos.Name} as e on a.{nameof(Exame)} = e.{nameof(Procedimentos.ID)} " +
                $"inner join {TipoProcedimento.Name} as f on e.{nameof(Procedimentos.Tipo)} = f.{nameof(TipoProcedimento.ID)} " +
                $"where " +
                $"  a.{nameof(Senha)} = @senha " +
                $"limit 500;";
            c.Parameters.AddWithValue("@senha", senha);
            _Lista_Procedimentos lista = new _Lista_Procedimentos();
            int i = 0;
            QueryRLoop($"Erro ao obter exames de lista. ({ErrorCodes.DB0038})", c, (r) => {
                if (i++ == 0) {
                    lista.ListaEspera = new _ListaEspera() {
                        Senha = r.GetInt32(0),
                        CPF = r.GetString(2),
                        Nome = r.GetString(3),
                        Fila = r.GetString(8)
                    };
                    lista.Procedimentos = new List<_Procedimento>();
                }
                lista.Procedimentos.Add(new _Procedimento() {
                    ID = r.GetInt32(1),
                    Descricao = r.GetString(4),
                    Tipo_ID = r.GetInt32(5),
                    Valor = r.GetDouble(6),
                    Tipo = r.GetString(7)
                });
            });
            return lista;
        }

    }
}
