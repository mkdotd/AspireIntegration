using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class Customers
    {
        public int ID { get; set; }
        public Guid? Guid { get; set; }
        public string? Name { get; set; }
        public int Number { get; set; }
        public string? Website { get; set; }
        public string? OwnerName { get; set; }        
        public string? IndustryName { get; set; }
        [NotMapped]
        public bool IsSeveraRecord { get { return Guid.HasValue; } }
    }

    public class CustomersJsonConverter : JsonConverter<Customers>
    {
        public override Customers? ReadJson(JsonReader reader, Type objectType, Customers? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var customer = new Customers();
            customer.Guid = (Guid)jsonObject["guid"];
                customer.Name = (string?)jsonObject["name"];
            customer.Number = (int)jsonObject["number"];
            customer.Website = (string?)jsonObject["website"];
            if(jsonObject["owner"].HasValues)
            {
                customer.OwnerName = jsonObject["owner"]?["name"]?.Value<string>();
            }
            if (jsonObject["industry"].HasValues)
            {
                customer.IndustryName = jsonObject["industry"]?["name"]?.Value<string>();
            }
            return customer;
        }

        public override void WriteJson(JsonWriter writer, Customers? value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("guid");
            writer.WriteValue(value.Guid);
            writer.WritePropertyName("name");
            writer.WriteValue(value.Name);
            writer.WritePropertyName("number");
            writer.WriteValue(value.Number);
            writer.WritePropertyName("website");
            writer.WriteValue(value.Website);
            writer.WritePropertyName("owner");
            writer.WriteStartObject();
            writer.WritePropertyName("name");
            writer.WriteValue(value.OwnerName);
            writer.WriteEndObject();
            writer.WritePropertyName("industry");
            writer.WriteStartObject();
            writer.WritePropertyName("name");
            writer.WriteValue(value.IndustryName);
            writer.WriteEndObject();
            writer.WriteEndObject();
        }
}
}
