using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Scripts.Entities {

    struct _Asos {

        public Pessoas Paciente;
        public _Empresa Empresa;
        public DateTime RealizadoEm;
        public bool Notificado;
        public DateTime NotificadoEm;

        public static _Asos DefaultValues {
            get => new _Asos() { 
                Paciente = null,
                Empresa = _Empresa.DefaultValues,
                Notificado = false,
                NotificadoEm = DateTime.MinValue,
                RealizadoEm = DateTime.MinValue
            };
        }

    }
}
