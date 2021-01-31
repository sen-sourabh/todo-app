using System.ComponentModel.DataAnnotations;

namespace TodoListAPI.Models
{
    public class ImgUploader
    {
        [Key]
        public long imgId { get; set; }
        public byte[] image { get; set; }
    }
}
