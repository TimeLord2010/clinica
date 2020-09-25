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

    public partial class CadastroClienteParcial : Window {

        #region Vars

        MySqlH sql;

        public string Nome {
            get => NomeTB.Text;
        }

        public string Contato {
            get => ContatoTB.Text;
        }

        #endregion

        public CadastroClienteParcial() {
            InitializeComponent();
            sql = App.Current.Properties["sql"] as MySqlH;
        }

        public static bool ShowDialog (out string nome, out string contato, string nomee = "") {
            var d = new CadastroClienteParcial();
            d.NomeTB.Text = nomee;
            if (d.ShowDialog() ?? false) {
                nome = d.NomeTB.Text;
                contato = d.ContatoTB.Text;
                return true;
            } else {
                nome = null;
                contato = null;
                return false;
            }
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e) {
            TempPacientes.Insert(Nome, Contato);
        }
    }
}
