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
using WPFClinica.Pages;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class EditarAtendimento : Window {

        #region Vars

        int senha;
        int Senha {
            get => senha;
            set {
                senha = value;
                var r = Scripts.DB.ListaEspera.Select(senha);
                CPF = r.Paciente;
                var p = Pessoas.Select(CPF);
                Nome = p.Nome;
                Encaminhado = r.Fila;
                Status = r._Status;
                var ps = Paciente_Sala.Select_Paciente(CPF);
                if (ps != null) {
                    var s = Salas.Select(ps.Funcionario);
                    Sala = s.Value.Nome;
                }
                var le_f = ListaEspera_Funcionario.Select(senha);
                if (le_f != null) FuncionarioTB.Text = le_f.Funcionario;
                var le_e = ListaEspera_Especializacao.Select(senha);
                if (le_e != null) {
                    for (int i = 0; i < EspecializacaoCB.Items.Count; i++) {
                        var item = EspecializacaoCB.Items[i] as ComboBoxItem;
                        if (item.Content.ToString() == le_e.Especializacao) {
                            EspecializacaoCB.SelectedIndex = i;
                            break;
                        }
                    }
                }
            }
        }

        public string CPF {
            get => CPFTB.Text;
            set => CPFTB.Text = value;
        }

        public string Nome {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        public Fila Encaminhado {
            get {
                switch (EncaminhamentoCB.SelectedIndex) {
                    case 0:
                        return Fila.Triagem;
                    case 1:
                        return Fila.Consultorio;
                    case 2:
                        return Fila.Laboratorio;
                    default:
                        throw new Exception("Nenhuma fila selecionada.");
                }
            }
            set {
                switch (value) {
                    case Fila.Triagem:
                        EncaminhamentoCB.SelectedIndex = 0;
                        break;
                    case Fila.Consultorio:
                        EncaminhamentoCB.SelectedIndex = 1;
                        break;
                    case Fila.Laboratorio:
                        EncaminhamentoCB.SelectedIndex = 2;
                        break;
                    default:
                        throw new NotImplementedException("A fila não foi implementada.");
                }
            }
        }

        public PStatus Status {
            get {
                switch (StatusCB.SelectedIndex) {
                    case 0:
                        return PStatus.EmAtendimento;
                    case 1:
                        return PStatus.ACaminho;
                    case 2:
                        return PStatus.Atendido;
                    case 3:
                        return PStatus.Esperando;
                    default:
                        throw new Exception("Nenhum status selecionado");
                }
            }
            set {
                switch (value) {
                    case PStatus.EmAtendimento:
                        StatusCB.SelectedIndex = 0;
                        break;
                    case PStatus.ACaminho:
                        StatusCB.SelectedIndex = 1;
                        break;
                    case PStatus.Atendido:
                        StatusCB.SelectedIndex = 2;
                        break;
                    case PStatus.Esperando:
                        StatusCB.SelectedIndex = 3;
                        break;
                    default:
                        throw new NotImplementedException("O status forncecido não foi implementado.");
                }
            }
        }

        string Sala {
            get => SalaTB.Text;
            set => SalaTB.Text = value;
        }

        #endregion

        public EditarAtendimento(int senha) {
            InitializeComponent();
            WindowsSO.ToCenterOfScreen(this);
            var r = Especializacoes.SelectLike("");
            for (int i = 0; i < r.Count; i++) {
                EspecializacaoCB.Items.Add(new ComboBoxItem() {
                    Content = r[i].Especializacao
                });
            }
            Senha = senha;
        }

        private void AtualizarB_Click(object sender, RoutedEventArgs e) {
            try {
                SQLOperations.ThrowExceptions = true;
                if (Encaminhado != Fila.Laboratorio) {
                    PacienteProcedimentos.Delete_Senha(Senha);
                }
                if (Status == PStatus.ACaminho || Status == PStatus.EmAtendimento) {
                    var sala = Salas.Select_Sala(Sala);
                    Paciente_Sala.InsertOrUpdate(sala.Funcionario, CPF);
                } else {
                    Paciente_Sala.DeletePaciente(CPF);
                }
                Scripts.DB.ListaEspera.Update(Senha, Encaminhado, Status);
                if (FuncionarioTB.Text == "[Null]") {
                    ListaEspera_Funcionario.Delete(Senha);
                } else {
                    ListaEspera_Funcionario.InsertOrUpdate(Senha, FuncionarioTB.Text);
                }
                if (EspecializacaoCB.SelectedIndex >= 0) {
                    ListaEspera_Especializacao.InsertOrUpdate(Senha, (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString());
                } else {
                    ListaEspera_Especializacao.Delete(Senha);
                }
                SQLOperations.ThrowExceptions = false;
            } catch (Exception) { }
            Close();
        }

        private void EscolherSalaB_Click(object sender, RoutedEventArgs e) {
            App.Current.Properties["sala"] = null;
            var a = new EscolherSala();
            a.ShowDialog();
            if (App.Current.Properties["sala"] is string sala) {
                Sala = sala;
            } else {
                Sala = "[Null]";
            }
        }

        private void StatusCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (Status == PStatus.ACaminho || Status == PStatus.EmAtendimento) {
                EscolherSalaB.IsEnabled = true;
                Sala = "[Null]";
            } else {
                EscolherSalaB.IsEnabled = false;
            }
        }

        private void EscolherFuncionarioB_Click(object sender, RoutedEventArgs e) {
            var a = new EscolherFuncionario();
            a.ShowDialog();
            if (App.Current.Properties["funcionario"] is string cpf) {
                FuncionarioTB.Text = cpf;
            } else {
                FuncionarioTB.Text = "[Null]";
            }
        }

        private void LimparB_Click(object sender, RoutedEventArgs e) {
            EspecializacaoCB.SelectedIndex = -1;
        }
    }
}
