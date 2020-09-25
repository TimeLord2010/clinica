using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WPFClinica.Windows;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using System.Globalization;
using System.IO;
using System.Text;
using MicrosoftWord;
using MicrosoftWord.Styles;
using MicrosoftWord.DocumentElements;
using static System.Convert;
using System.Collections.Generic;
using MicrosoftWord.Other;

namespace WPFClinica.Pages {

    public partial class FazerOrcamento : Page {

        #region Vars

        CultureInfo brasil = new CultureInfo("pt-BR");

        TimedEvent TimedEvent;
        TimedEvent TimedEvent2;

        string convenio_selecionado;
        string ConvenioSelecionado {
            get => convenio_selecionado;
            set {
                convenio_selecionado = value;
                UpdateProcedimentos();
            }
        }

        string NomeEstabelecimento {
            get => NomeEstabelecimentoTB.Text;
        }

        string Endereco {
            get => EnderecoTB.Text;
        }

        string Contato {
            get => ContatoTB.Text;
        }

        string CPF {
            get => CPFTB.Text;
        }

        string NomePaciente {
            get => NomePacienteTB.Text;
        }

        string Nascimento {
            get => NascimentoTB.Text;
        }

        string Sexo {
            get => SexoTB.Text;
        }

        string Convenio {
            get => ConvenioTB.Text;
            set => ConvenioTB.Text = value;
        }

        string Procedimento {
            get => ProcedimentoTB.Text;
        }

        string Desconto {
            get => DescontosTB.Text;
        }

        string Entrada {
            get => EntradaTB.Text;
        }

        string Observação {
            get => ObservaçãoTB.Text;
        }

        string Preview {
            get {
                return TextRange.Text;
            }
            set {
                TextRange.Text = value;
            }
        }

        double Total {
            get {
                double t = 0;
                for (int i = 0; i < AdicionadosDG.Items.Count; i++) {
                    var row = AdicionadosDG.Items[i] as Table1;
                    t += row.Valor;
                }
                return t;
            }
        }

        FlowDocument FlowDocument {
            get => PreviewRTB.Document;
        }

        TextRange TextRange {
            get => new TextRange(FlowDocument.ContentStart, FlowDocument.ContentEnd);
        }

        BlockCollection Blocks {
            get => FlowDocument.Blocks;
        }

        #endregion

        public FazerOrcamento() {
            InitializeComponent();
            ControlsH.CreateColumns(PesquisaDG, typeof(Table1));
            ControlsH.CreateColumns(AdicionadosDG, typeof(Table1));
            FlowDocument.LineHeight = 1;
            TimedEvent = new TimedEvent(1, 2, true, BuildDoc);
            TimedEvent.TryTigger();
            TimedEvent2 = new TimedEvent(1, 2, true, UpdateProcedimentos);
        }

        ~FazerOrcamento() {
            IO.SafeDelete("Estabelecimento.txt", "Financeiro.txt", "Observação.txt", "Paciente.txt", "Procedimentos.txt");
        }

