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
using MySql.Data.MySqlClient;
using WPFClinica.Windows;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class ListaEspera : Page {

        #region Vars

        Window W;
        TimedEvent TimedEvent;

        string CPF {
            get => CPFTB.Text;
            set => CPFTB.Text = value;
        }

        string Nome {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        Fila? Encaminhado {
            get {
                if (EncaminhadoCB.SelectedIndex == 0) {
                    return null;
                } else {
                    var a = (EncaminhadoCB.SelectedItem as ComboBoxItem).Content.ToString();
                    return ES.GetFila(a);
                }
            }
        }

        PStatus? Status {
            get {
                if (TodosStatusRB.IsChecked ?? false) {
                    return null;
                } else if (EmAtendimentoRB.IsChecked ?? false) {
                    return PStatus.EmAtendimento;
                } else if (AcaminhoRB.IsChecked ?? false) {
                    return PStatus.ACaminho;
                } else if (EsperandoRB.IsChecked ?? false) {
                    return PStatus.Esperando;
                } else {
                    return PStatus.Atendido;
                }
            }
        }

        bool? Prioridade {
            get {
                if (TodosPrioridadeRB.IsChecked ?? false) {
                    return null;
                } else if (SimPriridadeRB.IsChecked ?? false) {
                    return true;
                } else {
                    return false;
                }
            }
        }

        #endregion

        public ListaEspera() {
            InitializeComponent();
            W = App.Current.MainWindow;
            if (W.WindowState == WindowState.Normal) {
                W.Height = 600;
                WindowsSO.ToCenterOfScreen(W);
            }
            ControlsH.CreateColumns(PesquisaDG, typeof(Table1));
            TimedEvent = new TimedEvent(15, 2, false, UpdatePesquisa);
            TimedEvent.TriggerNow();
        }

        public void UpdatePesquisa() {
            PesquisaDG.Items.Clear();
            var r = Scripts.DB.ListaEspera.SelectLike(CPF, Nome, Prioridade, Encaminhado, Status);
            for (int i = 0; i < r.Count; i++) {
                var lista = r[i];
                var pessoa = Pessoas.Select(lista.Paciente);
                var t = new Table1() {
                    Senha = lista.Senha,
                    Encaminhado = ES.f[lista.Fila],
                    Status = EnumStrings.Get(lista._Status),
                    Prioridade = lista.Prioridade ? "✓" : "✗",
                    Horario = lista._DateTime.ToString("dd/MM/yyyy HH:mm"),
                    Nome = pessoa.Nome
                };
                PesquisaDG.Items.Add(t);
            }
        }

        DataGridTextColumn CreateColumn(string header, string binding = null) {
            var c = new DataGridTextColumn();
            c.Header = header;
            c.Binding = new Binding(binding ?? header);
            PesquisaDG.Columns.Add(c);
            return c;
        }

        void CreateColumns(params string[] headers) {
            for (int i = 0; i < headers.Length; i++) {
                CreateColumn(headers[i]);
            }
        }

        private void PesquisarB_Click(object sender, RoutedEventArgs e) {
            UpdatePesquisa();
        }

        private void PesquisaDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = DeletarB.IsEnabled = PesquisaDG.SelectedIndex >= 0;
        }

        private void EditarB_Click(object sender, RoutedEventArgs ev) {
            try {
                if (PesquisaDG.SelectedItem is Table1 table) {
                    var lista = Scripts.DB.ListaEspera.Select(table.Senha);
                    if (lista != null) {
                        var e = new EditarAtendimento(table.Senha) {
                            Owner = App.Current.MainWindow
                        };
                        e.ShowDialog();
                        UpdatePesquisa();
                    } else {
                        MessageBox.Show("Ocorreu um erro.");
                    }
                }
            } catch (Exception ex) {
                ErrorHandler.OnError(ex);
            }
        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void DeletarB_Click(object sender, RoutedEventArgs e) {
            try {
                if (PesquisaDG.SelectedItem is Table1 table) {
                    Scripts.DB.ListaEspera.Delete(table.Senha);
                    UpdatePesquisa();
                }
            } catch (Exception ex) {
                ErrorHandler.OnError(ex);
            }
        }

        class Table1 {

            public int Senha { get; set; }
            public string Nome { get; set; }
            public string Encaminhado { get; set; }
            public string Status { get; set; }
            public string Horario { get; set; }
            public string Prioridade { get; set; }

        }

    }
}
