using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class ChaptersDtoForMe
    {
        [Required]
        public IEnumerable<ChaptersChapterForMe> Chapters { get; set; }

        public class ChaptersChapterForMe
        {
            public long? Id { get; set; }
            [Required]
            public ChapterType Type { get; set; }
            public string? Title { get; set; }
            [Required]
            public IEnumerable<ChaptersEpisodeForMe> Episodes { get; set; }
        }

        public class ChaptersEpisodeForMe
        {
            [Required]
            public long Id { get; set; }
            [Required]
            public string Title { get; set; }
            [Required]
            public bool Private { get; set; }
        }

    }
}
