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

        public Sponsor Sponsor { get; set; }

        [Required]
        [StringLength(35, ErrorMessage = "Must be 35 characters or less")]
        public string Name { get; set; }

        /// <summary>
        /// Note on strings
        /// string is an alias in C# for System.String. So technically, there is no difference. It's like int vs. System.Int32.
        /// As far as guidelines, I think it's generally recommended to use string any time you're referring to an object. 
        /// https://stackoverflow.com/questions/7074/what-is-the-difference-between-string-and-string-in-c
        /// </summary>
        public string Description { get; set; }

        public int CatagoryId { get; set; }

        public virtual Catagory Catagory { get; set; }

        [Required]
        [Display(Name = "Retail Price")]
        [DataType(DataType.Currency)]
        public decimal RetailPrice { get; set; }

        [Required]
        //https://stackoverflow.com/questions/1165761/decimal-vs-double-which-one-should-i-use-and-when
        [Display(Name = "Starting Bid")]
        [DataType(DataType.Currency)]
        public decimal StartingBid { get; set; }

        // Has Many
        public ICollection<Media> Media { get; set; }

        public ICollection<Listing> Listings { get; set; }

        public ICollection<BidHistory> BidHistories { get; set; }

    }
}
