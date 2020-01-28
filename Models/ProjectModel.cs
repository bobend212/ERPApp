using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ERPApp.Models
{
    public class ProjectModel
    {
        [Key]
        public int ProjectId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string  Number { get; set; }
        [Required]
        public ProjectType Type { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }

    }
}
