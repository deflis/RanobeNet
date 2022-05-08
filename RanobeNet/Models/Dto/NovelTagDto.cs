using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class NovelTagDto
    {
        [Required]
        public string Tag { get; set; }
    }
}
