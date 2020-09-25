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

    public partial class AddConvenio : Window {

        #region Vars

        int ID;

        string NomeConvenio {
            get => ConvenioTB.Text;
            set => ConvenioTB.Text = value;
        }

        #endregion

        public AddConvenio(int id = -1) {
            InitializeComponent();
            ID = id;
            if (id > -1) {
                CriarB.Content = "Editar";
                var r = Convenios.Select(id);
                NomeConvenio = r == null ? "[Nome não encontrado]" : r.Convenio;
            }
        }

        private void CriarB_Click(object sender, RoutedEventArgs ev) {
            try {
                if (ID == -1) {
                    Convenios.Insert(NomeConvenio);
                } else {
                    Convenios.Update(ID, NomeConvenio);
                }
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
