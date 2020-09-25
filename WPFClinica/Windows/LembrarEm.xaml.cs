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

    public partial class LembrarEm : Window {

        #region Vars

        int Months {
            get {
                if (SixMonthsRB.IsChecked ?? false) {
                    return 6;
                } else if (TwelveMonthsRB.IsChecked ?? false) {
                    return 12;
                } else {
                    return Converter.TryToInt32(CustomTB.Text, 0);
                }
            }
        }

        #endregion

        public LembrarEm() {
            InitializeComponent();
            App.Current.Properties["meses"] = null;
        }

        private void OkB_Click(object sender, RoutedEventArgs e) {
            if (Months <= 0) {
                MessageBox.Show($"Você informou uma entrada inválida de meses.",
                    $"Quantidade inválida de meses.", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            App.Current.Properties["meses"] = Months;
            Close();
        }

        private void CustomRB_Checked(object sender, RoutedEventArgs e) {
            CustomTB.IsEnabled = true;
        }

        private void CustomRB_Unchecked(object sender, RoutedEventArgs e) {
            CustomTB.Text = "";
            CustomTB.IsEnabled = false;
        }
    }
}
