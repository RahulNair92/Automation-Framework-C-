using System;
using System.Collections.Generic;
using System.Text;

namespace UdemyRestSharpNetCore.Model
{
    public class LocationModel
    {
        public int id { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public List<Address> address { get; set; }
    }

    public class Address
    {
        public string street { get; set; }
        public string flat_no { get; set; }
        public int pincode { get; set; }
        public string type { get; set; }
    }
}
