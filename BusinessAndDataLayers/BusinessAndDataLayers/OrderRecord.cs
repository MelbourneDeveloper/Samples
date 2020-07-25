using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCoreGetSQL
{
    public class OrderRecord
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
