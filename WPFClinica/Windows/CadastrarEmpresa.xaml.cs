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

    public partial class CadastrarEmpresa : Window {

        int ID;

        public string NomeEmpresa {
            get => NomeEmpresaTB.Text;
            set => NomeEmpresaTB.Text = value;
        }

        public string CNPJ {
            get => CNPJ_TB.Text;
            set => CNPJ_TB.Text = value;
        }

        public string Observação {
            get => ObservacaoTB.Text;
            set => ObservacaoTB.Text = value;
        }

        public string Message {
            get => MessageTBL.Text;
            set => MessageTBL.Text = value;
        }

        public CadastrarEmpresa(int id = -1) {
            InitializeComponent();
            ID = id;
            if (ID >= 0) {
                CadastrarB.Content = $"Editar";
                var em = Empresa.Select(ID);
                NomeEmpresa = em.Nome;
                CNPJ = em.CNPJ;
                Observação = em.Observação;
            }
        }

        public bool IsValid() {
            Message = "";
            if (NomeEmpresa.Replace(" ", "").Length == 0) {
                Message = $"O nome é vazio. Espaços não contam para a contagem do tamanho do nome da empresa.";
                return false;
            }
            return true;
        }

        private void CadastrarB_Click(object sender, RoutedEventArgs e) {
            if (!IsValid()) {
                return;
            }
            try {
                if (ID >= 0) {
                    Empresa.Update(ID, NomeEmpresa, CNPJ, Observação);
                } else {
                    Empresa.Insert(NomeEmpresa, CNPJ, Observação);
                }
                DialogResult = true;
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro ao cadastrar empresa.");
                DialogResult = false;
            }
        }
    }
}
