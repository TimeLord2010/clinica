using System;
using System.Collections.Generic;
using System.Globalization;
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
using static System.Convert;

namespace WPFClinica.Windows {

    public partial class PagarConsulta : Window {

        #region Vars

        CultureInfo Brasil = new CultureInfo("pt-BR");

        string cpf_paciente;
        string CPF_Paciente {
            get => cpf_paciente;
            set {
                cpf_paciente = value;
                var p = Pessoas.Select(cpf_paciente);
                if (p != null) {
                    NomePaciente = p.Nome;
                } else {
                    NomePaciente = $"[Nulo]";
                }
            }
        }

        string cpf_funcionario;
        string CPF_Funcionário {
            get => cpf_funcionario;
            set {
                cpf_funcionario = value;
                var p = Pessoas.Select(cpf_funcionario);
                if (p != null) {
                    NomeFuncionario = p.Nome;
                } else {
                    NomeFuncionario = "[Nulo]";
                }
            }
        }

        string NomePaciente {
            get => NomePacienteTB.Text;
            set => NomePacienteTB.Text = value;
        }

        string NomeFuncionario {
            get => NomeFuncionarioTB.Text;
            set => NomeFuncionarioTB.Text = value;
        }

        string Especializacao {
            get => EspecializacaoTB.Text;
            set {
                var r = Especializacoes.Select(value);
                if (r != null) {
                    EspecializacaoTB.Text = value;
                    Total = r.ValorConsulta;
                } else {
                    EspecializacaoTB.Text = "[Nulo]";
                    TotalTB.Text = "[?]";
                }
            }
        }

        double Total {
            get => Converter.TryConvert(TotalTB.Text, (x) => ToDouble(x, Brasil), Double.NaN);
            set => TotalTB.Text = value.ToString(Brasil);
        }

        double Dinheiro {
            get => Converter.TryConvert(DinheiroTB.Text, (x) => ToDouble(x, Brasil), Double.NaN);
            set => DinheiroTB.Text = value.ToString(Brasil);
        }

        double Crédito {
            get => Converter.TryConvert(CréditoTB.Text, (x) => ToDouble(x, Brasil), Double.NaN);
            set => CréditoTB.Text = value.ToString(Brasil);
        }

        double Débito {
            get => Converter.TryConvert(DébitoTB.Text, (x) => ToDouble(x, Brasil), Double.NaN);
            set => DébitoTB.Text = value.ToString(Brasil);
        }

        double TotalPago {
            get {
                double total = 0;
                Data.DoFor((t) => {
                    if (!double.IsNaN(t)) {
                        total += t;
                    }
                }, Dinheiro, Crédito, Débito);
                return total;
            }
        }

        #endregion

        public PagarConsulta(string cpfPaciente, string cpfFuncionario, string especializacao) {
            InitializeComponent();
            CPF_Paciente = cpfPaciente;
            CPF_Funcionário = cpfFuncionario;
            Especializacao = especializacao;
            App.Current.Properties["agendamento"] = null;
        }

        private void SalvarB_Click(object sender, RoutedEventArgs e) {
            try {
                if (NomePaciente == "[Nulo]") {
                    throw new Exception("Não existe um paciente cadastrado.");
                }
                if (Data.Any(t => t < 0, Dinheiro, Débito, Crédito)) {
                    throw new Exception("Um dos valores fornecidos era negativo.");
                }
                if (!double.IsNaN(Total)) {
                    if (Math.Abs(Total - TotalPago) > 1) {
                        var r = MessageBox.Show($"O valor pago ({TotalPago}) é diferente do valor esperado ({Total}). Continuar?",
                            "Inconsistêcia no valor.", MessageBoxButton.YesNo, MessageBoxImage.Question);
                        if (r == MessageBoxResult.No) {
                            return;
                        }
                    }
                }
                SQLOperations.ThrowExceptions = true;
                var dt = DateTime.Now;
                Historico_Consultas.Insert(CPF_Paciente, dt, TotalPago);
                var hs = Historico_Consultas.Select(CPF_Paciente, dt.AddSeconds(-1), dt.AddSeconds(1));
                if (hs.Count == 0) {
                    throw new Exception("Não foi possível obter o identificador de historico de consulta.");
                }
                var h = hs[0];
                App.Current.Properties["id"] = h.ID;
                if (NomeFuncionario != "[Nulo]") {
                    Historico_Consultas.Update_Funcionario(h.ID, CPF_Funcionário);
                }
                if (Especializacao != "[Nulo]") {
                    Historico_Consultas.Update_Especialização(h.ID, Especializacao);
                }
                var cf = new Consultas_FormaPagamento();
                if (!double.IsNaN(Dinheiro) && Dinheiro > 0) {
                    cf.Insert(h.ID, 1 ,Dinheiro);
                }
                if (!double.IsNaN(Crédito) && Crédito > 0) {
                    cf.Insert(h.ID, 2, Crédito);
                }
                if (!double.IsNaN(Débito) && Débito > 0) {
                    cf.Insert(h.ID, 3, Débito);
                }
                DialogResult = true;
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro");
            } finally {
                SQLOperations.ThrowExceptions = false;
            }
        }

        private void CancelarB_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {

        }
    }
}
