using Abstract1;
using System;

namespace Concrete1
{
    public class Service : IService
    {
        public void Execute()
        {
            Console.WriteLine("OK");
        }
    }
}
