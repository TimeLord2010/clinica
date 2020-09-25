using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Convert;
using static WPFClinica.Scripts.DB.SQLOperations;

namespace WPFClinica.Scripts.DB {

    public class Especializacoes {

        public const string Name = "especializacoes";

        public string Especializacao { get; set; }
        public double ValorConsulta { get; set; }

        public static void CreateTable() {
            NonQuery("Erro ao criar a tabela de especializacoes",
                $"create table if not exists {Name} (" +
                $"  {nameof(Especializacao)} nvarchar(50)," +
                $"  {nameof(ValorConsulta)} double," +
                $"  primary key ({nameof(Especializacao)})" +
                $");");
        }

        public static void Insert(string especializacao, double valor) {
            NonQuery("Erro ao inserir especializacao.", (c) => {
                c.CommandText = $"insert into {Name} values (@esp, @v);";
                c.Parameters.AddWithValue("@esp", especializacao);
                c.Parameters.AddWithValue("@v", valor);
                return c;
            });
        }

        public static void Delete(string especializacao) {
            NonQuery("Erro ao deletar especializacao.", (c) => {
                c.CommandText = $"delete from {Name} where {nameof(Especializacao)} = @esp;";
                c.Parameters.AddWithValue("@esp", especializacao);
                return c;
            });
        }

        public static void Update(string oldEspecializacao, string newEspecializacao) {
            NonQuery("Erro ao atualizacao especializacao.", (c) => {
                c.CommandText = $"update {Name} " +
                $"set {nameof(Especializacao)} = @new_esp " +
                $"where {nameof(Especializacao)} = @old_esp;";
                c.Parameters.AddWithValue("@old_esp", oldEspecializacao);
                c.Parameters.AddWithValue("@new_esp", newEspecializacao);
                return c;
            });
        }

        public static void Update_Valor(string especializacao, double valor) {
            NonQuery("Erro ao atualizar o valor da consulta de uma especialização", (c) => {
                c.CommandText = $"update {Name} set {nameof(ValorConsulta)} = @v where {nameof(Especializacao)} = @e;";
                c.Parameters.AddWithValue("@v", valor);
                c.Parameters.AddWithValue("@e", especializacao);
                return c;
            });
        }

        public static Especializacoes Select(string especializacao) {
            var c = new MySqlCommand();
            c.CommandText = $"select {nameof(ValorConsulta)} from {Name} where {nameof(Especializacao)} = @esp;";
            c.Parameters.AddWithValue("@esp", especializacao);
            Especializacoes e = null;
            QueryRLoop("Erro ao obter especialização.", c, (r) => {
                e = new Especializacoes() {
                    Especializacao = especializacao,
                    ValorConsulta = r.GetDouble(0)
                };
            });
            return e;
        }

        public static List<Especializacoes> SelectAll() {
            var lista = new List<Especializacoes>();
            var c = new MySqlCommand();
            c.CommandText = $"select * from {Name}";
            QueryR("Erro ao obter todas as especializações.", c, (r) => {
                while (r.Read()) {
                    lista.Add(new Especializacoes() {
                        Especializacao = r.IsDBNull(0) ? null : r.GetString(0),
                        ValorConsulta = r.IsDBNull(1) ? double.NaN : r.GetDouble(1)
                    });
                }
            });
            return lista;

        }

        public static List<Especializacoes> SelectLike(string especializacao) {
            var c = new MySqlCommand();
            c.CommandText =
                $"select * " +
                $"from {Name} where {nameof(Especializacao)} like concat(@esp, '%');";
            c.Parameters.AddWithValue("@esp", especializacao);
            var a = new List<Especializacoes>();
            QueryRLoop("Erro ao pesquisar especialização.", c, (r) => {
                a.Add(new Especializacoes() {
                    Especializacao = r.GetString(0),
                    ValorConsulta = r.GetDouble(1)
                });
            });
            return a;
        }

        public static void TryInsert(string especializacao, double valor) {
            var r = SelectLike(especializacao);
            if (r.Count == 0) {
                Insert(especializacao, valor);
            }
        }

        public static void Fill(ComboBox cb) {
            var r = SelectAll();
            for (int i = 0; i < r.Count; i++) {
                cb.Items.Add(new ComboBoxItem() {
                    Content = r[i].Especializacao
                });
            }
        }

    }

}
