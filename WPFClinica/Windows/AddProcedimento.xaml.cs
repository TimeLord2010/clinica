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
using System.Globalization;
using WPFClinica.Scripts.DB;
using WPFClinica.Scripts;
using static System.Convert;

namespace WPFClinica.Windows {

    public partial class AddProcedimento : Window {

        #region Vars

        CultureInfo Brasil = new CultureInfo("pt-BR");
        int ID;

        string NomeProcedimento {
            get => ProcedimentoTB.Text;
            set => ProcedimentoTB.Text = value;
        }

        double Valor {
            get {
                try {
                    return ToDouble(ValorTB.Text, Brasil);
                } catch (Exception) {
                    return double.NaN;
                }
            }
            set => ValorTB.Text = value.ToString(Brasil);
        }

        #endregion

        public AddProcedimento(int id = -1) {
            InitializeComponent();
            TipoProcedimento.Fill(TipoCB);
            ID = id;
            if (ID >= 0) {
                AdicionarB.Content = "Editar";
                var r = Procedimentos.Select_ID(ID);
                TipoProcedimento.SetItem(TipoCB, r.Tipo_ID);
                NomeProcedimento = r.Descricao;
                Valor = r.Valor;
            }
        }

        private void AdicionarB_Click(object sender, RoutedEventArgs ev) {
            try {
                if (TipoCB.SelectedIndex < 0) {
                    throw new Exception("Selecione o tipo.");
                }
                var tipo = TipoProcedimento.Select(TipoCB);
                if (tipo == null) {
                    throw new Exception("Não foi possível obter o identificador do tipo.");
                }
                if (double.IsNaN(Valor)) {
                    throw new Exception("O número no campo 'valor' não é válido.");
                }
                if (ID >= 0) {
                    Procedimentos.Update(ID, NomeProcedimento, tipo.Value, Valor);
                } else {
                    Procedimentos.Insert(NomeProcedimento, tipo.Value, Valor);
                }
                Close();
            } catch (Exception ex) {
                MessageBox.Show(ex.Message, "Erro");
            }
        }
    }
}