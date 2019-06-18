namespace WebApi.Domain.DTO
{
    using System.Collections.Generic;

    public class UserFindDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public List<string> Roles { get; set; }
    }
}