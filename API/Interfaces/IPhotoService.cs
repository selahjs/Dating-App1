using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Interfaces
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        //each file uploaded in Cloudinary is going to be given a publicId, it is stored inside the Photo entity
        Task<DeletionResult> DeletePhotoAsync(string publicId);

    }
}