using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Enderecos {

        public const string Name = "enderecos";

        public string CPF { get; set; }
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }

        public static void CreateTable() {
            var p = new Pessoas();
            NonQuery($"Erro ao criar a tabela {Name}.",
                $"create table if not exists {Name} (" +
                $"  cpf varchar(15) not null," +
                $"  cep varchar(20) not null," +
                $"  estado nvarchar(50) not null," +
                $"  cidade nvarchar(50) not null," +
                $"  bairro nvarchar(50) not null," +
                $"  rua nvarchar(50) not null," +
                $"  numero nvarchar(50) not null," +
                $"  complemento nvarchar(50) not null," +
                $"  primary key (cpf)," +
                $"  foreign key (cpf) references {Pessoas.p.TableName} ({nameof(p.CPF)}) on delete cascade on update cascade" +
                $");");

        }

        public static void Insert(string cpf, string cep, string estado, string cidade, string bairro, string rua, string numero, string complemento) {
            NonQuery("Erro ao inserir endereço.", (c) => {
                c.CommandText = $"insert into {Name} values (@cpf, @cep, @estado, @cidade, @bairro, @rua, @numero, @complemento);";
                c.Parameters.AddWithValue("@cpf", cpf);
                c.Parameters.AddWithValue("@cep", cep);
                c.Parameters.AddWithValue("@estado", estado);
                c.Parameters.AddWithValue("@cidade", cidade);
                c.Parameters.AddWithValue("@bairro", bairro);
                c.Parameters.AddWithValue("@rua", rua);
                c.Parameters.AddWithValue("@numero", numero);
                c.Parameters.AddWithValue("@complemento", complemento);
                return c;
            });
        }

        public static void Update(string cpf, string cep, string estado, string cidade, string bairro, string rua, string numero, string complemento) {
            NonQuery($"Erro ao atualizar a tabela {Name}", (c) => {
                c.CommandText =
                $"update {Name} set " +
                $"cep = @cep, estado = @estado, cidade = @cidade, bairro = @bairro, rua = @rua, numero = @numero, complemento = @complemento " +
                $"where cpf = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                c.Parameters.AddWithValue("@cep", cep);
                c.Parameters.AddWithValue("@estado", estado);
                c.Parameters.AddWithValue("@cidade", cidade);
                c.Parameters.AddWithValue("@bairro", bairro);
                c.Parameters.AddWithValue("@rua", rua);
                c.Parameters.AddWithValue("@numero", numero);
                c.Parameters.AddWithValue("@complemento", complemento);
                return c;
            });
        }

        public static void InsertOrUpdate(string cpf, string cep, string estado, string cidade, string bairro, string rua, string numero, string complemento) {
            var r = Select(cpf);
            if (r == null) {
                Insert(cpf, cep, estado, cidade, bairro, rua, numero, complemento);
            } else {
                Update(cpf, cep, estado, cidade, bairro, rua, numero, complemento);
            }
        }

        public static Enderecos Select(string cpf) {
            var c = new MySqlCommand($"select * from {Name} where cpf = @cpf;");
            c.Parameters.AddWithValue("@cpf", cpf);
            Enderecos e = null;
            QueryRLoop("Erro ao se obter endereço.", c, (r) => {
                e = new Enderecos() {
                    CPF = r.GetString(0),
                    CEP = r.GetString(1),
                    Estado = r.GetString(2),
                    Cidade = r.GetString(3),
                    Bairro = r.GetString(4),
                    Rua = r.GetString(5),
                    Numero = r.GetString(6),
                    Complemento = r.GetString(7)
                };
            });
            return e;
        }

    }

}
