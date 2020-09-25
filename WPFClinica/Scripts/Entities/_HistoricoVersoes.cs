using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClinica.Scripts.Entities {

    public struct _HistoricoVersoes {

        public int ID;
        public string Versao;
        public DateTime DateTime;

        public static _HistoricoVersoes GetDefaultValues {
            get => new _HistoricoVersoes() { 
                ID = -1,
                Versao = "0.0.0",
                DateTime = DateTime.MinValue
            };
        }

        public static _HistoricoVersoes GetValues (MySqlDataReader r) {
            return new _HistoricoVersoes() { 
                ID = r.GetInt32(0),
                Versao = r.GetString(1),
                DateTime = r.GetMySqlDateTime(2).GetDateTime()
            };
        }

    }
}
