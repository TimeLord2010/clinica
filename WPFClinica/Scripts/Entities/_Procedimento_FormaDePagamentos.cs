using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Scripts.Entities {

    class _Procedimento_FormaDePagamentos {

        public Procedimentos Procedimento { get; set; }
        public FormaDePagamento Pagamento { get; set; }
        public double Valor { get; set; }

    }
}
