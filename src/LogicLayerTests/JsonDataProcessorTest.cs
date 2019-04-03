using System.Threading.Tasks;
using FluentAssertions;
using LogicLayer;
using LogicLayer.Model;
using NUnit.Framework;

namespace LogicLayerTests
{
    [TestFixture]
    internal class JsonDataProcessorTest
    {
        private JsonDataProcessor CreateSut()
        {
            return new JsonDataProcessor();
        }

        [Test]
        public async Task Process_WhenDataProvided_ThenItShouldProcess()
        {
            var data = new[]
            {
                new JsonData
                {
                    Value = 10
                },
                new JsonData
                {
                    Value = 20
                },
                new JsonData
                {
                    Value = 30
                },
                new JsonData
                {
                    Value = 40
                },
                new JsonData
                {
                    Value = 50
                }
            };

            var sut = CreateSut();

            var result = await sut.Process(data).ConfigureAwait(false);

            result.Maximum.Should().Be(50);
            result.Minimum.Should().Be(10);
            result.Variance.Should().Be(200);
            Assert.AreEqual(14.14, result.StandardDeviation, 0.01);
            Assert.AreEqual(33.16, result.RootMeanSquare, 0.01);
            result.SumOfIncreases.Should().Be(40);
            result.SumOfDecreases.Should().Be(0);
        }
    }
}