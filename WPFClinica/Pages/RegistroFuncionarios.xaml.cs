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
using WPFClinica.Windows;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Pages {

    public partial class RegistroFuncionarios : Page {

        Window W;

        bool Inativos {
            get => InativosChB.IsChecked ?? false;
        }

        public RegistroFuncionarios() {
            InitializeComponent();
            W = App.Current.MainWindow;
            W.MinWidth = 750;
            W.MinHeight = 400;
            if (W.WindowState == WindowState.Normal) {
                if (W.Width < 750) {
                    W.Width = 750;
                }
                if (W.Height < 400) {
                    W.Height = 400;
                }
                WindowsSO.ToCenterOfScreen(W);

            }
            CreateColumn("CPF");
            CreateColumn("Nome");
            CreateColumn("Sexo");
            CreateColumn("Nascimento");
            CreateColumn("Administrador");
            CreateColumn("Recepcionista");
            CreateColumn("Médico", "Medico");
            CreateColumn("Técnico em enfermagem", "Tecnico");
            CreateColumn("Ativo");
            UpdateRegistros();
        }

        void CreateColumn(string header, string binding = null) {
            var a = new DataGridTextColumn();
            a.Header = header;
            a.Binding = new Binding(binding == null ? header : binding);
            RegistroDG.Columns.Add(a);
        }

        public void UpdateRegistros() {
            RegistroDG.Items.Clear();
            var r = Funcionarios.Select(CPFTB.Text, NomeTB.Text);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                var t = new Table() {
                    CPF = row[0],
                    Nome = row[1],
                    Sexo = row[2],
                    Nascimento = row[3],
                    Ativo = row[4],
                    Administrador = row[5],
                    Recepcionista = row[6],
                    Tecnico = row[7],
                    Medico = row[8]
                };
                RegistroDG.Items.Add(t);
            }
        }

        void GetSelectedItem(Action<Table> action) {
            if (RegistroDG.SelectedIndex < 0) {
                MessageBox.Show("Nenhum item selecionado");
                return;
            }
            action.Invoke(RegistroDG.SelectedItem as Table);
        }

        private void CadastrarB_Click(object sender, RoutedEventArgs e) {
            var c = new CadastroFuncionario();
            c.ShowDialog();
            UpdateRegistros();
        }

        private void RemoverB_Click(object sender, RoutedEventArgs e) {
            GetSelectedItem((f) => {
                if (MessageBox.Show($"A ação fará com que o funcionário fique inativo no banco de dados. Deseja continuar?", "Confirmar", MessageBoxButton.OKCancel, MessageBoxImage.Warning) == MessageBoxResult.OK) {

                }
            });
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            GetSelectedItem((f) => {
                var c = new CadastroFuncionario(f.CPF);
                c.CadastrarB.Content = "Editar";
                c.Owner = App.Current.MainWindow;
                c.ShowDialog();
                UpdateRegistros();
            });
        }

        private void AtualizarMI_Click(object sender, RoutedEventArgs e) {
            UpdateRegistros();
        }

        class Table {

            public string CPF { get; set; }
            public string Nome { get; set; }
            public string Sexo { get; set; }
            public string Nascimento { get; set; }
            public string Ativo { get; set; }
            public string Administrador { get; set; }
            public string Recepcionista { get; set; }
            public string Tecnico { get; set; }
            public string Medico { get; set; }

        }
    }
}
