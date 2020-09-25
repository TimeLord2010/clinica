using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Globalization;
using WPFClinica.Windows;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class RegistroPacientes : Page {

        #region Vars

        TimedEvent TimedEvent;

        string CPF {
            get => CPFTB.Text;
        }

        string Nome {
            get => NomeTB.Text;
        }

        bool? Sexo {
            get {
                if (TodosRB.IsChecked ?? false) {
                    return null;
                } else if (MRB.IsChecked ?? false) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        DateTime Begin {
            get {
                try {
                    return DateTime.Today.AddYears(-ToInt32(IIdadeTB.Text));
                } catch (Exception) {
                    return new DateTime(1800, 1, 1);
                }
            }
        }

        DateTime End {
            get {
                try {
                    return DateTime.Today.AddYears(-ToInt32(FIdadeTB.Text));
                } catch (Exception) {
                    return DateTime.Today;
                }
            }
        }

        #endregion

        public RegistroPacientes() {
            InitializeComponent();
            ControlsH.CreateColumns(PacientesDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 1, true, UpdatePacientes);
            TimedEvent.TriggerNow();
        }

        void UpdatePacientes() {
            PacientesDG.Items.Clear();
            var r = Pacientes.SelectLike(CPF, Nome);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                if (Sexo != null) {
                    var s = Sexo.Value;
                    if (s && !row.Sexo) {
                        continue;
                    }
                    if (!s && row.Sexo) {
                        continue;
                    }
                }
                var dt = row.Nascimento;
                PacientesDG.Items.Add(new Table1() {
                    CPF = row.CPF,
                    Nome = row.Nome,
                    Nascimento = $"{dt.Day}/{dt.Month}/{dt.Year}",
                    Sexo = row.Sexo ? "M" : "F",
                    Email = row.Email,
                    Contato1 = row.Contato1,
                    Contato2 = row.Contato2,
                    Idade = Data.Age(dt) + " anos"
                });
            }
        }

        private void CadastrarB_Click(object sender, RoutedEventArgs e) {
            var a = new CadastrarPaciente();
            a.ShowDialog();
            TimedEvent.TriggerNow();
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            if (PacientesDG.SelectedItem is Table1 table1) {
                var a = new CadastrarPaciente(table1.CPF);
                a.ShowDialog();
                UpdatePacientes();
            } else {
                MessageBox.Show("Selecione um paciente.");
            }
        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void TodosRB_Checked(object sender, RoutedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void PacientesDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = PacientesDG.SelectedIndex >= 0;
        }

        class Table1 {

            public string CPF { get; set; }
            public string Nome { get; set; }
            public string Sexo { get; set; }
            public string Nascimento { get; set; }
            public string Idade { get; set; }
            public string Email { get; set; }
            public string Contato1 { get; set; }
            public string Contato2 { get; set; }

        }

    }
}