using System;
using System.ComponentModel.DataAnnotations;

namespace BusinessAndDataLayers
{
    public class OrderLine
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Item { get; set; }
        public int Count { get; set; }
    }
}
