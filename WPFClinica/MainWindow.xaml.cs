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
using WPFClinica.Pages;
using WPFClinica.Scripts;

namespace WPFClinica {

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            StaticLogger.Logger = new Logger($"Log.log");
            //ErrorHandler.OnError = (ex) => {
            //    MessageBox.Show($"{ex.Message}.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            //    StaticLogger.WriteLine(ex);
            //};
            Content = new Login();
        }

        protected override void OnClosed(EventArgs e) {
            Environment.Exit(Environment.ExitCode);
            base.OnClosed(e);
        }
    }
}
