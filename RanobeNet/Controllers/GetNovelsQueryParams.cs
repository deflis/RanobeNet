using System.ComponentModel;

namespace RanobeNet.Controllers
{
    public class GetNovelsQueryParams
    {
        [DefaultValue("id")]
        public NovelField? order { get; set; }
        [DefaultValue(10)]
        public int? size { get; set; }
        [DefaultValue(1)]
        public int? page { get; set; }
        [DefaultValue(false)]
        public bool? descending { get; set; }
    }
    public enum NovelField { id, title, modified, created }
}
