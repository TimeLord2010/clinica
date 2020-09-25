using System;
using System.Collections.Generic;
using System.Windows;
using MySql.Data.MySqlClient;

namespace WPFClinica.Scripts.DB {

    public class Pessoas : Template {

        public static readonly Pessoas p = new Pessoas();

        public string CPF { get; set; }
        public string Nome { get; set; }
        public DateTime Nascimento { get; set; }
        public bool Sexo { get; set; }
        public string Email { get; set; }
        public string Contato1 { get; set; }
        public string Contato2 { get; set; }
        public int Idade {
            get {
                var now = DateTime.Today;
                var age = now.Year - Nascimento.Year;
                if (Nascimento.Date > now.AddYears(-age)) age--;
                return age;
            }
        }

        public override string TableName => "pessoas";

        public Pessoas() { }

        /// <summary>
        /// Fills the structure with a instace in the database.
        /// </summary>
        /// <param name="cpf"></param>
        public Pessoas(string cpf) {
            CPF = cpf;
            Fill(cpf);
        }

        /// <summary>
        /// Preenche as propriedades da classe com uma linha do banco de dados.
        /// </summary>
        /// <param name="cpf">CPF da pessoa a se procurar no banco de dados.</param>
        public void Fill(string cpf) {
            var p = Pessoas.Select(cpf);
            if (p != null) {
                Nome = p.Nome;
                Nascimento = p.Nascimento;
                Sexo = p.Sexo;
                Email = p.Email;
                Contato1 = p.Contato1;
                Contato2 = p.Contato2;
            }
        }

        public static void Insert(string cpf, string nome, DateTime nascimento, bool sexo, string email, string contato1, string contato2) {
            var c = new MySqlCommand($"insert into {p.TableName} values (@cpf, @nome, @nascimento, @sexo, @email, @contato1, @contato2);");
            c.Parameters.AddWithValue("@cpf", cpf);
            c.Parameters.AddWithValue("@nome", nome);
            c.Parameters.AddWithValue("@nascimento", nascimento.ToString("yyyy-MM-dd"));
            c.Parameters.AddWithValue("@sexo", sexo);
            c.Parameters.AddWithValue("@email", email);
            c.Parameters.AddWithValue("@contato1", contato1);
            c.Parameters.AddWithValue("@contato2", contato2);
            p.NonQuery($"Erro ao inserir registro de em {p.TableName}.", c);
        }

        public static void InsertOrUpdate(string cpf, string nome, DateTime nascimento, bool sexo, string email, string contato1, string contato2) {
            var r = Select(cpf);
            if (r != null) {
                Update(cpf, nome, nascimento, sexo, email, contato1, contato2);
            } else {
                Insert(cpf, nome, nascimento, sexo, email, contato1, contato2);
            }
        }

        public static void EnsureExistence(string cpf, string nome, DateTime nascimento, bool sexo, string email, string contato1, string contato2) {
            var r = Select(cpf);
            if (r == null) {
                Insert(cpf, nome, nascimento, sexo, email, contato1, contato2);
            }
        }

        public static void Delete(string cpf) {
            var c = new MySqlCommand($"delete from {p.TableName} where {nameof(CPF)} = @cpf;");
            c.Parameters.AddWithValue("@cpf", cpf);
            p.NonQuery($"Erro ao deletar registro em {p.TableName}.", c);
        }

        public static void UpdateCPF(string oldCPF, string newCPF) {
            var c = new MySqlCommand($"update {p.TableName} set {nameof(CPF)} = @new where {nameof(CPF)} = @old;");
            c.Parameters.AddWithValue("@old", oldCPF);
            c.Parameters.AddWithValue("@new", newCPF);
            p.NonQuery($"Erro ao atualizar o cpf de {p.TableName}.", c);
        }

