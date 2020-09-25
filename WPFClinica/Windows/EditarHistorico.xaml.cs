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

namespace WPFClinica.Windows {

    public partial class EditarHistorico : Window {

        int consulta_ID;

        bool FoiPaga {
            get => PagoChB.IsChecked ?? false;
            set => PagoChB.IsChecked = value;
        }

        bool FoiRecebido {
            get => RecebidoChB.IsChecked ?? false;
            set => RecebidoChB.IsChecked = value;
        }

        public EditarHistorico(int consulta) {
            InitializeComponent();
            consulta_ID = consulta;
            var h = Historico_Consultas.Select(consulta);
            if (h != null) {
                FoiPaga = h.PagoEm != null;
                var cr = ConsultasRecebidas.Select(consulta);
                FoiRecebido = cr != null;
            }
        }

        private void SalvarB_Click(object sender, RoutedEventArgs e) {
            if (FoiPaga) {
                Historico_Consultas.Update_PagoEm(consulta_ID, DateTime.Now);
            } else {
                Historico_Consultas.Update_PagoEm(consulta_ID);
            }
            if (FoiRecebido) {
                var cr = ConsultasRecebidas.Select(consulta_ID);
                DateTime dt = DateTime.Now;
                if (cr == null) {
                    ConsultasRecebidas.Insert(consulta_ID, -1, dt);
                } else {
                    ConsultasRecebidas.Update_Valor(consulta_ID, -1);
                }
            } else {
                ConsultasRecebidas.Delete(consulta_ID);
            }
            Close();
        }
    }
}
