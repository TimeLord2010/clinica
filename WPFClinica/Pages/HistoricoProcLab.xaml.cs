using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using MicrosoftWord;
using MicrosoftWord.Styles;
using MicrosoftWord.DocumentElements;
using TR = System.Windows.Forms.TextRenderer;
using System.Diagnostics;
using static System.Convert;
using System.Threading;
using MySql.Data.MySqlClient;

namespace WPFClinica.Pages {

    public partial class HistoricoProcLab : Page {

        #region Vars

        TimedEvent TimedEvent;
        CultureInfo Brasil = new CultureInfo("pt-BR");
        Dictionary<string, double> dict = new Dictionary<string, double>();
        const string FileName = "Relatório Histório de Procedimentos Laboratoriais.txt";

        string Procedimento {
            get => ProcedimentoTB.Text;
            set => ProcedimentoTB.Text = value;
        }

        string Convenio {
            get => ConvenioTB.Text;
            set => ConvenioTB.Text = value;
        }

        DateTime Inicio {
            get {
                return Converter.Converter_DT(InicioTB.Text, Fim.AddDays(-30));
            }
        }

        DateTime Fim {
            get {
                return Converter.Converter_DT(FimTB.Text, DateTime.Now.AddDays(1));
            }
        }

        bool IncluirNaoPagos {
            get => IncluirNaoPagosChB.IsChecked ?? false;
        }

        double Total {
            get => ToDouble(TotalTBL.Text, Brasil);
            set => TotalTBL.Text = value.ToString(Brasil);
        }

        #endregion

        public HistoricoProcLab() {
            InitializeComponent();
            ControlsH.CreateColumns(ProcedimentosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 2, true, UpdateHistorico);
            var t = new Thread(() => {
                try {
                    var columns = SQLOperations.MySql_Handler.GetColumns(Historico_ProcedimentosLab.Name);
                    bool has_enabled = false;
                    foreach (var (name, accept_null, data_type, key_type) in columns) {
                        if (name.ToLower() == "is_enabled") has_enabled = true;
                    }
                    if (!has_enabled) {
                        SQLOperations.MySql_Handler.NonQuery(
                            $"alter table {Historico_ProcedimentosLab.Name} add {nameof(Historico_ProcedimentosLab.Is_enabled)} bit default 1;");
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }
                App.Current.Dispatcher.Invoke(() => {
                    TimedEvent.TriggerNow();
                });
            });
            t.Start();
        }

        ~HistoricoProcLab() {
            try {
                File.Delete(FileName);
            } catch (Exception) { }
        }

        public void UpdateHistorico() {
            ProcedimentosDG.Items.Clear();
            dict.Clear();
            try {
                var r = Historico_ProcedimentosLab.SelectLike(Procedimento, Convenio, Inicio, Fim, NomePacienteTB.Text, IncluirNaoPagos);
                double t = 0;
                dict.Add("Total", 0.0);
                var thread = new Thread(() => {
                    try {
                        foreach (var i in r) {
                            t += i.Pago;
                            dict["Total"] += i.Pago;
                            var total = $"{i.Pago.ToString(Brasil)}";
                            var fps = ProcedimentosLab_FormaPagamento.Select(i.ID);
                            foreach (var item in fps) {
                                if (!dict.ContainsKey(item.FormaDePagamento.Descricao)) {
                                    dict.Add(item.FormaDePagamento.Descricao, 0.0);
                                }
                                dict[item.FormaDePagamento.Descricao] += item.Valor;
                                total += $"\n{item.FormaDePagamento.Descricao}: {Converter.DoubleToString(item.Valor)}";
                            }
                            var t1 = new Table1() {
                                ID = i.ID,
                                Proc_ID = i.Procedimento_ID,
                                Con_ID = i.Con_ID,
                                CPF_Paciente = i.CPF,
                                NomePaciente = i.Nome,
                                Valor = total,
                                Convênio = i.Convenio,
                                PagoEm = i.PagoEm.HasValue ? i.PagoEm.Value.ToString("dd-MM-yyyy HH:mm:ss") : "[?]",
                                RealizadoEm = i.RealizadoEm.HasValue ? i.RealizadoEm.Value.ToString("dd-MM-yyyy HH:mm:ss") : "[Não realizado]",
                                Procedimento = i.ProcedimentoLab,
                                ValorClínica = Converter.DoubleToString(i.Pago * 0.3),
                                ValorLaboratório = Converter.DoubleToString(i.Pago * 0.7)
                            };
                            App.Current.Dispatcher.Invoke(() => {
                                ProcedimentosDG.Items.Add(t1);
                            });
                        }
                    } catch (Exception ex) {
                        ErrorHandler.OnError(ex);
                    }
                });
                thread.Start();
                Total = t;
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, $"Erro. ({ErrorCodes.E0002})");
            }
        }

