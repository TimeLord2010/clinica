using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows;
using WPFClinica.Scripts;

public class MySqlH {

    readonly string CS;
    public bool ExitOnError = false;

    public MySqlConnection ConnectionString {
        get => new MySqlConnection(CS);
    }

    public string Database {
        get => ConnectionString.Database;
    }

    public MySqlH(string cs) {
        CS = cs;
    }

    /// <summary>
    /// Avoid using this method. The prefered method is NonQuery(string, params (string, object)[])
    /// </summary>
    /// <param name="command"></param>
    public void NonQuery(MySqlCommand command) {
        using (command) {
            Connection((con) => {
                command.Connection = con;
                command.ExecuteNonQuery();
            });
        }
    }

    public void NonQuery(string query, params (string, object)[] parameters) {
        Command(query, (command) => {
            command.ExecuteNonQuery();
        }, parameters);
    }

    public void QueryR(MySqlCommand command, Action<MySqlDataReader> action) {
        Connection((con) => {
            using (command) {
                command.Connection = con;
                action.Invoke(command.ExecuteReader());
            }
        });
    }

    public void QueryRLoop(MySqlCommand command, Action<MySqlDataReader> action) {
        QueryR(command, (r) => {
            while (r.Read()) action(r);
        });
    }

    public void Reader (string query, Action<MySqlDataReader> action, params (string, object)[] parameters) {
        Command(query, (command) => {
            using var reader = command.ExecuteReader();
            action(reader);
        }, parameters);
    }

    public void ReaderLoop (string query, Action<MySqlDataReader> action, params (string, object)[] parameters) {
        Reader(query, (reader) => { 
            while (reader.Read()) {
                action(reader);
            }
        }, parameters);
    }

    public void Connection(Action<MySqlConnection> action) {
        using var connection = new MySqlConnection(CS);
        connection.Open();
        action(connection);
    }

    public void Command(string b, Action<MySqlCommand> a, params (string, object)[] parameters) {
        Connection((con) => {
            using var com = con.CreateCommand();
            com.CommandText = b;
            foreach (var (name, value) in parameters) {
                com.Parameters.AddWithValue(name, value);
            }
            a(com);
        });
    }

    public List<(string name, bool accept_null, string data_type, DatabaseKeyType key_type)> GetColumns (string table) {
        var list = new List<(string, bool, string, DatabaseKeyType)>();
        ReaderLoop(
            @"select 
                COLUMN_NAME, 
                IS_NULLABLE, 
                DATA_TYPE, 
                COLUMN_KEY 
            FROM INFORMATION_SCHEMA.COLUMNS " +
            $"WHERE TABLE_SCHEMA = '{Database}' AND TABLE_NAME = '{table}';", 
        (r) => {
            list.Add((r.GetString(0), r.GetString(1) == "YES", r.GetString(2), r.GetString(3) == "PRI" ? DatabaseKeyType.Primary : DatabaseKeyType.None));
        });
        return list;
    }

}