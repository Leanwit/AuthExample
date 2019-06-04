namespace WebApi.Domain
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set;}
        

        public UserDto(long id, string username, string password)
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