namespace WebApi.Domain.DTO
{
    using System.Collections.Generic;

    public static class UserMapper
    {
        public static UserDto MapToDto(this Domain.UserDto userDto)
        {
            return new UserDto()
            {
                Id = userDto.Id,
                Username = userDto.Username
            };
        }

        public static IEnumerable<UserDto> MapToDto(this IEnumerable<Domain.UserDto> users)
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