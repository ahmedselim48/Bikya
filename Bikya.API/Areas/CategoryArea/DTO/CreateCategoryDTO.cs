using System.ComponentModel.DataAnnotations;

namespace Bikya.API.Areas.CategoryArea.DTO
{
    public class CreateCategoryDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public int? ParentCategoryId { get; set; }
    }



}
