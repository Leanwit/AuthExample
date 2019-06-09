namespace WebApi.Domain.DTO
{
    using System.Collections.Generic;

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
                Username = user.Username,
                Password = user.Password
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