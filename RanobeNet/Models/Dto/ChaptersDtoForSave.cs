using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class ChaptersDtoForSave
    {
        [Required]
        public IEnumerable<ChaptersChapterForSave> Chapters { get; set; }

        public class ChaptersChapterForSave
        {
            public long? Id { get; set; }
            [Required]
            public ChapterType Type { get; set; }
            public string? Title { get; set; }
            [Required]
            public IEnumerable<ChaptersEpisodeForSave> Episodes { get; set; }
        }

        public class ChaptersEpisodeForSave
        {
            [Required]
            public long Id { get; set; }
        }

    }
}
