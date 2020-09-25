using System;
using System.Collections.Generic;
using System.IO;
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
using MySql.Data.MySqlClient;
using WPFClinica.Scripts;
using WPFClinica.UserControls;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Windows {

    public partial class CadastroFuncionario : Window {

        #region Vars

        Administradores admin = new Administradores();
        Fonoaudiologos fono = new Fonoaudiologos();
        Dentistas dent = new Dentistas();

        InformacoesPessoais Pessoais;
        Endereco Endereco;

        string PicturePath = null;
        string OriginalCPF;

        string Nome {
            get => Pessoais.Nome;
            set => Pessoais.Nome = value;
        }

        string CPF {
            get => Pessoais.CPF;
            set => Pessoais.CPF = value;
        }

        string DataNascimento {
            get => Pessoais.Nascimento;
            set => Pessoais.Nascimento = value;
        }

        string Contato1 {
            get => Pessoais.Contato1;
            set => Pessoais.Contato1 = value;
        }

        string Contato2 {
            get => Pessoais.Contato2;
            set => Pessoais.Contato2 = value;
        }

        bool IsMale {
            get => Pessoais.IsMale;
            set => Pessoais.IsMale = value;
        }

        string Email {
            get => Pessoais.Email;
            set => Pessoais.Email = value;
        }

        bool IsAdmin {
            get => AdministradorChB.IsChecked ?? false;
            set => AdministradorChB.IsChecked = value;
        }

        bool IsRecepcionista {
            get => RecepcionistaChB.IsChecked ?? false;
            set => RecepcionistaChB.IsChecked = value;
        }

        bool IsTecnico {
            get => TecnicoEnfermeiroChB.IsChecked ?? false;
            set => TecnicoEnfermeiroChB.IsChecked = value;
        }

        bool IsFono {
            get => FonoaudiologoChB.IsChecked ?? false;
            set => FonoaudiologoChB.IsChecked = value;
        }

        bool IsPsicologo {
            get => PsicologoChB.IsChecked ?? false;
            set => PsicologoChB.IsChecked = value;
        }

        bool IsNutri {
            get => NutricionistaChB.IsChecked ?? false;
            set => NutricionistaChB.IsChecked = value;
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

        string Salario {
            get => SalarioTB.Text;
            set => SalarioTB.Text = value;
        }

        string Observacoes {
            get => ObservacoesTB.Text;
            set => ObservacoesTB.Text = value;
        }

        string CRM {
            get => CRMTB.Text;
            set => CRMTB.Text = value;
        }

        string Porcentagem {
            get {
                if (PorcentagemConsultaTB.Text.Length == 0) return "0";
                return PorcentagemConsultaTB.Text.Replace("%", "");
            }
            set => PorcentagemConsultaTB.Text = value;
        }

        bool IsDoctor {
            get => MedicoChB.IsChecked ?? false;
            set {
                MedicoChB.IsChecked = value;
                if (value) {
                    EspecificacoesMedicoGB.IsEnabled = true;
                } else {
                    EspecificacoesMedicoGB.IsEnabled = false;
                    CRM = "";
                    Porcentagem = "";
                    EspecializacoesSP.Children.Clear();
                }
            }
        }

        bool IsDentist {
            get => DentistaChB.IsChecked ?? false;
            set {
                DentistaChB.IsChecked = value;
            }
        }

        string Senha {
            get => SenhaPB.Password;
            set => SenhaPB.Password = value;
        }

        string ConfirmarSenha {
            get => ConfirmarSenhaPB.Password;
            set => ConfirmarSenhaPB.Password = value;
        }

        bool IsAtivo {
            get => AtivoCB.IsChecked ?? false;
            set => AtivoCB.IsChecked = value;
        }

        #endregion

        public CadastroFuncionario(string cpf = null) {
            InitializeComponent();
            WindowsSO.ToCenterOfScreen(this);
            Endereco = new Endereco();
            MyGrid2.Children.Add(Endereco);
            Grid.SetRow(Endereco, 3);
            Grid.SetRowSpan(Endereco, 2);
            Grid.SetColumnSpan(Endereco, 4);
            Pessoais = new InformacoesPessoais();
            Pessoais.Editing = true;
            MyGrid2.Children.Add(Pessoais);
            Grid.SetRowSpan(Pessoais, 2);
            Grid.SetColumnSpan(Pessoais, 4);
            if (cpf != null) {
                OriginalCPF = cpf;
                CPF = cpf;
                var pessoa = Pessoas.Select(CPF);
                if (pessoa != null) {
                    Nome = pessoa.Nome;
                    DataNascimento = pessoa.Nascimento.ToString("dd/MM/yyyy").Replace("-", "/");
                    IsMale = pessoa.Sexo;
                    Email = pessoa.Email;
                    Contato1 = pessoa.Contato1;
                    Contato2 = pessoa.Contato2;
                }
                Funcionarios.SelectPic(cpf, out PicturePath, out byte[] data);
                if (PicturePath != null) {
                    using (var s = new FileStream(PicturePath, FileMode.Create, FileAccess.Write)) {
                        s.Write(data, 0, data.Length);
                    }
                    SetPic(PicturePath);
                }
                var funcionario = Funcionarios.Select(cpf);
                if (funcionario != null) {
                    Salario = funcionario.Salario + "";
                    Senha = funcionario.Senha;
                    Observacoes = funcionario.Observacao;
                    IsAtivo = funcionario.Ativo;
                }
                var endereco = Enderecos.Select(cpf);
                if (endereco != null) {
                    CEP = endereco.CEP;
                    Estado = endereco.Estado;
                    Cidade = endereco.Cidade;
                    Bairro = endereco.Bairro;
                    Rua = endereco.Rua;
                    Numero = endereco.Numero;
                    Complemento = endereco.Complemento;
                }
                var medico = Medicos.Select(cpf);
                if (medico != null) {
                    IsDoctor = true;
                    CRM = medico.CRM;
                    Porcentagem = medico.Porcentagem;
                    MedicoEspecializacoes.Select(cpf).ForEach(x => {
                        EspecializacoesSP.Children.Add(new Especializacao(x));
                    });
                }
                IsRecepcionista = Recepcionista.IsRecepcionista(cpf);
                IsAdmin = admin.IsProfissional(cpf);
                IsTecnico = Tecnico_Enfermagem.IsTecnico(cpf);
                IsNutri = Nutricionistas.IsNutricionista(cpf);
                IsFono = fono.IsProfissional(cpf);
                IsPsicologo = Psicologos.IsPsicologo(cpf);
                IsDentist = dent.IsProfissional(cpf);
                Anexos.SelectNames(CPF).ForEach(x => {
                    AnexosSP.Children.Add(new Anexo(x) { CPF = cpf });
                });
            }
        }

        void SetPic(string file) {
            AddImageR.Visibility = AddImageTBL.Visibility = Visibility.Hidden;
            PicturePath = file;
            AddImageI.Source = ImageH.ToBitmapImage(file);
        }

        private void AdicionarEspecializacao_Click(object sender, RoutedEventArgs e) {
            EspecializacoesSP.Children.Add(new Especializacao());
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (WindowsSO.ChooseFile(out string file, $"C:\\Users\\{Environment.UserName}\\Pictures", "Imagens|*.png;*.jpg;*.jpeg;*.gif", this)) {
                SetPic(file);
            }
        }

        private void AdicionarAnexo_Click(object sender, RoutedEventArgs e) {
            if (WindowsSO.ChooseFile(out string file, "C:\\", "Todos|*.*", this)) {
                AnexosSP.Children.Add(new Anexo(file));
            }
        }

        private void MedicoChB_Checked(object sender, RoutedEventArgs e) => IsDoctor = true;

        private void MedicoChB_Unchecked(object sender, RoutedEventArgs e) => IsDoctor = false;

        private void CadastrarB_Click(object sender, RoutedEventArgs e) {
            try {
                #region Validation
                if (Salario.Replace(" ", "").Length == 0) Salario = "0";
                if (!Double.TryParse(Salario, out double _)) throw new Exception("A entrada de salário não é um número.");
                Cursor = Cursors.Wait;
                if (Senha.Length < 4) throw new Exception("A senha precisa ter no mínimo 4 caracteres.");
                FileInfo fi = PicturePath == null ? null : new FileInfo(PicturePath);
                var parts = DataNascimento.Replace(" ", "").Split('/');
                if (parts.Length != 3) throw new Exception("Data de nascimento inválida.");
                if (parts[2].Length < 4) throw new Exception("A data precisa estar com o ano por extenso.\nCorreto: 1997\nErrado: 97");
                #endregion
                DateTime data = new DateTime(ToInt32(parts[2]), ToInt32(parts[1]), ToInt32(parts[0]));
                if (OriginalCPF != null) {
                    if (CPF != OriginalCPF) Pessoas.UpdateCPF(OriginalCPF, CPF);
                    Pessoas.Update(CPF, Nome, data, IsMale, Email, Contato1, Contato2);
                    Funcionarios.Update(CPF, Double.Parse(Salario), Senha, Observacoes, IsAtivo, fi);
                    if (File.Exists(PicturePath) && PicturePath.Count(x => x == '/') == 0) File.Delete(PicturePath);
                } else {
                    if (Senha != ConfirmarSenha) throw new Exception("As senhas não são iguais.");
                    SQLOperations.ThrowExceptions = true;
                    Pessoas.InsertOrUpdate(CPF, Nome, data, IsMale, Email, Contato1, Contato2);
                    Funcionarios.InsertOrUpdate(CPF, Double.Parse(Salario), Senha, Observacoes, IsAtivo, fi);
                    //Enderecos.InsertOrUpdate(CPF, CEP, Estado, Cidade, Bairro, Rua, Numero, Complemento);
                    for (int i = 0; i < AnexosSP.Children.Count; i++) Anexos.InsertOrUpdate(CPF, (AnexosSP.Children[i] as Anexo).Original);
                }
                Enderecos.InsertOrUpdate(CPF, CEP, Estado, Cidade, Bairro, Rua, Numero, Complemento);
                MedicoEspecializacoes.Delete(CPF);
                if (IsDoctor) {
                    Medicos.TryInsert(CPF, CRM, Porcentagem);
                    for (int i = 0; i < EspecializacoesSP.Children.Count; i++) {
                        var especializacao = EspecializacoesSP.Children[i] as Especializacao;
                        MedicoEspecializacoes.Insert(CPF, especializacao.Valor);
                    }
                } else {
                    Medicos.Delete(CPF);
                }
                admin.Update(IsAdmin, CPF);
                if (IsRecepcionista) {
                    Recepcionista.TryInsert(CPF);
                } else {
                    Recepcionista.Delete(CPF);
                }
                if (IsTecnico) {
                    Tecnico_Enfermagem.TryInsert(CPF);
                } else {
                    Tecnico_Enfermagem.Delete(CPF);
                }
                fono.Update(IsFono, CPF);
                if (IsPsicologo) {
                    Psicologos.TryInsert(CPF);
                } else {
                    Psicologos.Delete(CPF);
                }
                if (IsNutri) {
                    Nutricionistas.TryInsert(CPF);
                } else {
                    Nutricionistas.Delete(CPF);
                }
                dent.Update(IsDentist, CPF);
                Close();
            } catch (Exception ex) {
                //MessageBox.Show($"Classe: {ex.GetType()}\nMensagem: {ex.Message}", $"Erro ao {(OriginalCPF != null ? "editar" : "cadastrar")} funcionário.");
                ErrorHandler.OnError(ex);
            } finally {
                SQLOperations.ThrowExceptions = false;
                Cursor = Cursors.Arrow;
            }
        }
    }
}
