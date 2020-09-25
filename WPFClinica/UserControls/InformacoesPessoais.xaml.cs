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
using WPFClinica.Scripts;

namespace WPFClinica.UserControls {

    public partial class InformacoesPessoais : UserControl {

        #region Vars

        public bool Editing = false;

        public string Nome {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        public string CPF {
            get => CPFTB.Text;
            set => CPFTB.Text = value;
        }

        public bool IsMale {
            get => MRB.IsChecked ?? false;
            set {
                if (value) {
                    MRB.IsChecked = true;
                } else {
                    FRB.IsChecked = true;
                }
            }
        }

        public string Nascimento {
            get {
                var nascimento = DataNascimentoTB.Text;
                nascimento = nascimento.Replace(" ", "");
                if (nascimento.Length == 0) {
                    return "01/01/1800";
                }
                return nascimento;
            }
            set => DataNascimentoTB.Text = value;
        }

        public string Email {
            get => EmailTB.Text;
            set => EmailTB.Text = value;
        }

        public string Contato1 {
            get => Contato1TB.Text;
            set => Contato1TB.Text = value;
        }

        public string Contato2 {
            get => Contato2TB.Text;
            set => Contato2TB.Text = value;
        }

        #endregion

        public InformacoesPessoais() {
            InitializeComponent();
            var _ = new AcceptCPF(CPFTB.CampoTB);
        }
    }
}
