using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class EscolherNaoMedico : Window {

        string CPF;
        Fonoaudiologos fono = new Fonoaudiologos();
        Dentistas dent = new Dentistas();

        bool IsFono {
            get => FonoaudiologoRB.IsChecked ?? false;
        }

        bool IsPsicologo {
            get => PsicologoRB.IsChecked ?? false;
        }

        bool IsNutricionista {
            get => NutricionistaRB.IsChecked ?? false;
        }

        bool IsDentista {
            get => DentistaRB.IsChecked ?? false;
        }

        public EscolherNaoMedico(string cpf) {
            InitializeComponent();
            CPF = cpf;
            if (!fono.IsProfissional(CPF)) {
                FonoaudiologoRB.IsEnabled = false;
            }
            if (!Psicologos.IsPsicologo(CPF)) {
                PsicologoRB.IsEnabled = false;
            }
            if (!Nutricionistas.IsNutricionista(CPF)) {
                NutricionistaRB.IsEnabled = false;
            }
            if (!dent.IsProfissional(CPF)) {
                DentistaRB.IsEnabled = false;
            }
        }

        private void EscolherB_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e) {
            if (IsFono) {
                App.Current.Properties["profissao"] = Profissao.Fonoaudiologo;
            } else if (IsPsicologo) {
                App.Current.Properties["profissao"] = Profissao.Psicologo;
            } else if (IsNutricionista) {
                App.Current.Properties["profissao"] = Profissao.Nutricionista;
            } else if (IsDentista) {
                App.Current.Properties["profissao"] = Profissao.Dentista;
            } else {
                App.Current.Properties["profissao"] = null;
            }
            base.OnClosing(e);
        }
    }
}
