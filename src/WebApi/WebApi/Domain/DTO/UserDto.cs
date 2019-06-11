namespace WebApi.Domain.DTO
{
    using System.Collections.Generic;

    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public List<Role> Roles { get; set; }
    }
}