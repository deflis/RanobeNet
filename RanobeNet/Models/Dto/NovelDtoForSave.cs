using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class NovelDtoForSave
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public string? Author { get; set; }
        [Required]
        public IEnumerable<NovelLinkDto> Links { get; set; }
        [Required]
        public IEnumerable<NovelTagDto> Tags { get; set; }
    }
}
