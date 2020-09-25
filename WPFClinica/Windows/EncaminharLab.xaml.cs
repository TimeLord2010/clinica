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
using System.Windows.Shapes;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using static System.Convert;
using MicrosoftWord;
using MicrosoftWord.DocumentElements;
using MicrosoftWord.Styles;
using FontStyle = MicrosoftWord.Styles.FontStyle;
using System.Threading;

namespace WPFClinica.Windows {

    public partial class EncaminharLab : Window {

        #region Vars

        static CultureInfo Brasil = new CultureInfo("pt-BR");
        TimedEvent TimedEvent;
        string CPF_Paciente;
        bool Prioridade;

        double DinheiroPago {
            get => Converter.TryToDouble(DinheiroTB.Text, double.NaN, Brasil);
        }

        double CreditoPago {
            get => Converter.TryToDouble(CreditoTB.Text, double.NaN, Brasil);
        }

        double DebitoPago {
            get => Converter.TryToDouble(DebitoTB.Text, double.NaN, Brasil);
        }

        double TotalToPay {
            get {
                return AdicionadosDG.Items.OfType<Table1>().Aggregate(0.0, (total, item) => total + ToDouble(item.Valor, Brasil));
            }
        }

        double TotalPaid {
            get {
                double total = 0;
                Data.ForEach((item) => {
                    total += item;
                }, CreditoPago, DebitoPago, DinheiroPago);
                return total;
            }
        }

