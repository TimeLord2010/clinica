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
using WPFClinica.Scripts;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Pages {

    public partial class PesquisaPessoas : Page {

        #region Vars

        TimedEvent TimedEvent;

        #endregion

        public PesquisaPessoas() {
            InitializeComponent();
            ControlsH.CreateColumns(PessoasDG, typeof(Table1));
            ControlsH.CreateColumns(EnderecoDG, typeof(Table2));
            TimedEvent = new TimedEvent(1, 3, true, UpdatePessoas);
            TimedEvent.TriggerNow();
        }

        void UpdatePessoas () {
            var pessoas = Pessoas.SelectLike("", NomeTB.Text);
            for (int i = 0; i < pessoas.Count; i++) {
                var p = pessoas[i];
                PessoasDG.Items.Add(new Table1() {
                    CPF = p.CPF,
                    Nome = p.Nome,
                    Nascimento = p.Nascimento.ToString("dd/MM/yyyy"),
                    Email = p.Email,
                    Sexo = p.Sexo ? "M" : "F",
                    Idade = p.Idade+"",
                    Contato1 = p.Contato1,
                    Contato2 = p.Contato2
                });
            }
        }

        private void NomeTB_TextChanged(object sender, TextChangedEventArgs e) {

        }

        class Table1 {

            public string CPF { get; set; }
            public string Nome { get; set; }
            public string Nascimento { get; set; }
            public string Idade { get; set; }
            public string Sexo { get; set; }
            public string Email { get; set; }
            public string Contato1 { get; set; }
            public string Contato2 { get; set; }

        }

        class Table2 {

            public string CEP { get; set; }
            public string Estado { get; set; }
            public string Cidade { get; set; }
            public string Bairro { get; set; }
            public string Rua { get; set; }
            public string Numero { get; set; }
            public string Complemento { get; set; }

        }

        private void PessoasDG_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            EditarB.IsEnabled = DeletarB.IsEnabled = PessoasDG.SelectedIndex >= 0;
                EnderecoDG.Items.Clear();
            if (PessoasDG.SelectedItem is Table1 table1) {
                Enderecos.Select(table1.CPF);
            }
        }

        private void EditarB_Click(object sender, RoutedEventArgs e) {

        }

        private void DeletarB_Click(object sender, RoutedEventArgs e) {

        }
    }
}
