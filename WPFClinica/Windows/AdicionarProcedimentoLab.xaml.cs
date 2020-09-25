using System;
using System.Collections.Generic;
using System.Globalization;
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
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Windows {

    public partial class AdicionarProcedimentoLab : Window {

        #region Vars

        int ID = -1;
        string OriginalConvenio;

        string NomeProcedimento {
            get => ProcedimentoTB.Text;
            set => ProcedimentoTB.Text = value;
        }

        string NomeConvênio {
            get => ConvênioTB.Text;
            set => ConvênioTB.Text = value;
        }

        int ConvenioID {
            get {
                return Convenios.Select_ID(NomeConvênio);
            }
        }

        double Valor {
            get {
                return ToDouble(ValorTB.Text, CultureInfo.InvariantCulture);
            }
            set => ValorTB.Text = value + "";
        }

        #endregion

        public AdicionarProcedimentoLab() {
            InitializeComponent();
        }

        public AdicionarProcedimentoLab(int id_proc, string proc, string conv, double valor) {
            InitializeComponent();
            ID = id_proc;
            NomeProcedimento = proc;
            OriginalConvenio = NomeConvênio = conv;
            Valor = valor;
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            if (ValorTB.Text.Contains(",")) {
                var r = MessageBox.Show("Vírgulas serão ignoradas. Use ponto para o separador de casas decimais. Continuar mesmo assim?",
                    "Info", MessageBoxButton.OKCancel);
                if (r == MessageBoxResult.Cancel) return;
            }
            if (ValorTB.Text.Length == 0) {
                ValorTB.Text = "0";
            }
            try {
                ToDouble(ValorTB.Text);
            } catch (FormatException) {
                MessageBox.Show("O valor no campo 'Valor' não é um número válido.");
                return;
            }
            if (NomeProcedimento.Length > 0) {
                if (ID >= 0) {
                    ProcedimentosLab.Update(ID, NomeProcedimento);
                    Convenios.Update(OriginalConvenio, NomeConvênio);
                    ProcedimentoConvenio.Update_Valor(ID, ConvenioID, Valor);
                } else {
                    ProcedimentosLab.TryInsert(NomeProcedimento);
                    var a = ProcedimentosLab.SelectLike(NomeProcedimento);
                    ProcedimentoConvenio.Insert(a[0].Proc_ID, ConvenioID, Valor);
                }
                Close();
            } else {
                MessageBox.Show("Forneça o nome do procedimento.");
            }
        }
    }
}
