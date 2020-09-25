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

    public partial class EscolherMedico : Window {

        #region Vars

        TimedEvent TimedEvent;

        string Nome {
            get {
                return NomeTB.Text;
            }
        }

        string Especializacao {
            get {
                if (EspecializacaoCB.SelectedIndex < 1) {
                    return "";
                }
                var a = EspecializacaoCB.SelectedItem as ComboBoxItem;
                return a.Content.ToString();
            }
        }

        #endregion

        public EscolherMedico() {
            InitializeComponent();
            ControlsH.CreateColumns(MedicosDG, typeof(Medicos_Table));
            EspecializacaoCB.Items.Add(new ComboBoxItem() { Content = "[Todas]" });
            Especializacoes.Fill(EspecializacaoCB);
            EspecializacaoCB.SelectedIndex = 0;
            TimedEvent = new TimedEvent(1, 2, true, UpdateMedicos);
            TimedEvent.TriggerNow();
            TimedEvent.Stop();
            App.Current.Properties["medico"] = null;
        }

        void UpdateMedicos() {
            MedicosDG.Items.Clear();
            var r = Medicos.SelectLike("", Nome, Especializacao);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                MedicosDG.Items.Add(new Medicos_Table() {
                    CPF = row.CPF,
                    Nome = row.Nome
                });
            }
        }

        private void SelecionarB_Click(object sender, RoutedEventArgs e) {
            if (MedicosDG.SelectedItem is Pessoas p) {
                App.Current.Properties["medico"] = p;
                Close();
            }
        }

        private void MedicosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            SelecionarB.IsEnabled = MedicosDG.SelectedIndex > -1;
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void EspecializacaoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        public class Medicos_Table {
            public string CPF { get; set; }
            public string Nome { get; set; }
        }

    }
}
