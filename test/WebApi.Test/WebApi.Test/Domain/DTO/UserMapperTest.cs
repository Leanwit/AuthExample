namespace WebApi.Test.Domain.DTO
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Database;
    using WebApi.Domain;
    using WebApi.Domain.DTO;
    using Xunit;

    public class UserMapperTest
    {
        [Theory]
        [ClassData(typeof(InlineData))]
        private void UserDto_MapFromDto(string id, string username, string password)
        {
            var user = new User(id, username, password);
            var dto = user.MapToDto();

            Assert.IsType<UserDto>(dto);
            Assert.True(dto.Id == id);
            Assert.Equal(dto.Username, username);
            Assert.Equal(dto.Password, password);
        }

        private class InlineData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] {Guid.NewGuid().ToString(), UserSeed.Username, UserSeed.Password};
                yield return new object[] {Guid.NewGuid().ToString(), "", UserSeed.Password};
                yield return new object[] {Guid.NewGuid().ToString(), UserSeed.Username, ""};
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}