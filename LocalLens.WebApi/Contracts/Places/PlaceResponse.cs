namespace LocalLens.WebApi.Contracts.Places;

public class PlaceResponse
{
    public string location_name { get; set; }
    public string opening_hours { get; set; }
    public string place_name { get; set; }
    public List<string> facilities { get; set; }
    public string about_place { get; set; }
    public List<string> feature_list { get; set; }
    public string reason { get; set; }
    public string place_image { get; set; }

    public double latitude { get; set; }
    public double longitude { get; set; }

}
