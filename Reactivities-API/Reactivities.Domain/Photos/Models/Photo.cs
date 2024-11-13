using System.ComponentModel.DataAnnotations;

namespace Reactivities.Domain.Photos.Models
{
    public class Photo
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(100)]
        public required string StorageId { get; set; }

        [Required]
        [StringLength(300)]
        public required string Url { get; set; }
    }
}
