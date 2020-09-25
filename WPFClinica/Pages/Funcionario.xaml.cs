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
using WPFClinica.Windows;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using WPFClinica.UserControls;
using System.Threading;
//using Version = WPFClinica.Scripts.Version;
using System.Diagnostics;
using System.IO;

namespace WPFClinica.Pages {

    public partial class Funcionario : Page {

        #region Vars

        Administradores admin = new Administradores();
        Fonoaudiologos fono = new Fonoaudiologos();
        Dentistas dent = new Dentistas();

        Window W;
        string CPF;

        string sala1 {
            get => NomeTBL.Text;
            set => NomeTBL.Text = value;
        }

        string sala2 {
            get => NomeTBL2.Text;
            set => NomeTBL2.Text = value;
        }

        #endregion

        public Funcionario(string cpf) {
            try {
                InitializeComponent();
                CPF = cpf ?? throw new ArgumentNullException("CPF era nulo.");
                App.Current.Properties["cpf"] = CPF;
                W = App.Current.MainWindow;
                W.WindowStyle = WindowStyle.ThreeDBorderWindow;
                W.Icon = ImageH.ToBitmapImage(Properties.Resources.Logo);
                W.MinWidth = W.Width = 650;
                W.MinHeight = W.Height = 400;
                WindowsSO.ToCenterOfScreen(W);
                if (!Funcionarios.IsFuncionario(cpf)) throw new Exception("Funcionário não encontrado.");
                W.Title = Pessoas.Select(cpf).Nome;
                if (!admin.IsProfissional(cpf)) AdministradorMI.Visibility = Visibility.Collapsed;
                if (!Medicos.IsDoctor(cpf)) {
                    MedicoMI.Visibility = Visibility.Collapsed;
                } else {
                    var r = Salas.Select(cpf);
                    if (r != null) {
                        sala1 = r.Value.Nome;
                    } else {
                        sala1 = "[Sala sem nome]";
                    }
                }
                if (!Recepcionista.IsRecepcionista(cpf)) {
                    RecepcionistaMI.Visibility = Visibility.Collapsed;
                } else {
                    var a = new AlertaASO();
                    a.Show();
                }
                if (!Tecnico_Enfermagem.IsTecnico(cpf)) {
                    TecnicoEnfermagemMI.Visibility = Visibility.Collapsed;
                } else {
                    var r = Salas.Select(cpf);
                    if (r != null) {
                        sala2 = r.Value.Nome;
                    } else {
                        sala2 = "[Sala sem nome]";
                    }
                }
                if (!fono.IsProfissional(CPF) && !Psicologos.IsPsicologo(cpf) && !Nutricionistas.IsNutricionista(cpf) && !dent.IsProfissional(CPF)) {
                    ListaDeEsperaGeralMI.Visibility = Visibility.Collapsed;
                }
                App.Current.Properties["frame"] = MyFrame;
                MyFrame.Content = new BlankPage();
            } catch (Exception ex) {
                MessageBox.Show($"Classe: {ex.GetType()}\nMessagem: {ex.Message}", "Erro ao carregar pagina do funcionário.");
            }
        }

        private void RegistroMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new RegistroFuncionarios();
        }

        private void LogOutMI_Click(object sender, RoutedEventArgs e) {
            W.Content = new Login();
        }

