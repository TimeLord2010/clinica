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
using WPFClinica.Scripts.Entities;
using WPFClinica.Windows;

namespace WPFClinica.Pages {

    public partial class EscolherEmpresa : Page {

        string NomeEmpresa { 
            get => NomeEmpresaTB.Text; 
        }

        string CNPJ {
            get => CNPJ_TB.Text;
        }

        public EscolherEmpresa(bool isChoosing = false) {
            InitializeComponent();
            App.Current.Properties["id"] = null;
            ControlsH.CreateColumns(EmpresasDG, typeof(Table1));
            if (!isChoosing) {
                EscolherB.Visibility = Visibility.Hidden;
            }
            Update();
        }

        void Update() {
            EmpresasDG.Items.Clear();
            var search = Empresa.Select(NomeEmpresa, CNPJ);
            foreach (var item in search) {
                EmpresasDG.Items.Add(Table1.Instantiate(item));
            }
        }

        private void PesquisarTB_Click(object sender, RoutedEventArgs e) {
            Update();
        }

        class Table1 {

            public int ID;
            public string Nome { get; set; }
            public string CNPJ { get; set; }
            public string RegistradoEm { get; set; }
            public string Observação { get; set; }

            public static Table1 Instantiate (_Empresa empresa) {
                return new Table1() { 
                    ID = empresa.ID,
                    Nome = empresa.Nome,
                    CNPJ = empresa.CNPJ,
                    Observação = empresa.Observação,
                    RegistradoEm = empresa.Registro.ToString("dd/MM/yyyy")
                };
            }

        }

        private void CadastroB_Click(object sender, RoutedEventArgs e) {
            var a = new CadastrarEmpresa();
            a.ShowDialog();
            Update();
        }

        private void EmpresasDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = EmpresasDG.SelectedIndex >= 0;
        }

        private void EscolherB_Click(object sender, RoutedEventArgs e) {
            if (EmpresasDG.SelectedItem is Table1 t1) {
                App.Current.Properties["id"] = t1.ID;
                var w = Window.GetWindow(this);
                w.Close();
            } else {
                MessageBox.Show("Nenhum item selecionado!");
            }
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            if (EmpresasDG.SelectedItem is Table1 t1) {
            var a = new CadastrarEmpresa(t1.ID);
            a.ShowDialog();
            Update();
            }
        }
    }
}
