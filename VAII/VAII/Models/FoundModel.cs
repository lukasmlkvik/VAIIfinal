using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VAII.Models
{
    public class FoundModel
    {
        [Key]
        [NotNull]
        public string symbol { get; set; }
        public string description { get; set; }
        
    }

    public class FoundsData
    {
        public List<FoundModel> List { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public int MaxPage { get; set; }
    }
}
