using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DotnetLibraries.JsonConverter
{
    /*
     * reference
     * https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft?pivots=dotnet-core-3-1
     * https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/converters-how-to?pivots=dotnet-core-3-1
     * Newtonsoft parses NaN, Infinity, and -Infinity JSON string tokens.
     * In .NET Core 3.1, System.Text.Json doesn't support these tokens    
     * but you can write a custom converter to handle them.    
     * In .NET 5 and later versions, use JsonNumberHandling.AllowNamedFloatingPointLiterals.    
     * For information about how to use this setting, see Allow or write numbers in quotes.
     */

    // https://stackoverflow.com/questions/71343365/system-text-json-handling-infinity-values
    // https://github.com/dotnet/runtime/blob/main/src/libraries/System.Text.Json/src/System/Text/Json/Serialization/Converters/Value/DoubleConverter.cs
    class DoubleExtConverter : JsonConverter<double>
    {
        public override double Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                // some editor is okay, but it has error on my computer
                //return Convert.ToDouble(reader.GetString());

                string text = reader.GetString();
                if (string.Equals("NaN", text, StringComparison.OrdinalIgnoreCase))
                {
                    return double.NaN;
                }
                else if (string.Equals("-Infinity", text, StringComparison.OrdinalIgnoreCase))
                {
                    return double.NegativeInfinity;
                }
                else if (string.Equals("Infinity", text, StringComparison.OrdinalIgnoreCase))
                {
                    return double.PositiveInfinity;
                }
                else
                {
                    return double.NaN;
                }
            }

            return reader.GetDouble();
        }

        public override void Write(Utf8JsonWriter writer, double value, JsonSerializerOptions options)
        {
            if (double.IsNaN(value))
            {
                // "NaN":"NaN"
                writer.WriteStringValue("NaN");
                return;
            }
            else if (double.IsNegativeInfinity(value))
            {
                // "NegativeInfinity":"-Infinity"
                writer.WriteStringValue("-Infinity");
                return;
            }
            else if (double.IsPositiveInfinity(value))
            {
                // "PositiveInfinity":"Infinity"
                writer.WriteStringValue("Infinity");
                return;
            }

            //writer.WriteStringValue(value.ToString());
            writer.WriteNumberValue(value);
        }
    }
}
