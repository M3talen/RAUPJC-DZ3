using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrecaDz.Models
{
    public class AddTodoViewModel
    {
        [Required]
        [MaxLength(40)]
        public string Text { get; set; }
    }
}
