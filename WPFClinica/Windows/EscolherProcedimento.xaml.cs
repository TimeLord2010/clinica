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

    public partial class EscolherProcedimento : Window {

        #region Vars

        TimedEvent TimedEvent;

        string Nome_Procedimento {
            get => NomeProcedimentoTB.Text;
            set => NomeProcedimentoTB.Text = value;
        }

        #endregion

        public EscolherProcedimento() {
            InitializeComponent();
            ControlsH.CreateColumns(ProcedimentosDG, typeof(ProcedimentosLab));
            TimedEvent = new TimedEvent(1,3,true, UpdateProcedimentos);
            TimedEvent.TriggerNow();
        }

        void UpdateProcedimentos () {
            ProcedimentosDG.Items.Clear();
            var r = ProcedimentosLab.SelectLike2(Nome_Procedimento);
            for (int i = 0; i < r.Count; i++) {
                ProcedimentosDG.Items.Add(r[i]);
            }
        }

        private void EscolherB_Click(object sender, RoutedEventArgs e) {
            Close();
        }

        private void NomeProcedimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        protected override void OnClosing(CancelEventArgs e) {
            ProcedimentosLab a = null;
            if (ProcedimentosDG.SelectedItem is ProcedimentosLab procedimentos) {
                a = procedimentos;
            }
            App.Current.Properties["procedimento"] = a;
            base.OnClosing(e);
        }

    }
}
