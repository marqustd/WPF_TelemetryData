using System;

namespace LogicLayer.Model
{
    /// <summary>
    ///     Data read from Json file
    /// </summary>
    public sealed class JsonData
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
    }
}