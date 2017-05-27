using System;

namespace PasswordSafe
{
    internal class PasswordEntity
    {
        public int ID { get; }
        public string Username { get; }

        public string Password { get; }

        public string Url { get; }

        public string Notes { get; }

        public PasswordEntity(string username, string password, string url, string notes, int id = 0)
        {
            ID = id;
            Username = username;
            Password = password;
            Url = url;
            Notes = notes;
        }

        public override string ToString()
        {
            return $"ID: {ID}, username: {Username}, password: {Password}, url: {Url}, notes: {Notes}";
        }
    }
}