        void BuildDoc() {
            Blocks.Clear();
            var ne = new Paragraph(new Bold(new Run(NomeEstabelecimento)));
            ne.FontSize = 15;
            ne.TextAlignment = TextAlignment.Center;
            Add(ne);
            var ee = new Paragraph(new Run(Endereco));
            ee.TextAlignment = TextAlignment.Center;
            Add(ee);
            var ec = new Paragraph(new Run(Contato));
            ec.TextAlignment = TextAlignment.Center;
            Add(ec);
            int count = 70;
            var s1 = new Paragraph(new Run(new string('-', count)));
            Add(s1);
            var o = new Paragraph(new Bold(new Run("Orçamento")));
            Add(o);
            var c = new Paragraph(new Run($"Convênio: {Convenio}"));
            Add(c);
            var s2 = new Paragraph(new Run(new string('-', count)));
            Add(s2);
            var pc = new Paragraph(new Run($"CPF: {CPF}"));
            Add(pc);
            var pn = new Paragraph(new Run($"Nome: {NomePaciente}"));
            Add(pn);
            var pd = new Paragraph(new Run($"Data de Nascimento: {Nascimento}       Sexo: {Sexo}"));
            Add(pd);
            var s3 = new Paragraph(new Run(new string('-', count)));
            Add(s3);
            for (int i = 0; i < AdicionadosDG.Items.Count; i++) {
                Table1 table1 = AdicionadosDG.Items[i] as Table1;
                var item = new Paragraph(new Run($"{table1.Procedimento}         {table1.Valor}"));
                Add(item);
            }
            var s4 = new Paragraph(new Run(new string('-', count)));
            Add(s4);
            var total = Total;
            var st = new Paragraph(new Run($"SubTotal: {total}"));
            Add(st);
            var desconto = 0.0;
            var final = 0.0;
            try {
                if (Desconto.Contains("%")) {
                    var a = Desconto.Substring(0, Desconto.Length - 1);
                    desconto = ToDouble(a) / 100;
                    final = total - (desconto * total);
                } else {
                    desconto = ToDouble(Desconto);
                    final = total - desconto;
                }
                ToDouble(Desconto);
            } catch (Exception) {
                final = total;
            }
            var d = new Paragraph(new Run($"Desconto: {desconto.ToString(brasil)}"));
            Add(d);
            var t = new Paragraph(new Run($"Total: {final.ToString(brasil)}"));
            Add(t);
            double entrada = 0.0;
            try {
                entrada = ToDouble(Entrada, CultureInfo.InvariantCulture);
            } catch (Exception) {

            }
            var en = new Paragraph(new Run($"Entrada: {entrada}"));
            Add(en);
            var f = new Paragraph(new Run($"A pagar: {(final - entrada).ToString(brasil)}"));
            Add(f);
        }

        void UpdateProcedimentos() {
            PesquisaDG.Items.Clear();
            var r = ProcedimentosLab.SelectLike(ConvenioSelecionado, Procedimento);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                PesquisaDG.Items.Add(new Table1() {
                    Procedimento = row.Procedimento,
                    Valor = row.Valor
                });
            }
        }

        void Add(Block item) {
            Blocks.Add(item);
        }

        private void SelecionarB_Click(object sender, RoutedEventArgs e) {
            var a = new EscolherConvenio2();
            a.ShowDialog();
            if (App.Current.Properties["convenio"] is string convenio) {
                Convenio = ConvenioSelecionado = convenio;
                UpdateProcedimentos();
            }
        }

