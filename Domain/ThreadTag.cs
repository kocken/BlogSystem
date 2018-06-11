using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ThreadTag
    {
        [Key, Required]
        public int ThreadId { get; set; }

        [Key, Required]
        public int TagId { get; set; }

        [Required]
        public Thread Thread { get; set; }

        [Required]
        public Tag Tag { get; set; }
    }
}
