namespace WebApi.Domain
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password;

        public void SetPassword(string password)
        {
            this.Password = password;
        }

        public bool IsCorrectPassword(string password)
        {
            return this.Password.Equals(password);
        }
    }
}