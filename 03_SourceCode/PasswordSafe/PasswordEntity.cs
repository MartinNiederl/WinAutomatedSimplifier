using System;

namespace PasswordSafe
{
    public class PasswordEntity
    {
        public int ID { get; private set; }
        public string Username { get; private set; }

        public string Password { get; private set; }

        public string Url { get; private set; }

        public string Notes { get; private set; }

        public PasswordEntity(string username, string password, string url, string notes, int id = 0)
        {
            ID = id;
            Username = username;
            Password = password;
            Url = url;
            Notes = notes;
        }

        public PasswordEntity Update(PasswordEntity @new)
        {
            Username = @new.Username;
            Password = @new.Password;
            Url = @new.Url;
            Notes = @new.Notes;
            return this;
        }

        public override string ToString()
        {
            return $"ID: {ID}, username: {Username}, password: {Password}, url: {Url}, notes: {Notes}";
        }
    }
}
