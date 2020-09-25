using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFClinica.Scripts.DB;

namespace WPFClinica.Scripts.Entities {

    struct _Empresa {

        public int ID;
        public string Nome, CNPJ, Observação;
        public DateTime Registro;

        public _Empresa(int id) {
            var e = Empresa.Select(id);
            ID = id;
            Nome = e.Nome;
            CNPJ = e.CNPJ;
            Observação = e.Observação;
            Registro = e.Registro;
        }


        public static _Empresa DefaultValues {
            get => new _Empresa() {
                ID = -1,
                Nome = "",
                CNPJ = "",
                Observação = "",
                Registro = DateTime.MinValue
            };
        }

        public static _Empresa Get (MySqlDataReader r) {
            return new _Empresa() { 
                ID = r.GetInt32(0),
                Nome = r.GetString(1),
                CNPJ = r.GetString(2),
                Registro = r.GetMySqlDateTime(3).GetDateTime(),
                Observação = r.GetString(4)
            };
        }

    }

}
