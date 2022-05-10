using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class EpisodeDtoForMeList
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public bool Private { get; set; }
    }
}
