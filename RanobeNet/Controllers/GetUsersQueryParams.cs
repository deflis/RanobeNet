#nullable disable
using System.ComponentModel;

namespace RanobeNet.Controllers
{
    public class GetUsersQueryParams
    {
        [DefaultValue("id")]
        public UserField? order { get; set; }
        [DefaultValue(10)]
        public int? size { get; set; }
        [DefaultValue(1)]
        public int? page { get; set; }
        [DefaultValue(false)]
        public bool? descending { get; set; }
    }
    public enum UserField { id, name, modified, created }
}
