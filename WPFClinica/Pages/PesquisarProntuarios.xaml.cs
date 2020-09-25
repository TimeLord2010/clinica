using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using WPFClinica.Windows;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class PesquisarProntuarios : Page {

        #region Vars

        TimedEvent TimedEvent;

        string CPF;

        string NomePaciente {
            get => NomePacienteTB.Text;
        }

        const string AtomFileName = "Prontuário";
        string FileName;
        const string Extension = ".docx";

        #endregion

        public PesquisarProntuarios() {
            InitializeComponent();
            EspecializacaoCB.Items.Add(new ComboBoxItem() { Content = "[Todas]" });
            Especializacoes.Fill(EspecializacaoCB);
            EspecializacaoCB.SelectedIndex = 0;
            ControlsH.CreateColumns(PacientesDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 2, true, UpdatePacientes);
            TimedEvent.TriggerNow();
        }

        ~PesquisarProntuarios() {
            try {
                var di = new DirectoryInfo(Environment.CurrentDirectory);
                var files = di.GetFiles($"*{Extension}").Where(x => Regex.IsMatch(x.Name, @"Prontuário de \d?\.docx"));
                foreach (var fi in files) {
                    try {
                        File.Delete(fi.FullName);
                    } catch (Exception) { 
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show($"Erro ao tentar exluir o documento localmente. O arquivo ainda está no seu computador.\n{ex.Message}", "Erro ao excluir prontuário.");
            }
        }

        void UpdatePacientes() {
            PacientesDG.Items.Clear();
            List<Pessoas> ps = Pacientes.SelectLike("", NomePaciente);
            if (EspecializacaoCB.SelectedIndex > 0) {
                var esp = (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString();
                var r = Historico_Consultas.Select_Especialzação(NomePaciente, esp);
                for (int i = 0; i < r.Count; i++) {
                    var a = r[i];
                    for (int j = 0; j < r.Count; j++) {
                        if (i == j) continue;
                        var b = r[i];
                        if (a.Paciente == b.Paciente) {
                            r.Remove(a.RealizadoEm > b.RealizadoEm ? b : a);
                            i = -1;
                            break;
                        }
                    }
                }
                for (int i = 0; i < ps.Count; i++) {
                    var p = ps[i];
                    if (!r.Any(x => x.Paciente == p.CPF)) {
                        ps.RemoveAt(i--);
                    }
                }
            }
            for (int i = 0; i < ps.Count; i++) {
                var row = ps[i];
                PacientesDG.Items.Add(new Table1() {
                    CPF = row.CPF,
                    Nome = row.Nome,
                    Nascimento = row.Nascimento.ToString("dd/MM/yyyy"),
                    Idade = row.Idade + "",
                    Sexo = row.Sexo ? "M" : "F",
                    Email = row.Email,
                    Contato1 = row.Contato1,
                    Contato2 = row.Contato2
                });
            }
        }

        private void NomePacienteTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void AbrirProntuarioB_Click(object sender, RoutedEventArgs e) {
            if (PacientesDG.SelectedItem is Table1 table1) {
                try {
                    var tp = new EscolherTipoProntuário() {
                        Owner = Window.GetWindow(this)
                    };
                    var re = tp.ShowDialog();
                    if (!re.HasValue) {
                        return;
                    }
                    if (re.Value) {
                        var p = new Prontuario(table1.CPF);
                        p.ShowDialog();
                    } else {
                        CPF = table1.CPF;
                        Prontuarios.Select(table1.CPF, out byte[] fd);
                        if (fd == null) {
                            var r = MessageBox.Show("Paciente não possui prontuário. Deseja criar agora?",
                                "Confirmação", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                            if (r == MessageBoxResult.OK) {
                                Prontuarios.SelectOrCreate(table1.CPF, out fd);
                            } else {
                                return;
                            }
                        }
                        if (fd == null) {
                            throw new Exception("Erro ao carregar arquivo.");
                        }
                        FileName = $"{AtomFileName} de {CPF}{Extension}";
                        using (var s = new FileStream(FileName, FileMode.Create, FileAccess.Write)) {
                            s.Write(fd, 0, fd.Length);
                        }
                        Process.Start(FileName);
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AtualizarProntuarioB_Click(object sender, RoutedEventArgs e) {
            try {
                if (CPF == null) 
                    throw new Exception("CPF do funcionário era nulo");
                if (File.Exists(FileName)) {
                    Prontuarios.Update(CPF, new FileInfo(FileName));
                    AtualizarProntuarioB.IsEnabled = false;
                    File.Delete(FileName);
                    CPF = null;
                } else {
                    throw new Exception("Arquivo não encontrado.");
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        class Table1 {

            public string CPF { get; set; }
            public string Nome { get; set; }
            public string Nascimento { get; set; }
            public string Idade { get; set; }
            public string Sexo { get; set; }
            public string Email { get; set; }
            public string Contato1 { get; set; }
            public string Contato2 { get; set; }

        }

        private void PacientesDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AbrirProntuarioB.IsEnabled = AtualizarProntuarioB.IsEnabled = PacientesDG.SelectedIndex >= 0;
        }

        private void EspecializacaoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }
    }
}
