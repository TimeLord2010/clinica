using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPFClinica.Scripts.DB {

    public static class SQLOperations {

        static bool dummy;
        public static bool ThrowExceptions {
            get => dummy;
            set => dummy = value;
        }
        public static bool MessageExceptions = true;

        public static string Database {
            get => App.Current.Properties["database"] as string;
        }

        public static MySqlH MySql_Handler {
            get => App.Current.Properties["sql"] as MySqlH;
        }


        /// <summary>
        /// Gets the 'MySqlH' instante from 'App.Current.Properties["sql"]' and executes an action. If the code fails, a MessageBox is shown.
        /// </summary>
        /// <param name="title">The MessageBox title.</param>
        /// <param name="action">The action to execute the MySqlH instance.</param>
        private static void SQL(string title, Action<MySqlH> action, Action<Exception> onError = null, bool tryAgain = false) {
            var sql = App.Current.Properties["sql"] as MySqlH;
            int a = 0;
        A:
            try {
                //sql.ConnectionString.Open();
                action.Invoke(sql);
            } catch (Exception ex) {
                if (onError != null) onError.Invoke(ex);
                if (tryAgain && a == 0) {
                    a += 1;
                    goto A;
                }
                if (MessageExceptions) MessageBox.Show($"Classe: {ex.GetType()}\nMensagem: {ex.Message}", title, MessageBoxButton.OK, MessageBoxImage.Error);
                if (ThrowExceptions) throw ex;
            } finally {
                //if (sql != null && sql.Connection != null) sql.Connection.Close();
            }
        }

        //public static void Connection(string title, Action<MySqlConnection> action) {
        //    SQL(title, (sql) => {
        //        try {
        //            sql.Connection.Open();
        //            action.Invoke(sql.Connection);
        //        } catch (Exception ex) {
        //            throw ex;
        //        } finally {
        //            if (sql != null && sql.Connection != null) sql.Connection.Close();
        //        }
        //    });
        //}

        public static void NonQuery(string title, string nonQuery) {
            SQL(title, (sql) => {
                sql.NonQuery(nonQuery);
            });
        }

        public static void NonQuery(string title, MySqlCommand c, Action<Exception> onError = null, bool tryAgain = false) {
            SQL(title, (sql) => {
                sql.NonQuery(c);
            }, onError, tryAgain);
        }

        public static void NonQuery(string title, Func<MySqlCommand, MySqlCommand> action, Action<Exception> onError = null, bool tryAgain = false) {
            SQL(title, (sql) => {
                using var c = new MySqlCommand {
                    CommandTimeout = 10
                };
                var c2 = action(c);
                sql.NonQuery(c2);
            }, onError, tryAgain);
        }

        public static void QueryR(string title, MySqlCommand command, Action<MySqlDataReader> action, Action<Exception> onError = null, bool tryAgain = false) {
            SQL(title, (sql) => {
                sql.QueryR(command, action);
            }, onError, tryAgain);
        }

        public static void QueryRLoop(string title, string command, Action<MySqlDataReader> action, Action<Exception> onError = null, bool tryAgain = false) {
            QueryRLoop(title, new MySqlCommand(command), action, onError, tryAgain);
        }

        public static void QueryRLoop(string title, MySqlCommand c, Action<MySqlDataReader> action, Action<Exception> onError = null, bool tryAgain = false) {
            QueryR(title, c, (r) => {
                try {
                    while (r.Read()) {
                        action.Invoke(r);
                    }
                } catch (Exception ex) {
                    MessageBox.Show(ex.Message, $"Erro usar loop no leitor. ({ErrorCodes.DB0044})");
                }
            }, onError, tryAgain);
        }

        //public static void Command(string title, Action<MySqlCommand> action) {
        //    SQL(title, (sql) => {
        //        using var c = new MySqlCommand();
        //        c.Connection.Open();
        //        action(c);
        //    });
        //}

        //public static void DataAdapter(string title, string query, Action<MySqlDataAdapter> action) {
        //    Command(title, (co) => {
        //        var da = new MySqlDataAdapter();
        //        co.CommandText = query;
        //        da.SelectCommand = co;
        //        action.Invoke(da);
        //    });
        //}

    }
}
