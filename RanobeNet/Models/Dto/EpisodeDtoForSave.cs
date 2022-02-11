using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class EpisodeDtoForSave
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Story { get; set; }
    }
}
