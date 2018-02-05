using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project2_Cooperation.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }
        
        public string ShortDescription { get; set; }

        [Required]
        [ForeignKey("Member")]
        public string MemberId { get; set; }
        public ApplicationUser Member { get; set; }

        [DataType(DataType.Currency)]
        public decimal BuyPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal SalePrice { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }

        public bool IsLive { get; set; }
        
        public bool BoughtFromAdmin { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int StockInitOffer { get; set; }

        [Required]
        public string Category { get; set; }

        public bool Featured { get; set; }

        public static async Task CopyImageToServer(Product product)
        {
            if (product.ImageFile != null && product.ImageFile.Length != 0)
            {
                var dir = $"wwwroot/images/products/{product.Category}/";
                Directory.CreateDirectory(dir);

                string imgPath = dir + product.ImageFile.FileName;

                using (var stream = new FileStream(imgPath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = imgPath.Substring(7);
            }
        }
    }
}
