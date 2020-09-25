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
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class DetalhesConsultas : Page {

        #region Vars

        int? Dias {
            get {
                try {
                    if (DiasTB.Text.Contains('-')) {
                        MessageBox.Show("Não pode ser negativo.");
                        return null;
                    }
                    return ToInt32(DiasTB.Text);
                } catch (Exception) {
                    MessageBox.Show("'Dias' não é um número inteiro.");
                    return null;
                }
            }
        } 

        #endregion

        public DetalhesConsultas() {
            InitializeComponent();
        }

        private void SalvarB_Click(object sender, RoutedEventArgs e) {
            var a = Dias;
            if (a == null) {
                return;
            }
            Tempo_Retorno.InsertOrUpdate(a ?? 0);
            MessageBox.Show("Salvo");
        }
    }
}
