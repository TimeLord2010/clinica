using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClinica.Scripts.Entities {

    interface IFormaPagamento {

        int ID { get; set; }
        string Descrição { get; set; }

    }

}
