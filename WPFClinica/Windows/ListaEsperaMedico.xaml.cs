using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Diagnostics;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;
using System.ComponentModel;

namespace WPFClinica.Windows {

    public partial class ListaEsperaMedico : Window {

        #region Vars

        TimedEvent TimedEvent;
        string CPF_Medico { get; set; }
        bool InseridoHistorico = false;
        int senha = -1;
        int Senha {
            get => senha;
            set {
                try {
                    senha = value;
                    if (value >= 0) {
                        var lista = ListaEspera.Select(senha);
                        IsExamesVisible(Fila == Fila.ExamesMedicos);
                        if (lista != null) {
                            Paciente = lista.Paciente;
                        } else {
                            UpdatePacientes();
                        }
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, $"Erro ao atribuir senha. ({ErrorCodes.PP0001})");
                }
            }
        }
        string NomeProntuario {
            get => "Prontuário.docx";
            set {
                try {
                    SQLOperations.ThrowExceptions = true;
                    if (value == null) {
                        if (NomeProntuario != null && File.Exists(NomeProntuario)) {
                            Prontuarios.Update(Paciente, new FileInfo(NomeProntuario));
                            File.Delete(NomeProntuario);
                            AtualizarProntuarioB.IsEnabled = false;
                        }
                    } else {
                        AtualizarProntuarioB.IsEnabled = true;
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Erro ao atualizar prontuário.");
                } finally {
                    SQLOperations.ThrowExceptions = false;
                }
            }
        }
        string paciente;
        string Paciente {
            get => paciente;
            set {
                AbrirProntuarioB.IsEnabled = ConfirmarSaidaB.IsEnabled = value != null;
                paciente = value;
                ListaEspera.Update_DT_Senha(Senha);
                if (value == null) {
                    IsExamesVisible(false);
                    NomeProntuario = null;
                    Paciente_Sala.Delete(CPF_Medico);
                    PacienteTBL.Text = "???";
                    ListaEspera.Delete(Senha);
                    AbrirProntuarioB.IsEnabled = ConfirmarSaidaB.IsEnabled = false;
                    InseridoHistorico = false;
                    Senha = -1;
                } else {
                    PacienteTBL.Text = Pessoas.Select(Paciente).Nome;
                    ListaEspera.Update(Senha, PStatus.ACaminho);
                    Paciente_Sala.InsertOrUpdate(CPF_Medico, Paciente);
                    var le = ListaEspera_Exames.Select(Senha);
                    var procs = le.Procedimentos;
                    ExamesDG.Items.Clear();
                    for (int i = 0; procs != null && i < procs.Count; i++) {
                        var item = procs[i];
                        ExamesDG.Items.Add(new Table3() {
                            ID = item.ID,
                            Tipo_ID = item.Tipo_ID,
                            Exame = item.Descricao,
                            Tipo = item.Tipo
                        });
                        try {
                            var hp = Historico_Procedimento.Select(item.ID, paciente)[0];
                            Procedimento_Medico.Insert(hp.ID, CPF_Medico);
                        } catch (Exception) {

                        }
                    }
                    if (le.ListaEspera.Fila != ES.f[Fila.ExamesMedicos] && !InseridoHistorico) {
                        var id = ListaEspera_Agendamento.Select_Senha(Senha);
                        if (id == null) {
                            MessageBox.Show("Não foi possível salvar consulta no histórico por que o identificador não pôde ser encontrado.");
                        } else {
                            Historico_Consultas.Update_Funcionario(id ?? -1, CPF_Medico);
                            Historico_Consultas.Update_Profissão(id ?? -1, Profissao.Medico);
                            InseridoHistorico = true;
                        }
                    }
                }
            }
        }

        PStatus Status {
            get {
                switch (TipoFilaCB.SelectedIndex) {
                    case 0:
                        return PStatus.Atendido;
                    case 1:
                        return PStatus.Esperando;
                    default:
                        throw new IndexOutOfRangeException();
                }
            }
        }

        Fila Fila {
            get {
                switch (FilaCB.SelectedIndex) {
                    case 0:
                        return Fila.Consultorio;
                    case 1:
                        return Fila.ExamesMedicos;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        public ListaEsperaMedico(string cpf) {
            InitializeComponent();
            CPF_Medico = cpf;
            try {
                if (String.IsNullOrEmpty(cpf)) {
                    throw new Exception("O cpf do médico era nulo ou vazio.");
                }
                ControlsH.CreateColumns(ExamesDG, typeof(Table3));
                var ps = Paciente_Sala.Select_Funcionario(CPF_Medico);
                if (ps != null) {
                    if (String.IsNullOrEmpty(ps.Paciente)) {
                        throw new Exception("Existe um paciente na sala do médico, mas não foi possível obter o seu cpf.");
                    }
                    var lista = ListaEspera.Select_Paciente(ps.Paciente);
                    if (lista != null) {
                        var le = ListaEspera_Exames.Select(lista.Senha);
                        if (le != null && le.Procedimentos != null && le.Procedimentos.Count > 0) {
                            FilaCB.SelectedIndex = 1;
                        }
                        Senha = lista.Senha;
                    }
                }
                TimedEvent = new TimedEvent(1, 10, false, UpdatePacientes);
                UpdatePacientes();
            } catch (Exception ex) {
                MessageBox.Show($"{ex.Message}", $"Erro. ({ErrorCodes.I0001})");
            }
        }

        ~ListaEsperaMedico() {
            try {
                if (File.Exists(NomeProntuario)) File.Delete(NomeProntuario);
            } catch (Exception) {
                MessageBox.Show("Erro ao excluir prontuário.");
            }
        }

        void UpdatePacientes() {
            PacientesDG.Columns.Clear();
            PacientesDG.Items.Clear();
            try {
                if (Fila == Fila.Consultorio) {
                    ControlsH.CreateColumns(PacientesDG, typeof(Table2));
                    var r = ListaEspera.SelectM(CPF_Medico);
                    var esps = MedicoEspecializacoes.Select(CPF_Medico);
                    for (int i = 0; i < r.Count; i++) {
                        var row = r[i];
                        var esp = row.Especializacao;
                        if (String.IsNullOrEmpty(row.CPF_Funcionario)) {
                            if (!String.IsNullOrEmpty(esp) && !esps.Contains(esp)) {
                                continue;
                            }
                        }
                        PacientesDG.Items.Add(new Table2() {
                            Senha = row.Senha,
                            Nome = row.Nome,
                            Idade = Data.Age(row.Nascimento_Paciente) + " anos",
                            Atualizacao = row.Atualizacao.ToString("dd-MM-yyyy HH:mm"),
                            Status = row.Status,
                            Encaminhado = row.Fila,
                            Médico = row.CPF_Funcionario,
                            Especialização = esp
                        });
                    }
                } else {
                    ControlsH.CreateColumns(PacientesDG, typeof(Table1));
                    var r = ListaEspera.Select(Fila.ExamesMedicos, PStatus.Esperando);
                    for (int i = 0; i < r.Count; i++) {
                        var a = r[i];
                        PacientesDG.Items.Add(new Table1() {
                            Senha = a.Senha,
                            Nome = a.Nome,
                            Atualizacao = a.Atualizacao.ToString("dd-MM-yyyy HH:mm"),
                            Status = a.Status,
                            Encaminhado = a.Fila,
                            Idade = Data.Age(a.Nascimento_Paciente) + " anos"
                        });
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, $"Erro. ({ErrorCodes.E0005})");
            }
        }

        private void PacientesDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ChamarPacienteB.IsEnabled = PacientesDG.SelectedIndex >= 0;
        }

        private void ChamarPacienteB_Click(object sender, RoutedEventArgs e) {
            if (PacientesDG.SelectedItem is Table1 item) {
                Senha = item.Senha;
                ChamarPacienteB.IsEnabled = false;
                AtualizadoTBL.Visibility = Visibility.Hidden;
                UpdatePacientes();
            }
        }

        private void ConfirmarSaidaB_Click(object sender, RoutedEventArgs e) {
            Paciente = null;
        }

        private void TipoFilaCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void AbrirProntuarioB_Click(object sender, RoutedEventArgs e) {
            try {
                var tp = new EscolherTipoProntuário() {
                    Owner = this
                };
                var r = tp.ShowDialog();
                if (!r.HasValue) {
                    return;
                }
                ListaEspera.Update(Senha, Fila.Consultorio, PStatus.EmAtendimento);
                if (r.Value) {
                    var p = new Prontuario(Paciente);
                    p.Closed += new EventHandler((se,rv) => {
                        AtualizadoTBL.Visibility = Visibility.Visible;
                    });
                    p.ShowDialog();
                } else {
                    Prontuarios.SelectOrCreate(Paciente, out byte[] fileData);
                    using (var s = new FileStream(NomeProntuario, FileMode.Create, FileAccess.Write)) {
                        s.Write(fileData, 0, fileData.Length);
                    }
                    Process.Start(NomeProntuario);
                    AtualizarProntuarioB.IsEnabled = true;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro ao abrir prontuário.");
            }
        }

        private void FazerPrescricaoB_Click(object sender, RoutedEventArgs e) {

        }

        private void AtualizarProntuarioB_Click(object sender, RoutedEventArgs e) {
            if (File.Exists(NomeProntuario)) {
                NomeProntuario = null;
                AtualizadoTBL.Visibility = Visibility.Visible;
            } else {
                MessageBox.Show("Arquivo não pôde ser encontrado.");
            }
        }
        private void AtualizarB_Click(object sender, RoutedEventArgs e) {
            UpdatePacientes();
        }

        class Table1 {

            public int Senha { get; set; }
            public string Nome { get; set; }
            public string Idade { get; set; }
            public string Encaminhado { get; set; }
            public string Status { get; set; }
            public string Atualizacao { get; set; }

        }

        class Table2 : Table1 {

            public string Especialização { get; set; }
            public string Médico { get; set; }

        }

        class Table3 {
            public int ID;
            public int Tipo_ID;
            public string Exame { get; set; }
            public string Tipo { get; set; }
        }

        private void FilaCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TriggerNow();
            IsExamesVisible(Fila == Fila.ExamesMedicos);
        }

        private void IsExamesVisible(bool a) {
            if (TablesG == null) return;
            var cds = TablesG.ColumnDefinitions;
            var cd = cds.ElementAt(1);
            cd.Width = new GridLength(a ? 1 : 0, GridUnitType.Star);
        }

    }
}
