using System;
using Newtonsoft.Json;

namespace LoootCreate.Models
{
    public class LotteryModel
    {
        public int drwNo { get; set; }
        public decimal totSellamnt { get; set; }
        public bool returnValue { get; set; }
        public DateTime drwNoDate { get; set; }
        public decimal firstWinamnt { get; set; }
        public decimal firstPrzwnerCo { get; set; }
        public decimal firstAccumamnt { get; set; }
        public byte drwtNo1 { get; set; }
        public byte drwtNo2 { get; set; }
        public byte drwtNo3 { get; set; }
        public byte drwtNo4 { get; set; }
        public byte drwtNo5 { get; set; }
        public byte drwtNo6 { get; set; }
        public byte bnusNo { get; set; }
        public bool confirm { get; set; }

        /* {
                "--totSellamnt": 56561977000,
                --"returnValue": "success",
                --"drwNoDate": "2004-10-30",
                --"firstWinamnt": 3315315525,
                --"drwtNo6": 42,
                --"drwtNo4": 23,
                --"firstPrzwnerCo": 4,
                --"drwtNo5": 37,
                --"bnusNo": 6,
                --"firstAccumamnt": 0,
                --"drwNo": 100,
                --"drwtNo2": 7,
                --"drwtNo3": 11,
                --"drwtNo1": 1
            }
         */
    }

    public class CustomBooleanJsonConverter : JsonConverter<bool>
    {
        public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            return Convert.ToBoolean(reader.ValueType == typeof(string) ? (reader.Value.ToString() == "success") ? true : false : reader.Value); ;
        }

        public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}