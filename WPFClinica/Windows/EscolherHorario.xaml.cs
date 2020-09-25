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
using static System.Convert;

namespace WPFClinica.Windows {

    public partial class EscolherHorario : Window {

        #region Vars

        DateTime Inicio {
            get {
                try {
                    return new DateTime(2000, 1, 1, ToInt32(IHoraTB.Text), ToInt32(IMinutoTB.Text), 0);
                } catch (Exception ex) {
                    MessageBox.Show($"Classe: {ex.GetType()}\nMessagem: {ex.Message}", "Erro");
                    return new DateTime();
                }
            }
        }

        DateTime Fim {
            get {
                try {
                    return new DateTime(2000, 1, 1, ToInt32(FHoraTB.Text), ToInt32(FMinutoTB.Text), 0);
                } catch (Exception ex) {
                    MessageBox.Show($"Classe: {ex.GetType()}\nMessagem: {ex.Message}", "Erro");
                    return new DateTime();
                }
            }
        }

        #endregion

        public EscolherHorario() {
            InitializeComponent();
            WindowsSO.ToCenterOfScreen(this);
            App.Current.Properties["horario"] = null;
        }

        private void OkB_Click(object sender, RoutedEventArgs e) {
            if (Inicio > Fim) {
                MessageBox.Show("O horário de inicio tem que ser menor que o horário de fim.","Erro");
            } else if (Inicio == Fim) {
                MessageBox.Show("Os horários tem que ser diferentes","Erro");
            } else {
                App.Current.Properties["horario"] = $"{Inicio.Hour:00}:{Inicio.Minute:00}-{Fim.Hour:00}:{Fim.Minute:00}";
                Close();
            }
        }
    }
}
