using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.AbstractClasses;

namespace WPFClinica.Scripts.DB {

    class Dentistas : Profissoes {

        public override string TableName => nameof(Dentistas);
    }

}
