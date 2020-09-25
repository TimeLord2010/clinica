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
using MySql.Data.MySqlClient;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFClinica.UserControls;
using WPFClinica.Scripts;

namespace WPFClinica.Pages {

    public partial class Encaminhamentos : Page {

        Window W;
        ColunaEncaminhamentos triagem, consultorio, laboratorio;

        public Encaminhamentos() {
            InitializeComponent();
            W = App.Current.MainWindow;
            W.MinWidth = W.Width = 800;
            W.MinHeight = W.Height = 500;
            W.WindowStyle = WindowStyle.SingleBorderWindow;
            W.PreviewKeyDown += W_PreviewKeyDown;
            WindowsSO.ToCenterOfScreen(W);
            Grid grid = MyGrid;
            triagem = new ColunaEncaminhamentos(Fila.Triagem);
            grid.Children.Add(triagem);
            triagem.Margin = new Thickness(10, 0, 5, 0);
            consultorio = new ColunaEncaminhamentos(Fila.Consultorio) {
                Margin = new Thickness(5, 0, 5, 0)
            };
            grid.Children.Add(consultorio);
            Grid.SetColumn(consultorio, 1);
            laboratorio = new ColunaEncaminhamentos(Fila.Laboratorio) {
                Margin = new Thickness(5, 0, 10, 0)
            };
            grid.Children.Add(laboratorio);
            Grid.SetColumn(laboratorio, 2);
        }
        

        private void W_PreviewKeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.F) {
                W.WindowStyle = WindowStyle.None;
                W.WindowState = WindowState.Maximized;
                W.ResizeMode = ResizeMode.NoResize;
                //W.Topmost = true;
            } else if (e.Key == Key.Escape) {
                W.WindowStyle = WindowStyle.SingleBorderWindow;
                W.WindowState = WindowState.Normal;
                W.ResizeMode = ResizeMode.CanResize;
                //W.Topmost = false;
            }
        }
    }
}
