using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using Correios;

namespace WPFClinica.UserControls {

    public partial class Endereco : UserControl {

        #region Vars

        public string CEP {
            get => CEPTB.Text;
            set => CEPTB.Text = value;
        }

        public string Estado {
            get => EstadoTB.Text;
            set => EstadoTB.Text = value;
        }

        public string Cidade {
            get => CidadeTB.Text;
            set => CidadeTB.Text = value;
        }

        public string Bairro {
            get => BairroTB.Text;
            set => BairroTB.Text = value;
        }

        public string Rua {
            get => RuaTB.Text;
            set => RuaTB.Text = value;
        }

        public string Numero {
            get => NumeroTB.Text;
            set => NumeroTB.Text = value;
        }

        public string Complemento {
            get => ComplementoTB.Text;
            set => ComplementoTB.Text = value;
        }

        #endregion

        public Endereco() {
            InitializeComponent();
        }

        private void CEPTB_TextChanged(object sender, TextChangedEventArgs e) {
            var len = CEP.Length;
            var cep = CEP;
            if (len == 8) {
                Cursor = Cursors.AppStarting;
                Thread t = new Thread(() => {
                    var c = new CorreiosApi();
                    try {
                        var endereco = c.consultaCEP(cep);
                        if (endereco == null) {
                            throw new Exception("A busca não retornou um resultado.");
                        } else {
                            App.Current.Dispatcher.Invoke(() => {
                                Estado = endereco.uf;
                                Cidade = endereco.cidade;
                                Bairro = endereco.bairro;
                                Rua = endereco.end;
                            });
                        }
                    } catch (Exception ex) {
                        MessageBox.Show($"Classe: {ex.GetType()}\nMessagem: {ex.Message}", "Erro o buscar endereço por CEP");
                    } finally {
                        App.Current.Dispatcher.Invoke(() => {
                            Cursor = Cursors.Arrow;
                        });
                    }
                });
                t.Start();
            }
        }
    }
}
