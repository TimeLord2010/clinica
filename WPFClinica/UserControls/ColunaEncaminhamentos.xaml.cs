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
using System.Windows.Threading;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.UserControls {

    public partial class ColunaEncaminhamentos : UserControl {

        #region Vars

        Fila Fila;
        int Capacity = 3;
        DispatcherTimer DT = new DispatcherTimer();


        bool prioridade;
        public bool Prioridade {
            get {
                return prioridade;
            }
            set {
                prioridade = value;
                if (prioridade) {
                    TipoTBL.Foreground = Brushes.Orange;
                    TipoTBL.Text = "Prioridade";
                } else {
                    TipoTBL.Foreground = Brushes.Black;
                    TipoTBL.Text = "Normal";
                }
            }
        }

        public string Senha {
            get => SenhaTBL.Text;
            set => SenhaTBL.Text = value;
        }

        public string Nome {
            get => NomeTBL.Text;
            set => NomeTBL.Text = value;
        }

        public string Sala {
            get => String.Join("", Data.Tail(SalaTBL.Text.Split(':').ToList()));
            set {
                SalaTBL.Text = "Sala: " + value;
            }
        }

        #endregion

        public ColunaEncaminhamentos(Fila fila) {
            InitializeComponent();
            var title = "";
            Fila = fila;
            if (fila.ToString("G") == "Laboratorio") {
                title = "Laboratório";
            } else if (fila.ToString("G") == "Consultorio") {
                title = "Consultório";
            } else {
                title = "Triagem";
            }
            TitleTBL.Text = title;
            var encaminhados = new LinhaEncaminhados();
            LastCallsSP.Children.Add(encaminhados);
            var encaminhamentos = new LinhaEncaminhamentos();
            NextCallsSP.Children.Add(encaminhamentos);
            DT.Interval = new TimeSpan(0, 0, 2);
            DT.Tick += DT_Tick;
            DT.Start();
        }

        public void AddNextPerson(string nome, string code, bool prioridade) {
            if (NextCallsSP.Children.Count < Capacity) {
                var a = new LinhaEncaminhamentos(nome, code, prioridade);
                NextCallsSP.Children.Insert(1, a);
            }
        }

        void AddCurrentCalled (string senha, string paciente, string sala, bool prioridade) {
            SenhaTBL.Text = senha + "";
            NomeTBL.Text = paciente;
            SalaTBL.Text = $"Sala: {sala}";
            TipoTBL.Text = $"{(prioridade? "Prioridade":"Normal")}";
        }

        void AddLastCalled (string senha, string paciente, string sala) {
            NextCallsSP.Children.Add(new LinhaEncaminhados(senha, paciente, sala));
        }

        private void DT_Tick(object sender, EventArgs e) {
            while (NextCallsSP.Children.Count > 1) NextCallsSP.Children.RemoveAt(1);
            AddCurrentCalled("??", "??", "??", false);
            var r = ListaEspera.SelectLike("", "", Fila, PStatus.Esperando);
            for (int i = 0; i < r.Count && i < Capacity; i++) {
                var row = r[i];
                AddNextPerson(row.Nome, row.Senha + "", row.Prioridade);
            }
            while (LastCallsSP.Children.Count > 1) LastCallsSP.Children.RemoveAt(1);
            r = ListaEspera.SelectLike("","", Fila, PStatus.ACaminho);
            for (int i = 0; i < r.Count && i < 5; i++) {
                var row = r[i];
                var ps = Paciente_Sala.Select_Paciente(row.CPF);
                var s = Salas.Select(ps.Funcionario);
                if (i == 0) {
                    AddCurrentCalled(row.Senha+"", row.Nome, s.Value.Nome , row.Prioridade);
                } else {
                    AddLastCalled(row.Senha+"", row.Nome, s.Value.Nome);
                }
            }
        }

    }
}
