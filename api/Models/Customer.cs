using System;
using System.ComponentModel.DataAnnotations;

namespace api.Models
{
   public class Customer
   {
      public int Id { get; set; }

      [Required]
      public string Name { get; set; }

      public string Phone { get; set; }

      public Gender Gender { get; set; }

      public City City { get; set; }

      public Region Region { get; set; }

      public DateTime LastPurchase { get; set; }

      public Classification Classification { get; set; }

      public User User { get; set; }
   }
}
