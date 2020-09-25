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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using WPFClinica.Windows;

namespace WPFClinica.Pages {

    public partial class ConveniosAdmin : Page {

        #region Vars

        TimedEvent TimedEvent;

        string NomeConvenio {
            get => ConvenioTB.Text;
        }

        #endregion

        public ConveniosAdmin() {
            InitializeComponent();
            ControlsH.CreateColumns(ConveniosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 2, true, UpdateConvenios);
            TimedEvent.TriggerNow();
        }

        void UpdateConvenios() {
            ConveniosDG.Items.Clear();
            var r = Convenios.SelectLike(NomeConvenio);
            foreach (var item in r) {
                ConveniosDG.Items.Add(new Table1() {
                    ID = item.ID,
                    Convenio = item.Convenio
                });
            }
        }

        private void ConvenioTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            var a = new AddConvenio();
            a.ShowDialog();
            UpdateConvenios();
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            if (ConveniosDG.SelectedItem is Table1 table1) {
                var a = new AddConvenio(table1.ID);
                a.ShowDialog();
            }
            UpdateConvenios();
        }

        private void RemoverB_Click(object sender, RoutedEventArgs ev) {
            try {
                if (ConveniosDG.SelectedItem is Table1 table1) {
                    Convenios.Delete(table1.ID);
                }
                UpdateConvenios();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            UpdateConvenios();
        }

        class Table1 {

            public int ID;
            public string Convenio { get; set; }

        }

    }
}
