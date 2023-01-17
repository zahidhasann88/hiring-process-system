using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace hps_api.Models
{
    [Table("info")]
    public partial class Info
    {
       [Key]
       [Column("id")]
       public int? Id { get; set; }

       [Column("name")]
        public string Name { get; set; }

       [Column("country")]
       public string Country { get; set; }
       [Column("city")]
       public string City { get; set; }

       [Column("skills")]
       public string Skills { get; set; }

       [Column("date_of_birth")]
       public DateTime DateOfBirth { get; set; }

       [Column("resume")]
       public string Resume { get; set; }
    }
}
