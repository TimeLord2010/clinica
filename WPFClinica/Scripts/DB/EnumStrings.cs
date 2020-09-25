using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFClinica.Scripts.DB {

    public static class EnumStrings {

        public static string Get(PStatus status) {
            if (status == PStatus.Esperando) {
                return "Esperando";
            } else if (status == PStatus.EmAtendimento) {
                return "Em atendimento";
            } else if (status == PStatus.ACaminho) {
                return "A caminho";
            } else if (status == PStatus.Atendido) {
                return "Atendido";
            } else {
                throw new NotImplementedException();
            }
        }

        public static string Get(SStatus status) {
            if (status == SStatus.Aguardando) {
                return "Aguardando";
            } else if (status == SStatus.EmAtendimento) {
                return "Em atendimento";
            } else if (status == SStatus.Livre) {
                return "Livre";
            } else if (status == SStatus.Ocupado) {
                return "Ocupado";
            } else {
                throw new NotImplementedException();
            }
        }

        public static string Get(ProcedimentoStatus status) {
            switch (status) {
                case ProcedimentoStatus.EmAndamento:
                    return "Em andamento";
                case ProcedimentoStatus.Pendente:
                    return "Pendente";
                case ProcedimentoStatus.Pronto:
                    return "Pronto";
                default:
                    throw new NotImplementedException("String de 'status de procedimento' não foi implementada.");
            }
        }

        public static string Get(Funcao funcao) {
            switch (funcao) {
                case Funcao.Administrador:
                    return "Administrador";
                case Funcao.Medico:
                    return "Medico";
                case Funcao.Recepcionista:
                    return "Recepcionista";
                case Funcao.Tecnico:
                    return "Tecnico";
                default:
                    throw new NotImplementedException("'Funcao' não implementada.");
            }
        }

        public static string Get (Profissao profissao) {
            return profissao.ToString();
        }

        public static Profissao SetProfissao (string profissao) {
            var values = Enum.GetValues(typeof(Profissao)).Cast<Profissao>();
            foreach (var item in values) {
                if (item.ToString() == profissao) return item;
            }
            throw new KeyNotFoundException();

        }

        public static PStatus SetPStatus(string status) {
            if (status == "Esperando") {
                return PStatus.Esperando;
            } else if (status.Replace("A", "a") == "Em atendimento") {
                return PStatus.EmAtendimento;
            } else if (status.Replace("C", "c") == "A caminho") {
                return PStatus.ACaminho;
            } else if (status == "Atendido") {
                return PStatus.Atendido;
            } else {
                throw new FormatException();
            }
        }

        public static ProcedimentoStatus SetProcedimento(string status) {
            switch (status) {
                case "Pendente":
                    return ProcedimentoStatus.Pendente;
                case "Em andamento":
                    return ProcedimentoStatus.EmAndamento;
                case "Pronto":
                    return ProcedimentoStatus.Pronto;
                default:
                    throw new NotImplementedException("A opção de 'procedimento-status' foi mapeada em string.");
            }
        }

        public static SStatus SetSStatus (string status) {
            switch (status) {
                case "Aguardando":
                    return SStatus.Aguardando;
                case "Em atendimento":
                    return SStatus.EmAtendimento;
                case "Livre":
                    return SStatus.Livre;
                case "Ocupado":
                    return SStatus.Ocupado;
                default:
                    throw new NotImplementedException("Erro ao obter enum de SStatus pela string de entrada.");

            }
        }

    }

}
