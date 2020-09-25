using System;
using System.Threading;
using System.Windows;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class AlertaASO : Window {

        public AlertaASO() {
            InitializeComponent();
            ControlsH.CreateColumns(AsosDG, typeof(Table1));
            Update();
        }

        public bool ShouldClose = false;

        public new void Show () {
            if (!ShouldClose) base.Show();
        }

        public void Update() {
            AsosDG.Items.Clear();
            var r = Asos.Select("", "", "", DateTime.MinValue, DateTime.Now, false);
            if (r.Count == 0) {
                ShouldClose = true;
                Close();
                return;
            }
            foreach (var item in r) {
                AsosDG.Items.Add(new Table1 {
                    Paciente = item.Paciente.Nome,
                    Empresa = item.Empresa.Nome,
                    AlertarEm = item.NotificadoEm.ToString("dd/MM/yyyy"),
                    UltimaVisita = item.RealizadoEm.ToString("dd/MM/yyyy")
                });
            }
            var t = new Thread(() => {
                foreach (var item in r) {
                    Asos.Update(item.Paciente.CPF, true);
                }
            });
            t.Start();
        }

        public class Table1 {
            public string Paciente { get; set; }
            public string UltimaVisita { get; set; }
            public string AlertarEm { get; set; }
            public string Empresa { get; set; }
        }

    }
}
