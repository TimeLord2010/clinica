using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Medicos {

        public const string Name = "medicos";

        public string CPF { get; set; }
        public string CRM { get; set; }
        public string Porcentagem { get; set; }

        public static void CreateTable() {
            var f = new Funcionarios();
            var title = $"Erro ao criar tabela {Name}.";
            var q = $"create table if not exists {Name} (" +
                $"  {nameof(CPF)} varchar(15) not null, " +
                $"  {nameof(CRM)} varchar(20) not null, " +
                $"  {nameof(Porcentagem)} varchar(10) not null, " +
                $"  primary key ({nameof(CPF)}), " +
                $"  foreign key ({nameof(CPF)}) references {Funcionarios.Name} ({nameof(f.CPF)}) ON DELETE CASCADE ON UPDATE CASCADE" +
                $");";
            NonQuery(title, q);
        }

        public static void Insert(string cpf, string crm, string porcentagem) {
            Insert(cpf, crm, ToDouble(porcentagem));
        }

        public static void Insert(string cpf, string crm, double porcentagem) {
            NonQuery("Erro ao inserir médico", (c) => {
                c.CommandText = $"insert into {Name} values (@cpf, @crm, @porcentagem);";
                c.Parameters.AddWithValue("@cpf", cpf);
                c.Parameters.AddWithValue("@crm", crm);
                c.Parameters.AddWithValue("@porcentagem", porcentagem);
                return c;
            });
        }

        public static void TryInsert(string cpf, string crm, string porcentagem) {
            TryInsert(cpf, crm, ToDouble(porcentagem));
        }

        public static void TryInsert(string cpf, string crm, double porcentagem) {
            if (Select(cpf) == null) {
                Insert(cpf, crm, porcentagem);
            }
        }

        public static void Delete(string cpf) {
            NonQuery("Erro ao deletar médico.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(CPF)} = @cpf;";
                c.Parameters.AddWithValue("@cpf", cpf);
                return c;
            });
        }

        /// <summary>
        /// CPF, Nome, Especializacao
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="nome"></param>
        public static List<_MedicoEspecializacoes> SelectLike(string cpf, string nome, string especializacao) {
            var c = new MySqlCommand();
            c.CommandText = $"select a.{nameof(CPF)}, {nameof(Pessoas.Nome)}, {nameof(MedicoEspecializacoes.Especializacao)} " +
            $"from {Name} as a " +
            $"inner join {Pessoas.p.TableName} as b on a.{nameof(CPF)} = b.{nameof(Pessoas.CPF)} " +
            $"left join {MedicoEspecializacoes.Name} as c on a.{nameof(CPF)} = c.{nameof(MedicoEspecializacoes.Medico)} " +
            $"where a.{nameof(CPF)} like concat(@cpf, '%') and {nameof(Pessoas.Nome)} like concat(@nome, '%') " +
            $"and ({nameof(MedicoEspecializacoes.Especializacao)} like concat(@esp, '%') or {nameof(MedicoEspecializacoes.Especializacao)} is null) " +
            $"limit 100;";
            c.Parameters.AddWithValue("@cpf", cpf);
            c.Parameters.AddWithValue("@nome", nome);
            c.Parameters.AddWithValue("@esp", especializacao);
            var lista = new List<_MedicoEspecializacoes>();
            QueryRLoop("Erro ao procurar médicos.", c, (r) => {
                lista.Add(new _MedicoEspecializacoes() {
                    CPF = r.IsDBNull(0) ? "[Nulo]" : r.GetString(0),
                    Nome = r.IsDBNull(1) ? "[Nulo]" : r.GetString(1),
                    Especializacao = r.IsDBNull(2) ? "[Nulo]" : r.GetString(2)
                });
            });
            return lista;
        }

        /// <summary>
        /// Obtem a instencia de 'Medicos' com as informações references ao cpf dado.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public static Medicos Select(string cpf) {
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name} where {nameof(CPF)} = @cpf limit 100;";
            c.Parameters.AddWithValue("@cpf", cpf);
            Medicos m = null;
            QueryRLoop("Erro ao obter médico.", c, (r) => {
                m = new Medicos() {
                    CPF = r.GetString(0),
                    CRM = r.GetString(1),
                    Porcentagem = r.GetString(2)
                };
            });
            return m;
        }

        public static bool IsDoctor(string funcionario) {
            bool a = false;
            var c = new MySqlCommand();
            c.CommandText = $"select cpf from {Name} where {nameof(CPF)} = @medico;";
            c.Parameters.AddWithValue("@medico", funcionario);
            QueryRLoop("Erro ao verificar se cpf é de um médico", c, (r) => {
                a = true;
            });
            return a;
        }

    }

}
