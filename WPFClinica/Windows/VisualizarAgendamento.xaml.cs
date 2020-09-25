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
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class VisualizarAgendamento : Window {

        #region Vars

        int ID = -1;

        public bool IsEditing {
            get {
                return PacienteCPFTB.Visibility == Visibility.Hidden;
            }
            set {
                var v = value ? Visibility.Visible : Visibility.Hidden;
                PacienteCPFTB.Visibility = PacienteNomeTB.Visibility = PacienteContatoTB.Visibility = MedicoCPFTB.Visibility = MedicoNomeTB.Visibility = DataTB.Visibility = HorarioTB.Visibility = v;
                if (value) {
                    PacienteCPFTB.Text = PacienteCPFTBL.Text;
                    PacienteNomeTB.Text = PacienteNomeTBL.Text;
                    PacienteContatoTB.Text = PacienteContatoTBL.Text;
                    MedicoCPFTB.Text = MedicoCPFTBL.Text;
                    MedicoNomeTB.Text = MedicoNomeTBL.Text;
                    DataTB.Text = DataTBL.Text;
                    HorarioTB.Text = HorarioTBL.Text;
                } else {
                    PacienteCPFTBL.Text = PacienteCPFTB.Text;
                    PacienteNomeTBL.Text = PacienteNomeTB.Text;
                    PacienteContatoTBL.Text = PacienteContatoTB.Text;
                    MedicoCPFTBL.Text = MedicoCPFTB.Text;
                    MedicoNomeTBL.Text = MedicoNomeTB.Text;
                    DataTBL.Text = DataTB.Text;
                    HorarioTBL.Text = HorarioTB.Text;
                }
            }
        }

        DateTime Data {
            set {
                DataTBL.Text = value.ToString("dd/MM/yyyy");
            }
        }

        #endregion

        public VisualizarAgendamento(int id) {
            InitializeComponent();
            ID = id;
            WindowsSO.ToCenterOfScreen(this);
            var agendamento = Agendamentos.Select(id);
            if (agendamento != null) {
                Data = agendamento._Data;
                var i = agendamento.Inicio;
                var f = agendamento.Fim;
                HorarioTBL.Text = $"{i.Hours:00}:{i.Minutes:00}-{f.Hours:00}:{f.Minutes:00}";
                var am = AgendamentoFuncionario.Select(id);
                if (am != null) {
                    var pessoa = Pessoas.Select(am.Funcionario);
                    MedicoCPFTBL.Text = pessoa.CPF;
                    MedicoNomeTBL.Text = pessoa.Nome;
                }
                var atp = AgendamentoTempPaciente.Select(id);
                if (atp != null) {
                    var contato = TempPacientes.Select(atp.TempPaciente);
                    PacienteNomeTBL.Text = atp.TempPaciente;
                    PacienteContatoTBL.Text = contato;
                } else {
                    var ap = AgendamentoPaciente.Select(id);
                    PacienteCPFTBL.Text = ap.Paciente;
                    var pessoa = Pessoas.Select(ap.Paciente);
                    PacienteNomeTBL.Text = pessoa.Nome;
                    PacienteContatoTBL.Text = pessoa.Contato1;
                }
            }
        }

        private void OkB_Click(object sender, RoutedEventArgs e) {
            if (IsEditing) {
                var r = MessageBox.Show("", "Confirmar", MessageBoxButton.OKCancel, MessageBoxImage.Question);
                if (r == MessageBoxResult.OK) {
                    IsEditing = false;
                    EditarB.IsEnabled = true;
                }
            } else {
                Close();
            }
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
            IsEditing = true;
            EditarB.IsEnabled = false;
        }
    }
}
