using System.Threading;
using System.Windows;
using System.Windows.Input;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class ConfigurationsLogin : Window {

        Properties.Settings Settings {
            get => Properties.Settings.Default;
        }

        public string Server {
            get => Settings.Server;
            set {
                Settings.Server = value;
                Settings.Save();
            }
        }

        public string Banco {
            get => Settings.DB;
            set {
                Settings.DB = value;
                Settings.Save();
            }
        }

        string CS {
            get => Settings.CS.Replace("{Server}", IPTB.Text).Replace("{DB}", BancoTB.Text);
        }

        public ConfigurationsLogin() {
            InitializeComponent();
            WindowsSO.ToCenterOfScreen(this);
            IPTB.Text = Server;
            BancoTB.Text = Banco;
        }

        private void SaveB_Click(object sender, RoutedEventArgs e) {
            Server = IPTB.Text;
            Banco = BancoTB.Text;
            Close();
        }

        private void CriarB_Click(object sender, RoutedEventArgs e) {
            App.Current.Properties["sql"] = new MySqlH(CS);
            Cursor = Cursors.Wait;
            CriarB.IsEnabled = false;
            Thread t = new Thread(() => {
                Database.EnsureCreation();
                App.Current.Dispatcher.Invoke(() => {
                    Cursor = Cursors.Arrow;
                    CriarB.IsEnabled = true;
                });
            });
            t.Start();
        }
    }
}
