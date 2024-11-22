using Reactivities.Domain.Photos.Dtos;

namespace Reactivities.Domain.Photos.Interfaces
{
    public interface IPhotoStorage
    {
        Task<PhotoUploadResult> Add(Stream stream);
        Task Delete(string storageId);
    }
}