        private void ProcedimentoTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void ConvenioTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void InicioTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void FimTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        class Table1 {
            public int ID;
            public string CPF_Paciente;
            public int Proc_ID;
            public int Con_ID;
            public string NomePaciente { get; set; }
            public string Procedimento { get; set; }
            public string Convênio { get; set; }
            public string RealizadoEm { get; set; }
            public string PagoEm { get; set; }
            public string ValorLaboratório { get; set; }
            public string ValorClínica { get; set; }
            public string Valor { get; set; }
        }

        private void ImprimirB_Click(object sender, RoutedEventArgs e) {
            try {
                var d = new WordDocument();
                d.Orientation = MicrosoftWord.Orientation.Landscape;
                var header_style = new ParagraphStyle {
                    Bold = true,
                    FontSize = 16,
                    ParagraphAligment = wdHorizontalAlignment.Center
                };
                var header = new _Paragraph("Relatório de Procedimentos Laboratoriais", header_style);
                d.Content.Add(header);
                var subheader_style = new ParagraphStyle() {
                    Bold = true,
                    ParagraphAligment = wdHorizontalAlignment.Center,
                    FontSize = 14
                };
                var subheader = new _Paragraph($"{Inicio.ToString("dd/MM/yyyy")} - {Fim.ToString("dd/MM/yyyy")}", subheader_style);
                d.Content.Add(subheader);
                var separator = new _Paragraph("");
                d.Content.Add(separator);
                if (App.Current.Properties["cpf"] is string cpf) {
                    var requirendo = new _Paragraph($"Requerido por: {Pessoas.Select(cpf).Nome}.");
                    d.Content.Add(requirendo);
                }
                d.Content.Add($"Data e hora do requerimento: {DateTime.Now.ToString($"dd/MM/yyyy HH:mm")}.");
                var table = new _Table(ProcedimentosDG.Items.Count + 1, 7);
                int j = 0;
                var tableheader_style = new ParagraphStyle() {
                    Bold = true
                };
                table.Insert(0, j++, new _Paragraph("Paciente", tableheader_style));
                table.Insert(0, j++, new _Paragraph("Procedimento", tableheader_style));
                table.Insert(0, j++, new _Paragraph("Convênio", tableheader_style));
                table.Insert(0, j++, new _Paragraph("Pago em", tableheader_style));
                table.Insert(0, j++, new _Paragraph("Laboratório", tableheader_style));
                table.Insert(0, j++, new _Paragraph("Clínica", tableheader_style));
                table.Insert(0, j++, new _Paragraph("Total", tableheader_style));
                for (int i = 0; i < ProcedimentosDG.Items.Count; i++) {
                    if (ProcedimentosDG.Items[i] is Table1 t1) {
                        j = 0;
                        table.Insert(i + 1, j++, t1.NomePaciente);
                        table.Insert(i + 1, j++, t1.Procedimento);
                        table.Insert(i + 1, j++, t1.Convênio);
                        table.Insert(i + 1, j++, t1.PagoEm);
                        table.Insert(i + 1, j++, t1.ValorLaboratório);
                        table.Insert(i + 1, j++, t1.ValorClínica);
                        table.Insert(i + 1, j++, t1.Valor);
                    }
                }
                d.Content.Add(table);
                d.Content.Add("");
                foreach (var item in dict) {
                    d.Content.Add($"{item.Key}: {item.Value}.");
                }
                d.Create();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, $"Erro. ({ErrorCodes.E0003})");
            }
        }

        private void IncluirNaoPagosChB_Checked(object sender, RoutedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void IncluirNaoPagosChB_Unchecked(object sender, RoutedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void NomePacienteTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent?.TryTigger();
        }

        private void DeleteMI_Click(object sender, RoutedEventArgs e) {
            try {
                if (ProcedimentosDG.SelectedItem is Table1 t1) {
                    Historico_ProcedimentosLab.Update_IsEnabled(t1.ID, false);
                    ProcedimentosDG.Items.Remove(t1);
                    //TimedEvent.TriggerNow();
                }
            } catch (Exception ex) {
                ErrorHandler.OnError(ex);
            }
        }

        private void ProcedimentosDG_ContextMenuOpening(object sender, ContextMenuEventArgs e) {
            DeleteMI.IsEnabled = ProcedimentosDG.SelectedIndex >= 0;
        }
    }
}
