namespace PasswordSafe
{
    internal class PasswordEntity
    {
        public string Username { get; }

        public string Password { get; }

        public string Url { get; }

        public string Notes { get; }

        public PasswordEntity(string username, string password, string url, string notes)
        {
            Username = username;
            Password = password;
            Url = url;
            Notes = notes;
        }

        public PasswordEntity(object username, object password, object url, object notes)
        {
            Username = username.ToString();
            Password = password.ToString();
            Url = url.ToString();
            Notes = notes.ToString();
        }

        public override string ToString()
        {
            return $"username: {Username}, password: {Password}, url: {Url}, notes: {Notes}";
        }
    }
}
