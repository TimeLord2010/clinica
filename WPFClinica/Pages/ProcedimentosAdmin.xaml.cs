using System;
using System.Collections.Generic;
using System.Globalization;
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
using WPFClinica.Scripts;
using WPFClinica.Windows;

namespace WPFClinica.Pages {

    public partial class ProcedimentosAdmin : Page {

        #region Vars

        TimedEvent TimedEvent;
        CultureInfo Brasil = new CultureInfo("pt-BR");

        int? Tipo_ID {
            get {
                if (TipoCB.SelectedIndex < 0 || TipoCB.SelectedIndex == TipoCB.Items.Count - 1) {
                    return null;
                }
                return TipoProcedimento.Select((TipoCB.SelectedItem as ComboBoxItem).Content.ToString());
            }
        }

        #endregion

        public ProcedimentosAdmin() {
            InitializeComponent();
            Fill();
            ControlsH.CreateColumns(ProcedimentosDG, typeof(Procedimento_Table));
            TimedEvent = new TimedEvent(1, 2, true, UpdateProcedimentos);
            TimedEvent.TriggerNow();
        }

        void UpdateProcedimentos() {
            ProcedimentosDG.Items.Clear();
            if (Tipo_ID == null) {
                return;
            }
            var r = Procedimentos.SelectLike(Tipo_ID.Value, ProcedimentoTB.Text);
            foreach (var item in r) {
                ProcedimentosDG.Items.Add(new Procedimento_Table() {
                    ID = item.ID,
                    Procedimento = item.Descricao,
                    Valor = item.Valor.ToString(Brasil)
                });
            }
        }

        private void TipoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TipoCB.SelectedItem is ComboBoxItem item && item.Content.ToString() == "<Adicionar>") {
                var a = new AddTipoProcedimento();
                a.ShowDialog();
                Fill();
            }
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void ProcedimentosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = RemoverB.IsEnabled = ProcedimentosDG.SelectedIndex >= 0;
        }

        private void ProcedimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            var a = new AddProcedimento();
            a.ShowDialog();
            UpdateProcedimentos();
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            if (ProcedimentosDG.SelectedItem is Procedimento_Table table) {
                var a = new AddProcedimento(table.ID);
                a.ShowDialog();
            }
            UpdateProcedimentos();
        }

        private void RemoverB_Click(object sender, RoutedEventArgs ev) {
            if (ProcedimentosDG.SelectedItem is Procedimento_Table table) {
                Procedimentos.Delete(table.ID);
                UpdateProcedimentos();
            }
        }

        private void Fill () {
            TipoCB.Items.Clear();
            TipoCB.SelectedIndex = -1;
            var r = TipoProcedimento.SelectLike("");
            foreach (var item in r) {
                var a = new ComboBoxItem() {
                    Content = item.Tipo
                };
                a.PreviewMouseRightButtonDown += A_PreviewMouseRightButtonDown;
                TipoCB.Items.Add(a);
            }
            var add = new ComboBoxItem() {
                Content = "<Adicionar>"
            };
            TipoCB.Items.Add(add);
            TipoCB.SelectedIndex = -1;
        }

        private void A_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e) {
            var item = sender as ComboBoxItem;
            if (item == null) {
                MessageBox.Show("Erro ao obter seleção");
                return;
            }
            var id = TipoProcedimento.Select(item);
            if (id == null) {
                MessageBox.Show("Não foi possível obter identificador.");
                return;
            }
            var a = new AddTipoProcedimento(id.Value);
            a.ShowDialog();
            Fill();
        }

        class Procedimento_Table {

            public int ID;
            public string Procedimento { get; set; }
            public string Valor { get; set; }

        }

    }
}
