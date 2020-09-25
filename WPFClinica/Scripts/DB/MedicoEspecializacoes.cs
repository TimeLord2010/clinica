using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class MedicoEspecializacoes {

        public const string Name = "medico_especializacoes";

        public string Medico { get; set; }
        public string Especializacao { get; set; }

        public static void CreateTable() {
            var m = new Medicos();
            var r = new Especializacoes();
            NonQuery($"Erro ao criar tabela {Name}.",
                $"create table if not exists {Name} (" +
                $"  {nameof(Medico)} varchar(15)," +
                $"  {nameof(Especializacao)} nvarchar(50)," +
                $"  primary key ({nameof(Medico)}, {nameof(Especializacao)})," +
                $"  foreign key ({nameof(Medico)}) references {Medicos.Name} ({nameof(m.CPF)}) on delete cascade on update cascade," +
                $"  foreign key ({nameof(Especializacao)}) references {Especializacoes.Name} ({nameof(r.Especializacao)}) on delete cascade on update cascade" +
                $");");
        }

        public static void Insert(string medico, string especializacao) {
            NonQuery("Erro ao inserir uma associação de médico com especialização.", (c) => {
                c.CommandText = $"insert into {Name} values (@medico, @especializacao);";
                c.Parameters.AddWithValue("@medico", medico);
                c.Parameters.AddWithValue("@especializacao", especializacao);
                return c;
            });
        }

        public static void Delete(string medico, string especializacao) {
            NonQuery("Erro ao deletar associação de médico com especialização.", (c) => {
                c.CommandText = $"delete from {Name} " +
                $"where {nameof(Medico)} = @medico and {nameof(Especializacao)} = @especializacao;";
                c.Parameters.AddWithValue("@medico", medico);
                c.Parameters.AddWithValue("@especializacao", especializacao);
                return c;
            });
        }

        public static void Delete(string medico) {
            NonQuery("Erro ao deletar associação de médico com especialização.", (c) => {
                c.CommandText = $"delete from {Name} " +
                $"where {nameof(Medico)} = @medico;";
                c.Parameters.AddWithValue("@medico", medico);
                return c;
            });
        }

        public static void UpdateMedico(string oldMedico, string newMedico, string especializacao) {
            NonQuery("Erro ao atualizar médico da associação médico-especializacao.", (c) => {
                c.CommandText = $"update {Name} " +
                $"set {nameof(Medico)} = @new_medico " +
                $"where {nameof(Especializacao)} = @especializacao and {nameof(Medico)} = @old_medico;";
                c.Parameters.AddWithValue("@new_medico", newMedico);
                c.Parameters.AddWithValue("@especializacao", especializacao);
                c.Parameters.AddWithValue("@old_medico", oldMedico);
                return c;
            });
        }

        public static void UpdateEspecializacao(string medico, string oldEspecializacao, string newEspecializacao) {
            NonQuery("Erro ao atualizar especializacao da associação médico-especializacao.", (c) => {
                c.CommandText = $"update {Name} " +
                $"set {nameof(Especializacao)} = @new_esp " +
                $"where {nameof(Especializacao)} = @old_esp and {nameof(Medico)} = @medico;";
                c.Parameters.AddWithValue("@medico", medico);
                c.Parameters.AddWithValue("@old_esp", oldEspecializacao);
                c.Parameters.AddWithValue("@new_esp", newEspecializacao);
                return c;
            });
        }

        public static List<string> Select(string medico) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(Especializacao)} from {Name} where {nameof(Medico)} = @cpf";
            c.Parameters.AddWithValue("@cpf", medico);
            var lista = new List<string>();
            QueryRLoop("Erro ao obter especializações do médico.", c, (r) => {
                lista.Add(r.GetString(0));
            });
            return lista;
        }

    }

}
