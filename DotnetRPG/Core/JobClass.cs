using System.Text.Json.Serialization;

namespace DotnetRpg.Core
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum JobClass
    {
        Knight,
        Mage,
        Archer,
        Assassin,
        Titan
    }
}