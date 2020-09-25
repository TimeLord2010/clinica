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

    public partial class EscolherSala : Window {

        #region Vars 

        TimedEvent TimedEvent;

        string Funcionario {
            get => FuncionarioTB.Text;
        }

        string Sala {
            get => SalaTB.Text;
        }

        Fila Fila {
            get {
                switch (FuncaoCB.SelectedIndex) {
                    case 0:
                        return Fila.Triagem;
                    case 1:
                        return Fila.Consultorio;
                    case 2:
                        return Fila.Laboratorio;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        SStatus Status {
            get {
                switch (StatusCB.SelectedIndex) {
                    case 0:
                        return SStatus.EmAtendimento;
                    case 1:
                        return SStatus.Ocupado;
                    case 2:
                        return SStatus.Livre;
                    case 3:
                        return SStatus.Aguardando;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        string Especializacao {
            get => (EspecializacaoCB.SelectedItem as ComboBoxItem).Content.ToString();
        }

        #endregion

        public EscolherSala() {
            InitializeComponent();
            ControlsH.CreateColumns(SalasDG, typeof(Table1));
            TimedEvent = new TimedEvent(1,3, true, UpdateSalas);
            var r = Especializacoes.SelectLike("");
            for (int i = -1; i < r.Count; i++) {
                EspecializacaoCB.Items.Add(new ComboBoxItem() {
                    Content = i < 0? "[Todas]" : r[i].Especializacao
                });
            }
            EspecializacaoCB.SelectedIndex = 0;
            UpdateSalas();
        }

        void UpdateSalas () {
            SalasDG.Items.Clear();
            var r = Salas.SelectLike(Funcionario, Sala, Fila, Status);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                SalasDG.Items.Add(new Table1() {
                    NomeFuncionario = row.Nome_Funcionario,
                    Sala = row.Nome,
                    Função = row.Funcao,
                    Status = row.Status
                });
            }
        }

        private void OkB_Click(object sender, RoutedEventArgs e) {
            if (SalasDG.SelectedItem is Table1 table1) {
                App.Current.Properties["sala"] = table1.Sala;
                Close();
            }
        }

        private void SalasDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            OkB.IsEnabled = SalasDG.SelectedIndex >= 0;
        }

        class Table1 {

            public string NomeFuncionario { get; set; }
            public string Sala { get; set; }
            public string Função { get; set; }
            public string Status { get; set; }

        }

        private void FuncionarioTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void SalaTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void FuncaoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void StatusCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void EspecializacaoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }
    }
}
