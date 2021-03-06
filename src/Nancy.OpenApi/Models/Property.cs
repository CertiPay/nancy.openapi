using System.Collections.Generic;

namespace Nancy.OpenApi.Models
{
    public class Property
    {
        public string Format { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public List<string> Enum { get; set; }

        public Property()
        {
            Enum = new List<string>();
        }
    }
}