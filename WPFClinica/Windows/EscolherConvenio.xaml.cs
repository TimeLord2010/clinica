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

    public partial class EscolherConvenio : Window {

        #region Vars

        TimedEvent TimedEvent;

        int id_procedimento;
        public int ID_Procedimento {
            get => id_procedimento;
            set {
                id_procedimento = value;
                if (value >= 0) {
                    NomeProcedimentoTB.Text = Nome_Procedimento;
                } else {
                    NomeConvenioTB.Text = "???";
                }
            }
        }
        public string Nome_Procedimento {
            get => ProcedimentosLab.Select(ID_Procedimento);
        }
        string Nome_Convenio {
            get => NomeConvenioTB.Text;
            set => NomeConvenioTB.Text = value;
        }

        #endregion

        public EscolherConvenio(int procedimento) {
            InitializeComponent();
            ID_Procedimento = procedimento;
            ControlsH.CreateColumns(ConveniosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1,3, true, UpdateConvenios);
            TimedEvent.TriggerNow();
        }

        void UpdateConvenios () {
            ConveniosDG.Items.Clear();
            var r = ProcedimentoConvenio.SelectLike(Nome_Procedimento, Nome_Convenio);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                ConveniosDG.Items.Add(new Table1() {
                    Con_ID = row.Con_ID,
                    Convenio = row.Convenio,
                    Valor = row.Valor
                });
            }
        }

        private void NomeConvenioTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void EscolherB_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        protected override void OnClosing(CancelEventArgs e) {
            int a = -1;
            if (ConveniosDG.SelectedItem is Table1 table1) {
                a = table1.Con_ID;
            }
            App.Current.Properties["convenio"] = a;
            base.OnClosing(e);
        }

        class Table1 {

            public int Con_ID;
            public string Convenio { get; set; }
            public double Valor { get; set; }

        }

    }
}
