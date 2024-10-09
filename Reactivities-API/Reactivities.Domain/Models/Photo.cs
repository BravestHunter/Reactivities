using System.ComponentModel.DataAnnotations;

namespace Reactivities.Domain.Models
{
    public class Photo
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public string StorageId { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string Url { get; set; } = string.Empty;

        [Required]
        public bool IsMain { get; set; }
    }
}
