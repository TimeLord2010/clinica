using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClinica.Scripts.DB {

    class ProntuariosText : Template {

        static readonly ProntuariosText pt = new ProntuariosText();
        public override string TableName => nameof(ProntuariosText);

        static readonly string ColumnValues =
            $"  pressaoArterial = @pa, " +
            $"  glicemia = @g," +
            $"  perimetro_comprimento = @p_c," +
            $"  perimetro_altura = @p_a," +
            $"  peso = @p," +
            $"  data_consulta = @d_c," +
            $"  queixas = @q," +
            $"  historico_doenca = @h_d," +
            $"  medicacoes_uso = @m_u," +
            $"  historico_familia = @h_f," +
            $"  exame_fisico = @e_f," +
            $"  exames = @e," +
            $"  hipotese_diagnostica = @hp_d," +
            $"  conduta = @c ";

        public override Action GetCT() {
            return () => {
                pt.NonQuery("Erro ao criar prontuários-texto.",
                    $"create table if not exists {TableName} (" +
                    $"  cpf varchar(15)," +
                    $"  pressaoArterial nvarchar(100)," +
                    $"  glicemia nvarchar(100)," +
                    $"  perimetro_comprimento nvarchar(100)," +
                    $"  perimetro_altura nvarchar(100)," +
                    $"  peso nvarchar(100)," +
                    $"  data_consulta datetime," +
                    $"  queixas longtext," +
                    $"  historico_doenca longtext," +
                    $"  medicacoes_uso longtext," +
                    $"  historico_familia longtext," +
                    $"  exame_fisico longtext," +
                    $"  exames longtext," +
                    $"  hipotese_diagnostica longtext," +
                    $"  conduta longtext," +
                    $"	primary key (cpf)," +
                    $"	foreign key (cpf) references pessoas (cpf) on update cascade" +
                    $");");
            };
        }

        public static void Insert(_ProntuariosText pts) {
            var c = FillCommand(pts, $"insert into {pt.TableName} values (@cpf, @pa, @g, @p_c, @p_a, @p, @d_c, @q, @h_d, @m_u, @h_f, @e_f, @e, @hp_d, @c)" +
                $"on duplicate key update {ColumnValues};");
            pt.NonQuery("Erro ao inserir prontuario-t.", c);
        }

        public static void Delete(string cpf) {
            var c = new MySqlCommand($"delete from {pt.TableName} where cpf = @cpf;");
            c.Parameters.AddWithValue("@cpf", cpf);
            pt.NonQuery("Erro ao deletar prontuario-t.", c);
        }

        public static void Update(string cpf, _ProntuariosText pts) {
            pts.CPF = cpf;
            var c = FillCommand(pts, $"update {pt.TableName} set " + 
                ColumnValues +
                $"where " +
                $"  cpf = @cpf;");
            pt.NonQuery("Erro ao atualizar prontuarios-t.", c);
        }

        public static _ProntuariosText Select(string cpf) {
            var c = new MySqlCommand($"select * from {pt.TableName} where cpf = @cpf;");
            c.Parameters.AddWithValue("@cpf", cpf);
            var r = GetPT(c);
            if (r.CPF == null) r.CPF = cpf;
            return r;
        }

        static MySqlCommand FillCommand(_ProntuariosText pts, string q = null) {
            var c = new MySqlCommand(q ?? "");
            c.Parameters.AddWithValue("@cpf", pts.CPF);
            c.Parameters.AddWithValue("@pa", pts.PressaoArterial);
            c.Parameters.AddWithValue("@g", pts.Glicemia);
            c.Parameters.AddWithValue("@p_c", pts.Perimetro_Comprimento);
            c.Parameters.AddWithValue("@p_a", pts.Perimetro_Altura);
            c.Parameters.AddWithValue("@p", pts.Peso);
            c.Parameters.AddWithValue("@d_c", Converter.Converter_DT(pts.DataConsulta).ToString("yyyy-MM-dd HH:mm:ss"));
            c.Parameters.AddWithValue("@q", pts.Queixas);
            c.Parameters.AddWithValue("@h_d", pts.HistoricoDoenca);
            c.Parameters.AddWithValue("@m_u", pts.MedicacoesUso);
            c.Parameters.AddWithValue("@h_f", pts.HistoricoFamilia);
            c.Parameters.AddWithValue("@e_f", pts.ExameFisico);
            c.Parameters.AddWithValue("@e", pts.Exames);
            c.Parameters.AddWithValue("@hp_d", pts.HipoteseDiagnostica);
            c.Parameters.AddWithValue("@c", pts.Conduta);
            return c;
        }

        static _ProntuariosText GetPT(MySqlCommand c) {
            var a = _ProntuariosText.Default;
            pt.QueryRLoop("Erro ao obter prontuario-t.", c, (r) => {
                a = new _ProntuariosText() {
                    CPF = r.GetString(0),
                    PressaoArterial = r.GetString(1),
                    Glicemia = r.GetString(2),
                    Perimetro_Comprimento = r.GetString(3),
                    Perimetro_Altura = r.GetString(4),
                    Peso = r.GetString(5),
                    DataConsulta = r.GetMySqlDateTime(6).GetDateTime().ToString("dd/MM/yyyy"),
                    Queixas = r.GetString(7),
                    HistoricoDoenca = r.GetString(8),
                    MedicacoesUso = r.GetString(9),
                    HistoricoFamilia = r.GetString(10),
                    ExameFisico = r.GetString(11),
                    Exames = r.GetString(12),
                    HipoteseDiagnostica = r.GetString(13),
                    Conduta = r.GetString(14)
                };
            });
            return a;
        }

    }

    public struct _ProntuariosText {
        public int ID;
        public string CPF;
        public string PressaoArterial;
        public string Glicemia;
        public string Perimetro_Comprimento;
        public string Perimetro_Altura;
        public string Peso;
        public string DataConsulta;
        public string Queixas;
        public string HistoricoDoenca;
        public string MedicacoesUso;
        public string HistoricoFamilia;
        public string ExameFisico;
        public string Exames;
        public string HipoteseDiagnostica;
        public string Conduta;

        public static _ProntuariosText Default {
            get => new _ProntuariosText() {
                ID = -1,
                CPF = null,
                DataConsulta = DateTime.Now.ToString("dd/MM/yyyy")
            };
        }

    }

}
