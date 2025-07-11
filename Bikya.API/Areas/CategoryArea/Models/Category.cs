using Bikya.Data.Models;

namespace Bikya.API.Areas.Category.Models
{

    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public ICollection<Category>? SubCategories { get; set; }
        public ICollection<Product> Products { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }



}
