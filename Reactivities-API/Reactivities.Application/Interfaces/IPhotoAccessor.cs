using Microsoft.AspNetCore.Http;
using Reactivities.Application.Photos;

namespace Reactivities.Application.Interfaces
{
    public interface IPhotoAccessor
    {
        public Task<PhotoUploadResult> AddPhoto(IFormFile file);
        public Task<string> DeletePhoto(string publicId);
    }
}
