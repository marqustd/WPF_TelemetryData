using System;
using Newtonsoft.Json;

namespace LogicLayer.Model
{
    /// <summary>
    ///     JsonData after processing
    /// </summary>
    public sealed class ProcessedData
    {
        public DateTime Date { get; set; }
        public double RootMeanSquare { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public double StandardDeviation { get; set; }
        public double Variance { get; set; }
        public double SumOfIncreases { get; set; }
        public double SumOfDecreases { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}