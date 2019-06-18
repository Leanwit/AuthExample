namespace Web.Test.Utils
{
    using Web.Utils;
    using Xunit;

    public class Md5HelperTest
    {
        [Theory]
        [InlineData("aPassword")]
        [InlineData("1231dds-/-:@*")]
        [InlineData("d8YP5M,A*atkuH")]
        public void EncryptString_And_DecryptStrin_Get_Same_Value(string password)
        {
            var valueEncrypted = Md5Helper.EncryptString(password);
            var valueDecrypted = Md5Helper.DecryptString(valueEncrypted);

            Assert.Equal(password, valueDecrypted);
            Assert.IsType<string>(valueDecrypted);
        }

        [Fact]
        public void DecryptString_Assert_Return()
        {
            var valueToRecover = "4OiRA8twXHccO8hZ2HSMPqTjhPKvxQgWYEq/5491cHM=";

            var value = Md5Helper.DecryptString(valueToRecover);

            Assert.IsType<string>(value);
            Assert.NotEqual(value, valueToRecover);
            Assert.NotEmpty(value);
        }

        [Fact]
        public void EncryptString_Assert_Return()
        {
            var valueToHash = "aPassword";

            var hash = Md5Helper.EncryptString(valueToHash);

            Assert.IsType<string>(hash);
            Assert.NotEqual(valueToHash, hash);
            Assert.NotEmpty(hash);
        }
    }
}