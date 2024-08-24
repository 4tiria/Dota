namespace Dota.Statistics.Services;

public interface IWeatherService
{
    IEnumerable<WeatherForecast> Forecast();
}
