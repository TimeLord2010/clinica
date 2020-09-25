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

namespace WPFClinica.Windows {

    public partial class ObterTexto : Window {

        public ObterTexto(string textLabel, string buttonLabel) {
            InitializeComponent();
            TituloTBL.Text = textLabel;
            IrB.Content = buttonLabel;
        }

        private void MyStackPanel_Loaded(object sender, RoutedEventArgs e) {
            Width = MyStackPanel.ActualWidth + 20;
        }

        private void IrB_Click(object sender, RoutedEventArgs e) {
            App.Current.Properties["resposta"] = EntradaTB.Text;
            Close();
        }
    }
}
