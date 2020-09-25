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

namespace WPFClinica.Pages {

    public partial class ValoresPorConsulta : Page {

        #region Vars

        TimedEvent TimedEvent;

        public string Nome_Especializacao {
            get => EspecializacaoTB.Text;
            set => EspecializacaoTB.Text = value;
        }

        #endregion

        public ValoresPorConsulta() {
            InitializeComponent();
            ControlsH.CreateColumns(EspecializacoesDG, typeof(Especializacoes));
            TimedEvent = new TimedEvent(1,2, true, UpdateEspecializacoes);
            TimedEvent.TriggerNow();
        }

        void UpdateEspecializacoes () {
            EspecializacoesDG.Items.Clear();
            var r = Especializacoes.SelectLike(Nome_Especializacao);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                EspecializacoesDG.Items.Add(new Especializacoes() {
                    Especializacao = row.Especializacao,
                    ValorConsulta = row.ValorConsulta
                });
            }
        }

        private void EspecializacoesDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarEspecializacaoB.IsEnabled = RemoverEspecializacaoB.IsEnabled = EspecializacoesDG.SelectedIndex >= 0;
        }

        private void AdicionarEspecializacaoB_Click(object sender, RoutedEventArgs e) {
            var a = new CriarEspecializacao();
            a.ShowDialog();
            UpdateEspecializacoes();
        }

        private void EditarEspecializacaoB_Click(object sender, RoutedEventArgs e) {
            if (EspecializacoesDG.SelectedItem is Especializacoes especializacao) {
                var a = new CriarEspecializacao(especializacao.Especializacao);
                a.ShowDialog();
                UpdateEspecializacoes();
            }
        }

        private void RemoverEspecializacaoB_Click(object sender, RoutedEventArgs e) {
            if (EspecializacoesDG.SelectedItem is Especializacoes especializacao) {
                Especializacoes.Delete(especializacao.Especializacao);
                UpdateEspecializacoes();
            }
        }

        private void EspecializacaoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }
    }
}
