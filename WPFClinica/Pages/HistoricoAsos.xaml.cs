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
using WPFClinica.Scripts.DB;

namespace WPFClinica.Pages {

    public partial class HistoricoAsos : Page {

        public string NomePaciente { get => NomePacienteTB.Text; }
        public string NomeEmpresa { get => NomeEmpresaTB.Text; }
        public string CNPJ { get => CNPJ_TB.Text; }
        public DateTime Inicio { get => Converter.Converter_DT(InicioTB.Text, Fim.AddMonths(-1)); }
        public DateTime Fim { get => Converter.Converter_DT(FimTB.Text, DateTime.Now); }

        public HistoricoAsos() {
            InitializeComponent();
            ControlsH.CreateColumns(ASO_DG, typeof(Table1));
            Update();
        }

        void Update () {
            ASO_DG.Items.Clear();
            var r = Asos.Select(NomePaciente, NomeEmpresa, CNPJ, Inicio, Fim);
            foreach (var item in r) {
                ASO_DG.Items.Add(new Table1() { 
                    Paciente = item.Paciente.Nome,
                    Nascimento = Data.Age(item.Paciente.Nascimento) + " anos",
                    Empresa = item.Empresa.Nome,
                    CNPJ = item.Empresa.CNPJ,
                    Realizacao = item.RealizadoEm.ToString("dd/MM/yyyy"),
                    NotificarEm = item.NotificadoEm.ToString("dd/MM/yyyy"),
                    Notificado = item.Notificado ? "Sim" : "Não"
                });
            }
        }

        private void PesquisarB_Click(object sender, RoutedEventArgs e) {
            Update();
        }

        class Table1 {

            public string Paciente { get; set; }
            public string Nascimento { get; set; }
            public string Empresa { get; set; }
            public string CNPJ { get; set; }
            public string Realizacao { get; set; }
            public string NotificarEm { get; set; }
            public string Notificado { get; set; }

        }

    }
}
