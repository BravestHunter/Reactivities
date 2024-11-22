using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Photos.Dtos;
using Reactivities.Domain.Photos.Interfaces;
using Reactivities.Infrastructure.Configuration;

namespace Reactivities.Infrastructure.Photos
{
    internal sealed class CloudinaryPhotoStorage : IPhotoStorage
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryPhotoStorage(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.ApiKey, config.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        public async Task<PhotoUploadResult> Add(Stream stream)
        {
            if (stream.Length == 0)
            {
                throw new BadRequestException("Can't upload empty image");
            }

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(Guid.NewGuid().ToString(), stream),
                Folder = "Reactivities",
                Transformation = new Transformation().Height(500).Width(500).Crop("fill")
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new BadRequestException($"Failed to upload image: {uploadResult.Error.Message}");
            }

            return new PhotoUploadResult
            {
                StorageId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString()
            };
        }

        public async Task Delete(string storageId)
        {
            var deleteParams = new DeletionParams(storageId);

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result != "ok")
            {
                throw new FailedToDeleteEntityException($"Failed to remove photo {storageId}");
            }
        }
    }
}
