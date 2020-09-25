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
using WPFClinica.UserControls;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Windows {

    public enum ChooseMode {
        View, ChooseCreate, ChooseCreated
    }

    public partial class PesquisarAgenda : Window {

        #region Vars

        MySqlH sql;
        ChooseMode ChooseMode;
        string CPF;

        string Ano {
            get => YearTBL.Text;
            set => YearTBL.Text = value;
        }

        string Intervalo {
            get => IntervaloTBL.Text;
            set => IntervaloTBL.Text = value;
        }

        DateTime LIntervalo {
            get {
                var l = Intervalo.Split('-')[0];
                l = l.Substring(0, l.Length - 1);
                var day = ToInt32(l.Split('/')[0]);
                //var month = ToInt32(l.Split('/')[1]);
                var month = Segunda.Month;
                var year = 0;
                if (Ano.Contains("-")) {
                    var lyear = Ano.Split('-')[0];
                    lyear = lyear.Substring(0, lyear.Length - 1);
                    year = ToInt32(lyear);
                } else {
                    year = ToInt32(Ano);
                }
                return new DateTime(year, month, day);
            }
            set {
                var RInterval = value.AddDays(5);
                if (value.Year != RInterval.Year) {
                    Ano = $"{value.Year} - {RInterval.Year}";
                } else {
                    Ano = value.Year + "";
                }
                var l = $"{value.Day}/{Month(value.Month)}";
                var r = $"{RInterval.Day}/{Month(RInterval.Month)}";
                Intervalo = $"{l} - {r}";
                Segunda = value;
                Terca = value.AddDays(1);
                Quarta = value.AddDays(2);
                Quinta = value.AddDays(3);
                Sexta = value.AddDays(4);
                Sabado = value.AddDays(5);
            }
        }

        DateTime Segunda {
            get => Get(MondayTBL);
            set => MondayTBL.Text = $"{value.Day} / {value.Month}";
        }

        DateTime Terca {
            get => Get(TercaTBL);
            set => TercaTBL.Text = $"{value.Day} / {value.Month}";
        }

        DateTime Quarta {
            get => Get(QuartaTBL);
            set => QuartaTBL.Text = $"{value.Day} / {value.Month}";
        }

        DateTime Quinta {
            get => Get(QuintaTBL);
            set => QuintaTBL.Text = $"{value.Day} / {value.Month}";
        }

        DateTime Sexta {
            get => Get(SextaTBL);
            set => SextaTBL.Text = $"{value.Day} / {value.Month}";
        }

        DateTime Sabado {
            get => Get(SabadoTBL);
            set => SabadoTBL.Text = $"{value.Day} / {value.Month}";
        }

        #endregion

        public PesquisarAgenda(ChooseMode choose, string cpf = null) {
            InitializeComponent();
            App.Current.Properties["data"] = null;
            sql = App.Current.Properties["sql"] as MySqlH;
            var dt = DateTime.Now;
            while (dt.DayOfWeek != DayOfWeek.Monday) {
                dt = dt.AddDays(-1);
            }
            LIntervalo = dt;
            ChooseMode = choose;
            if (choose == ChooseMode.View) {

            } else if (choose == ChooseMode.ChooseCreate) {
                var c = Cursors.Hand;
                MondaySP.Cursor = TercaSP.Cursor = QuartaSP.Cursor = QuintaSP.Cursor = SextaSP.Cursor = SabadoSP.Cursor = c;
                MondaySP.MouseLeftButtonDown += MondaySP_MouseLeftButtonDown;
                TercaSP.MouseLeftButtonDown += TercaSP_MouseLeftButtonDown;
                QuartaSP.MouseLeftButtonDown += QuartaSP_MouseLeftButtonDown;
                QuintaSP.MouseLeftButtonDown += QuintaSP_MouseLeftButtonDown;
                SextaSP.MouseLeftButtonDown += SextaSP_MouseLeftButtonDown;
                SabadoSP.MouseLeftButtonDown += SabadoSP_MouseLeftButtonDown;
            } 
            if (cpf != null) {
                SelecionarB.IsEnabled = false;
                CPF = cpf;
                var pessoa = Pessoas.Select(cpf);
                if (pessoa != null) {
                    NomeTB.Text = pessoa.Nome;
                }
            }
            UpdateSchedules();
        }

        void UpdateSchedules() {
            var container = ScheduledG.Children.OfType<StackPanel>();
            for (int i = 0; i < container.Count(); i++) {
                container.ElementAt(i).Children.Clear();
            }
            var r = Agendamentos.Select(Segunda, Sabado, CPF);
            for (int i = 0; i < r.Count; i++) {
                var agendamento = r[i];
                var a = agendamento.Inicio;
                var b = agendamento.Fim;
                string nome;
                AgendamentoTempPaciente tp;
                if (CPF == null) {
                    tp = AgendamentoTempPaciente.Select(agendamento.ID);
                } else {
                    tp = AgendamentoTempPaciente.Select(agendamento.ID, CPF);
                }
                if (tp == null) {
                    AgendamentoPaciente rr;
                    if (CPF == null) {
                        rr = AgendamentoPaciente.Select(agendamento.ID);
                    } else {
                        rr = AgendamentoPaciente.Select(agendamento.ID, CPF);
                    }
                    if (rr != null) {
                        var pessoa = Pessoas.Select(rr.Paciente);
                        nome = pessoa.Nome;
                    } else {
                        nome = "[Não foi encontrado paciente]";
                    }
                } else {
                    nome = tp.TempPaciente;
                }
                var p = new LinhaAgendamento(ChooseMode, nome, $"{a.Hours:00}:{a.Minutes:00}-{b.Hours:00}:{b.Minutes:00}");
                p.ID = agendamento.ID;
                if (agendamento._Data == Segunda) {
                    MondaySP.Children.Add(p);
                } else if (agendamento._Data == Terca) {
                    TercaSP.Children.Add(p);
                } else if (agendamento._Data == Quarta) {
                    QuartaSP.Children.Add(p);
                } else if (agendamento._Data == Quinta) {
                    QuintaSP.Children.Add(p);
                } else if (agendamento._Data == Sexta) {
                    SextaSP.Children.Add(p);
                } else if (agendamento._Data == Sabado) {
                    SabadoSP.Children.Add(p);
                }
            }
        }

        DateTime Get(TextBlock tb) {
            var a = tb.Text.Replace(" ", "");
            var parts = a.Split('/');
            var day = ToInt32(parts[0]);
            var month = ToInt32(parts[1]);
            var year = Ano.Replace(" ", "");
            if (year.Contains("-")) {
                var parts2 = year.Split('-');
                if (day < 15) {
                    year = parts2[1];
                } else {
                    year = parts2[0];
                }
            }
            return new DateTime(ToInt32(year), month, day);
        }

        string Month(int i) {
            while (i < 0) {
                i += 12;
            }
            switch (i) {
                case 1:
                    return "Janeiro";
                case 2:
                    return "Fevereiro";
                case 3:
                    return "Março";
                case 4:
                    return "Abril";
                case 5:
                    return "Maio";
                case 6:
                    return "Junho";
                case 7:
                    return "Julho";
                case 8:
                    return "Agosto";
                case 9:
                    return "Setembro";
                case 10:
                    return "Outubro";
                case 11:
                    return "Novembro";
                case 12:
                    return "Dezembro";
                default:
                    throw new IndexOutOfRangeException();
            }
        }

        void Conclude(DateTime dt) {
            var a = new EscolherHorario();
            a.ShowDialog();
            if (App.Current.Properties["horario"] is string horario) {
                App.Current.Properties["data"] = dt;
                Close();
            }
        }

        private void AnteriorTB_Click(object sender, RoutedEventArgs e) {
            LIntervalo = LIntervalo.AddDays(-7);
            UpdateSchedules();
        }

        private void ProximoTB_Click(object sender, RoutedEventArgs e) {
            LIntervalo = LIntervalo.AddDays(7);
            UpdateSchedules();
        }

        private void MondaySP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Conclude(Segunda);
        }

        private void TercaSP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Conclude(Terca);
        }

        private void QuartaSP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Conclude(Quarta);
        }

        private void QuintaSP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Conclude(Quinta);
        }

        private void SextaSP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Conclude(Sexta);
        }

        private void SabadoSP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Conclude(Sabado);
        }

        private void SelecionarB_Click(object sender, RoutedEventArgs e) {
            var a = new EscolherMedico();
            a.ShowDialog();
            if (App.Current.Properties["medico"] is Pessoas medico) {
                CPF = medico.CPF;
                NomeTB.Text = medico.Nome;
                UpdateSchedules();
            }
        }

        private void OkTB_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}
