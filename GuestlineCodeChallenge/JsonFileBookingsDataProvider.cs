using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using GuestlineCodeChallenge.Core.Data;
using GuestlineCodeChallenge.Core.Data.Models;

namespace GuestlineCodeChallenge;

public class JsonFileBookingsDataProvider(string bookingsJsonPath, string hotelsJsonPath)
    : IBookingsDataProvider
{
    private static readonly JsonSerializerOptions BookingsDataJsonSerializerOptions =
        new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new CustomDateTimeOnlyJsonConverter() },
        };

    public async Task<IReadOnlyList<Hotel>> GetHotelsAsync()
    {
        var hotels = JsonSerializer.Deserialize<IReadOnlyList<Hotel>>(
            await File.ReadAllTextAsync(hotelsJsonPath),
            BookingsDataJsonSerializerOptions
        );
        return hotels ?? throw new Exception("Error when parsing. Got null hotels.");
    }

    public async Task<IReadOnlyList<Booking>> GetBookingsAsync()
    {
        var bookings = JsonSerializer.Deserialize<IReadOnlyList<Booking>>(
            await File.ReadAllTextAsync(bookingsJsonPath),
            BookingsDataJsonSerializerOptions
        );
        return bookings ?? throw new Exception("Error when parsing. Got null bookings.");
    }

    private class CustomDateTimeOnlyJsonConverter : JsonConverter<DateOnly>
    {
        public override DateOnly Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        ) => DateOnly.ParseExact(reader.GetString()!, "yyyyMMdd", CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateOnly dateOnlyValue,
            JsonSerializerOptions options
        ) =>
            writer.WriteStringValue(
                dateOnlyValue.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
            );
    }
}
