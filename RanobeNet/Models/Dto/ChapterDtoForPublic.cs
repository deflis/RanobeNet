using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class ChapterDtoForPublic
    {
        [Required]
        public ChapterType Type { get; set; }
        public string? Title { get; set; }
        [Required]
        public IEnumerable<EpisodeDtoForPublic> Episodes { get; set; }
    }

    public enum ChapterType { NonChapter , Chapter }
}
