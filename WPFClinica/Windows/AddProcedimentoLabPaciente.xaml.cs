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
using WPFClinica.Scripts.DB;

namespace WPFClinica.Windows {

    public partial class AddProcedimentoLabPaciente : Window {

        #region Vars

        string cpf_paciente;
        private string CPF_Paciente {
            get => cpf_paciente;
            set {
                cpf_paciente = value;
                if (value == null) {
                    MessageBox.Show("Paciente não informado.");
                    Close();
                }
                NomePacienteTB.Text = Pessoas.Select(cpf_paciente).Nome;
            }
        }
        int id_procedimento;
        public int ID_Procedimento {
            get => id_procedimento;
            set {
                id_procedimento = value;
                if (value >= 0) {
                    NomeProcedimentoTB.Text = Nome_Procedimento;
                } else {
                    NomeProcedimentoTB.Text = "???";
                }
            }
        }
        public string Nome_Procedimento {
            get => ProcedimentosLab.Select(ID_Procedimento);
        }
        public string Nome_Convênio {
            get => NomeConvenioTB.Text;
            set {
                NomeConvenioTB.Text = value;
                ValorTB.Text = value == null ? "???" : (Valor + "");
            }
        }

        int convenioID;
        public int ConvenioID {
            get {
                return convenioID;
            }
            set {
                convenioID = value;
                var r = Convenios.Select(value);
                Nome_Convênio = r.Convenio;
            }
        }

        public double Valor {
            get => ProcedimentoConvenio.Select(ID_Procedimento, ConvenioID);
        }

        #endregion

        public AddProcedimentoLabPaciente(string paciente) {
            InitializeComponent();
            CPF_Paciente = paciente;
        }

        private void MudarProcedimentoB_Click(object sender, RoutedEventArgs e) {
            var a = new EscolherProcedimento();
            a.ShowDialog();
            if (App.Current.Properties["procedimento"] is ProcedimentosLab procedimentos) {
                ID_Procedimento = procedimentos.ID;
            } else {
                ID_Procedimento = -1;
            }
        }

        private void MudarConvênioB_Click(object sender, RoutedEventArgs e) {
            var a = new EscolherConvenio(ID_Procedimento);
            a.ShowDialog();
            if (App.Current.Properties["convenio"] is int con_id) {
                ConvenioID = con_id;
            }
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs e) {
            if (Nome_Procedimento != "???" && ConvenioID > -1) {
                var le = ListaEspera.Select_Paciente(CPF_Paciente);
                PacienteProcedimentos.Insert(le.Senha, ID_Procedimento, ConvenioID, Scripts.ProcedimentoStatus.Pronto);
                Close();
            } else {
                MessageBox.Show("Selecione convênio e procedimento.");
            }
        }
    }
}
