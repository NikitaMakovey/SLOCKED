using System;
using System.Collections;
using System.Collections.Generic;

namespace SLOCKED
{
    public class City
    {
        public long id { get; set; }
        public string name { get; set; }
        public string country { get; set; }
        public Coord coord { get; set; }
    }

    public class Coord
    {
        public double lon { get; set; }
        public double lat { get; set; }
    }

    public class LikedCity
    {
        public string name { get; set; }
        public string country { get; set; }
    }
}
