using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SilentAuction.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Sponsor")]
        [Display(Name = "Sponsor")]
        public int SponsorId { get; set; }

        public virtual Sponsor Sponsor { get; set; }

        [Required]
        [StringLength(256, ErrorMessage = "Must be 256 characters or less")]
        public string Name { get; set; }

        /// <summary>
        /// Note on strings
        /// string is an alias in C# for System.String. So technically, there is no difference. It's like int vs. System.Int32.
        /// As far as guidelines, I think it's generally recommended to use string any time you're referring to an object. 
        /// https://stackoverflow.com/questions/7074/what-is-the-difference-between-string-and-string-in-c
        /// </summary>
        public string Description { get; set; }

        [Required]
        [ForeignKey(nameof(Models.Category))]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [Required]
        [Display(Name = "Retail Price")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal RetailPrice { get; set; }

        // Has Many
        public virtual ICollection<ItemMedia> ItemMedia { get; set; }

        public ICollection<Listing> Listings { get; set; }

        public ICollection<BidHistory> BidHistories { get; set; }

        public string Terms { get; set; }

        
        [Display(Name = "Offer Expires")]
        [DataType(DataType.Date)]
        public string OfferExpires { get; set; }

    }
}
