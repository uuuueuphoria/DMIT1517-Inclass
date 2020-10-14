using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DBSystem.ENTITIES
{
    [Table("Schools")]
    public class Schools
    {
        [Key]
        public string SchoolName { get; set; }
        public string SchoolCode { get; set; }
    }
}
