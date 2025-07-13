using System.ComponentModel.DataAnnotations;

namespace Bikya.Services.Interfaces
{
    public class CategoryDTO
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }

        public int? ParentCategoryId { get; set; }

        public DateTime CreatedAt { get; set; }


    }

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
    public class UpdateCategoryDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public int? ParentCategoryId { get; set; }
    }

}