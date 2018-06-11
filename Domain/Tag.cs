using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Tag
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 1)]
        public string Name { get; set; }
    }
}
