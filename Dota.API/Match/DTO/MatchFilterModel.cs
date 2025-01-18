using Newtonsoft.Json;

namespace Dota.API.Models.FilterModels;

public class MatchFilterModel
{
    [JsonProperty("minDurationInMinutes")]
    public int? MinDurationInMinutes { get; set; }

    [JsonProperty("maxDurationInMinutes")]
    public int? MaxDurationInMinutes { get; set; }

    [JsonProperty("minStartedMillisecondsBefore")]
    public long? MinStartedMillisecondsBefore { get; set; }

    [JsonProperty("maxStartedMillisecondsBefore")]
    public long? MaxStartedMillisecondsBefore { get; set; }

    [JsonProperty("selfTeam")]
    public List<Domain.Mongo.API.Hero> SelfTeam { get; set; } = [];

    [JsonProperty("otherTeam")]
    public List<Domain.Mongo.API.Hero> OtherTeam { get; set; } = [];

    [JsonProperty("skip")]
    public int? Skip { get; set; }

    [JsonProperty("take")]
    public int? Take { get; set; }

    [JsonProperty("daysAgo")]
    public int? DaysAgo { get; set; }
}