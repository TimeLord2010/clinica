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
using WPFClinica.Scripts.DB;

namespace WPFClinica.UserControls {

    public partial class Especializacao : UserControl {

        public string Valor {
            get => (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString();
        }

        public Especializacao(string especalizacao = null) {
            InitializeComponent();
            try {
                EspecializacaoCB.Items.Clear();
                var esps = Especializacoes.SelectLike("");
                if (esps.Count == 0) {
                    MessageBox.Show("Nenhuma especializacao cadastrada");
                    Image_MouseDown(null, null);
                }
                for (int i = 0; i < esps.Count; i++) {
                    EspecializacaoCB.Items.Add(new ComboBoxItem() {
                        Content = esps[i].Especializacao
                    });
                }
                EspecializacaoCB.SelectedIndex = 0;
                if (especalizacao != null) {
                    bool found = false;
                    for (int i = 0; i < EspecializacaoCB.Items.Count; i++) {
                        string a = (EspecializacaoCB.Items[i] as ComboBoxItem).Content.ToString();
                        if (a == especalizacao) {
                            EspecializacaoCB.SelectedIndex = i;
                            found = true;
                            break;
                        }
                    }
                    if (!found) {
                        MessageBox.Show($"Não foi encontrada a especializacao no elemento Especialização (UC).\nEspecialização: {especalizacao}");
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro ao carregar selecionador de especializacao.");
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e) {
            var parent = this.Parent;
            if (parent is Panel panel) {
                panel.Children.Remove(this);
            }
        }
    }
}
