// ReSharper disable InconsistentNaming

namespace Domain.Mongo.API.Mappers.Hero.DTO;

public class SeedHeroDto
{
    public string name { get; set; }
    public string localized_name { get; set; }
    public string attack_type { get; set; }
    public string primary_attr { get; set; }
    public List<string> roles { get; set; }
}