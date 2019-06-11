namespace WebApi.Domain
{
    using System.Collections.Generic;

    public class User
    {
        public User(long id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }

        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Role> Roles { get; set; }

        public bool IsPassword(string password)
        {
            return Password.Equals(password);
        }
    }
}