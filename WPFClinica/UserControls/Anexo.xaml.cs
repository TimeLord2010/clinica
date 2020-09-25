using System;
using System.Collections.Generic;
using System.IO;
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
using WPFClinica.Windows;

namespace WPFClinica.UserControls {

    public partial class Anexo : UserControl {

        MySqlH sql;
        public string CPF = null;
        public string Original;

        public string Arquivo {
            get => TitleTBL.Text + ExtentionTBL.Text;
        }

        public Anexo(string file) {
            InitializeComponent();
            sql = App.Current.Properties["sql"] as MySqlH;
            Original = file;
            DownloadI.Source = ImageH.ToBitmapImage(Properties.Resources.download);
            var tt = new ToolTip();
            tt.Content = file;
            TitleTBL.ToolTip = tt;
            ExtentionTBL.Text = file.Substring(file.LastIndexOf('.'));
            int j = file.LastIndexOf('\\');
            if (j != -1) {
                file = file.Substring(j);
                TitleTBL.Text = file.Substring(1, file.LastIndexOf('.') - 1);
            } else {
                TitleTBL.Text = file;
            }
        }

        private void RemoveI_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (CPF != null) {
                var r = MessageBox.Show($"Vocês está prestes a deletar o anexo {Arquivo}. Deseja continuar? ", "Confirmar", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (r == MessageBoxResult.Yes) {
                    Anexos.Delete(CPF, Arquivo);
                }
            }
            if (Parent is Panel panel) {
                panel.Children.Remove(this);
            }
        }

        private void DownloadI_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (CPF != null) {
                if (WindowsSO.ChooseFolder(out string path)) {
                    var anexo = Anexos.Select(CPF, Arquivo);
                    using (var s = new FileStream($"{path}\\{anexo.Nome}", FileMode.Create, FileAccess.Write)) {
                        s.Write(anexo._Data, 0 , anexo.Tamanho);
                    }
                }
            } else {
                if (WindowsSO.ChooseFolder(out string path)) {
                    var fi = new FileInfo(Arquivo);
                    File.Move(Arquivo, $"{path}\\{fi.Name}");
                }
            }
        }
    }
}
