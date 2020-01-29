using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "Date")]
        public DateTime DateAdded { get; set; }

    }
}
