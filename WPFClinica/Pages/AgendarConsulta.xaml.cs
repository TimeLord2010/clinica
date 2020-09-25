using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFClinica.Windows;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static Data;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class AgendarConsulta : Page {

        #region Vars

        TimedEvent P, F;

        string Paciente {
            get => NomeTB.Text;
        }

        string Contato {
            get => ContatoTB.Text;
        }

        string CPFPaciente {
            get => CPFPacienteTB.Text;
            set => CPFPacienteTB.Text = value;
        }

        string CPFMedico {
            get => CPFMedicoTBL.Text == "???" ? null : CPFMedicoTBL.Text;
        }

        string NomeMedico {
            get => NomeMedicoTB.Text;
        }

        string Especializacao {
            get => (EspecializacoesCB.SelectedItem as ComboBoxItem).Content.ToString();
        }

        #endregion

        public AgendarConsulta() {
            InitializeComponent();
            Window w = App.Current.MainWindow;
            if (w.WindowState == WindowState.Normal) {
                int h = 450;
                w.MinHeight = h;
                w.Height = h;
            }
            EspecializacoesCB.Items.Add(new ComboBoxItem() { Content = "[Todos]", FontWeight = FontWeights.SemiBold });
            Especializacoes.Fill(EspecializacoesCB);
            EspecializacoesCB.SelectedIndex = 0;
            ControlsH.CreateColumns(PacientesDG, typeof(Paciente_Table));
            ControlsH.CreateColumns(MedicosDG, typeof(Medicos_Table));
            P = new TimedEvent(1, 2, true, UpdatePacientes);
            P.TriggerNow();
            F = new TimedEvent(1, 2, true, UpdateFuncionarios);
            F.TriggerNow();
        }

        private void CadastrarB_Click(object sender, RoutedEventArgs e) {
            App.Current.Properties["temp_paciente"] = null;
            var a = new EscolherCadastroParcial();
            a.ShowDialog();
            if (App.Current.Properties["temp_paciente"] is TempPacientes paciente) {
                PacienteCPFTBL.Text = "???";
                NomePacienteTBL.Text = paciente.Nome;
            }
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            P.TryTigger();
        }

        private void ContatoTB_TextChanged(object sender, TextChangedEventArgs e) {
            P.TryTigger();
        }

        public void UpdatePacientes() {
            PacientesDG.Items.Clear();
            var r = Pacientes.SelectLike(CPFPaciente, Paciente);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                PacientesDG.Items.Add(new Paciente_Table() {
                    CPF = row.CPF,
                    Nome = row.Nome
                });
            }
        }

        public void UpdateFuncionarios() {
            MedicosDG.Items.Clear();
            var e = Especializacao == "[Todos]" ? "" : Especializacao;
            var r = Medicos.SelectLike("", NomeMedico, e);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                MedicosDG.Items.Add(new Medicos_Table() {
                    CPF = row.CPF,
                    Nome = row.Nome,
                    Especialização = row.Especializacao
                });
            }
        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            P.TryTigger();
        }

        private void CPFMedicoTB_TextChanged(object sender, TextChangedEventArgs e) {
            F.TryTigger();
        }

        private void NomeMedicoTB_TextChanged(object sender, TextChangedEventArgs e) {
            F.TryTigger();
        }

        private void EspecializacoesCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (F != null) F.TryTigger();
        }

        private void MedicosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (MedicosDG.SelectedItem is Medicos_Table p) {
                CPFMedicoTBL.Text = p.CPF;
                NomeMedicoTBL.Text = p.Nome;
                EspecializacaoTBL.Text = p.Especialização;
                F.Stop();
            }
        }

        private void SelecionarHorarioB_Click(object sender, RoutedEventArgs e) {
            var w = new PesquisarAgenda(ChooseMode.ChooseCreate, CPFMedico);
            w.Owner = App.Current.MainWindow;
            w.ShowDialog();
            if (App.Current.Properties["data"] is DateTime dt && App.Current.Properties["horario"] is string horario) {
                DataTBL.Text = $"{dt.Day:00}/{dt.Month:00}/{dt.Year:0000}";
                HorarioTBL.Text = horario;
            }
        }

        private void AgendarB_Click(object sender, RoutedEventArgs e) {
            int id = -1;
            try {
                if (Any((TextBlock tb) => tb.Text == "???", NomePacienteTBL, DataTBL, HorarioTBL)) {
                    MessageBox.Show("Todos os campos de 'Seleção' precisam ser preenchidos (exceto cpf, em certos casos).");
                    return;
                }
                var dparts = DataTBL.Text.Split('/');
                var day = ToInt32(dparts[0]);
                var mes = ToInt32(dparts[1]);
                var year = ToInt32(dparts[2]);
                var hparts = HorarioTBL.Text.Split('-');
                var h1 = hparts[0].Split(':');
                var hora1 = ToInt32(h1[0]);
                var minuto1 = ToInt32(h1[1]);
                var h2 = hparts[1].Split(':');
                var hora2 = ToInt32(h2[0]);
                var minuto2 = ToInt32(h2[1]);
                var dt = new DateTime(year, mes, day);
                var begin = new TimeSpan(hora1, minuto1, 0);
                var end = new TimeSpan(hora2, minuto2, 0);
                Agendamentos.Insert(dt, begin, end);
                var result = Agendamentos.Select(dt, begin, end);
                if (result == null) {
                    MessageBox.Show("Erro ao achar identificador de agendamento");
                    return;
                }
                id = result.Value;
                if (PacienteCPFTBL.Text == "???") {
                    AgendamentoTempPaciente.Insert(id, NomePacienteTBL.Text);
                } else {
                    AgendamentoPaciente.Insert(id, PacienteCPFTBL.Text);
                }
                if (CPFMedicoTBL.Text != "???") AgendamentoFuncionario.Insert(id, CPFMedicoTBL.Text);
                MessageBox.Show("Agendado");
            } catch (Exception ex) {
                if (id >= 0) Agendamentos.Delete(id);
                MessageBox.Show($"Classe: {ex.GetType()}\nMessagem: {ex.Message}", "Erro");
            }
        }

        private void PacientesDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (PacientesDG.SelectedItem is Paciente_Table row) {
                PacienteCPFTBL.Text = row.CPF;
                NomePacienteTBL.Text = row.Nome;
                P.Stop();
            }
        }

        public class Medicos_Table {
            public string CPF;
            public string Nome { get; set; }
            public string Especialização { get; set; }
        }

        public class Paciente_Table {
            public string CPF { get; set; }
            public string Nome { get; set; }
        }

    }
}
