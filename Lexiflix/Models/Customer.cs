
﻿using System.ComponentModel.DataAnnotations;

namespace Lexiflix.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Billing adress")]
        public string BillingAddress { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Billing City")]
        public string BillingCity { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Postal code")]
        public string BillingZip { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Delivery adress")]
        public string DeliveryAddress { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Billing City")]
        public string DeliveryCity { get; set; }
        [Required]
        [StringLength(10)]
        [Display(Name = "Postal code")]
        public string DeliveryZip { get; set; }

        [Required]
        [StringLength(255)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(20)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}

