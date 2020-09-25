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
using WPFClinica.Windows;
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;
using System.Globalization;
using static System.Convert;

namespace WPFClinica.Pages {

    public partial class FinalizarPaciente : Page {

        #region Vars

        TimedEvent TimedEvent;
        CultureInfo Brasil = new CultureInfo("pt-BR");

        string CPF_Paciente {
            get => CPFTB.Text;
            set => CPFTB.Text = value;
        }

        string Nome_Paciente {
            get => NomeTB.Text;
            set => NomeTB.Text = value;
        }

        bool MostrarTodos {
            get => MostrarTodosChB.IsChecked ?? false;
            set => MostrarTodosChB.IsChecked = value;
        }

        PStatus? Status {
            get {
                if (MostrarTodos) {
                    return null;
                } else {
                    return PStatus.Atendido;
                }
            }
        }

        double TotalPagar {
            set {
                if (Double.IsNaN(value)) {
                    ValorTotalTBL.Text = "R$ 000.00";
                } else {
                    ValorTotalTBL.Text = "R$ " + value.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        #endregion

        public FinalizarPaciente() {
            InitializeComponent();
            ControlsH.CreateColumns(PacientesDG, typeof(Table1));
            ControlsH.CreateColumns(ProcedimentosDG, typeof(Table2));
            TimedEvent = new TimedEvent(1, 2, true, UpdatePacientes);
            TimedEvent.TriggerNow();
        }

        void UpdatePacientes() {
            PacientesDG.Items.Clear();
            var r = Scripts.DB.ListaEspera.SelectLike(CPF_Paciente, Nome_Paciente, null, Status);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                PacientesDG.Items.Add(new Table1() {
                    Senha = row.Senha+"",
                    Atualização = row.Atualizacao.ToString("dd/MM/yyyy HH:mm"),
                    Fila = row.Fila,
                    Status = row.Status,
                    CPF = row.CPF,
                    Nome = row.Nome
                });
            }
        }

        void UpdateProcedimentos(string senha) {
            UpdateProcedimentos(ToInt32(senha));
        }

        void UpdateProcedimentos(int senha) {
            ProcedimentosDG.Items.Clear();
            var r = PacienteProcedimentos.Select(senha);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                ProcedimentosDG.Items.Add(new Table2() {
                    Proc_ID = row.Proc_ID,
                    Procedimento = row.Procedimento,
                    Conv_ID = row.Con_ID,
                    Convênio = row.Convenio,
                    Valor = row.Valor.ToString(Brasil)
                });
            }
        }

        private void PacientesDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (PacientesDG.SelectedItem is Table1 table1) {
                UpdateProcedimentos(ToInt32(table1.Senha));
                double total = 0;
                for (int i = 0; i < ProcedimentosDG.Items.Count; i++) {
                    var item = ProcedimentosDG.Items[i] as Table2;
                    total += ToDouble(item.Valor);
                }
                TotalPagar = total;
            } else {
                ProcedimentosDG.Items.Clear();
                TotalPagar = Double.NaN;
            }
        }

        private void ProcedimentosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            RemoverProcedimentoB.IsEnabled = EditarProcedimentoB.IsEnabled = ProcedimentosDG.SelectedIndex >= 0;
        }

        private void EditarConsultaB_Click(object sender, RoutedEventArgs e) {

        }

        private void SalvarB_Click(object sender, RoutedEventArgs e) {
            if (PacientesDG.SelectedItem is Table1 t) {
                for (int i = 0; i < ProcedimentosDG.Items.Count; i++) {
                    var r = ProcedimentosDG.Items[i] as Table2;
                    Historico_ProcedimentosLab.Update_PagoEm(t.CPF, r.Proc_ID, r.Conv_ID, DateTime.Now, ToDouble(r.Valor, Brasil));
                }
                Scripts.DB.ListaEspera.Delete(ToInt32(t.Senha));
                UpdatePacientes();
            }
        }

        private void MostrarTodosChB_Checked(object sender, RoutedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void MostrarTodosChB_Unchecked(object sender, RoutedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        private void CPFTB_TextChanged(object sender, TextChangedEventArgs e) {
            TimedEvent.TryTigger();
        }

        class Table1 {

            public string Senha { get; set; }
            public string Atualização { get; set; }
            public string Fila { get; set; }
            public string Status { get; set; }
            public string CPF { get; set; }
            public string Nome { get; set; }

        }

        class Table2 {

            public int Proc_ID;
            public int Conv_ID;
            public string Procedimento { get; set; }
            public string Convênio { get; set; }
            public string Valor { get; set; }

        }

        private void RemoverProcedimentoB_Click(object sender, RoutedEventArgs e) {
            if (ProcedimentosDG.SelectedItem is Table2 table2 && PacientesDG.SelectedItem is Table1 table1) {
                PacienteProcedimentos.Delete(table1.Senha, table2.Procedimento);
                UpdateProcedimentos(table1.Senha);
            }
        }

        private void EditarProcedimentoB_Click(object sender, RoutedEventArgs e) {
            if (ProcedimentosDG.SelectedItem is Table2 table2) {
                MessageBox.Show("Não implementado.");
            }
        }

        private void AdicionarProcedimentoB_Click(object sender, RoutedEventArgs e) {
            var a = new AddProcedimentoLabPaciente(CPF_Paciente);
            a.ShowDialog();
            if (PacientesDG.SelectedItem is Table1 table1) {
                UpdateProcedimentos(table1.Senha);
            }
        }

    }
}
