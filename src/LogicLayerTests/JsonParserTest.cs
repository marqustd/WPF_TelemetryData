using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using LogicLayer;
using LogicLayer.Model;
using Moq;
using NUnit.Framework;

namespace LogicLayerTests
{
    [TestFixture]
    internal class JsonParserTest
    {
        private JsonParser CreateSut(JsonData[] dataToReturn)
        {
            var dataProcesor = new Mock<IJsonDataProcessor>();
            var fileReader = new Mock<IJsonFileReader>();

            fileReader.Setup(f => f.ParseFileToJsonData(It.IsNotNull<string>(), It.IsNotNull<int>()))
                .ReturnsAsync(dataToReturn);
            dataProcesor.Setup(d => d.Process(It.IsNotNull<JsonData[]>())).ReturnsAsync(new ProcessedData());

            return new JsonParser(dataProcesor.Object, fileReader.Object);
        }

        [Test]
        public async Task ParseFile_WhenFilepathProvided_ItShouldReturnParsedData()
        {
            var jsonData = new[]
            {
                new JsonData
                {
                    Date = new DateTime(2018, 11, 21, minute: 0, second: 0, hour: 21)
                },
                new JsonData
                {
                    Date = new DateTime(2018, 11, 21, minute: 0, second: 0, hour: 21)
                },
                new JsonData
                {
                    Date = new DateTime(2018, 11, 21, minute: 0, second: 0, hour: 22)
                },
                new JsonData
                {
                    Date = new DateTime(2018, 11, 21, minute: 0, second: 0, hour: 22)
                },
                new JsonData
                {
                    Date = new DateTime(2018, 11, 21, minute: 0, second: 0, hour: 23)
                },
                new JsonData
                {
                    Date = new DateTime(2018, 11, 22, minute: 0, second: 0, hour: 23)
                },
                new JsonData
                {
                    Date = new DateTime(2018, 11, 23, minute: 0, second: 0, hour: 23)
                },
                new JsonData
                {
                    Date = new DateTime(2018, 11, 24, minute: 0, second: 0, hour: 23)
                }
            };
            var sut = CreateSut(jsonData);

            var result = await sut.ParseFile("some filepath", 3).ConfigureAwait(false);
            result.Count().Should().Be(6);
        }

        [Test]
        public void ParseFile_WhenWrongIntProvided_ItShouldThrowArgumentEx()
        {
            var sut = CreateSut(new JsonData[1]);

            Assert.ThrowsAsync<ArgumentException>(async () => await sut.ParseFile("filepath", -3));
        }
    }
}