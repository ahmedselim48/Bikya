namespace Bikya.API.Areas.CategoryArea.DTO
{
    public class ShowCategoryDTO
    {

        public int Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public string? IconUrl { get; set; }

        public int? ParentCategoryId { get; set; }

        public DateTime CreatedAt { get; set; }


    }
}