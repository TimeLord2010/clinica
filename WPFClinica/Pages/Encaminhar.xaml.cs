using System;
using System.Collections.Generic;
using System.Data;
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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using WPFClinica.Windows;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class Encaminhar : Page {

        #region Vars

        TimedEvent TE;

        public string CPF {
            get => CPFTB.Text;
        }

        public string Nome {
            get => NomeTB.Text;
        }

        public string Selecao {
            get => SelecionadoTBL.Text;
            set => SelecionadoTBL.Text = value;
        }

        public bool Prioridade {
            get => PrioridadeChB.IsChecked ?? false;
        }

        public string Funcionario {
            get => FuncionarioTBL.Text;
        }

        public string CPF_Funcionario {
            get {
                if (Funcionario.Contains("|")) {
                    var medico = Funcionario.Replace(" ", "");
                    var parts = medico.Split('|');
                    return parts[0];
                } else {
                    return null;
                }
            }
        }

        public Fila Fila {
            get {
                switch (EncaminharParaCB.SelectedIndex) {
                    case 0:
                        return Fila.Triagem;
                    case 1:
                        return Fila.Laboratorio;
                    case 2:
                        return Fila.ExamesMedicos;
                    default:
                        throw new Exception("Fila não pode ter seu objeto correspondente.");
                }
            }
        }

        bool IsAso {
            get => AsoChB.IsChecked ?? false;
        }

        #endregion

        public Encaminhar() {
            InitializeComponent();
            App.Current.Properties["agendamento"] = null;
            try {
                Window W = App.Current.MainWindow;
                if (W.WindowState == WindowState.Normal) {
                    int a = 500;
                    W.MinHeight = a;
                    W.Height = a;
                    WindowsSO.ToCenterOfScreen(W);
                }
                EspecializacaoCB.Items.Clear();
                var esps = Especializacoes.SelectLike("");
                for (int i = 0; i < esps.Count; i++) {
                    EspecializacaoCB.Items.Add(new ComboBoxItem() {
                        Content = esps[i].Especializacao
                    });
                }
                ControlsH.CreateColumns(PesquisaDG, typeof(Table1));
                PesquisaDG.Columns.ElementAt(1).Width = new DataGridLength(2, DataGridLengthUnitType.Star);
                TE = new TimedEvent(1, 2, true, () => {
                    var r = Pacientes.SelectLike(CPF, Nome);
                    SelecionadoTBL.Text = "Nenhum";
                    PesquisaDG.Items.Clear();
                    for (int i = 0; i < r.Count; i++) {
                        var row = r[i];
                        var t1 = new Table1 {
                            CPF = row.CPF,
                            Nome = row.Nome,
                            Sexo = row.Sexo ? "M" : "F",
                            Idade = row.Idade + ""
                        };
                        PesquisaDG.Items.Add(t1);
                    };
                });
                TE.TriggerNow();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e) {
            var c = new CadastrarPaciente();
            c.ShowDialog();
            TE.TriggerNow();
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            TE.TryTigger();
        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            TE.TryTigger();
        }

        private void PesquisaDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (PesquisaDG.SelectedItem is Table1 table1) {
                SelecionadoTBL.Text = $"{table1.CPF} | {table1.Nome}";
            }
        }

        private void EncaminharB_Click(object sender, RoutedEventArgs e) {
            try {
                if (EspecializacaoCB.SelectedIndex < 0 && Fila == Fila.Triagem && !Funcionario.Contains("|")) {
                    MessageBox.Show("Selecione uma especialização.");
                    return;
                }
                if (!Selecao.Contains("|")) {
                    MessageBox.Show("Selecione um paciente.");
                    return;
                }
                var parts = Selecao.Replace(" | ", "|").Split('|');
                var cpf = parts[0];
                if (Fila == Fila.Laboratorio) {
                    Scripts.DB.ListaEspera.Delete(cpf);
                } else if (Scripts.DB.ListaEspera.Select_Paciente(cpf) != null) {
                    var r = MessageBox.Show($"Deseja excluí-lo da lista?\nCUIDADO: Essa ação fará com que exames e procedimentos pendentes anexados ao paciente sejam excluídos!",
                        $"O paciente já está na lista de espera.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (r == MessageBoxResult.No) {
                        return;
                    } else {
                        Scripts.DB.ListaEspera.Delete(cpf);
                    }
                }
                if (Fila == Fila.Laboratorio) {
                    var a = new EncaminharLab(cpf, Prioridade);
                    a.ShowDialog();
                } else if (Fila == Fila.Triagem) {
                    if (!DealWithASO(cpf)) return;
                    string esp = null;
                    if (EspecializacaoCB.SelectedIndex >= 0) esp = (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString();
                    App.Current.Properties["id"] = null;
                    var a = new PagarConsulta(cpf, CPF_Funcionario, esp);
                    if (!a.ShowDialog() ?? false) {
                        return;
                    }
                    Scripts.DB.ListaEspera.Insert(cpf, PStatus.Esperando, Fila, Prioridade);
                    var le = Scripts.DB.ListaEspera.Select_Paciente(cpf);
                    if (le == null) {
                        MessageBox.Show("Não foi possível obter senha de lista de espera.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (esp != null) ListaEspera_Especializacao.Insert(le.Senha, esp);
                    if (Funcionario.Contains("|")) {
                        ListaEspera_Funcionario.Insert(le.Senha, CPF_Funcionario);
                    }
                    if (App.Current.Properties["id"] is int id) {
                        ListaEspera_Agendamento.Insert(le.Senha, id);
                    }
                } else if (Fila == Fila.ExamesMedicos) {
                    if (App.Current.Properties["frame"] is Frame frame1) {
                        frame1.Content = new EncaminharExames(cpf, Prioridade);
                    }
                    return;
                }
                if (App.Current.Properties["frame"] is Frame frame) frame.Content = new BlankPage();
            } catch (Exception ex) {
                ErrorHandler.OnError(ex);
            }
        }

        /// <summary>
        /// Handles ASO operation.
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns>Successful or not</returns>
        bool DealWithASO(string cpf) {
            if (IsAso) {
                DateTime notificaEm = DateTime.Now;
                var r = MessageBox.Show(
                    "Deseja informar a empresa do paciente?\nEste passo é opcional, é possível encaminhar o paciente sem fornecer a empresa",
                    "Fornecer empresa do paciente?",
                    MessageBoxButton.YesNoCancel,
                    MessageBoxImage.Question);
                if (r == MessageBoxResult.Cancel) {
                    return false;
                }
                if (r == MessageBoxResult.Yes) {
                    var w = new Window() {
                        WindowStartupLocation = WindowStartupLocation.CenterScreen,
                        Width = 400,
                        Height = 400
                    };
                    w.Content = new EscolherEmpresa(true);
                    w.ShowDialog();
                }
                var cm = new LembrarEm();
                cm.ShowDialog();
                if (App.Current.Properties["meses"] is int months) {
                    notificaEm = notificaEm.AddMonths(months);
                } else {
                    return false;
                }
                if (App.Current.Properties["id"] is int empresaID) {
                    Asos.Insert(cpf, empresaID, notificaEm);
                } else {
                    Asos.Insert(cpf, 1, notificaEm);
                }
            }
            return true;
        }

        private void DeAgendamentosB_Click(object sender, RoutedEventArgs e) {
            App.Current.Properties["agendamento"] = null;
            var a = new PesquisarAgenda(ChooseMode.ChooseCreated);
            a.ShowDialog();
            if (App.Current.Properties["agendamento"] is int id) {
                var r = AgendamentoTempPaciente.Select(id);
                if (r != null) {
                    var contato = TempPacientes.Select(r.TempPaciente);
                    var c = new CadastrarPaciente {
                        Nome = r.TempPaciente,
                        Contato1 = contato,
                        OriginalName = r.TempPaciente
                    };
                    c.ShowDialog();
                } else {
                    var agendamento = AgendamentoPaciente.Select(id);
                    var nome = Pessoas.Select(agendamento.Paciente).Nome;
                    if (agendamento == null) {
                        throw new Exception("Nenhum paciente encontrado para o agendamento.");
                    } else {
                        SelecionadoTBL.Text = $"{agendamento.Paciente} | {nome}";
                    }
                }
            }
        }

        class Table1 {

            public string CPF { get; set; }
            public string Nome { get; set; }
            public string Idade { get; set; }
            public string Sexo { get; set; }

        }

        private void EscolherB_Click(object sender, RoutedEventArgs e) {
            var a = new EscolherFuncionario();
            a.ShowDialog();
            if (App.Current.Properties["funcionario"] is string cpf) {
                FuncionarioTBL.Text = $"{cpf} | {Pessoas.Select(cpf).Nome}";
            } else {
                FuncionarioTBL.Text = "???";
            }
        }

        private void ClinicaGeralChB_Checked(object sender, RoutedEventArgs e) {
            EspecializacaoCB.SelectedIndex = -1;
            EspecializacaoCB.IsEnabled = false;
        }

        private void ClinicaGeralChB_Unchecked(object sender, RoutedEventArgs e) {
            EspecializacaoCB.IsEnabled = true;
        }
    }
}
