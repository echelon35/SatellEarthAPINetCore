using GeoJSON.Net;
using GeoJSON.Net.Geometry;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using NetGeometry = NetTopologySuite.Geometries.Geometry;

namespace SatellEarthAPI.Infrastructure.Converter;
public class GeoJSONToGeomConverter
{

    public static NetGeometry Convert(GeoJSONObject geoJson)
    {
        NetGeometry geometry;

        var serializer = GeoJsonSerializer.Create();
        using (var stringReader = new StringReader(JsonConvert.SerializeObject(geoJson)))
        using (var jsonReader = new JsonTextReader(stringReader))
        {
            geometry = serializer.Deserialize<NetGeometry>(jsonReader);
        }
        return geometry;
    }

    public static string ConvertBack(NetGeometry geometry)
    {
        string geoJson;
        GeoJSONObject data;

        var serializer = GeoJsonSerializer.Create();
        using (var stringWriter = new StringWriter())
        using (var jsonWriter = new JsonTextWriter(stringWriter))
        {
            serializer.Serialize(jsonWriter, geometry);
            geoJson = stringWriter.ToString();
        }

        return geoJson;
    }
}
