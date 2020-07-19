using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreGetSQL
{
    public class OrderLine
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Item { get; set; }
        public int Count { get; set; }
    }
}
