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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using WPFClinica.Windows;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class ProcedimentosLabAdmin : Page {

        #region Vars

        TimedEvent TimedEvent;
        CultureInfo Brasil = new CultureInfo("pt-BR");

        string Procedimento {
            get => ProcedimentoTB.Text;
        }

        #endregion

        public ProcedimentosLabAdmin() {
            InitializeComponent();
            ControlsH.CreateColumns(ProcedimentosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1,2, true, UpdateProcedimentos);
            TimedEvent.TriggerNow();
        }

        void UpdateProcedimentos () {
            ProcedimentosDG.Items.Clear();
            var r = ProcedimentosLab.SelectLike2(Procedimento);
            foreach (var item in r) {
                ProcedimentosDG.Items.Add(new Table1() {
                    Proc_ID = item.ID,
                    Procedimento = item.Procedimento
                });
            }
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            var a = new CriarProcedimentoLab();
            a.ShowDialog();
            UpdateProcedimentos();
        }

        private void DeletarB_Click(object sender, RoutedEventArgs e) {
            if (ProcedimentosDG.SelectedItem is Table1 item) {
                ProcedimentosLab.Delete(item.Proc_ID);
                UpdateProcedimentos();
            }
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            if (ProcedimentosDG.SelectedItem is Table1 item) {
                var a = new CriarProcedimentoLab(item.Proc_ID);
                a.ShowDialog();
                UpdateProcedimentos();
            }
        }

        private void ProcedimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void ProcedimentosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            DeletarB.IsEnabled = EditarB.IsEnabled = ProcedimentosDG.SelectedIndex >= 0;
        }

        class Table1 {

            public int Proc_ID;
            public string Procedimento { get; set; }

        }

        private void PrecosB_Click(object sender, RoutedEventArgs e) {
            if (ProcedimentosDG.SelectedItem is Table1 item) {
                var a = new ProcedimentoConvenios(item.Proc_ID);
                a.ShowDialog();
            }
        }
    }
}
