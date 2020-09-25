using System.Windows;

namespace WPFClinica.Windows {

    public partial class EscolherTipoProntuário : Window {

        public EscolherTipoProntuário() {
            InitializeComponent();
        }

        private void WordDocumentB_Click(object sender, RoutedEventArgs e) {
            DialogResult = false;
            Close();
        }

        private void WindowB_Click(object sender, RoutedEventArgs e) {
            DialogResult = true;
            Close();
        }
    }
}
