namespace WebApi.Domain.DTO
{
    using System.Collections.Generic;

    public class UserDto
    {
        public string Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public List<string> Roles { get; set; }
    }
}