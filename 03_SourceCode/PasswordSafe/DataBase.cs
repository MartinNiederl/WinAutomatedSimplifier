using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace PasswordSafe
{
    internal class DataBase : IDisposable
    {
        private SQLiteConnection Connection { get; }
        private SQLiteCommand Command { get; }

        public DataBase(string dataSource)
        {
            Connection = new SQLiteConnection();
            Connection.ConnectionString = "Data Source=" + dataSource;

            Connection.Open();

            Command = new SQLiteCommand(Connection);
        }

        private void VerifyPassword()
        {
            Command.CommandText = "SELECT notes FROM passwords LIMIT 1";
            using (SQLiteDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    
                }
                reader.Close();
            }
        }

        public void CreateTable()
        {
            Command.CommandText = "CREATE TABLE IF NOT EXISTS passwords ( id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, username VARCHAR(40) NOT NULL, password VARCHAR(40) NOT NULL, url VARCHAR(40), notes VARCHAR(200));";
            Command.ExecuteNonQuery();

            // Einfügen eines PasswortTest-Datensatzes.
            Command.CommandText = "INSERT INTO passwords (id, username, password) VALUES(NULL, ' ', 'PasswortTest-Datensatz')";
            Command.ExecuteNonQuery();

        }

        public void Read()
        {
            Command.CommandText = "SELECT id, name FROM passwords ORDER BY id DESC LIMIT 0, 1";

            using (SQLiteDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                    Console.WriteLine("Dies ist der {0}. eingefügte Datensatz mit dem Wert: \"{1}\"",
                        reader[0], reader[1]);

                reader.Close();
            }
        }

        public List<PasswordEntity> ReadEntity()
        {
            var list = new List<PasswordEntity>();
            Command.CommandText = "SELECT username, password, url, notes FROM table LIMIT -1 OFFSET 1";

            using (SQLiteDataReader reader = Command.ExecuteReader())
            {
                while (reader.Read())
                    list.Add(new PasswordEntity(reader[0], reader[1], reader[2], reader[3]));

                reader.Close();
            }

            return list;
        }

        public void ClearDB()
        {
            Command.CommandText = "DROP TABLE passwords";
            Command.ExecuteNonQuery();
        }

        public void CloseConnection()
        {
            Connection.Close();
            Connection.Dispose();
            Command.Dispose();
        }

        public void Dispose() => CloseConnection();
    }
}
