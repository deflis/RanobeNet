using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class UserDtoForMe
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
