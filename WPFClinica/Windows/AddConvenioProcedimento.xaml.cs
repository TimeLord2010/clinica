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
using WPFClinica.Scripts.DB;
using System.Globalization;
using static System.Convert;

namespace WPFClinica.Windows {

    public partial class AddConvenioProcedimento : Window {

        #region Vars

        bool Editing = false;
        int Proc_ID;
        CultureInfo Brasil = new CultureInfo("pt-BR");

        string Convenio {
            get {
                if (ConvenioCB.SelectedIndex >= 0) {
                    return (ConvenioCB.SelectedItem as ComboBoxItem).Content.ToString();
                } else {
                    return null;
                }
            }
        }

        int Conv_ID {
            get {
                if (Convenio == null) return -1;
                try {
                    return Convenios.Select_ID(Convenio);
                } catch (Exception) {
                    return -1;
                }
            }
        }

        double Valor {
            get {
                try {
                    return ToDouble(ValorTB.Text, Brasil);
                } catch (Exception) {
                    return double.NaN;
                }
            }
        }

        #endregion

        public AddConvenioProcedimento(int proc_id, int conv_id = -1) {
            InitializeComponent();
            Proc_ID = proc_id;
            var r = Convenios.SelectLike("");
            foreach (var item in r) {
                ConvenioCB.Items.Add(new ComboBoxItem() {
                    Content = item.Convenio
                });
            }
            ProcedimentoTBL.Text = ProcedimentosLab.Select(proc_id);
            if (conv_id > -1) {
                Title = "Editar";
                AdicionarB.Content = "Editar";
                Editing = true;
                var conv = Convenios.Select(conv_id).Convenio;
                for (int i = 0; i < ConvenioCB.Items.Count; i++) {
                    var item = ConvenioCB.Items[i] as ComboBoxItem;
                    if (conv == item.Content.ToString()) {
                        ConvenioCB.SelectedIndex = i;
                        break;
                    }
                }
                ConvenioCB.IsEnabled = false;
            }
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs ev) {
            if (Convenio == null) {
                MessageBox.Show("Selecione um convênio.");
                return;
            }
            if (Conv_ID == -1) {
                MessageBox.Show("Identificador do convênio não encontrado.");
                return;
            }
            if (double.IsNaN(Valor)) {
                MessageBox.Show("O valor não é válido.");
                return;
            }
            try {
                if (Editing) {
                    ProcedimentoConvenio.Update_Valor(Proc_ID, Conv_ID, Valor);
                } else {
                    ProcedimentoConvenio.Insert(Proc_ID, Conv_ID, Valor);
                }
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
