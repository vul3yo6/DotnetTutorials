using NUnit.Framework;
using System.Text.Json.Nodes;

namespace DotnetLibrariesTests
{
    // ref: https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/use-dom#create-a-jsonnode-dom-with-object-initializers-and-make-changes
    public class JsonNodeTests
    {
        [Test]
        public void Test1()
        {
            // Arrange
            var expected = new JsonArray("\"kentseng@mirle.com.tw\"", "\"misnhsieh@mirle.com.tw\"", "\"wilsonhsc@mirle.com.tw\"");

            // Act
            var text = "[\"kentseng@mirle.com.tw\",\"misnhsieh@mirle.com.tw\",\"wilsonhsc@mirle.com.tw\" ]";
            var jsonNode = JsonNode.Parse(text);
            var actual = jsonNode == null ? new JsonArray() : jsonNode.AsArray();

            //var temp = JsonNode.Parse("[]");
            //temp = JsonNode.Parse("\"AAA\"");
            //temp = JsonNode.Parse("[\"AAA\"]");
            //temp = JsonNode.Parse("[\"AAA\",]");
            //temp = JsonNode.Parse("[\"AAA\"");

            // Assert
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}