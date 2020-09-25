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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class EscolherFuncionario : Window {

        #region Vars

        TimedEvent TimedEvent;

        string CPF {
            get => CPFTB.Text;
            set => CPFTB.Text = value;
        }

        string Nome {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        #endregion

        public EscolherFuncionario() {
            InitializeComponent();
            App.Current.Properties["funcionario"] = null;
            WindowsSO.ToCenterOfScreen(this);
            ControlsH.CreateColumns(FuncionariosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 3, true, UpdateFuncionarios);
            TimedEvent.TriggerNow();
        }

        void UpdateFuncionarios() {
            FuncionariosDG.Items.Clear();
            var r = Funcionarios.Select(CPF, Nome);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                FuncionariosDG.Items.Add(new Table1() {
                    CPF = row[0],
                    Nome = row[1]
                });
            }
        }

        private void OkB_Click(object sender, RoutedEventArgs e) {
            if (FuncionariosDG.SelectedItem is Table1 table1) {
                App.Current.Properties["funcionario"] = table1.CPF;
                Close();
            } else {
                MessageBox.Show("Nenhum funcionário selecionado.");
            }
        }

        class Table1 {

            public string CPF { get; set; }
            public string Nome { get; set; }

        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }
    }
}
