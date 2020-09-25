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
using WPFClinica.Scripts.DB;
using WPFClinica.Windows;

namespace WPFClinica.UserControls {

    public partial class SalaMedico : UserControl {

        #region Vars

        private readonly string Funcionario;

        Properties.Settings Settings {
            get => Properties.Settings.Default;
        }

        string Room {
            get => Settings.Room;
            set {
                Salas.InsertOrUpdate(Funcionario, sala, SStatus.Ocupado, Fila);
                Settings.Room = value;
                Settings.Save();
            }
        }

        public Fila Fila {
            get {
                if (TriagemRB.IsChecked ?? false) {
                    return Fila.Triagem;
                } else if (ConsultorioRB.IsChecked ?? false) {
                    return Fila.Consultorio;
                } else {
                    return Fila.Laboratorio;
                }
            }
        }

        string sala {
            get => NomeTBL.Text;
            set => NomeTBL.Text = value;
        }

        #endregion

        public SalaMedico(string funcionario) {
            InitializeComponent();
            Funcionario = funcionario;
            sala = Room;
        }

        private void MudarNomeMI_Click(object sender, RoutedEventArgs e) {
            ObterTexto a = new ObterTexto("Nome da sala","Ir");
            a.ShowDialog();
            if (App.Current.Properties["resposta"] is string resposta) {
                Room = sala = resposta;
            }
        }

        private void TriagemRB_Checked(object sender, RoutedEventArgs e) {
            Salas.InsertOrUpdate(Funcionario, sala, SStatus.Ocupado, Fila);
        }
    }
}
