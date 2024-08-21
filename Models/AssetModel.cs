using System.ComponentModel.DataAnnotations;

namespace MultisiteEventing.Models
{
    public class AssetModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string UnitOfMeasure { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime UpdatedDate { get; set; }
    }
}

