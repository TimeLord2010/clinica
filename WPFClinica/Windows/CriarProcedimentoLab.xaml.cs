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

    public partial class CriarProcedimentoLab : Window {

        #region Vars

        int ID;

        string NomeProcedimento {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        #endregion

        public CriarProcedimentoLab(int id = -1) {
            InitializeComponent();
            ID = id;
            if (ID >= 0) {
                NomeProcedimento = ProcedimentosLab.Select(ID);
                Title = "Editar Procedimento Laboratorial";
                CriarB.Content = "Editar";
            }
        }

        private void CriarB_Click(object sender, RoutedEventArgs e) {
            try {
                if (ID == -1) {
                    ProcedimentosLab.Insert(NomeProcedimento);
                } else {
                    ProcedimentosLab.Update(ID, NomeProcedimento);
                }
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro");
            }
        }
    }
}
