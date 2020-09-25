using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public partial class EscolherConvenio2 : Window {

        #region Vars

        TimedEvent TimedEvent;

        string Convenio {
            get => ConveniosTB.Text;
        }

        #endregion

        public EscolherConvenio2() {
            InitializeComponent();
            ControlsH.CreateColumns(ConveniosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1,2,true, UpdateConvenios);
            TimedEvent.TriggerNow();
        }

        void UpdateConvenios () {
            ConveniosDG.Items.Clear();
            var r = Convenios.SelectLike(Convenio);
            for (int i = 0; i < r.Count; i++) {
                ConveniosDG.Items.Add(new Table1() {
                    Convenio = r[i].Convenio
                });
            }
        }

        private void ConveniosTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        class Table1 {

            public string Convenio { get; set; }

        }

        private void SelecionarB_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e) {
            var t1 = ConveniosDG.SelectedItem as Table1;
            App.Current.Properties["convenio"] = t1 == null ? null : t1.Convenio;
            base.OnClosing(e);
        }

    }

}
