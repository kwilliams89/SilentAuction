using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SilentAuction.Models
{
    public class Sponsor
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Must be 256 characters or less")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        // Tailand phone number length?
        [Required]
        [StringLength(11, ErrorMessage = "Must be 11 characters or less")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        // Has Many
        public ICollection<Item> Items { get; set; }
    }
}
