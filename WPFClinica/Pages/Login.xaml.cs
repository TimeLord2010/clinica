using System;
using System.Collections.Generic;
using System.IO;
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
using WPFClinica.Scripts.DB;
using WPFClinica.Windows;

namespace WPFClinica.Pages {
    public partial class Login : Page {

        #region Vars

        string Old_cpf = "";
        string CPF {
            get {
                return CPFTB.Text;
            }
            set {
                CPFTB.Text = value;
            }
        }

        string Password {
            get {
                return SenhaPB.Password;
            }
            set {
                SenhaPB.Password = value;
            }
        }

        Properties.Settings Settings {
            get => Properties.Settings.Default;
        }

        string LastCPF {
            get => Settings.LastCPF;
            set {
                Settings.LastCPF = value;
                Settings.Save();
            }
        }

        bool LembrarCPF {
            get => Settings.RememberCPF;
            set {
                Settings.RememberCPF = value;
                Settings.Save();
            }
        }

        string CS {
            get => Settings.CS.Replace("{Server}", Settings.Server).Replace("{DB}", Settings.DB);
        }

        Window W;

        bool IsEditing = false;

        #endregion

        public Login() {
            InitializeComponent();
            W = App.Current.MainWindow;
            int width = 200, height = 300;
            W.MinHeight = height;
            W.Height = height;
            W.MinWidth = width;
            W.Width = width;
            W.Title = "Entrar";
            W.WindowStyle = WindowStyle.ToolWindow;
            W.ShowInTaskbar = true;
            WindowsSO.ToCenterOfScreen(W);
            GearI.Source = ImageH.ToBitmapImage(Properties.Resources.gear);
            LembrarCPFChB.IsChecked = LembrarCPF;
            CPF = LastCPF;
            CPFTB.Focus();
            CPFTB.SelectAll();
        }

        private void EncaminhamentosB_Click(object sender, RoutedEventArgs e) {
            var sql = new MySqlH(CS);
            App.Current.Properties["sql"] = sql;
            W.Content = new Encaminhamentos();
        }

        private void LoginB_Click(object sender, RoutedEventArgs e) {
            App.Current.Properties["sql"] = new MySqlH(CS);
            App.Current.Properties["database"] = Settings.DB;
            try {
                SQLOperations.ThrowExceptions = true;
                if (Funcionarios.Authenticate(CPF, Password)) {
                    LembrarCPF = LembrarCPFChB.IsChecked ?? false;
                    LastCPF = LembrarCPF ? CPF : "";
                    W.Content = new Funcionario(CPF);
                } else {
                    MessageBox.Show("CPF ou Senha não correspondem as informações registradas.", "Erro", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            } catch (Exception) {
            } finally {
                SQLOperations.ThrowExceptions = false;
            }
        }

        private void GearI_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var c = new ConfigurationsLogin {
                Owner = W
            };
            c.ShowDialog();
        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (IsEditing) return;
            IsEditing = true;
            int ci = CPFTB.CaretIndex;
            int len = CPF.Length;
            CPF = CPF.Replace("-", "");
            len -= CPF.Length;
            ci -= len;
            len = CPF.Length;
            CPF = CPF.Replace(".", "");
            len -= CPF.Length;
            ci -= len;
            string n = "";
            for (int i = 0; i < CPF.Length; i++) {
                string c = CPF[i] + "";
                if (Data.Any((ii) => ii == i, 3, 6, 9)) {
                    if (i == 9) {
                        if (c != "-") {
                            ci++;
                            n += "-";
                        }
                    } else {
                        if (c != ".") {
                            ci++;
                            n += ".";
                        }
                    }
                }
                n += c;
            }
            CPF = n;
            CPFTB.CaretIndex = ci < 0 ? 0 : ci;
            if (Old_cpf == CPF) {
                string _n = "";
                for (int i = 0; i < CPF.Length; i++) {
                    if (i == ci - 1) continue;
                    _n += CPF[i];
                }
                CPF = _n;
            }
            Old_cpf = CPF;
            IsEditing = false;
        }

        private void SenhaPB_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) LoginB_Click(null, null);
        }

        void ToogleEditing(bool a) {
            Cursor = a ? Cursors.Arrow : Cursors.Wait;
            CPFTB.IsEnabled = SenhaPB.IsEnabled = LembrarCPFChB.IsEnabled = LoginB.IsEnabled = EncaminhamentosB.IsEnabled = GearI.IsEnabled = a;
        }

    }
}
