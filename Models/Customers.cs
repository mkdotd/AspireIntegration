using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Models
{
    public class Customers
    {
        public int ID { get; set; }
        public Guid Guid { get; set; }
        public string? Name { get; set; }
        public int Number { get; set; }
        public string? Website { get; set; }
        public string? OwnerName { get; set; }
        public string? IndustryName { get; set; }
    }
}
