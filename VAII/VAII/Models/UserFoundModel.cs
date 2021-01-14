using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace VAII.Models
{

    public class UserFoundModel
    {

        [Key]
        [NotNull]
        public string Email { get; set; }
        [Key]
        [NotNull]
        public string symbol { get; set; }
    }
}
