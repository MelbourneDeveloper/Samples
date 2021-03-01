using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared
{
    public class Context : DbContext
    {
        public Tests Tests { get; set; } = new Tests();
    }

    public class Test
    {
        [Key]
        public int Key { get; set; }
    }

    public class Tests : DbSet<Test>
    {

    }

}
