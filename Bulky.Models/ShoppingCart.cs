using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlulkyBook.Models
{
    public class ShoppingCart
    {
        [Key]
        public int Id { get; set; }
        public int ProductID { get; set; }
        [ValidateNever]
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
        [Range(1, 1000, ErrorMessage = "Please Enter 1 To 1000")]
        public int Count { get; set; }
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
