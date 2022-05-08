using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class NovelLinkDto
    {
        [Required]
        public string Link { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
