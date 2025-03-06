using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace ModelLayer.Model
{
    public class RequestModel
    {
        [Key]
        public string key { get; set; }

        [Required]
        public string value { get; set; }
    }
}