        bool AreValidPaidValues {
            get {
                if (Data.Any(x => double.IsNaN(x), CreditoPago, DebitoPago, DinheiroPago)) {
                    MessageBox.Show("Um ou mais dos valores inseridos não são números.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                if (TotalPaid != TotalToPay) {
                    var r = MessageBox.Show(
                        $"A soma dos valores ({TotalPaid.ToString(Brasil)}) não é igual ao valor esperado ({TotalToPay.ToString(Brasil)}). " +
                        $"Deseja continuar?",
                        "Erro", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    return r == MessageBoxResult.Yes;
                }
                return true;
            }
        }

        #endregion

        public EncaminharLab(string paciente, bool prioridade) {
            InitializeComponent();
            CPF_Paciente = paciente;
            Prioridade = prioridade;
            WindowsSO.ToCenterOfScreen(this);
            Pessoas pessoa = new Pessoas(paciente);
            PacienteTBL.Text = pessoa.Nome;
            ControlsH.CreateColumns(ProcedimentosDG, typeof(Table1));
            ControlsH.CreateColumns(AdicionadosDG, typeof(Table1));
            TimedEvent = new TimedEvent(1, 3, true, UpdateProcedimentos);
            TimedEvent.TriggerNow();
        }

        public void UpdateProcedimentos() {
            ProcedimentosDG.Items.Clear();
            var r = ProcedimentosLab.SelectLike(ProcedimentosTB.Text);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                var item = new Table1() {
                    Proc_ID = row.Proc_ID,
                    Conv_ID = row.Con_ID,
                    Procedimento = row.Procedimento,
                    Convenio = row.Convenio,
                    Valor = row.Valor.ToString(Brasil)
                };
                ProcedimentosDG.Items.Add(item);
            }
        }

        void UpdateTotal() {
            var total = 0.0;
            foreach (var item in AdicionadosDG.Items.OfType<Table1>()) {
                total += ToDouble(item.Valor, Brasil);
            }
            TotalTBL.Text = $"{total.ToString(Brasil)}";
        }

        private void EncaminharB_Click(object sender, RoutedEventArgs e) {
            try {
                if (AdicionadosDG.Items.Count == 0) {
                    MessageBox.Show("Selecionar ao menos um procedimento ao paciente.");
                    return;
                }
                if (!AreValidPaidValues) return;
                Dictionary<int, double> dict = new Dictionary<int, double>() {
                    {(int)FP.Credito, CreditoPago}, { (int)FP.Debito, DebitoPago}, { (int)FP.Dinheiro, DinheiroPago}
                };
                Cursor = Cursors.Wait;
                SQLOperations.ThrowExceptions = true;
                ListaEspera.InsertOrUpdate(CPF_Paciente, Prioridade, Fila.Laboratorio, PStatus.Esperando);
                var le = ListaEspera.SelectLike(CPF_Paciente, "", Prioridade, Fila.Laboratorio, PStatus.Esperando)[0];
                var dt = DateTime.Now;
                for (int i = 0; i < AdicionadosDG.Items.Count; i++) {
                    var t = AdicionadosDG.Items[i] as Table1;
                    var valor = ToDouble(t.Valor, Brasil);
                    Historico_ProcedimentosLab.Insert(CPF_Paciente, t.Proc_ID, t.Conv_ID, null, dt, valor);
                    var h = Historico_ProcedimentosLab.Select(CPF_Paciente, t.Proc_ID, t.Conv_ID, dt, dt)[0];
                    PacienteProcedimentos.Insert(le.Senha, t.Proc_ID, t.Conv_ID, ProcedimentoStatus.Pendente);
                    var porcentage = valor / TotalToPay;
                    foreach (var kv in dict) {
                        if (kv.Value == 0) continue;
                        ProcedimentosLab_FormaPagamento.Insert(h.ID, kv.Key, kv.Value * porcentage);
                    }
                }
                var nome_paciente = PacienteTBL.Text;
                var list = new List<Table1>();
                for (var i = 0; i < AdicionadosDG.Items.Count; i++) {
                    list.Add(AdicionadosDG.Items[i] as Table1);
                }
                var thread = new Thread(() => {
                    PrintDocument(nome_paciente, list);
                });
                thread.Start();
                Close();
            } catch (Exception ex) {
                ListaEspera.Delete(CPF_Paciente);
                ErrorHandler.OnError(ex);
            } finally {
                Cursor = Cursors.Arrow;
                SQLOperations.ThrowExceptions = false;
            }
        }

        static void PrintDocument (string paciente, IEnumerable<Table1> rows) {
            try {
                var wd = new WordDocument();
                wd.Content.Add(new _Paragraph($"Procedimentos Laboratoriais do Paciente", new ParagraphStyle() {
                    Bold = true,
                    FontSize = 16,
                    ParagraphAligment = wdHorizontalAlignment.Center
                }));
                wd.Content.Add(new _Paragraph($"{DateTime.Now:dd/MM/yyyy HH:mm}", new ParagraphStyle() {
                    ParagraphAligment = wdHorizontalAlignment.Center,
                    Italic = true
                }));
                wd.Content.Add("");
                wd.Content.Add($"Paciente: {paciente}.");
                var table = new _Table(rows.Count() + 1, 3);
                int j = 0;
                var headerS = new ParagraphStyle() {
                    Bold = true
                };
                table.Insert(0, j++, new _Paragraph($"Procedimento", headerS));
                table.Insert(0, j++, new _Paragraph($"Convênio", headerS));
                table.Insert(0, j++, new _Paragraph($"Valor", headerS));
                for (int i = 0; i < rows.Count(); i++) {
                    var item = rows.ElementAt(i);
                    j = 0;
                    table.Insert(i + 1, j++, new _Paragraph(item.Procedimento));
                    table.Insert(i + 1, j++, new _Paragraph(item.Convenio));
                    table.Insert(i + 1, j++, new _Paragraph(item.Valor));
                }
                wd.Content.Add(table);
                wd.Create();
            } catch (Exception) {
            }
        }

        private void ProcedimentosTB_TextChanged(object sender, TextChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        class Table1 {

            public int Proc_ID;
            public int Conv_ID;
            public string Procedimento { get; set; }
            public string Convenio { get; set; }
            public string Valor { get; set; }

        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            if (ProcedimentosDG.SelectedItem is Table1 table1) {
                if (AdicionadosDG.Items.Contains(table1)) {
                    MessageBox.Show("Item já adicionado.");
                } else {
                    AdicionadosDG.Items.Add(table1);
                    UpdateTotal();
                }
            }
        }

        private void ProcedimentosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AdicionarB.IsEnabled = ProcedimentosDG.SelectedIndex >= 0;
        }

        private void RemoverB_Click(object sender, RoutedEventArgs e) {
            if (AdicionadosDG.SelectedItem is Table1 t1) {
                AdicionadosDG.Items.Remove(t1);
                UpdateTotal();
            }
        }

        private void AdicionadosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RemoverB.IsEnabled = AdicionadosDG.SelectedIndex >= 0;
        }

    }
}
