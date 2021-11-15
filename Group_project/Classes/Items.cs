using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Group_project.Classes
{
    class productItem
    {
        public string Article { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public string Info { get; set; }
    }
    class orderItem
    {
        public string Article { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Sum { get; set; }
    }
    class reportItem
    {
        public string Article { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Price { get; set; }
        public int Count { get; set; }
        public decimal Sum { get; set; }
    }
}
