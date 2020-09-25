using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using TextRenderer = System.Windows.Forms.TextRenderer;
using WordApp = Microsoft.Office.Interop.Word.Application;

namespace WPFClinica.Scripts {

    class Orçamento {

        //private Logger Logger;
        private const string FileName = "Orçamento.log";

        public Orc_Estabelecimento Estabelecimento = new Orc_Estabelecimento();
        public Paciente Paciente = new Paciente();
        public Orc_Procedimentos Procedimentos = new Orc_Procedimentos();
        public Financeiro Financeiro = new Financeiro();
        public string Observação = "";

        public Orçamento() {
            try {
                //Logger = new Logger(FileName);
                Estabelecimento = new Orc_Estabelecimento("Estabelecimento.txt");
                Paciente = new Paciente("Paciente.txt");
                Procedimentos = new Orc_Procedimentos("Procedimentos.txt");
                Financeiro = new Financeiro("Financeiro.txt");
                using (var st = new StreamReader("Observação.txt", Encoding.UTF8)) {
                    while (!st.EndOfStream) {
                        Observação += st.ReadLine();
                    }
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

        public void CreateDoc() {
            try {
                var app = new WordApp {
                    ShowAnimation = true,
                    Visible = true
                };
                object missing = Missing.Value;
                var d = app.Documents.Add(ref missing, ref missing, ref missing, ref missing);
                d.PageSetup.PaperSize = WdPaperSize.wdPaperA4;
                CreatePara(d, Estabelecimento.Nome, WdParagraphAlignment.wdAlignParagraphCenter, true, 14f);
                CreatePara(d, Estabelecimento.Endereço, WdParagraphAlignment.wdAlignParagraphCenter);
                CreatePara(d, Estabelecimento.Contato, WdParagraphAlignment.wdAlignParagraphCenter);
                AddHorizontalLine(d);
                CreatePara(d, $"CPF: {Paciente.CPF}");
                CreatePara(d, $"Nome: {Paciente.Nome}");
                CreatePara(d, $"Data de nascimento: {Paciente.Nome}");
                CreatePara(d, $"Sexo: {Paciente.Sexo}");
                AddHorizontalLine(d);
                var f = new System.Drawing.Font("Consolas", 11f);
                var total = 500;
                for (int i = 0; i < Procedimentos.Procs.Count; i++) {
                    var x = Procedimentos.Procs[i];
                    CreatePara(d, $"{x.Nome}{CalcSeparator(total, f, x.Nome, x.Valor)}{x.Valor}");
                }
                d.Content.InsertBreak(WdBreakType.wdSectionBreakContinuous);
                AddHorizontalLine(d);
                var l = "SubTotal:";
                var r = Financeiro.SubTotal;
                CreatePara(d, $"{l}{CalcSeparator(total, f, l, r)}{r}");
                l = "Desconto:";
                r = Financeiro.Desconto;
                CreatePara(d, $"{l}{CalcSeparator(total, f, l, r)}{r}");
                l = "Total:";
                r = Financeiro.Total;
                CreatePara(d, $"{l}{CalcSeparator(total, f, l, r)}{r}");
                l = "A pagar:";
                r = Financeiro.APagar;
                CreatePara(d, $"{l}{CalcSeparator(total, f, l, r)}{r}");
                CreatePara(d, " ");
                l = "Observação:";
                r = Observação;
                CreatePara(d, $"{l} {r}.");
                object fileName = $"{Environment.CurrentDirectory}\\Orçamento.docx";
                d.SaveAs2(ref fileName);
                d = null;
                app = null;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro");
            }
        }

        void AddHorizontalLine (Document d) {
            object missing = Missing.Value;
            var line = d.Paragraphs.Last.Range.InlineShapes.AddHorizontalLineStandard(ref missing);
            line.Height = 1;
            line.Fill.Solid();
            line.HorizontalLineFormat.NoShade = true;
            line.Fill.ForeColor.RGB = ColorTranslator.ToOle(Color.Black);
            line.HorizontalLineFormat.PercentWidth = 100;
            line.HorizontalLineFormat.Alignment = WdHorizontalLineAlignment.wdHorizontalLineAlignCenter;
        }

        static string CalcSeparator(int total, System.Drawing.Font f, string left, string right) {
            var w1 = TextRenderer.MeasureText(left, f).Width;
            var w2 = TextRenderer.MeasureText(right, f).Width;
            var t1 = total - (w1 + w2);
            var separator = " ";
            while (TextRenderer.MeasureText(separator, f).Width <= t1) {
                separator += " ";
            }
            return separator;
        }

        static void CreatePara(Document d, string text, WdParagraphAlignment alignment = WdParagraphAlignment.wdAlignParagraphLeft, bool bold = false, float fs = 11f) {
            object missing = Missing.Value;
            var p = d.Content.Paragraphs.Add(ref missing);
            p.LineSpacing = 1.5f;
            p.Range.Font.Name = "Consolas";
            p.Range.Font.Size = fs;
            p.Range.Font.Bold = bold ? 1 : 0;
            p.Range.Text = text;
            if (text.Contains(":")) {
                object objStart = p.Range.Start;
                object objEnd = p.Range.Start + text.IndexOf(":");
                var rngBold = d.Range(ref objStart, ref objEnd);
                rngBold.Bold = 1;
            }
            p.Alignment = alignment;
            p.Range.InsertParagraphAfter();
        }

    }

    class Orc_Estabelecimento {

        public string Nome { get; set; }
        public string Endereço { get; set; }
        public string Contato { get; set; }

        public Orc_Estabelecimento() { }

        public Orc_Estabelecimento(string fileName) {
            using (var s = new StreamReader(fileName, Encoding.UTF8)) {
                int i = 0;
                while (!s.EndOfStream) {
                    var line = s.ReadLine();
                    switch (i++) {
                        case 0:
                            Nome = line;
                            break;
                        case 1:
                            Endereço = line;
                            break;
                        case 2:
                            Contato = line;
                            break;
                    }
                }
            }
        }

        public Orc_Estabelecimento(string nome, string endereço, string contato) {
            Nome = nome;
            Endereço = endereço;
            Contato = contato;
        }

    }

    class Paciente {

        public string CPF { get; set; }
        public string Nome { get; set; }
        public string Nascimento { get; set; }
        public string Sexo { get; set; }

        public Paciente() { }

        public Paciente(string fileName) {
            using (var s = new StreamReader(fileName, Encoding.UTF8)) {
                int i = 0;
                while (!s.EndOfStream) {
                    var line = s.ReadLine();
                    switch (i++) {
                        case 0:
                            CPF = line;
                            break;
                        case 1:
                            Nome = line;
                            break;
                        case 2:
                            Nascimento = line;
                            break;
                        case 3:
                            Sexo = line;
                            break;
                    }
                }
            }
        }

        public Paciente(string cpf, string nome, string nascimento, string sexo) {
            CPF = cpf;
            Nome = nome;
            Nascimento = nascimento;
            Sexo = sexo;
        }

    }

    class Orc_Procedimento {

        public string Nome { get; set; }
        public string Valor { get; set; }

        public Orc_Procedimento() { }

        public Orc_Procedimento(string nome, string valor) {
            Nome = nome;
            Valor = valor;
        }

    }

    class Orc_Procedimentos {

        public string Convenio { get; set; }
        public List<Orc_Procedimento> Procs { get; set; } = new List<Orc_Procedimento>();

        public Orc_Procedimentos() { }

        public Orc_Procedimentos(string fileName) {
            using (var s = new StreamReader(fileName, Encoding.UTF8)) {
                while (!s.EndOfStream) {
                    string line = s.ReadLine(), valor = "";
                    int i = line.Length - 1;
                    while (char.IsDigit(line[i]) || line[i] == '.' || line[i] == ',') {
                        valor = line[i] + valor;
                        line = line.Substring(0, i--);
                    }
                    while (line[i] == ' ') line = line.Substring(0, i--);
                    Procs.Add(new Orc_Procedimento(line, valor));
                }
            }
        }

    }

    class Financeiro {

        public string SubTotal { get; set; }
        public string Desconto { get; set; }
        public string Total { get; set; }
        public string Entrada { get; set; }
        public string APagar { get; set; }

        public Financeiro() { }

        public Financeiro(string fileName) {
            using (var s = new StreamReader(fileName, Encoding.UTF8)) {
                int i = 0;
                while (!s.EndOfStream) {
                    var line = s.ReadLine();
                    switch (i++) {
                        case 0:
                            SubTotal = line;
                            break;
                        case 1:
                            Desconto = line;
                            break;
                        case 2:
                            Total = line;
                            break;
                        case 3:
                            Entrada = line;
                            break;
                        case 4:
                            APagar = line;
                            break;
                    }
                }
            }
        }

    }

}
