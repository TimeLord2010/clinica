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
using WPFClinica.UserControls;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Windows {

    public partial class CadastrarPaciente : Window {

        #region Vars

        MySqlH sql;
        InformacoesPessoais InformacoesPessoais;
        Endereco Endereco;
        public string OriginalCPF;
        public string OriginalName;

        string CPF {
            get => InformacoesPessoais.CPF;
            set => InformacoesPessoais.CPF = value;
        }

        public string Nome {
            get => InformacoesPessoais.Nome;
            set => InformacoesPessoais.Nome = value;
        }

        string Email {
            get => InformacoesPessoais.Email;
            set => InformacoesPessoais.Email = value;
        }

        public string Contato1 {
            get => InformacoesPessoais.Contato1;
            set => InformacoesPessoais.Contato1 = value;
        }

        string Contato2 {
            get => InformacoesPessoais.Contato2;
            set => InformacoesPessoais.Contato2 = value;
        }

        bool IsMale {
            get => InformacoesPessoais.IsMale;
            set => InformacoesPessoais.IsMale = value;
        }

        string Nascimento {
            get => InformacoesPessoais.Nascimento;
            set => InformacoesPessoais.Nascimento = value;
        }

        string CEP {
            get => Endereco.CEP;
            set => Endereco.CEP = value;
        }

        string Estado {
            get => Endereco.Estado;
            set => Endereco.Estado = value;
        }

        string Cidade {
            get => Endereco.Cidade;
            set => Endereco.Cidade = value;
        }

        string Bairro {
            get => Endereco.Bairro;
            set => Endereco.Bairro = value;
        }

        string Rua {
            get => Endereco.Rua;
            set => Endereco.Rua = value;
        }

        string Numero {
            get => Endereco.Numero;
            set => Endereco.Numero = value;
        }

        string Complemento {
            get => Endereco.Complemento;
            set => Endereco.Complemento = value;
        }

        bool ContatoAtualizacao {
            get => ContatoAtualizacaoChB.IsChecked ?? false;
            set => ContatoAtualizacaoChB.IsChecked = value;
        }

        bool ContatoDesconto {
            get => ContatoDescontoChB.IsChecked ?? false;
            set => ContatoDescontoChB.IsChecked = value;
        }

        bool EmailAtualizacao {
            get => EmailAtualizacaoChB.IsChecked ?? false;
            set => EmailAtualizacaoChB.IsChecked = value;
        }

        bool EmailDesconto {
            get => EmailDescontoChB.IsChecked ?? false;
            set => EmailDescontoChB.IsChecked = value;
        }

        #endregion

        public CadastrarPaciente(string cpf = null) {
            InitializeComponent();
            sql = App.Current.Properties["sql"] as MySqlH;
            InformacoesPessoais = new InformacoesPessoais();
            MyGrid.Children.Add(InformacoesPessoais);
            Grid.SetColumnSpan(InformacoesPessoais, 4);
            Grid.SetRowSpan(InformacoesPessoais, 2);
            Endereco = new Endereco();
            MyGrid.Children.Add(Endereco);
            Grid.SetRow(Endereco, 4);
            Grid.SetRowSpan(Endereco, 2);
            Grid.SetColumnSpan(Endereco, 4);
            if (cpf != null) {
                OriginalCPF = cpf;
                CPF = cpf;
                var paciente = Pacientes.Select(cpf);
                ContatoAtualizacao = paciente.AC;
                ContatoDesconto = paciente.NC;
                EmailAtualizacao = paciente.AE;
                EmailDesconto = paciente.NE;
                var pessoa = Pessoas.Select(cpf);
                Nome = pessoa.Nome;
                var d = pessoa.Nascimento;
                Nascimento = $"{d.Day}/{d.Month}/{d.Year}";
                IsMale = pessoa.Sexo;
                Email = pessoa.Email;
                Contato1 = pessoa.Contato1;
                Contato2 = pessoa.Contato2;
                var endereco = Enderecos.Select(cpf);
                CEP = endereco.CEP;
                Estado = endereco.Estado;
                Cidade = endereco.Cidade;
                Bairro = endereco.Bairro;
                Rua = endereco.Rua;
                Numero = endereco.Numero;
                Complemento = endereco.Complemento;
            }
        }

        private void CadastrarB_Click(object sender, RoutedEventArgs e) {
            try {
                var parts = Nascimento.Split('/');
                if (parts.Length != 3) {
                    var nascimento = Nascimento.Replace(" ", "");
                    if (nascimento.Length == 0) {
                        parts = new string[] { "01", "01", "1800" };
                    } else {
                        throw new Exception("A data de nascimento não está no formato correto.");
                    }
                }
                if (parts[2].Length != 4) throw new Exception("O ano da data de nascimento deve ter 4 dígitos.");
                var converted = Array.ConvertAll(parts, x => ToInt32(x));
                var dt = new DateTime(converted[2], converted[1], converted[0]);
                SQLOperations.ThrowExceptions = true;
                if (OriginalCPF == null) {
                    Pessoas.EnsureExistence(CPF, Nome, dt, IsMale, Email, Contato1, Contato2);
                    Pacientes.InsertOrUpdate(CPF, ContatoAtualizacao, ContatoDesconto, EmailAtualizacao, EmailDesconto);
                    Enderecos.InsertOrUpdate(CPF, CEP, Estado, Cidade, Bairro, Rua, Numero, Complemento);
                } else {
                    if (OriginalCPF != CPF) Pessoas.UpdateCPF(OriginalCPF, CPF);
                    Pessoas.Update(CPF, Nome, dt, IsMale, Email, Contato1, Contato2);
                    Pacientes.Update(CPF, ContatoAtualizacao, ContatoDesconto, EmailAtualizacao, EmailDesconto);
                    Enderecos.Update(CPF, CEP, Estado, Cidade, Bairro, Rua, Numero, Complemento);
                    if (OriginalName != null) TempPacientes.Delete(OriginalName);
                }
                SQLOperations.ThrowExceptions = false;
                Close();
            } catch (Exception ex) {
                var op = OriginalCPF == null ? "cadastrar" : "editar";
                MessageBox.Show($"Classe: {ex.GetType()}\nMensagem: {ex.Message}", $"Erro ao {op} cliente");
            }
        }
    }
}
