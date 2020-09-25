using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WPFClinica.Scripts {
    class ES {

        public static Dictionary<Fila, string> f = new Dictionary<Fila, string>() {
            { Fila.Consultorio, "Consultório"},
            { Fila.Laboratorio, "Laboratório"},
            { Fila.Triagem, "Triagem"},
            { Fila.ExamesMedicos, "Exames Médicos"},
            { Fila.Nulo, "[Nulo]"}
        };

        public static Dictionary<PStatus, string> ps = new Dictionary<PStatus, string>() {
            { PStatus.ACaminho, "A caminho"},
            { PStatus.Atendido, "Atendido" },
            { PStatus.EmAtendimento, "Em atendimento"},
            { PStatus.Esperando, "Esperando"}
        };

        public static Dictionary<Profissao, string> prf = new Dictionary<Profissao, string>() {
            { Profissao.Administrador, "Administrador" },
            {Profissao.Fonoaudiologo, "Fonoaudiólogo" },
            {Profissao.Medico, "Médico" },
            {Profissao.Nutricionista, "Nutricionista" },
            {Profissao.Psicologo, "Psicólogo" },
            {Profissao.Recepcionista, "Recepcionista" },
            {Profissao.TecnicoEnfermagem, "Técnico de Enfermagem" }
        };

        public static Dictionary<SStatus, string> ss = new Dictionary<SStatus, string>() {
            {SStatus.Aguardando, "Aguardando" },
            {SStatus.EmAtendimento, "Em atendimento" },
            {SStatus.Livre, "Livre" },
            {SStatus.Ocupado, "Ocupado" }
        };

        public static Dictionary<ProcedimentoStatus, string> prs = new Dictionary<ProcedimentoStatus, string>() {
            { ProcedimentoStatus.EmAndamento, "Em andamento"},
            {ProcedimentoStatus.Pendente, "Pendente" },
            {ProcedimentoStatus.Pronto, "Pronto" }
        };

        public static Dictionary<Exame, string> exa = new Dictionary<Exame, string>() {
            {Exame.Laboratorial, "Laboratorial" },
            {Exame.Ultrassom, "Ultrassom" },
            {Exame.RaioX, "Raio X" },
            {Exame.Tomografia, "Tomografia" },
            {Exame.Outros, "Outros" }
        };

        public static Fila GetFila(string fila) {
            try {
                return f.First(x => x.Value == fila).Key;
            } catch (Exception) {
                return Fila.Nulo;
            }
        }

        public static ProcedimentoStatus GetProcedimentoStatus(ComboBox input) {
            return prs.First(x => x.Value == (input.SelectedItem as ComboBoxItem).Content.ToString()).Key;
        }

        public static ProcedimentoStatus GetProcedimentoStatus(string input) {
            return prs.First(x => x.Value == input).Key;
        }

        public static Profissao GetProfissão(string input) {
            return prf.First(x => x.Value == input).Key;
        }

        public static PStatus GetPStatus(string input) {
            return ps.First(x => x.Value == input).Key;
        }

        public static Exame GetExame(ComboBox comboBox) {
            return GetExame((comboBox.SelectedItem as ComboBoxItem).Content.ToString());
        }

        public static Exame GetExame(string input) {
            return exa.First(x => x.Value == input).Key;
        }

    }
}
