using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class ChaptersDto
    {
        [Required]
        public IEnumerable<Chapter> Chapters { get; set; }

        public class Chapter
        {
            public long? Id { get; set; }
            [Required]
            public ChapterType Type { get; set; }
            public string? Title { get; set; }
            [Required]
            public IEnumerable<Episode> Episodes { get; set; }
        }

        public class Episode
        {
            [Required]
            public long Id { get; set; }
        }

    }
}