        public static void Update(string cpf, string nome, DateTime nascimento, bool sexo, string email, string contato1, string contato2) {
            var c = new MySqlCommand($"update {p.TableName} set " +
                $"{nameof(Nome)} = @nome, " +
                $"{nameof(Nascimento)} = @nascimento, " +
                $"{nameof(Sexo)} = @sexo, " +
                $"{nameof(Email)} = @email, " +
                $"{nameof(Contato1)} = @contato1, " +
                $"{nameof(Contato2)} = @contato2 " +
                $"where {nameof(CPF)} = @cpf;");
            c.Parameters.AddWithValue("@nome", nome);
            c.Parameters.AddWithValue("@nascimento", nascimento.ToString("yyyy-MM-dd"));
            c.Parameters.AddWithValue("@sexo", sexo);
            c.Parameters.AddWithValue("@email", email);
            c.Parameters.AddWithValue("@contato1", contato1);
            c.Parameters.AddWithValue("@contato2", contato2);
            c.Parameters.AddWithValue("@cpf", cpf);
            p.NonQuery($"Erro ao atualizar {p.TableName}.", c);
        }

        static void A(string title, Action action) {
            try {
                action.Invoke();
            } catch (Exception ex) {
                MessageBox.Show($"{title}\n{ex.Message}");
            }
        }

        public static Pessoas Select(string cpf) {
            var c = new MySqlCommand($"select * from {p.TableName} where {nameof(CPF)} = @cpf;");
            c.Parameters.AddWithValue("@cpf", cpf);
            Pessoas pe = null;
            p.QueryRLoop("Erro ao obter pessoa.", c, (r) => {
                pe = new Pessoas();
                A("Não foi possível obter o cpf.", () => pe.CPF = r.GetString(0));
                A("Não foi possível obter o nome.", () => pe.Nome = r.GetString(1));
                A("Não foi possível obter a data de nascimento.", () => pe.Nascimento = r.GetMySqlDateTime(2).GetDateTime());
                A("Não foi possível obter o sexo.", () => pe.Sexo = r.GetBoolean(3));
                A("Não foi possível obter o email.", () => pe.Email = r.GetString(4));
                A("Não foi possível obter contato1.", () => pe.Contato1 = r.GetString(5));
                A("Não foi possível obter contato2.", () => pe.Contato2 = r.GetString(6));
            });
            return pe;
        }

        public static List<Pessoas> SelectLike(string cpf, string nome) {
            var lista = new List<Pessoas>();
            var c = new MySqlCommand {
                CommandText =
                $"select * " +
                $"from {p.TableName} " +
                $"where " +
                $"  {nameof(CPF)} like concat(@cpf, '%') and " +
                $"  {nameof(Nome)} like concat(@nome, '%') " +
                $"limit 200;"
            };
            c.Parameters.AddWithValue("@cpf", cpf);
            c.Parameters.AddWithValue("@nome", nome);
            p.QueryRLoop("Erro ao obter registros de pessoas.", c, (r) => {
                lista.Add(new Pessoas() {
                    CPF = r.GetString(0),
                    Nome = r.GetString(1),
                    Nascimento = r.GetMySqlDateTime(2).GetDateTime(),
                    Sexo = r.GetBoolean(3),
                    Email = r.GetString(4),
                    Contato1 = r.GetString(5),
                    Contato2 = r.GetString(6)
                });
            });
            return lista;
        }

        public override Action GetCT() => () => {
            var q = $"create table if not exists {p.TableName} (" +
                $"   {nameof(CPF)} varchar(15) not null," +
                $"   {nameof(Nome)} nvarchar(100) not null," +
                $"   {nameof(Nascimento)} date not null," +
                $"   {nameof(Sexo)} bool not null," +
                $"   {nameof(Email)} nvarchar(50) not null," +
                $"   {nameof(Contato1)} varchar(25) not null," +
                $"   {nameof(Contato2)} varchar(25) not null," +
                $"   primary key ({nameof(CPF)})" +
                $");";
            p.NonQuery($"Erro ao criar a tabela {p.TableName}.", q);
            InsertOrUpdate("033.380.952-10", "Vinícius Gabriel dos Santos Velloso", DateTime.Now, true, "", "", "");
        };

        public static explicit operator string(Pessoas p) {
            string a = "";
            a += $"CPF: {p.CPF}\n";
            a += $"Nome: {p.Nome}\n";
            a += $"Nascimento: {p.Nascimento.ToString("dd-MM-yyyy HH:mm:ss")}\n";
            a += $"Email: {p.Email}\n";
            a += $"Contato1: {p.Contato1}\n";
            a += $"Contato2: {p.Contato2}\n";
            a += $"Sexo: {p.Sexo}\n";
            return a;
        }

    }
}
