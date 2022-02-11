using System.ComponentModel.DataAnnotations;

namespace RanobeNet.Models.Dto
{
    public class UserDtoForSave
    {
        [Required]
        public string Name { get; set; }
    }
}
