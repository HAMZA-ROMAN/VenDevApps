using System.ComponentModel.DataAnnotations;

namespace VenDevApplication.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nom")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Déscription")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Prix")]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        [Display(Name = "Quantité")]
        public double Quantite { get; set; }
        [Display(Name = "EnStock")]
        public bool? Enstock { get; set; }
    }
}