        private void PesquisaDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AdicionarB.IsEnabled = PesquisaDG.SelectedIndex >= 0;
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            if (PesquisaDG.SelectedItem is Table1 table1) {
                AdicionadosDG.Items.Add(table1);
                TimedEvent.TryTigger();
            }
        }

        private void AdicionadosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RemoverB.IsEnabled = AdicionadosDG.SelectedIndex >= 0;
        }

        private void NomeEstabelecimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void EnderecoTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void ContatoTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void NascimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void NomePacienteTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void SexoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void ConvenioTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void DescontosTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void EntradaTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void ObservaçãoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        class Table1 {

            public string Procedimento { get; set; }
            public double Valor { get; set; }


        }

        private void ProcedimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent2.TryTigger();
        }

        private void RemoverB_Click(object sender, RoutedEventArgs e) {
            if (AdicionadosDG.SelectedItem is Table1 table1) {
                AdicionadosDG.Items.Remove(table1);
                TimedEvent.TryTigger();
            }
        }

        private void AbrirDocumentoB_Click(object sender, RoutedEventArgs e) {
            try {
                /*using (var s = new StreamWriter("Estabelecimento.txt", false, Encoding.UTF8)) {
                    s.WriteLine(NomeEstabelecimento);
                    s.WriteLine(Endereco);
                    s.WriteLine(Contato);
                }
                using (var s = new StreamWriter("Paciente.txt", false, Encoding.UTF8)) {
                    s.WriteLine(CPF);
                    s.WriteLine(NomePaciente);
                    s.WriteLine(Nascimento);
                    s.WriteLine(Sexo);
                }
                using (var s = new StreamWriter("Procedimentos.txt", false, Encoding.UTF8)) {
                    for (int i = 0; i < AdicionadosDG.Items.Count; i++) {
                        var item = AdicionadosDG.Items[i] as Table1;
                        s.WriteLine($"{item.Procedimento}{new string(' ', 10)}{item.Valor.ToString(brasil)}");
                    }
                }
                double desconto = 0.0f;
                double final = 0.0f;
                GetFinal(ref desconto, ref final);
                using (var s = new StreamWriter("Financeiro.txt", false, Encoding.UTF8)) {
                    s.WriteLine(Total.ToString(brasil));
                    s.WriteLine(desconto.ToString(brasil));
                    s.WriteLine(final.ToString(brasil));
                    s.WriteLine(Entrada.Length == 0 ? "0" : Entrada);
                    s.WriteLine((final - desconto).ToString(brasil));
                }
                using (var s = new StreamWriter("Observação.txt", false, Encoding.UTF8)) {
                    s.WriteLine(Observação.Length == 0 ? " " : Observação);
                }*/
                var wd = new WordDocument();
                var beginBoldTwoDots = new ConditionalFormatting();
                beginBoldTwoDots.regex = "^.+:";
                beginBoldTwoDots.FontStyle.Bold = true;
                wd.ConditionalFormattings.Add(beginBoldTwoDots);
                var estabelecimento_style = new ParagraphStyle() {
                    FontSize = 18,
                    Bold = true,
                    Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter
                };
                var estabelecimentoP = new _Paragraph(NomeEstabelecimento, estabelecimento_style);
                wd.Content.Add(estabelecimentoP);
                var endereço_style = new ParagraphStyle() {
                    Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter
                };
                var endereçoP = new _Paragraph(Endereco, endereço_style);
                wd.Content.Add(endereçoP);
                var contatoP = new _Paragraph(Contato, endereço_style);
                wd.Content.Add(contatoP);
                wd.Content.Add("");
                wd.Content.Add(new _HorizontalLine());
                var cpf = new _Paragraph($"CPF: {CPF}");
                wd.Content.Add(cpf);
                var nomeP = new _Paragraph($"Nome: {NomePaciente}");
                wd.Content.Add(nomeP);
                var nascimentoP = new _Paragraph($"Nascimento: {Nascimento}");
                wd.Content.Add(nascimentoP);
                var sexoP = new _Paragraph($"Sexo: {Sexo}");
                wd.Content.Add(sexoP);
                wd.Content.Add(new _HorizontalLine());
                var table = new _Table(AdicionadosDG.Items.Count, 2) {
                    HasBorders = false
                };
                for (int i = 0; i < AdicionadosDG.Items.Count; i++) {
                    var item = AdicionadosDG.Items[i] as Table1;
                    var left = new _Paragraph(item.Procedimento);
                    table.Insert(i, 0, left);
                    var rightAlign = new ParagraphStyle() {
                        Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight
                    };
                    var right = new _Paragraph(item.Valor.ToString(brasil), rightAlign);
                    table.Insert(i, 1, right);
                }
                wd.Content.Add(table);
                wd.Content.Add(new _HorizontalLine());
                var subtotalP = new _Paragraph($"Subtotal: {Total.ToString(brasil)}");
                wd.Content.Add(subtotalP);
                GetFinal(out double desconto, out double final);
                var descontoP = new _Paragraph($"Desconto: {Desconto}");
                wd.Content.Add(descontoP);
                var entradaP = new _Paragraph($"Entrada: {Entrada}");
                wd.Content.Add(entradaP);
                var finalP = new _Paragraph($"A pagar: {(final - (desconto + Converter.TryToDouble(Entrada, 0))).ToString(brasil)}");
                wd.Content.Add(finalP);
                wd.Create();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        void GetFinal(out double desconto, out double final) {
            try {
                if (Desconto.Contains("%")) {
                    var a = Desconto.Substring(0, Desconto.Length - 1);
                    a = a.Replace(" ", "");
                    desconto = ToDouble(a, brasil) / 100;
                    final = Total - (desconto * Total);
                } else {
                    desconto = Converter.TryToDouble(Desconto, 0);
                    final = Total - desconto;
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro ao obter valor final.");
                desconto = 0;
                final = Total;
            }
        }

    }
}