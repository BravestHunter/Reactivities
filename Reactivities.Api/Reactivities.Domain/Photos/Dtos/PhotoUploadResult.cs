namespace Reactivities.Domain.Photos.Dtos
{
    public class PhotoUploadResult
    {
        public required string StorageId { get; set; }
        public required string Url { get; set; }
    }
}
