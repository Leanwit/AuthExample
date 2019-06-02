namespace WebApi.Domain
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set;}
        

        public User(long id, string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
      
        public bool IsPassword(string password)
        {
            return this.Password.Equals(password);
        }
    }
}