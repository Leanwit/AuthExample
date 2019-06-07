namespace WebApi.Domain.DTO
{
    using System.Collections.Generic;
    using System.Linq;

    public static class UserMapper
    {
        public static UserDto MapToDto(this User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username
            };
        }

        public static IEnumerable<UserDto> MapToDto(this IEnumerable<User> users)
        {
            var usersDto = new List<UserDto>();
            foreach (var user in users)
            {
                usersDto.Add(MapToDto(user));
            }

            return usersDto;
        }
    }
}