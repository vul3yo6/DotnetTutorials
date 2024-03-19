using DotnetLibraries;
using DotnetLibrariesTests.Models;
using NUnit.Framework;

namespace DotnetLibrariesTests
{
    public class CryptographyHelperTests
    {
        [Test]
        public void Test_RSA_Case1()
        {
            // Arrange
            var rsa = CryptographyHelper.GenerateRSAKeys();
            var expected = "Keep Going~!@#$%^&*()_-+=";

            // Act
            var encrypt = CryptographyHelper.EncryptRSA(rsa.Item1, expected);
            var decrypt = CryptographyHelper.DecryptRSA(rsa.Item2, encrypt);
            var actual = decrypt;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_RSA_Case2()
        {
            // Arrange
            var rsa = CryptographyHelper.GenerateRSAKeys();
            var expected = "Keep Going~!@#$%^&*()_-+=";

            // Act
            var encrypt = CryptographyHelper.EncryptRSA(rsa.Item2, expected);
            var decrypt = CryptographyHelper.DecryptRSA(rsa.Item2, encrypt);
            var actual = decrypt;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_RSA_Case3()
        {
            // Arrange
            var rsa = CryptographyHelper.GenerateRSAKeys(2048);
            var expected = new string('*', 245);    // max length is 245

            // Act
            var encrypt = CryptographyHelper.EncryptRSA(rsa.Item1, expected);
            var decrypt = CryptographyHelper.DecryptRSA(rsa.Item2, encrypt);
            var actual = decrypt;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_RSA_Case4()
        {
            // Arrange
            var rsa = CryptographyHelper.GenerateRSAKeys(2048);

            //string guid = System.Guid.NewGuid().ToString();
            // {"SysId":"EDM-A_63ec1709-946a-4fee-8e1b-db5546f6585e","SysType":"EDM1234567890","CompanyName":"MIRLE1234567890", "ExpiryDate":"2025-01-01T00:00:00Z","Hardware":"8BD65274EBCAE225E51C00DD740859D8","modifyDate":"2024-02-21T10:23:49Z","rnd":"12345"}
            var expected = "{\"SysId\":\"EDM-A_63ec1709-946a-4fee-8e1b-db5546f6585e\",\"SysType\":\"EDM1234567890\",\"CompanyName\":\"MIRLE1234567890\", \"ExpiryDate\":\"2025-01-01T00:00:00Z\",\"Hardware\":\"8BD65274EBCAE225E51C00DD740859D8\",\"modifyDate\":\"2024-02-21T10:23:49Z\",\"rnd\":\"12345\"}";

            // Act
            var encrypt = CryptographyHelper.EncryptRSA(rsa.Item1, expected);
            var decrypt = CryptographyHelper.DecryptRSA(rsa.Item2, encrypt);
            var actual = decrypt;

            // Json Deserialize
            var obj = GlobalFunctions.JsonDeserialize<License>(actual);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_AES_MSDN_Case1()
        {
            // Arrange
            var aes = CryptographyHelper.GenerateAES();
            var expected = "Keep Going~!@#$%^&*()_-+=";

            // Act
            var encrypt = CryptographyHelper.EncryptAES(expected, aes.Item1, aes.Item2);
            var decrypt = CryptographyHelper.DecryptAES(encrypt, aes.Item1, aes.Item2);
            var actual = decrypt;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_AES_MSDN_Case2()
        {
            // Arrange
            var key = "12345678901234567890123456789012";
            var iv = "1234567890abcdef";
            var expected = "Keep Going~!@#$%^&*()_-+=";

            // Act
            var encrypt = CryptographyHelper.EncryptAES256(expected, key, iv);
            var decrypt = CryptographyHelper.DecryptAES256(encrypt, key, iv);
            var actual = decrypt;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_MD5_Case1()
        {
            // Arrange
            var text = "Keep Going~!@#$%^&*()_-+="; 
            var expected = CryptographyHelper.EncryptMD5(text);

            // Act
            var actual = CryptographyHelper.EncryptMD5(text);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_SHA_Case1()
        {
            // Arrange
            var text = "Keep Going~!@#$%^&*()_-+=";
            var expected = CryptographyHelper.EncryptSHA256(text);

            // Act
            var actual = CryptographyHelper.EncryptSHA256(text);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_SHA_Case2()
        {
            // Arrange
            var text = "Keep Going~!@#$%^&*()_-+=";
            var expected = CryptographyHelper.EncryptSHA512(text);

            // Act
            var actual = CryptographyHelper.EncryptSHA512(text);

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}