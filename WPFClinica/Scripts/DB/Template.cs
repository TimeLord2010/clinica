using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFClinica.Scripts.DB {

    public abstract class Template {

        public abstract string TableName { get; }

        public abstract Action GetCT();

        public void CreateTable() {
            GetCT().Invoke();
        }

        private void Create (Exception ex) {
            if (ex.Message.Contains("doesn't exist")) {
                GetCT().Invoke();
                CreateTable();
            }
        }

        protected void QueryRLoop (string title, MySqlCommand c, Action<MySqlDataReader> action) {
            SQLOperations.QueryRLoop(title, c, action, Create, true);
        }

        protected void NonQuery (string title, MySqlCommand c) {
            SQLOperations.NonQuery(title, c, Create, true);
        }

        public void NonQuery (string title, string q) {
            var c = new MySqlCommand(q);
            SQLOperations.NonQuery(title, c, Create, true);
        }

    }
}
