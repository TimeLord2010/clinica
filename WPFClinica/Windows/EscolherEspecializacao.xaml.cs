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
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class EscolherEspecializacao : Window {

        public string Especializacao {
            get => (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString();
        }

        public EscolherEspecializacao() {
            InitializeComponent();
            var r = Especializacoes.SelectLike("");
            EspecializacaoCB.Items.Clear();
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                EspecializacaoCB.Items.Add(new ComboBoxItem() {
                    Content = row.Especializacao,
                    VerticalContentAlignment = VerticalAlignment.Center
                });
            }
            EspecializacaoCB.SelectedIndex = -1;
        }

        private void OkB_Click(object sender, RoutedEventArgs e) {
            if (EspecializacaoCB.SelectedIndex >= 0) {
                App.Current.Properties["esp"] = Especializacao;
                Close();
            } else {
                MessageBox.Show("Escolha a especialização");
            }
        }
    }
}
