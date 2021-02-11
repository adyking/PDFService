using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DocStore.Data.Models
{
    public class Doc
    {
        [JsonIgnore]
        public int Id { get; set; }
     
        public string Name { get; set; }
        public string Location { get; set; }
        public string FileSize { get; set; }

        [JsonIgnore]
        public byte[] Data { get; set; }


    }
}
