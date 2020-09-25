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

namespace WPFClinica.UserControls {

    public partial class LinhaEncaminhados : UserControl {

        #region Vars

        public string Senha {
            get => SenhaTBL.Text;
            set => SenhaTBL.Text = value;
        }

        public string Nome {
            get => NomeTBL.Text;
            set => NomeTBL.Text = value;
        }

        public string Sala {
            get => SalaTBL.Text;
            set => SenhaTBL.Text = value;
        }

        public bool Prioridade = false;

        #endregion

        public LinhaEncaminhados () {
            InitializeComponent();
            SenhaTBL.Text = "Senha";
            NomeTBL.Text = "Nome";
            SalaTBL.Text = "Sala";
            SenhaTBL.FontWeight = NomeTBL.FontWeight = SalaTBL.FontWeight = FontWeights.DemiBold;
        }

        public LinhaEncaminhados(string senha, string nome, string sala, bool p = false) {
            InitializeComponent();
            SenhaTBL.Text = senha + "";
            NomeTBL.Text = nome;
            SalaTBL.Text = sala;
            Prioridade = p;
        }
    }
}
