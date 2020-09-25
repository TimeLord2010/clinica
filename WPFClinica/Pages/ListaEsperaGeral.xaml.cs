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
using System.IO;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using System.Diagnostics;
using System.Threading;
using static System.Convert;
using WPFClinica.Windows;

namespace WPFClinica.Pages {

    public partial class ListaEsperaGeral : Page {

        #region Vars

        TimedEvent TimedEvent;
        string CPF_Funcionario { get; set; }
        int senha = -1;
        int Senha {
            get => senha;
            set {
                ChamarProximoB.IsEnabled = ConfirmarSaidaB.IsEnabled = AbrirProntuarioB.IsEnabled = PacientesDG.SelectedIndex >= 0;
                if (value >= 0) {
                    senha = value;
                    if (IsTriagem) {
                        Scripts.DB.ListaEspera.Update(value, Fila.Triagem, PStatus.ACaminho);
                    } else {
                        Scripts.DB.ListaEspera.Update(value, Fila.Consultorio, PStatus.ACaminho);
                        var l = ListaEspera_Agendamento.Select_Senha(value);
                        if (l.HasValue) {
                            Historico_Consultas.Update_Profissão(l.Value, Profissao);
                        } else {
                            MessageBox.Show(
                                "Não foi possível encontrar identificador de agendamento.",
                                $"Erro. ({ErrorCodes.E0004})",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                    }
                    Paciente_Sala.InsertOrUpdate(CPF_Funcionario, CPF_Paciente);
                    NomePacienteTBL.Text = Pessoas.Select(CPF_Paciente).Nome;
                    AbrirProntuarioB.IsEnabled = ConfirmarSaidaB.IsEnabled = true;
                } else {
                    if (IsTriagem) {
                        Scripts.DB.ListaEspera.Update(senha, Fila.Consultorio, PStatus.Esperando);
                        Paciente_Sala.DeletePaciente(CPF_Paciente);
                    } else {
                        Scripts.DB.ListaEspera.Delete(senha);
                    }
                    Salas.Update(CPF_Funcionario, Sala, SStatus.Livre, IsTriagem ? Fila.Triagem : Fila.Consultorio);
                    if (File.Exists(ProntuarioFileName)) File.Delete(ProntuarioFileName);
                    AtualizarProntuarioB.IsEnabled = AbrirProntuarioB.IsEnabled = false;
                    NomePacienteTBL.Text = "???";
                }
                senha = value;
            }
        }
        string CPF_Paciente {
            get {
                var r = Scripts.DB.ListaEspera.Select(senha);
                return r.Paciente;
            }
        }

        const string ProntuarioFileName = "Prontuário.docx";

        string NomeSala {
            get => NomeSalaTB.Text;
            set => NomeSalaTB.Text = value;
        }

        string NomePaciente {
            get => NomePacienteTBL.Text;
            set => NomePacienteTBL.Text = value;
        }

        SStatus Status {
            get {
                if (NomePaciente == "???") {
                    return SStatus.Ocupado;
                } else {
                    return SStatus.EmAtendimento;
                }
            }
        }

        string Sala {
            get {
                var r = Salas.Select(CPF_Funcionario);
                if (r != null) {
                    CrieSalaTBL.Visibility = Visibility.Hidden;
                    return r.Value.Nome;
                } else {
                    CrieSalaTBL.Visibility = Visibility.Visible;
                    MyGrid.IsEnabled = false;
                    return null;
                }
            }
            set {
                if (value.Length == 0) {
                    MessageBox.Show("Nome da sala vazio.");
                    return;
                }
                Salas.InsertOrUpdate(CPF_Funcionario, value, Status, IsTriagem ? Fila.Triagem : Fila.Consultorio);
                CrieSalaTBL.Visibility = Visibility.Hidden;
                MyGrid.IsEnabled = true;
            }
        }

        bool IsTriagem { get; set; }

        Profissao Profissao { get; set; }

        #endregion

        public ListaEsperaGeral(string funcionario, bool triagem, Profissao profissao) {
            InitializeComponent();
            CPF_Funcionario = funcionario;
            NomeSala = Sala ?? "";
            IsTriagem = triagem;
            Profissao = profissao;
            ControlsH.CreateColumns(PacientesDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 10, false, UpdatePacientes);
            AtivarLayout();
        }

        ~ListaEsperaGeral() {
            try {
                if (File.Exists(ProntuarioFileName)) File.Delete(ProntuarioFileName);
            } catch (Exception) {
                MessageBox.Show("Erro ao excluir prontuário.");
            }
        }

        void AtivarLayout() {
            TimedEvent.Start();
            var ps = Paciente_Sala.Select_Funcionario(CPF_Funcionario);
            if (ps != null) {
                if (CPF_Funcionario == ps.Funcionario) {
                    var le = Scripts.DB.ListaEspera.Select_Paciente(ps.Paciente);
                    if (le != null) Senha = le.Senha;
                }
            }
        }

        void UpdatePacientes() {
            PacientesDG.Items.Clear();
            if (IsTriagem) {
                var r = Scripts.DB.ListaEspera.Select(Fila.Triagem, PStatus.Esperando);
                for (int i = 0; i < r.Count; i++) {
                    var row = r[i];
                    PacientesDG.Items.Add(new Table1() {
                        Senha = row.Senha,
                        Nome = row.Nome,
                        Atualizacao = row.Atualizacao.ToString("dd-MM-yyyy HH:mm"),
                        Prioridade = row.Prioridade ? "Sim" : "Não",
                        Idade = Data.Age(row.Nascimento_Paciente) + " anos"
                    });
                }
            } else {
                var r = Scripts.DB.ListaEspera.Select2(CPF_Funcionario);
                for (int i = 0; i < r.Count; i++) {
                    var row = r[i];
                    PacientesDG.Items.Add(new Table1() {
                        Senha = row.Senha,
                        Nome = row.Nome,
                        Atualizacao = row.Atualizacao.ToString("dd-MM-yyyy HH:mm"),
                        Prioridade = row.Prioridade ? "Sim" : "Não",
                        Idade = Data.Age(row.Nascimento_Paciente) + " anos"
                    });
                }
            }
        }

        private void SalvarB_Click(object sender, RoutedEventArgs e) {
            Sala = NomeSala;
            AtivarLayout();
        }

        class Table1 {

            public int Senha { get; set; }
            public string Nome { get; set; }
            public string Atualizacao { get; set; }
            public string Prioridade { get; set; }
            public string Idade { get; set; }

        }

        private void PacientesDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ChamarProximoB.IsEnabled = PacientesDG.SelectedIndex >= 0;
        }

        private void AbrirProntuarioB_Click(object sender, RoutedEventArgs e) {
            var tp = new EscolherTipoProntuário();
            var r = tp.ShowDialog();
            if (!r.HasValue) {
                return;
            }
            Scripts.DB.ListaEspera.Update(Senha, IsTriagem ? Fila.Triagem : Fila.Consultorio, PStatus.EmAtendimento);
            if (r.Value) {
                var p = new Prontuario(CPF_Paciente);
                p.Closed += new EventHandler((se,ev) => {
                    AtualizadoTBL.Visibility = Visibility.Visible;
                });
                p.ShowDialog();
            } else {
                Prontuarios.SelectOrCreate(CPF_Paciente, out byte[] fd);
                using (var s = new FileStream(ProntuarioFileName, FileMode.Create, FileAccess.Write)) {
                    s.Write(fd, 0, fd.Length);
                }
                Process.Start(ProntuarioFileName);
                AtualizarProntuarioB.IsEnabled = true;
            }
        }

        private void AtualizarProntuarioB_Click(object sender, RoutedEventArgs e) {
            AtualizadoTBL.Visibility = Visibility.Visible;
            try {
                Prontuarios.Update(CPF_Paciente, new FileInfo(ProntuarioFileName));
                AtualizadoTBL.Text = "Atualizado!";
            } catch (UnauthorizedAccessException) {
                MessageBox.Show("Feche o programa de edição de texto para atualizar o documento na base de dados.");
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro ao atualiar prontuário");
            }
            Thread t = new Thread(() => {
                Thread.Sleep(4000);
                App.Current.Dispatcher.Invoke(() => {
                    AtualizadoTBL.Visibility = Visibility.Hidden;
                });
            });
            t.Start();
        }

        private void ChamarB_Click(object sender, RoutedEventArgs e) {
            if (PacientesDG.SelectedItem is Table1 table1) {
                Senha = table1.Senha;
                UpdatePacientes();
            }
        }

        private void ConfirmarSaidaB_Click(object sender, RoutedEventArgs e) {
            Senha = -1;
        }

        private void AtualizarB_Click(object sender, RoutedEventArgs e) {
            UpdatePacientes();
        }
    }
}
