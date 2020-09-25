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

    public partial class AdicionarConsulta : Window {

        #region Vars

        const string empty = "[Nulo]";

        string cpf_paciente;
        string CPF_Paciente {
            get => cpf_paciente;
            set {
                if ((cpf_paciente = value) != null) NomePacienteTB.Text = Nome_Paciente;
            }
        }
        string Nome_Paciente {
            get => Pessoas.Select(CPF_Paciente).Nome;
        }

        string cpf_funcionario;
        string CPF_Funcionario {
            get => cpf_funcionario;
            set {
                Nome_Funcionario = (cpf_funcionario = value) == null ? empty : Nome_Funcionario;
            }
        }

        string Nome_Funcionario {
            get => Pessoas.Select(CPF_Funcionario).Nome;
            set {
                NomeFuncionarioTB.Text = value;
            }
        }

        string profissao {
            get => (ProfissaoCB.SelectedItem as ComboBoxItem).Content.ToString();
        }
        Profissao Profissao {
            get => EnumStrings.SetProfissao(profissao);
            set {
                for (int i = 0; i < ProfissaoCB.Items.Count; i++) {
                    var item = ProfissaoCB.Items[i] as ComboBoxItem;
                    if (item.Content.ToString() == value.ToString()) {
                        ProfissaoCB.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        string Especializacao {
            get => (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString();
            set {
                for (int i = 0; i < EspecializacaoCB.Items.Count; i++) {
                    var item = (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString();
                    if (item == value) {
                        EspecializacaoCB.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        #endregion

        public AdicionarConsulta(string paciente) {
            InitializeComponent();
            var a = Enum.GetValues(typeof(Profissao)).Cast<Profissao>();
            for (int i = 0; i < a.Count(); i++) {
                ProfissaoCB.Items.Add(new ComboBoxItem() {
                    Content = a.ElementAt(i).ToString()
                });
            }
            var r = Especializacoes.SelectLike("");
            for (int i = 0; i < r.Count; i++) {
                EspecializacaoCB.Items.Add(new ComboBoxItem() {
                    Content = r[i].Especializacao
                });
            }
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            if (ProfissaoCB.SelectedIndex >= 0 && NomeFuncionarioTB.Text != "???") {
                var dt = DateTime.Now;
                Historico_Consultas.Insert(CPF_Funcionario, CPF_Paciente, Profissao, dt);
                if (Profissao == Profissao.Medico && Especializacao != "[Nulo]") {
                    var a = Historico_Consultas.Select(CPF_Paciente, dt.AddSeconds(-1), dt.AddSeconds(1));
                    Historico_Consultas.Update_Especialização(a[0].ID, Especializacao);
                }
                Close();
            } else {
                MessageBox.Show("Escolha uma profissao e um funcionario.");
            }
            Close();
        }

        private void EscolherFuncionarioB_Click(object sender, RoutedEventArgs e) {
            var a = new EscolherFuncionario();
            a.ShowDialog();
            if (App.Current.Properties["funcionario"] is string cpf) {
                CPF_Funcionario = cpf;
            }
        }

        private void ProfissaoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EspecializacaoCB.IsEnabled = Profissao == Profissao.Medico;
        }
    }
}
