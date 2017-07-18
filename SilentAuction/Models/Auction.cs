using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilentAuction.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }

        [StringLength(128)]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        // Has Many
        public ICollection<Listing> Listings { get; set; }
    }
}
