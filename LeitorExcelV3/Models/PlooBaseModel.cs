using System.Text.Json.Serialization;

namespace LeitorExcelV3.Models;

public class PlooBaseModel<T>
{
    [JsonPropertyName("@odata.context")]
    public string Context { get; set; }
    [JsonPropertyName("value")]
    public List<T> Value { get; set; }
}
