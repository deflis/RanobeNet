using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class NovelDtoForPublic
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public IEnumerable<ChapterDtoForPublic> Chapters { get; set; }
    }
}
