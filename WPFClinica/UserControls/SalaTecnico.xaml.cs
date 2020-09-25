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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;

namespace WPFClinica.UserControls {

    public partial class SalaTecnico : UserControl {

        string Tecnico;

        Properties.Settings Settings {
            get => Properties.Settings.Default;
        }

        string Room {
            get => Settings.Room;
            set {
                Settings.Room = value;
                Settings.Save();
            }
        }

        string sala {
            get => NomeTBL2.Text;
            set => NomeTBL2.Text = value;
        }

        public SalaTecnico(string tecnico) {
            InitializeComponent();
            Tecnico = tecnico;
            sala = Room;
        }

        private void MudarNomeMI2_Click(object sender, RoutedEventArgs e) {
            var a = new ObterTexto("Nome da sala","Ir");
            a.ShowDialog();
            if (App.Current.Properties["resposta"] is string resposta) {
                Room = sala = resposta;
                Salas.InsertOrUpdate(Tecnico, sala, SStatus.Ocupado, Fila.Laboratorio);
            }
        }
    }
}
