using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class ChapterDtoForMeList
    {
        [Required]
        public ChapterType Type { get; set; }
        public string? Title { get; set; }
        [Required]
        public IEnumerable<EpisodeDtoForMeList> Episodes { get; set; }
    }
}
