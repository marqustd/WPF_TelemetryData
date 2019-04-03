using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LogicLayer.Model;

namespace LogicLayer
{
    public static class Utilities
    {
        public static bool IsDateEqual(DateTime first, DateTime second)
        {
            Require.NotNull(first, nameof(first));
            Require.NotNull(second, nameof(second));

            return first.Day == second.Day && first.Month == second.Month && first.Year == second.Year;
        }

        public static bool IsHourEqual(DateTime first, DateTime second)
        {
            Require.NotNull(first, nameof(first));
            Require.NotNull(second, nameof(second));

            return first.Hour == second.Hour;
        }

        public static async Task<Dictionary<string, Dictionary<string, ProcessedData>>> PrepareDictionaries(
            ProcessedData[] processedData)
        {
            Require.NotEmpty(processedData, nameof(processedData));

            var organized = new Dictionary<string, Dictionary<string, ProcessedData>>();
            var myData = processedData[0];
            var shortDate = myData.Date.ToShortDateString();
            var currentDayDictionary = new Dictionary<string, ProcessedData>();
            foreach (var data in processedData)
            {
                if (IsDateEqual(myData.Date, data.Date))
                {
                    currentDayDictionary.Add(data.Date.ToShortTimeString(), data);
                }
                else
                {
                    organized.Add(shortDate, currentDayDictionary);
                    currentDayDictionary = new Dictionary<string, ProcessedData>();
                    myData = data;
                    shortDate = data.Date.ToShortDateString();
                }
            }

            return await Task.FromResult(organized);
        }
    }
}