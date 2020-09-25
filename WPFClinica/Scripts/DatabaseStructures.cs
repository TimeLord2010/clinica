using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFClinica.Scripts.DB;
using static System.Convert;

namespace WPFClinica.Scripts {

    public enum FP {
        Dinheiro = 1,
        Credito = 2,
        Debito = 3
    }

    public enum Profissao {
        Medico, TecnicoEnfermagem, Fonoaudiologo, Psicologo, Nutricionista, Recepcionista, Administrador, Dentista
    }

    public enum SStatus {
        EmAtendimento, Ocupado, Livre, Aguardando
    }

    public enum PStatus {
        Esperando, EmAtendimento, Atendido, ACaminho
    }

    public enum ProcedimentoStatus {
        Pendente, EmAndamento, Pronto
    }

    public enum Fila {
        Triagem,
        Consultorio,
        Laboratorio,
        ExamesMedicos,
        Nulo
    }

    public enum Funcao {
        Medico, Recepcionista, Tecnico, Administrador
    }

    public enum Exame {
        Laboratorial, Ultrassom, RaioX, Tomografia, Outros
    }

    public interface IProcedimentoLab {
        int Proc_ID { get; set; }
        string ProcedimentoLab { get; set; }
    }

    public interface IConvenio {
        int Con_ID { get; set; }
        string Convenio { get; set; }
    }

    public interface IProcedimentoLabConvenios : IProcedimentoLab, IConvenio {
        int Valor { get; set; }
    }

    public struct _ProcedimentoConvenio {
        public int Proc_ID;
        public string Procedimento;
        public int Con_ID;
        public string Convenio;
        public double Valor;
    }

    public struct _Procedimento {
        public int ID;
        public int Tipo_ID;
        public string Descricao;
        public string Tipo;
        public double Valor;
    }

    public class _Lista_Procedimentos {
        public _ListaEspera ListaEspera;
        public List<_Procedimento> Procedimentos;
    }

    public struct _HistoricoProcedimento {
        public int ID;
        public int Proc_ID;
        public int Tipo_ID;
        public string Procedimento;
        public string Tipo;
        public string CPF;
        public string Nome;
        public DateTime RealizadoEm;
        public DateTime? PagoEm;
        public double Valor;
    }

    public struct _HistoricoConsulta {
        public int ID;
        public string CPF_Paciente;
        public string Nome_Paciente;
        public string CPF_Funcionario;
        public string Nome_Funcionario;
        public string Profissão;
        public string Especializacao;
        public bool Retorno;
        public double Valor;
        public DateTime RealizadoEm;
        public DateTime? PagoEm;
        public DateTime? RecebidoEm;
    }

    public struct _MedicoEspecializacoes {
        public string CPF;
        public string Nome;
        public string Especializacao;
    }

    public struct _Historico_ProcedimentosLab {
        public int ID;
        public string CPF;
        public string Nome;
        public int Procedimento_ID;
        public string ProcedimentoLab;
        public int Con_ID;
        public string Convenio;
        public double Pago;
        public DateTime? RealizadoEm;
        public DateTime? PagoEm;
    }

    public struct _ListaEspera {
        public int Senha;
        public DateTime Atualizacao;
        public string Fila;
        public string Status;
        public string CPF;
        public string Nome;
        public bool Prioridade;
        public string CPF_Funcionario;
        public string Nome_Funcionario;
        public DateTime Nascimento_Paciente;
        public string Especializacao;
    }

    public struct _Sala {
        public string CPF_Funcionario;
        public string Nome_Funcionario;
        public string Nome;
        public string Funcao;
        public string Status;
    }

    public struct _PacienteProcedimento {
        public int Senha;
        public string CFP_Paciente;
        public string Nome_Paciente;
        public int Proc_ID;
        public int Con_ID;
        public string Procedimento;
        public string Convenio;
        public double Valor;
        public string Status;
        public DateTime Atualização;
    }

}