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

    public partial class AddTipoProcedimento : Window {

        #region Vars

        int ID;

        string NomeTipo {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        #endregion

        public AddTipoProcedimento(int id = -1) {
            InitializeComponent();
            ID = id;
            if (ID >= 0) {
                CriarB.Content = "Editar";
                try {
                    NomeTipo = TipoProcedimento.Select(id);
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, "Erro ao obter nome do tipo de procedimento.");
                }
            } else {
                DeletarB.IsEnabled = false;
            }
        }

        private void CriarB_Click(object sender, RoutedEventArgs e) {
            try {
                if (NomeTipo.Length < 3) {
                    throw new Exception("O nome é muito curto.");
                }
                if (ID >= 0) {
                    TipoProcedimento.Update(ID, NomeTipo);
                } else {
                    TipoProcedimento.Insert(NomeTipo);
                }
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

        private void DeletarB_Click(object sender, RoutedEventArgs e) {
            try {
                if (ID >= 0) {
                    TipoProcedimento.Delete(ID);
                }
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro");
            }
        }
    }
}
