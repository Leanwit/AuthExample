namespace WebApi.Domain
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class User
    {
        public User(string id, string username, string password)
        {
            if (!IsGuid(id)) throw new InvalidDataException($"{id} is not a valid GUID");

            Id = id;
            Username = username;
            Password = password;
            Roles = new List<Role>();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public List<Role> Roles { get; set; }

        public bool IsPassword(string password)
        {
            return Password.Equals(password);
        }

        private bool IsGuid(string value)
        {
            return Guid.TryParse(value, out var x);
        }
    }
}