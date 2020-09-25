using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
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

    public partial class PacientesLab : Page {

        #region Vars

        TimedEvent UPa;
        CultureInfo Brasil = new CultureInfo("pt-BR");
        Table1 LastSelection = null;

        string Nome {
            get => NomeTB.Text;
        }

        ProcedimentoStatus? PesquisaStatus {
            get {
                switch (PesquisaStatusCB.SelectedIndex) {
                    case 0:
                        return null;
                    case 1:
                        return ProcedimentoStatus.Pendente;
                    case 2:
                        return ProcedimentoStatus.EmAndamento;
                    case 3:
                        return ProcedimentoStatus.Pronto;
                    default:
                        throw new Exception("Escolha de staus inválida.");
                }
            }
        }

        string Procedimento {
            get {
                if (PesquisaProcedimentoCB.SelectedIndex == 0) {
                    return null;
                } else {
                    return (PesquisaProcedimentoCB.SelectedItem as ComboBoxItem).Content.ToString();
                }
            }
        }

        ProcedimentoStatus SelectedStatus {
            get => EnumStrings.SetProcedimento((SelectedStatusCB.SelectedItem as ComboBoxItem).Content.ToString());
            set {
                switch (value) {
                    case ProcedimentoStatus.Pendente:
                        SelectedStatusCB.SelectedIndex = 0;
                        break;
                    case ProcedimentoStatus.EmAndamento:
                        SelectedStatusCB.SelectedIndex = 1;
                        break;
                    case ProcedimentoStatus.Pronto:
                        SelectedStatusCB.SelectedIndex = 2;
                        break;
                }
            }
        }

        int SelectedIndex {
            set {
                SelectedStatusCB.SelectedIndex = value;
            }
        }

        string Observacao {
            get => ObservacaoTB.Text;
            set => ObservacaoTB.Text = value;
        }

        #endregion

        public PacientesLab() {
            InitializeComponent();
            ControlsH.CreateColumns(PesquisaDG, typeof(Table1));
            ControlsH.CreateColumns(PesquisaProcedimentosDG, typeof(Table2));
            UPa = new TimedEvent(1, 3, true, UpdatePacientes);
            UPa.TriggerNow();
        }

        void UpdatePacientes() {
            PesquisaDG.Items.Clear();
            var r = PacienteProcedimentos.SelectLike_NomePaciente(Nome);
            for (int i = 0; i < r.Count; i++) {
                var item = r[i];
                PesquisaDG.Items.Add(new Table1() {
                    CPF = item.CFP_Paciente,
                    Senha = item.Senha + "",
                    Nome = item.Nome_Paciente,
                    Horario = item.Atualização.ToString("dd-MM-yyyy HH:mm")
                });
            }
        }

        void UpdateProcedimentos(int senha) {
            PesquisaProcedimentosDG.Items.Clear();
            var r = PacienteProcedimentos.Select(senha);
            for (int i = 0; i < r.Count; i++) {
                var row = r[i];
                PesquisaProcedimentosDG.Items.Add(new Table2() {
                    Procedimento_ID = row.Proc_ID,
                    Procedimento = row.Procedimento,
                    Con_ID = row.Con_ID,
                    Convênio = row.Convenio,
                    Status = row.Status,
                    Valor = row.Valor.ToString(Brasil)
                });
            }
        }

        public class Table1 {

            public string CPF;
            public string Senha { get; set; }
            public string Nome { get; set; }
            public string Horario { get; set; }
        }

        public class Table2 {

            public int Procedimento_ID;
            public int Con_ID;
            public string Procedimento { get; set; }
            public string Convênio { get; set; }
            public string Status { get; set; }
            public string Valor { get; set; }

        }

        private void PesquisaProcedimentosDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = PesquisaProcedimentosDG.SelectedIndex >= 0;
            if (PesquisaProcedimentosDG.SelectedItem is Table2 table2) {
                SelectedStatus = EnumStrings.SetProcedimento(table2.Status);
            }
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {
            UPa.TryTigger();
        }

        private void PesquisaProcedimentoCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void PesquisaStatusCB_SelectionChanged(object sender, SelectionChangedEventArgs e) {

        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            try {
                if (PesquisaProcedimentosDG.SelectedItem is Table2 table2) {
                    var senha = ToInt32(LastSelection.Senha);
                    PacienteProcedimentos.Update(senha, table2.Procedimento_ID, SelectedStatus);
                    var cpf = LastSelection.CPF;
                    var proc_id = table2.Procedimento_ID;
                    var con_id = table2.Con_ID;
                    try {
                        Historico_ProcedimentosLab.Update_RealizadoEm(cpf, proc_id, table2.Con_ID, SelectedStatus == ProcedimentoStatus.Pronto ? DateTime.Now : (DateTime?)null);
                    } catch (Exception ex) {
                        ErrorHandler.OnError(ex, $"Erro ao atualizar histórico de procedimentos: CPF: {cpf}, ProcID: {proc_id}, ConID: {con_id}.");
                    }
                    UpdateProcedimentos(senha);
                    if (PacienteProcedimentos.Select(senha).All(x => EnumStrings.SetProcedimento(x.Status) == ProcedimentoStatus.Pronto)) {
                        UpdatePacientes();
                        PesquisaProcedimentosDG.Items.Clear();
                        SelectedStatusCB.SelectedIndex = -1;
                        Observacao = "";
                        var t = new Thread(() => {
                            try {
                                Scripts.DB.ListaEspera.Delete(senha);
                            } catch (Exception ex) {
                                ErrorHandler.OnError(ex);
                            }
                        });
                        t.Start();
                    }
                }
            } catch (Exception ex) {
                ErrorHandler.OnError(ex);
            }
        }

        private void PesquisaDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (PesquisaDG.SelectedItem is Table1 table1) {
                LastSelection = table1;
                UpdateProcedimentos(ToInt32(table1.Senha));
            } else {
                LastSelection = null;
                PesquisaDG.Items.Clear();
            }
        }
    }
}
