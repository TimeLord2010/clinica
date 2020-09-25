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

    public partial class CriarEspecializacao : Window {

        #region Vars

        string OriginalEsp;

        string Nome_Especializacao {
            get => NomeEspecializacaoTB.Text;
            set => NomeEspecializacaoTB.Text = value;
        }

        double Valor_Consulta {
            get {
                if (Double.TryParse(ValorConsultaTB.Text, out double r)) {
                    return r;
                } else {
                    return Double.NaN;
                }
            }
            set => ValorConsultaTB.Text = value + "";
        }

        #endregion

        public CriarEspecializacao(string originalEsp = null) {
            InitializeComponent();
            if (originalEsp != null) {
                Title = "Editar especialização";
                CriarB.Content = "Editar";
                Nome_Especializacao = OriginalEsp = originalEsp;
                var r = Especializacoes.Select(originalEsp);
                Valor_Consulta = r.ValorConsulta;
            }
        }

        private void CriarB_Click(object sender, RoutedEventArgs e) {
            if (Nome_Especializacao.Length < 2) {
                MessageBox.Show("Nome da especialização é muito curto.");
                return;
            }
            if (ValorConsultaTB.Text.Contains(",")) {
                var re = MessageBox.Show("Use ponto para o separador de casas decimais. Vírgulas serão ignoradas. Deseja continuar?", "Confirmação",
                    MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (re == MessageBoxResult.Cancel) {
                    return;
                }
            }
            if (Double.IsNaN(Valor_Consulta)) {
                MessageBox.Show("Valor da consulta não é valido.");
                return;
            }
            if (OriginalEsp != null) {
                if (Nome_Especializacao != OriginalEsp) {
                    Especializacoes.Update(OriginalEsp, Nome_Especializacao);
                }
                Especializacoes.Update_Valor(Nome_Especializacao, Valor_Consulta);
                Close();
            } else {
                var r = Especializacoes.Select(Nome_Especializacao);
                if (r == null) {
                    Especializacoes.Insert(Nome_Especializacao, Valor_Consulta);
                    Close();
                } else {
                    MessageBox.Show("Uma especialização com o mesmo nome já existe.");
                }
            }
        }
    }
}
