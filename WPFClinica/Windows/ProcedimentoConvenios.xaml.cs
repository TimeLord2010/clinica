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
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class ProcedimentoConvenios : Window {

        #region Vars

        int Proc_ID;
        CultureInfo Brasil = new CultureInfo("pt-BR");

        #endregion

        public ProcedimentoConvenios(int proc_id) {
            InitializeComponent();
            Proc_ID = proc_id;
            ControlsH.CreateColumns(ConveniosDG, typeof(Table1));
            try {
                NomeProcedimentoTBL.Text = ProcedimentosLab.Select(proc_id);
                UpdateC();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            WindowsSO.ToCenterOfScreen(this);
        }

        void UpdateC() {
            ConveniosDG.Items.Clear();
            var r = ProcedimentoConvenio.SelectLike(NomeProcedimentoTBL.Text, "");
            foreach (var item in r) {
                if (item.Proc_ID != Proc_ID) {
                    continue;
                }
                ConveniosDG.Items.Add(new Table1() {
                    Con_ID = item.Con_ID,
                    Convenio = item.Convenio,
                    Valor = item.Valor.ToString(Brasil)
                });
            }
        }

        private void ConveniosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = RemoverB.IsEnabled = ConveniosDG.SelectedIndex >= 0;
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            var a = new AddConvenioProcedimento(Proc_ID);
            a.ShowDialog();
            UpdateC();
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            if (ConveniosDG.SelectedItem is Table1 table1) {
                var a = new AddConvenioProcedimento(Proc_ID, table1.Con_ID);
                a.ShowDialog();
                UpdateC();
            }
        }

        private void RemoverB_Click(object sender, RoutedEventArgs ev) {
            if (ConveniosDG.SelectedItem is Table1 item) {
                var r = MessageBox.Show("Deseja remover o convênio deste procedimento?", "Confirmar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (r == MessageBoxResult.Cancel) {
                    return;
                }
                ProcedimentoConvenio.Delete(Proc_ID, item.Con_ID);
                UpdateC();
            }
        }

        class Table1 {

            public int Con_ID;
            public string Convenio { get; set; }
            public string Valor { get; set; }

        }

    }
}
