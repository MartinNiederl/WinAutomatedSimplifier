using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace PasswordSafe
{
    internal class DataBase : IDisposable
    {
        private static DataBase instance;
        private SQLiteConnection Connection { get; }
        private SQLiteCommand Command { get; }

        private DataBase(string dataSource)
        {
            Connection = new SQLiteConnection { ConnectionString = "Data Source=" + dataSource };
            Connection.Open();

            Command = new SQLiteCommand(Connection);
        }

        public static DataBase GetInstance(string dataSource)
        {
            return instance ?? (instance = new DataBase(dataSource));
        }

        public string GetValidationString()
        {
            Command.CommandText = "SELECT testtext FROM validation";
            return (string)Command.ExecuteReader()[0];
        }

        public void Initialize(string validation)
        {
            ExecuteScript(
                "CREATE TABLE IF NOT EXISTS passwords ( id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, username VARCHAR(100) NOT NULL, password VARCHAR(100) NOT NULL, url VARCHAR(100), notes VARCHAR(300));" +
                "CREATE TABLE IF NOT EXISTS validation ( testtext VARCHAR(100) NOT NULL);" +
                $"INSERT INTO validation (testtext) VALUES('{validation}');");
        }

        public List<PasswordEntity> ReadEntity()
        {
            var list = new List<PasswordEntity>();
            Command.CommandText = "SELECT id, username, password, url, notes FROM passwords";

            using (SQLiteDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                    list.Add(new PasswordEntity((string)reader[1], (string)reader[2], (string)reader[3], (string)reader[4], (int)reader[0]));

                reader.Close();
            }

            return list;
        }

        /// <summary>
        /// Insert new PasswordEntry into the database.
        /// </summary>
        /// <returns>The autoincremented ID.</returns>
        public int InsertEntry(string username, string password, string url, string notes)
        {
            Command.CommandText = $"INSERT INTO passwords (username, password, url, notes) VALUES ('{username}', '{password}', '{url}', '{notes}') ";
            Command.ExecuteNonQuery();
            Command.CommandText = "SELECT last_insert_rowid()";
            return (int)Command.ExecuteReader()[0];
        }
        
        public void UpdateEntry(string username, string password, string url, string notes, int id)
        {
            string updateStatement = "UPDATE passwords SET";

            updateStatement += UpdateAppend(username, "username", 3) + UpdateAppend(password, "password", 5) +
                UpdateAppend(url, "url") + UpdateAppend(notes, "notes");

            updateStatement = updateStatement.TrimEnd(',') + $" WHERE id = '{id}'";

            Command.CommandText = updateStatement;
            Command.ExecuteNonQuery();
        }

        public void DeleteEntry(int id)
        {
            Command.CommandText = $"DELETE FROM passwords WHERE id = {id}";
            Command.ExecuteNonQuery();
        }

        public void ClearDB() => ExecuteScript("DROP TABLE passwords;" + "DROP TABLE validation;");

        public void CloseConnection()
        {
            Connection.Close();
            Connection.Dispose();
            Command.Dispose();
        }

        private void ExecuteScript(string script = "")
        {
            foreach (string command in script.Trim(';').Split(';'))
            {
                Command.CommandText = command;
                Command.ExecuteNonQuery();
            }
        }

        private static string UpdateAppend(string var, string colname, int gt = 0)
        {
            if (var != null && var.Length >= gt)
                return $" {colname} = '{var}'";
            return "";
        }

        public void Dispose() => CloseConnection();
    }
}
