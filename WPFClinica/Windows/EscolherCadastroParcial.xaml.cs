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
using System.Windows.Shapes;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class EscolherCadastroParcial : Window {

        #region Vars 

        MySqlH SQL;
        TimedEvent TimedEvent;

        string Nome {
            get => NomeTB.Text;
        }

        string Contato {
            get => ContatoTB.Text;
        }

        #endregion

        public EscolherCadastroParcial() {
            InitializeComponent();
            SQL = App.Current.Properties["sql"] as MySqlH;
            TimedEvent = new TimedEvent(1, 3, true, UpdatePacientes);
            TimedEvent.TriggerNow();
            CreateColumns("Nome", "Contato");
        }

        void CreateColumns(params string[] headers) {
            for (int i = 0; i < headers.Length; i++) {
                CreateColumn(headers[i]);
            }
        }

        void CreateColumn(string header, string binding = null) {
            var c = new DataGridTextColumn();
            c.Header = header;
            c.Binding = new Binding(binding ?? header);
            c.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
            PacientesTempDG.Columns.Add(c);
        }

        void UpdatePacientes() {
            PacientesTempDG.Items.Clear();
            TempPacientes.Select(Nome, Contato).ForEach(x => {
                PacientesTempDG.Items.Add(new TempPacientes() {
                    Nome = x.Nome,
                    Contato = x.Contato
                });
            });
        }

        private void CriarB_Click(object sender, RoutedEventArgs e) {
            var r = MessageBox.Show($"Criar registro temporário de paciente?\nNome: {Nome}\nContato: {Contato}", "Confirmar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (r == MessageBoxResult.OK) {
                TempPacientes.Insert(Nome, Contato);
                TimedEvent.TriggerNow();
            }
        }

        private void SelecionarB_Click(object sender, RoutedEventArgs e) {
            if (PacientesTempDG.SelectedItem is TempPacientes paciente) {
                App.Current.Properties["temp_paciente"] = paciente;
                Close();
            }
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void ContatoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }
    }
}
