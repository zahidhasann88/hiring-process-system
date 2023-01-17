using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace hps_api.Models
{
    [Table("country")]
    public class Country
    {
        [Key]
        [Column("id")]
        public int? Id { get; set; }

        [Column("country")]
        public string CountryName { get; set; }
    }
}
