using System.Windows;
using System.Windows.Input;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class Prontuario : Window {

        public Prontuario(string cpf) {
            InitializeComponent();
            CPF = cpf;
            FillForm();
        }

        #region Properties

        private string CPF { get; }

        string PressaoArterial {
            get => PressaoArterialTB.Text;
            set => PressaoArterialTB.Text = value;
        }

        string Glicemia {
            get => GlicemiaTB.Text;
            set => GlicemiaTB.Text = value;
        }

        string Comprimento {
            get => ComprimentoTB.Text;
            set => ComprimentoTB.Text = value;
        }

        string Altura {
            get => AlturaTB.Text;
            set => AlturaTB.Text = value;
        }

        string Peso {
            get => PesoTB.Text;
            set => PesoTB.Text = value;
        }

        string Nome {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        string Idade {
            get => IdadeTB.Text;
            set => IdadeTB.Text = value;
        }

        string Data {
            get => DataTB.Text;
            set => DataTB.Text = value;
        }

        string Queixas {
            get => QueixasTB.Text;
            set => QueixasTB.Text = value;
        }

        string HistoriaDoenca {
            get => HistoriaDoencaTB.Text;
            set => HistoriaDoencaTB.Text = value;
        }

        string Medicacoes {
            get => MedicacoesUsoTB.Text;
            set => MedicacoesUsoTB.Text = value;
        }

        string HistoricoFamiliar {
            get => HistoricoFamiliarTB.Text;
            set => HistoricoFamiliarTB.Text = value;
        }

        string ExameFisico {
            get => ExameFisicoTB.Text;
            set => ExameFisicoTB.Text = value;
        }

        string Exames {
            get => ExamesTB.Text;
            set => ExamesTB.Text = value;
        }

        string HipotesesDiagnosticas {
            get => HipotesesTB.Text;
            set => HipotesesTB.Text = value;
        }

        string Conduta {
            get => CondutaTB.Text;
            set => CondutaTB.Text = value;
        }

        #endregion

        public void FillForm() {
            var r = ProntuariosText.Select(CPF);
            PressaoArterial = r.PressaoArterial;
            Glicemia = r.Glicemia;
            Comprimento = r.Perimetro_Comprimento;
            Altura = r.Perimetro_Altura;
            Peso = r.Peso;
            var p = Pessoas.Select(r.CPF);
            if (p != null) {
                Nome = p.Nome;
                Idade = p.Idade + " anos";
            }
            Data = r.DataConsulta;
            Queixas = r.Queixas;
            HistoriaDoenca = r.HistoricoDoenca;
            Medicacoes = r.MedicacoesUso;
            HistoricoFamiliar = r.HistoricoFamilia;
            ExameFisico = r.ExameFisico;
            Exames = r.Exames;
            HipotesesDiagnosticas = r.HipoteseDiagnostica;
            Conduta = r.Conduta;
        }

        void Save () {
            var r = new _ProntuariosText() {
                CPF = CPF,
                PressaoArterial = PressaoArterial,
                Glicemia = Glicemia,
                Perimetro_Comprimento = Comprimento,
                Perimetro_Altura = Altura,
                Peso = Peso,
                DataConsulta = Data,
                Queixas = Queixas,
                HistoricoDoenca = HistoriaDoenca,
                MedicacoesUso = Medicacoes,
                HistoricoFamilia = HistoricoFamiliar,
                ExameFisico = ExameFisico,
                Exames = Exames,
                HipoteseDiagnostica = HipotesesDiagnosticas,
                Conduta = Conduta
            };
            ProntuariosText.Insert(r);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Cursor = Cursors.Wait;
            Save();
        }
    }
}
