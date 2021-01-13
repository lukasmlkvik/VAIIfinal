using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VAII.Models
{
    public class ServerInfoModel
    {
        [Key]
        [NotNull]
        public int id { get; set; }
        public DateTime date { get; set; }
        public string info { get; set; }
    }
}
