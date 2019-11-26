using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sample.Model
{
    [Serializable]
    public class MegaPers
    {
        public string Type { get; set; }

        public string Name { get; set; }

        public string GUID { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public string Group { get; set; }

        public byte[] Image { get; set; }
    }
}