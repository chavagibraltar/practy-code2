using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
namespace practy_code2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public List<string> HtmlTags { get; set; }
        public List<string> HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            var jsonTags = File.ReadAllText("seed/HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<List<string>>(jsonTags);
            var jsonVoidTags = File.ReadAllText("seed/HtmlTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<List<string>>(jsonVoidTags);           
        }
    }
}
