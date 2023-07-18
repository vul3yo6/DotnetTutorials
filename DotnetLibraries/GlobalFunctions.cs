using DotnetLibraries.JsonConverter;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetLibraries
{
    public static class GlobalFunctions
    {
        public static string JsonSerializeLowerCase(object value)
        {
            return JsonSerialize(value, true, false);
        }
        public static string JsonSerialize(object value)
        {
            return JsonSerialize(value, false, false);
        }

        public static string JsonSerialize(object value, bool isLowerCase, bool writeIndented)
        {
            // https://docs.microsoft.com/zh-tw/dotnet/standard/serialization/system-text-json-customize-properties?pivots=dotnet-6-0
            // https://stackoverflow.com/questions/65956172/system-text-json-jsonserializer-ignores-dictionarykeypolicy-when-serializing-dic
            var serializeOptions = new JsonSerializerOptions
            {
                IgnoreReadOnlyProperties = true,    // 唯獨屬性, 不序列化
                WriteIndented = writeIndented,
                //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = isLowerCase ? new LowerCaseNamingPolicy() : null,    // 是否小寫
                DictionaryKeyPolicy = new NormalCaseNamingPolicy(),                         // 字典的 key 維持不變
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new DoubleExtConverter() }
            };
            return JsonSerializer.Serialize(value, value.GetType(), serializeOptions);
        }

        public static T JsonDeserializeOrCreateNew<T>(string value) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new T();
            }

            T t = JsonDeserialize<T>(value);
            return t ?? new T();
        }

        public static T JsonDeserialize<T>(string value)
        {
            // https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft?pivots=dotnet-7-0#json-strings-property-names-and-string-values
            // System.Text.Json only accepts property names and string values in double quotes
            // 只支援雙引號的格式
            // 若不想格式化文字內的單引號, 可以參考 https://stackoverflow.com/questions/74058315/dont-escape-single-quotes-with-system-text-json

            // https://stackoverflow.com/questions/45782127/json-net-case-insensitive-deserialization-not-working
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase), new DoubleExtConverter() },
            };
            return JsonSerializer.Deserialize<T>(value, serializeOptions);
        }
    }

    // https://stackoverflow.com/questions/69505378/how-do-i-serialize-object-properties-to-lower-case-using-system-text-json
    internal class LowerCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || !char.IsUpper(name[0]))
                return name;

            return name.ToLower();
        }
    }
    internal class NormalCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return name;
        }
    }
}
