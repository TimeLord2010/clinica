using MicrosoftWord;
using MicrosoftWord.DocumentElements;
using MicrosoftWord.Styles;
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

namespace WPFClinica.Pages {

    public partial class HistoricoProcsAdmin : Page {

        #region Vars

        TimedEvent TimedEvent;
        CultureInfo Brasil = new CultureInfo("pt-BR");
        Dictionary<string, double> dict = new SafeDict<string, double>(0);

        string Procedimento {
            get => ProcedimentoTB.Text;
            set => ProcedimentoTB.Text = value;
        }

        string Tipo {
            get {
                if (TipoCB.SelectedIndex < 1) {
                    return "";
                }
                var a = (TipoCB.SelectedItem as ComboBoxItem).Content.ToString();
                return a;
            }
        }

        DateTime Inicio {
            get => Converter.Converter_DT(InicioTB.Text, Fim.AddMonths(-1));
        }

        DateTime Fim {
            get => Converter.Converter_DT(FimTB.Text, DateTime.Now).AddDays(1);
        }

        #endregion

        public HistoricoProcsAdmin() {
            InitializeComponent();
            ControlsH.CreateColumns(HistoricoProcedimentosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 2, true, UpdateProcedimentos);
            TimedEvent.TriggerNow();
        }

        public void UpdateProcedimentos() {
            HistoricoProcedimentosDG.Items.Clear();
            dict.Clear();
            var r = Historico_Procedimento.Select_Realizado(Procedimento, Tipo, Inicio, Fim);
            var pfp = new Procedimento_FormaPagamentos();
            dict.Add("Total", 0);
            foreach (var hp in r) {
                var (cpfMedico, nomeMedico) = Procedimento_Medico.Select(hp.ID);
                var item = new Table1() {
                    CPF = hp.CPF,
                    Proc_ID = hp.Proc_ID,
                    NomePaciente = hp.Nome,
                    Medico = nomeMedico ?? "[?]",
                    Procedimento = hp.Procedimento,
                    Tipo = hp.Tipo,
                    RealizadoEm = hp.RealizadoEm.ToString("dd-MM-yyyy HH:mm"),
                    PagoEm = hp.PagoEm.HasValue ? hp.PagoEm.Value.ToString("dd-MM-yyyy HH:mm") : "[?]",
                    ValorMédico = $"{Converter.DoubleToString(hp.Valor * 0.7)}",
                    ValorClínica = $"{Converter.DoubleToString(hp.Valor * 0.3)}",
                    Valor = hp.Valor.ToString(Brasil)
                };
                dict["Total"] += hp.Valor;
                var fps = pfp.Select(hp.ID);
                foreach (var fp in fps) {
                    item.Valor += $"\n{fp.Pagamento.Descricao} = {Converter.DoubleToString(fp.Valor)}";
                    if (!dict.ContainsKey(fp.Pagamento.Descricao)) dict.Add(fp.Pagamento.Descricao, 0);
                    dict[fp.Pagamento.Descricao] += fp.Valor;
                }
                HistoricoProcedimentosDG.Items.Add(item);
            }
        }

        private void ProcedimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void TipoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void InicioTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void FimTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        class Table1 {
            public int Proc_ID;
            public string CPF;
            public string NomePaciente { get; set; }
            public string Medico { get; set; }
            public string Procedimento { get; set; }
            public string Tipo { get; set; }
            public string RealizadoEm { get; set; }
            public string PagoEm { get; set; }
            public string ValorMédico { get; set; }
            public string ValorClínica { get; set; }
            public string Valor { get; set; }
        }

        private void ImprimirB_Click(object sender, RoutedEventArgs e) {
            try {
                var wd = new WordDocument() { 
                    Orientation = MicrosoftWord.Orientation.Landscape
                };
                wd.Content.Add(new _Paragraph("Relatório de Exames de Imagem") {
                    ParagraphStyle = new ParagraphStyle() {
                        Bold = true,
                        Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter,
                        FontSize = 18
                    }
                });
                wd.Content.Add(new _Paragraph($"{Inicio.ToString("dd/MM/yyyy")} - {Fim.ToString("dd/MM/yyyy")}") {
                    ParagraphStyle = new ParagraphStyle() {
                        Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter
                    }
                });
                wd.Content.Add("");
                wd.Content.Add(new _Paragraph($"Requerido por: {Pessoas.Select(App.Current.Properties["cpf"] as string).Nome}."));
                wd.Content.Add($"Data de requisição: {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}.");
                var table = new _Table( HistoricoProcedimentosDG.Items.Count + 1, 9);
                int i = 0;
                var table_headerS = new ParagraphStyle() { 
                    Bold = true
                };
                table.Insert(0, i++, new _Paragraph("Paciente", table_headerS));
                table.Insert(0, i++, new _Paragraph("Médico", table_headerS));
                table.Insert(0, i++, new _Paragraph("Procedimento", table_headerS));
                table.Insert(0, i++, new _Paragraph("Tipo", table_headerS));
                table.Insert(0, i++, new _Paragraph("Realização", table_headerS));
                table.Insert(0, i++, new _Paragraph("Pago em", table_headerS));
                table.Insert(0, i++, new _Paragraph("Valor do médico", table_headerS));
                table.Insert(0, i++, new _Paragraph("Valor Clínica", table_headerS));
                table.Insert(0, i++, new _Paragraph("Total", table_headerS));
                wd.Content.Add(table);
                for (i = 0; i < HistoricoProcedimentosDG.Items.Count; i++) {
                    if (HistoricoProcedimentosDG.Items[i] is Table1 t) {
                        var j = 0;
                        table.Insert(i+1, j++, $"{t.NomePaciente}");
                        table.Insert(i+1, j++, $"{t.Medico}");
                        table.Insert(i+1, j++, $"{t.Procedimento}");
                        table.Insert(i+1, j++, $"{t.Tipo}");
                        table.Insert(i+1, j++, $"{t.RealizadoEm}");
                        table.Insert(i+1, j++, $"{t.PagoEm}");
                        table.Insert(i+1, j++, $"{t.ValorMédico}");
                        table.Insert(i+1, j++, $"{t.ValorClínica}");
                        table.Insert(i+1, j++, $"{t.Valor}");
                    }
                }
                wd.Content.Add("");
                foreach (var kv in dict) {
                    wd.Content.Add($"{kv.Key}: {kv.Value}.");
                }
                wd.Create();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro ao produzir documento.");
            }
        }
    }
}
