namespace WebApi.Domain.DTO
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }

        private string Password;

        public string PasswordDefined
        {
            set { this.Password = value; }
        }
    }
}