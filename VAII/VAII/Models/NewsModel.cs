using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VAII.Models
{
    public class NewsModel
    {
        [Key]
        [NotNull]
        public int id { get; set; }
        public string image { get; set; }
        public string url { get; set; }
        public string headline { get; set; }
    }
}
