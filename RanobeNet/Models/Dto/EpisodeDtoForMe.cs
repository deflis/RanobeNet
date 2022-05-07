using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class EpisodeDtoForMe
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long? ChapterId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Story { get; set; }
        [Required]
        public bool Private { get; set; }
    }
}
