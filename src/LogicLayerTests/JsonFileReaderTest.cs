using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using LogicLayer;
using Moq;
using NUnit.Framework;

namespace LogicLayerTests
{
    [TestFixture]
    internal class JsonFileReaderTest
    {
        private JsonFileReader CreateSut(MemoryStream stream)
        {
            var fileHelper = new Mock<IFileHelper>();

            fileHelper.Setup(f => f.BuildStreamReader(It.IsNotNull<string>())).ReturnsAsync(new StreamReader(stream));

            return new JsonFileReader(fileHelper.Object);
        }

        [Test]
        public async Task ParseFileToJsonData_WhenJsonPrivded_ItShouldReturnJsonData()
        {
            var json = @"{
            'Data': {
                '2018-09-30 22:00:00Z': 44.0,
                '2018-09-30 22:00:42Z': 44.0,
                '2018-09-30 22:02:42Z': 44.0,
                '2018-09-30 22:04:43Z': 44.0,
                '2018-09-30 22:06:43Z': 44.0,
                '2018-09-30 22:08:43Z': 44.0,
                '2018-09-30 22:10:43Z': 44.0,
                '2018-09-30 22:12:43Z': 44.0,
                '2018-09-30 22:14:44Z': 44.0,
                '2018-10-31 22:57:18Z': 68.0,
                '2018-10-31 22:57:28Z': 70.0,
                '2018-10-31 22:58:18Z': 70.0
            }
        }";
            var byteArray = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(byteArray);

            var sut = CreateSut(stream);

            var result = await sut.ParseFileToJsonData("filepath", 3).ConfigureAwait(false);

            result.Length.Should().Be(12);
            result.Last().Value.Should().Be(70);
            result.First().Value.Should().Be(44);
        }

        [Test]
        public void ParseFileToJsonData_WhenWrongIntProvided_ItShouldThrowArgumentEx()
        {
            var sut = CreateSut(new MemoryStream());

            Assert.ThrowsAsync<ArgumentException>(async () => await sut.ParseFileToJsonData("filepath", -3));
        }
    }
}