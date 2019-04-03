using System;
using System.Linq;
using System.Threading.Tasks;
using LogicLayer.Model;

namespace LogicLayer
{
    /// <summary>
    ///     Logic for processing JsonData
    /// </summary>
    public interface IJsonDataProcessor
    {
        Task<ProcessedData> Process(JsonData[] data);
    }

    internal class JsonDataProcessor : IJsonDataProcessor
    {
        public async Task<ProcessedData> Process(JsonData[] data)
        {
            Require.NotEmpty(data, nameof(data));

            var values = data.Select(d => d.Value).ToArray();
            var average = values.Average();
            var variance = 0.0;
            var rootMeanSquare = 0.0;
            var amount = values.Count();

            var maximum = values[0];
            var minimum = values[0];
            var sumOfDecreases = 0.0;
            var sumOfIncreases = 0.0;

            variance += (values[0] - average) * (values[0] - average);
            rootMeanSquare += values[0] * values[0];

            for (var i = 1; i < values.Length; i++)
            {
                if (values[i] > maximum)
                {
                    maximum = values[i];
                }
                else if (values[i] < minimum)
                {
                    minimum = values[i];
                }

                var movement = values[i] - values[i - 1];
                if (movement > 0)
                {
                    sumOfIncreases += movement;
                }
                else
                {
                    sumOfDecreases += movement;
                }

                variance += (values[i] - average) * (values[i] - average);
                rootMeanSquare += values[i] * values[i];
            }

            variance = variance / amount;
            rootMeanSquare = Math.Sqrt(rootMeanSquare / amount);
            var standardDeviation = Math.Sqrt(variance);

            return await Task.FromResult(new ProcessedData
            {
                Date = CreateFullHourDateTime(data[0].Date).ToUniversalTime(),
                Maximum = maximum,
                Minimum = minimum,
                RootMeanSquare = rootMeanSquare,
                StandardDeviation = standardDeviation,
                Variance = variance,
                SumOfDecreases = sumOfDecreases,
                SumOfIncreases = sumOfIncreases
            }).ConfigureAwait(false);
        }

        private DateTime CreateFullHourDateTime(DateTime toParse)
        {
            return new DateTime(toParse.Year, toParse.Month, toParse.Day, toParse.Hour, 0, 0);
        }
    }
}