        private void EncaminharMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new Encaminhar();
        }

        private void ListaEsperaMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new ListaEspera();
        }

        private void AgendarMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new AgendarConsulta();
        }

        private void AgendamentosMI_Click(object sender, RoutedEventArgs e) {
            var a = new PesquisarAgenda(ChooseMode.View);
            a.ShowDialog();
        }

        private void ListaEsperaMedicoMI_Click(object sender, RoutedEventArgs e) {
            Salas.InsertOrUpdate(CPF, sala1, SStatus.Ocupado, Fila.Consultorio);
            var a = new ListaEsperaMedico(CPF);
            a.ShowDialog();
        }

        private void RegistroPacientesMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new RegistroPacientes();
        }

        private void ListaPacientesMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new PacientesLab();
        }

        private void MudarNomeMI_Click(object sender, RoutedEventArgs e) {
            ObterTexto a = new ObterTexto("Nome da sala", "Ir");
            a.ShowDialog();
            if (App.Current.Properties["resposta"] is string resposta) {
                sala2 = sala1 = resposta;
                Salas.InsertOrUpdate(CPF, sala1, SStatus.Ocupado, Fila.Consultorio);
            }
        }

        private void MudarNomeMI2_Click(object sender, RoutedEventArgs e) {
            var a = new ObterTexto("Nome da sala", "Ir");
            a.ShowDialog();
            if (App.Current.Properties["resposta"] is string resposta) {
                sala2 = sala1 = resposta;
                Salas.InsertOrUpdate(CPF, sala2, SStatus.Ocupado, Fila.Laboratorio);
            }
        }

        private void ProcedimentosLabAdminMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new ProcedimentosLabAdmin();
        }

        private void FinalizarMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new FinalizarPaciente();
        }

        private void SalasMI_Click(object sender, RoutedEventArgs e) {

        }

        private void HistoricoMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new HistoricoConsultas();
        }

        private void ListaDeEsperaGeralMI_Click(object sender, RoutedEventArgs e) {
            var a = fono.IsProfissional(CPF);
            var b = Psicologos.IsPsicologo(CPF);
            var c = Nutricionistas.IsNutricionista(CPF);
            var isDent = dent.IsProfissional(CPF);
            var d = new List<bool> {
                a,
                b,
                c,
                isDent
            };
            if (d.Count(x => x) == 1) {
                Profissao profissao;
                if (a) {
                    profissao = Profissao.Fonoaudiologo;
                } else if (b) {
                    profissao = Profissao.Psicologo;
                } else if (c) {
                    profissao = Profissao.Nutricionista;
                } else {
                    profissao = Profissao.Dentista;
                }
                MyFrame.Content = new ListaEsperaGeral(CPF, false, profissao);
            } else {
                var f = new EscolherNaoMedico(CPF);
                f.ShowDialog();
                if (App.Current.Properties["profissao"] is Profissao profissao) {
                    MyFrame.Content = new ListaEsperaGeral(CPF, false, profissao);
                }
            }
        }

        private void TriagemMI_Click(object sender, RoutedEventArgs e) {
            var a = new ListaEsperaGeral(CPF, true, Profissao.TecnicoEnfermagem);
            MyFrame.Content = a;
        }

        private void ValorConsultaMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new ValoresPorConsulta();
        }

        private void PessoasMI_Click(object sender, RoutedEventArgs e) {

        }

        private void ProntuariosMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new PesquisarProntuarios();
        }

        private void AtendimentosMI_Click(object sender, RoutedEventArgs e) {

        }

        private void ProcedimentosLabMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new ProcedimentosLabAdmin();
        }

        private void FazerOrcamentoMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new FazerOrcamento();
        }

        private void ConveniosMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new ConveniosAdmin();
        }

        private void SalaMI_SubmenuOpened(object sender, RoutedEventArgs e) {
            sala1 = GetSala();
        }

        private void SalaMI2_SubmenuOpened(object sender, RoutedEventArgs e) {
            sala2 = GetSala();
        }

        private string GetSala() {
            try {
                var s = Salas.Select(CPF);
                if (s.HasValue) {
                    return s.Value.Nome;
                } else {
                    return "[Nulo]";
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, $"Erro ao obter nome da sala. ({ErrorCodes.E0001})");
            }
            return null;
        }

        private void HistoricoProcLabMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new HistoricoProcLab();
        }

        private void RegistroProcedimentosMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new ProcedimentosAdmin();
        }

        private void HistoricoProcedimentosMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new HistoricoProcsAdmin();
        }

        private void ConsultasMI_Click(object sender, RoutedEventArgs e) {

        }

        private void CheckUpdatesMI_Click(object sender, RoutedEventArgs e) {
            //try {
            /*Properties.Settings.Default.Version = "1.0.0";
            var h = new HistoricoVersoes();
            var versions = HistoricoVersoes.Select();
            var current = new Version(Properties.Settings.Default.Version);
            if (versions.ID < 0) {
                var version = new Version(versions.Versao);
                if (version.Compare(current) != 1) {
                    MessageBox.Show("A sua versão já é a mais recente.", "Atualização desnecessária", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                Properties.Settings.Default.Version = version.ToString();
            } else {
                MessageBox.Show("Não foi encontra uma versão mais recente.",
                    "Atualização desnecessária", MessageBoxButton.OK, MessageBoxImage.Information);
                HistoricoVersoes.Insert(current.ToString());
                return;
            }*/
            var r = MessageBox.Show(
                "Você está prestes a baixa uma pasta compactada (*.rar) com a versão mais nova do aplicativo. Deseja continuar?\n" +
                "Depois da confirmação, você será notificado quando o download acabar.",
                "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (r == MessageBoxResult.No) return;
            Thread thread = new Thread(() => {
                try {
                    string fn = $"{Environment.CurrentDirectory}\\WPFClinica.rar";
                    var fi = new FileInfo(fn);
                    string newFileName = @"C:\Users\" + Environment.UserName + @"\Downloads\" + fi.Name;
                    File.Delete(newFileName);
                    FileDownloader.DownloadFileFromURLToPath($"https://drive.google.com/open?id=1hRUA6O4jWfhFsX34e7rzKW3YtkospHDb", fn);
                    File.Move(fn, newFileName);
                    MessageBox.Show($"Você poderá achar a pasta compactada no directório {newFileName}.", "Concluído", MessageBoxButton.OK, MessageBoxImage.Information);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Erro ao baixar arquivo.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                Properties.Settings.Default.Save();
                MessageBox.Show("Done!");
            });
            thread.Start();
            //} catch (Exception ex) {
            // MessageBox.Show(ex.Message, "Erro");
            //}
        }

        private void HistoricoLaboratorioRecepcionistaMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new HistoricoProcLab();
        }

        private void HistoricoExamesImageRecepcionistaMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new HistoricoProcsAdmin();
        }

        private void CadastroEmpresasAdminMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new EscolherEmpresa();
        }

        private void ASOsMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new HistoricoAsos();
        }

        private void CadastroEmpresaMI_Click(object sender, RoutedEventArgs e) {
            MyFrame.Content = new EscolherEmpresa();
        }
    }
}
