using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreGetSQL
{
    public class Order
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
    }
}
