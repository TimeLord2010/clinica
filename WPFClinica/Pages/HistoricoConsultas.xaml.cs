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
using MicrosoftWord;
using MicrosoftWord.DocumentElements;
using MicrosoftWord.Styles;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class HistoricoConsultas : Page {

        #region Vars

        TimedEvent TimedEvent;
        CultureInfo Brasil = new CultureInfo("pt-BR");
        Dictionary<string, double> FormasDePagamento = new Dictionary<string, double>();
        double TotalReceived;

        string Funcionario {
            get => FuncionarioTB.Text;
            set => FuncionarioTB.Text = value;
        }

        string Paciente {
            get => PacienteTB.Text;
        }

        DateTime Inicio {
            get => FromText(InicioTB.Text, Fim.AddMonths(-1));
        }

        DateTime Fim {
            get => FromText(FimTB.Text, DateTime.Now);
        }

        bool IsRetorno {
            get => RetornoChB.IsChecked ?? false;
        }

        bool WasPaid {
            get => PagoChB.IsChecked ?? false;
        }

        bool WasReceived {
            get => RecebidaChB.IsChecked ?? false;
        }

        #endregion

        public HistoricoConsultas() {
            InitializeComponent();
            ControlsH.CreateColumns(HistoricoDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 2, true, UpdateHistorico);
            TimedEvent.TriggerNow();
        }

        DateTime FromText(string input, DateTime @default) {
            try {
                var parts = Array.ConvertAll(input.Split('/'), x => ToInt32(x));
                return new DateTime(parts[2], parts[1], parts[0]);
            } catch (Exception) {
                return @default;
            }
        }

        void UpdateHistorico() {
            HistoricoDG.Items.Clear();
            FormasDePagamento.Clear();
            TotalReceived = 0;
            var r = Historico_Consultas.Select2(Paciente, Funcionario, Inicio, Fim, IsRetorno, WasPaid, WasReceived);
            var cf = new Consultas_FormaPagamento();
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                TotalReceived += row.Valor;
                string vc = "", vf = "", total = $"{row.Valor.ToString(Brasil)}";
                var r3 = cf.Select(row.ID, null);
                foreach (var item in r3) {
                    if (!FormasDePagamento.ContainsKey(item.FormaDePagamento.Descricao)) {
                        FormasDePagamento.Add(item.FormaDePagamento.Descricao, 0);
                    }
                    FormasDePagamento[item.FormaDePagamento.Descricao] += item.Valor;
                    total += $"\n{item.FormaDePagamento.Descricao} = {item.Valor}";
                }
                if (ES.prf[Profissao.Medico].Replace("é", "e") == row.Profissão) {
                    var r2 = Medicos.Select(row.CPF_Funcionario);
                    if (r2 == null) {
                        r2 = new Medicos() { 
                            Porcentagem = "0%"
                        };
                    }
                    r2.Porcentagem = r2.Porcentagem.Replace("%", "");
                    vf = $"({r2.Porcentagem}%)\n";
                    var porcetagem = ToDouble(r2.Porcentagem, Brasil);
                    vc = $"({(100 - porcetagem).ToString(Brasil)}%)\n";
                    porcetagem /= 100;
                    double fvalor = row.Valor * porcetagem;
                    vf += $"{fvalor.ToString(Brasil)}";
                    vc += $"{(row.Valor - fvalor).ToString(Brasil)}";
                } else {
                    vf = "0";
                    vc = $"{row.Valor.ToString(Brasil)}";
                }
                var dt = row.RealizadoEm;
                HistoricoDG.Items.Add(new Table1() {
                    ID = row.ID,
                    Nome_Paciente = row.Nome_Paciente,
                    NomeFuncionario = row.Nome_Funcionario,
                    Data = $"{dt.Day:00}/{dt.Month:00}/{dt.Year:0000}",
                    Horário = $"{dt.Hour:00}:{dt.Minute:00}",
                    Profissão = row.Profissão,
                    Especialização = row.Especializacao,
                    Total = total,
                    ValorClinica = $"{vc}\n",
                    ValorFuncionário = $"{vf}\n"
                });
            }
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            if (HistoricoDG.SelectedItem is Table1 table1) {
                var a = new EditarHistorico(table1.ID);
                a.ShowDialog();
                UpdateHistorico();
            }
        }

        private void FuncionarioTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void PacienteTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void InicioTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void FimTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void HistoricoDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = HistoricoDG.SelectedIndex >= 0;
        }

        class Table1 {

            public int ID;
            public string Nome_Paciente { get; set; }
            public string NomeFuncionario { get; set; }
            public string Data { get; set; }
            public string Horário { get; set; }
            public string Profissão { get; set; }
            public string Especialização { get; set; }
            public string ValorClinica { get; set; }
            public string ValorFuncionário { get; set; }
            public string Total { get; set; }

        }

        private void RetornoChB_Checked(object sender, RoutedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void RetornoChB_Unchecked(object sender, RoutedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void ImprimirB_Click(object sender, RoutedEventArgs e) {
            var document = new MicrosoftWord.WordDocument();
            document.Orientation = MicrosoftWord.Orientation.Landscape;
            var header_style = new ParagraphStyle {
                FontSize = 20,
                Bold = true,
                Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter
            };
            var header = new _Paragraph {
                Text = "Relatório de Consultas",
                ParagraphStyle = header_style
            };
            document.Content.Add(header);
            var subheader_style = new ParagraphStyle {
                Bold = true,
                Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter,
                FontSize = 16
            };
            var subheader = new _Paragraph {
                Text = $"{Inicio.ToString("dd/MM/yyyy")} - {Fim.ToString("dd/MM/yyyy")}",
                ParagraphStyle = subheader_style
            };
            document.Content.Add(subheader);
            document.Content.Add("");
            document.Content.Add($"Requerido por: {App.Current.MainWindow.Title}.");
            document.Content.Add($"Data do requerimento: {DateTime.Now.ToString("dd/MM/yyyy")}.");
            var t = new _Table(HistoricoDG.Items.Count + 1, 8);
            var tableheader_style = new ParagraphStyle() {
                Bold = true
            };
            t.Insert(0, 0, new _Paragraph("Nome do paciente") { ParagraphStyle = tableheader_style });
            t.Insert(0, 1, new _Paragraph("Nome do funcionário") { ParagraphStyle = tableheader_style });
            t.Insert(0, 2, new _Paragraph("Data e horário") { ParagraphStyle = tableheader_style });
            t.Insert(0, 3, new _Paragraph("Profissão") { ParagraphStyle = tableheader_style });
            t.Insert(0, 4, new _Paragraph("Especialização") { ParagraphStyle = tableheader_style });
            t.Insert(0, 5, new _Paragraph("Valor Clínica") { ParagraphStyle = tableheader_style });
            t.Insert(0, 6, new _Paragraph("Valor Funcionário") { ParagraphStyle = tableheader_style });
            t.Insert(0, 7, new _Paragraph("Total") { ParagraphStyle = tableheader_style });
            for (int i = 0, c = 0; i < HistoricoDG.Items.Count; i++, c = 0) {
                var item = HistoricoDG.Items[i] as Table1;
                t.Insert(i + 1, c++, item.Nome_Paciente);
                t.Insert(i + 1, c++, item.NomeFuncionario);
                t.Insert(i + 1, c++, $"{item.Data} {item.Horário}");
                t.Insert(i + 1, c++, item.Profissão);
                t.Insert(i + 1, c++, item.Especialização);
                t.Insert(i + 1, c++, item.ValorClinica);
                t.Insert(i + 1, c++, item.ValorFuncionário);
                t.Insert(i + 1, c++, item.Total);
            }
            document.Content.Add(t);
            document.Content.Add("");
            document.Content.Add($"Total: {TotalReceived}");
            for (int i = 0; i < FormasDePagamento.Count; i++) {
                var item = FormasDePagamento.ElementAt(i);
                document.Content.Add($"{item.Key}: {item.Value.ToString(Brasil)}");
            }
            document.Create();
        }
    }
}
