using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilentAuction.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Listing")]
        [Display(Name = "Listing")]
        public int ListingId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
    }
}
