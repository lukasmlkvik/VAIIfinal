using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace VAII.Models
{

    public class UserFoundModel
    {
        public UserFoundModel(string id, string symbol)
        {
            Id = id;
            this.symbol = symbol;
        }

        [Key]
        [NotNull]
        public string Id { get; set; }
        [Key]
        [NotNull]
        public string symbol { get; set; }
    }
}
