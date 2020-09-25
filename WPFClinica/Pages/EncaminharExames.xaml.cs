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
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class EncaminharExames : Page {

        #region Vars

        readonly TimedEvent TimedEvent;
        readonly CultureInfo Brasil = new CultureInfo("pt-BR");

        string cpf;
        string CPF {
            get => cpf;
            set {
                cpf = value;
                NomePaciente = Pessoas.Select(cpf).Nome;
            }
        }

        string NomePaciente {
            set => PacienteTBL.Text = value;
        }

        bool Prioridade { get; set; }

        string TipoSelecao {
            get {
                if (TipoCB.SelectedIndex < 0) {
                    return null;
                }
                return (TipoCB.SelectedItem as ComboBoxItem).Content.ToString();
            }
        }

        int? Tipo_ID {
            get {
                if (TipoSelecao == null) {
                    return null;
                }
                return TipoProcedimento.Select(TipoSelecao);
            }
        }

        string NomeProcedimento {
            get => PesquisaProcedimentoTB.Text;
        }

        double Total {
            get {
                return ToDouble(TotalTBL.Text, Brasil);
            }
            set {
                TotalTBL.Text = $"{value.ToString(Brasil)}";
            }
        }

        double DebitoPago {
            get => Converter.TryToDouble(DebitoTB.Text, double.NaN, Brasil);
        }

        double CreditoPago {
            get => Converter.TryToDouble(CreditoTB.Text, double.NaN, Brasil);
        }

        double DinheiroPago {
            get => Converter.TryToDouble(DinheiroTB.Text, double.NaN, Brasil);
        }

        #endregion

        public EncaminharExames(string cpf, bool prioridade) {
            InitializeComponent();
            CPF = cpf;
            Prioridade = prioridade;
            ControlsH.CreateColumns(ProcedimentosDG, typeof(Procedimento_Table));
            ControlsH.CreateColumns(AdicionadosDG, typeof(Adicionados_Table));
            TipoProcedimento.Fill(TipoCB);
            TimedEvent = new TimedEvent(1, 1, true, UpdateProcedimentos);
            TimedEvent.Start();
        }

        void UpdateProcedimentos() {
            ProcedimentosDG.Items.Clear();
            if (Tipo_ID == null) return;
            foreach (var item in Procedimentos.SelectLike(Tipo_ID.Value, NomeProcedimento)) {
                ProcedimentosDG.Items.Add(new Procedimento_Table() {
                    Proc_ID = item.ID,
                    Nome = item.Descricao,
                    Valor = item.Valor.ToString(Brasil)
                });
            }
        }

        private void TipoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (TimedEvent != null) TimedEvent.TryTigger();
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            if (Tipo_ID == null) {
                MessageBox.Show("Identificador de tipo não pôde ser obtido.");
                return;
            }
            if (ProcedimentosDG.SelectedItem is Procedimento_Table item) {
                AdicionadosDG.Items.Add(new Adicionados_Table() {
                    Proc_ID = item.Proc_ID,
                    Tipo = TipoSelecao,
                    Procedimento = item.Nome,
                    Valor = item.Valor
                });
                CalcTotal();
            }
        }

        private void AdicionadosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RemoverB.IsEnabled = AdicionadosDG.SelectedIndex >= 0;
        }

        private void RemoverB_Click(object sender, RoutedEventArgs e) {
            if (AdicionadosDG.SelectedIndex >= 0) {
                AdicionadosDG.Items.RemoveAt(AdicionadosDG.SelectedIndex);
                CalcTotal();
            }
        }

        private void ProcedimentosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            AdicionarB.IsEnabled = ProcedimentosDG.SelectedIndex >= 0;
        }

        private void EncaminharSalvarB_Click(object sender, RoutedEventArgs e) {
            try {
                var dict = new Dictionary<string, double>() {
                    {"Dinheiro", DinheiroPago },
                    {"Crédito", CreditoPago },
                    {"Débito", DebitoPago }
                };
                foreach (var item in dict) {
                    if (double.IsNaN(item.Value)) {
                        MessageBox.Show($"O valor em '{item.Key}' não é um número.", $"Erro no valor em '{item.Key}'", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (item.Value < 0) {
                        MessageBox.Show($"O valor em '{item.Key}' não pode ser menor que zero.", $"Erro no valor em '{item.Key}'", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                double valorFornecido = dict.Aggregate(0.0, (ac, kv) => kv.Value + ac);
                if ((Total - valorFornecido) != 0) {
                    var r = MessageBox.Show(
                        $"O somatório dos valores fornecidos '{valorFornecido.ToString(Brasil)}' é diferente do valor esperado '{Total.ToString(Brasil)}'. " +
                        $"Deseja continuar?",
                        $"Valor pago diferente do valor esperado.", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                    if (r == MessageBoxResult.Cancel) return;
                }
                DateTime dt = DateTime.Now;
                var ad = AdicionadosDG.Items.OfType<Adicionados_Table>();
                var func = new Func<string, bool>((x) => Data.Any((y) => y == x.ToLower(), "ultrassom", "ultrassonografia"));
                var lista = ad.Where(x => func(x.Tipo));
                if (lista.Count() > 0) {
                    Scripts.DB.ListaEspera.Insert(CPF, dt, PStatus.Esperando, Fila.ExamesMedicos, Prioridade);
                    var senha = Scripts.DB.ListaEspera.Select_Paciente(CPF).Senha;
                    foreach (var item in lista) ListaEspera_Exames.Insert(senha, item.Proc_ID);
                }
                var p_fp = new Procedimento_FormaPagamentos();
                foreach (var item in ad) {
                    var valor = ToDouble(item.Valor, Brasil);
                    Historico_Procedimento.Insert(item.Proc_ID, CPF, dt, dt, valor);
                    var historico = Historico_Procedimento.Select(item.Proc_ID, CPF)[0];
                    int i = 0;
                    double percentage = valor / Total;
                    foreach (var keypair in dict) {
                        i++;
                        if (keypair.Value == 0) continue;
                        p_fp.Insert(historico.ID, i, keypair.Value * percentage);
                    }
                }
                if (App.Current.Properties["frame"] is Frame frame) frame.Content = new BlankPage();
            } catch (Exception ex) {
                ErrorHandler.OnError(ex);
            }
        }

        void CalcTotal() {
            double total = 0;
            foreach (var item in AdicionadosDG.Items.OfType<Adicionados_Table>()) {
                total += ToDouble(item.Valor, Brasil);
            }
            Total = total;
        }

        class Procedimento_Table {
            public int Proc_ID;
            public string Nome { get; set; }
            public string Valor { get; set; }
        }

        class Adicionados_Table {
            public int Proc_ID;
            public string Procedimento { get; set; }
            public string Tipo { get; set; }
            public string Valor { get; set; }
        }

    }
}
