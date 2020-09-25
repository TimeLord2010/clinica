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
using WPFClinica.Windows;
using WPFClinica.Scripts.DB;

namespace WPFClinica.UserControls {

    public partial class LinhaAgendamento : UserControl {

        #region Vars

        public int ID = -1;
        ChooseMode ChooseMode;

        string Paciente {
            get => PacienteTBL.Text;
            set => PacienteTBL.Text = value;
        }

        string Horario {
            get => HorarioTBL.Text;
            set => HorarioTBL.Text = value;
        }

        Brush background = Brushes.White;
        public Brush _Background {
            get => background;
            set {
                background = value;
                MyGrid.Background = value;
            }
        }

        #endregion

        public LinhaAgendamento(ChooseMode choose, string paciente, string horario) {
            InitializeComponent();
            ChooseMode = choose;
            Paciente = paciente;
            Horario = horario;
        }

        private void MyGrid_MouseEnter(object sender, MouseEventArgs e) {
            MyGrid.Background = Brushes.LightGray;
        }

        private void MyGrid_MouseLeave(object sender, MouseEventArgs e) {
            MyGrid.Background = Brushes.White;
        }

        private void MyGrid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (ChooseMode == ChooseMode.ChooseCreate || ChooseMode == ChooseMode.View) {
                var a = new VisualizarAgendamento(ID);
                a.ShowDialog();
            } else if (ChooseMode == ChooseMode.ChooseCreated) {
                App.Current.Properties["agendamento"] = ID;
                var Element = Parent as FrameworkElement;
                while (Element != null && !(Element is PesquisarAgenda) && Element.Parent != null) {
                    Element = Element.Parent as FrameworkElement;
                }
                if (Element is PesquisarAgenda agenda) {
                    agenda.Close();
                } else {
                    MessageBox.Show("Não foi achado PesquisaAgenda");
                }
            }
        }

        private void DeletarMI_Click(object sender, RoutedEventArgs e) {
            Agendamentos.Delete(ID);
            if (Parent is Panel panel) {
                panel.Children.Remove(this);
            }
        }
    }
}