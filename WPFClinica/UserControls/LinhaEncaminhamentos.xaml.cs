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

    public partial class LinhaEncaminhamentos : UserControl {

        #region Vars

        public string CPF;

        public string Senha {
            get => SenhaTBL.Text;
            set => SenhaTBL.Text = value;
        }

        public string Nome {
            get => NomeTBL.Text;
            set => NomeTBL.Text = value;
        }

        public bool Prioridade;

        #endregion

        public LinhaEncaminhamentos() {
            InitializeComponent();
            Senha = "Senha";
            Nome = "Nome";
            SenhaTBL.FontWeight = NomeTBL.FontWeight = FontWeights.DemiBold;
        }

        public LinhaEncaminhamentos(string nome, string senha, bool prioridade) {
            InitializeComponent();
            Senha = senha;
            Nome = nome;
            Prioridade = prioridade;
        }
    }
}
