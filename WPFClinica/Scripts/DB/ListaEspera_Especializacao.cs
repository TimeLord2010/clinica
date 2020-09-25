using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    class ListaEspera_Especializacao {

        public const string Name = nameof(ListaEspera_Especializacao);

        public int Senha { get; set; }
        public string Especializacao { get; set; }

        public static void CreateTable() {
            var le = new ListaEspera();
            var e = new Especializacoes();
            NonQuery("Erro ao criar associação de lista de espera e especialização.", (c) => {
                c.CommandText = $"create table if not exists {nameof(ListaEspera_Especializacao)} (" +
                $"  {nameof(Senha)} int," +
                $"  {nameof(Especializacao)} nvarchar(50)," +
                $"  primary key ({nameof(Senha)})," +
                $"  foreign key ({nameof(Senha)}) references {ListaEspera.Name} ({nameof(le.Senha)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Especializacao)}) references {Especializacoes.Name} ({nameof(e.Especializacao)}) on delete cascade on update cascade " +
                $");";
                return c;
            });
        }

        public static void Insert(int senha, string especializacao) {
            NonQuery("Erro ao inserir associação de lista de espera e especialização.", (c) => {
                c.CommandText = $"insert into {nameof(ListaEspera_Especializacao)} values (@senha, @esp);";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@esp", especializacao);
                return c;
            }, (ex) => {
                if (ex.Message.Contains("doesn't exist")) {
                    CreateTable();
                }
            }, true);
        }

        public static void Delete(int senha) {
            NonQuery("Erro ao deletar associação de lista de espera e especializacao.", (c) => {
                c.CommandText = $"delete from {nameof(ListaEspera_Especializacao)} where {nameof(Senha)} = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                return c;
            });
        }

        public static ListaEspera_Especializacao Select(int senha) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {nameof(ListaEspera_Especializacao)} where {nameof(Senha)} = @senha;";
            c.Parameters.AddWithValue("@senha", senha);
            ListaEspera_Especializacao le = null;
            QueryRLoop($"Erro ao obter associação {Name}. ({ErrorCodes.DB0002})", c, (r) => {
                le = new ListaEspera_Especializacao() {
                    Senha = r.GetInt32(0),
                    Especializacao = r.GetString(1)
                };
            });
            return le;
        }

        public static void Update(int senha, string especializacao) {
            NonQuery($"Erro ao atualizar associação {Name}. ({ErrorCodes.DB0001})", (c) => {
                c.CommandText = $"update {nameof(ListaEspera_Especializacao)} set {nameof(Especializacao)} = @esp " +
                $"where senha = @senha;";
                c.Parameters.AddWithValue("@senha", senha);
                c.Parameters.AddWithValue("@esp", especializacao);
                return c;
            });
        }

        public static void InsertOrUpdate(int senha, string especializacao) {
            var r = Select(senha);
            if (r == null) {
                Insert(senha, especializacao);
            } else {
                Update(senha, especializacao);
            }
        }

    }
}
