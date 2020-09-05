using System.ComponentModel.DataAnnotations;

namespace BusinessAndDataLayers
{
    public class OrderRecord
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
