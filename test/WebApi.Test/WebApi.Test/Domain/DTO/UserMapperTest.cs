namespace WebApi.Test.Domain.DTO
{
    using WebApi.Domain;
    using WebApi.Domain.DTO;
    using Xunit;

    public class UserMapperTest
    {
        [Theory]
        [InlineData(10, "user", "pass")]
        [InlineData(15, "", "pass")]
        [InlineData(20, "user", "")]
        private void UserDto_MapFromDto(long id, string username, string password)
        {
            User user = new User(id, username, password);
            var dto = user.MapToDto();

            Assert.IsType<UserDto>(dto);
            Assert.True(dto.Id == id);
            Assert.Equal(dto.Username, username);
            Assert.Equal(dto.Password, password);
        }
    }
}