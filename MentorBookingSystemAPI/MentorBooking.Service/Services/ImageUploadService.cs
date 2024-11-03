using MentorBooking.Repository.Data;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class ImageUploadService : IImageUploadService
{
    private readonly IUserRepository _userRepository;
    private readonly IImageRepository _imageRepository;

    public ImageUploadService(IUserRepository userRepository, IImageRepository imageRepository)
    {
        _userRepository = userRepository;
        _imageRepository = imageRepository;
    }

    public async Task<ImageUploadModelResponse> UploadImageAsync(Guid userId, ImageUploadModelRequest request)
    {
        // var userUpdateImage = await _dbContext.Users.SingleOrDefaultAsync(user => user.Id == request.UserId);
        var userUpdateImage = await _userRepository.FindByIdAsync(userId.ToString());
        if (userUpdateImage == null)
            return new ImageUploadModelResponse()
            {
                Status = "Error",
                Message = "User does not exist."
            };
        var fileImage = request.Image;
        if (fileImage.Length < 1)
            return new ImageUploadModelResponse
            {
                Status = "Error",
                Message = "Please provide a file image."
            };
        var allowedFileTypes = new[] { "jpg", "jpeg", "png" };
        var fileExtension = Path.GetExtension(fileImage.FileName)?.TrimStart('.').ToLower();
        if (!allowedFileTypes.Contains(fileExtension) || string.IsNullOrEmpty(fileImage.FileName))
            return new ImageUploadModelResponse
            {
                Status = "Error",
                Message = "Please provide a file format is image."
            };
        var fileName = Path.GetFileNameWithoutExtension(fileImage.FileName) + $"_{userId}.{fileExtension}";
        var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images");
        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
        }

        if (!string.IsNullOrEmpty(userUpdateImage.Image))
        {
            var oldImage = userUpdateImage.Image.Replace("/images/", "");
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images", oldImage);
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }
        var path = Path.Combine(imagesDirectory, fileName);
        await using (var stream = new FileStream(path, FileMode.Create))
        {
            await fileImage.CopyToAsync(stream);
        }

        var imagePath = $"/images/{fileName}";
        if (!await _imageRepository.UpdateImageAsync(userUpdateImage, imagePath))
        {
            return new ImageUploadModelResponse()
            {
                Status = "ServerError",
                Message = "Image upload failed, database error."
            };
        }

        return new ImageUploadModelResponse
        {
            Status = "Success",
            Message = "Image uploaded successfully."
        };
    }

    public async Task<ImageGetModelResponse> GetImageByIdAsync(ImageGetModelRequest request)
    {
        var imageUrlResponse = await _imageRepository.GetImageUrlAsync(Guid.Parse(request.UserId.ToString()));
        if (imageUrlResponse == null)
            return new ImageGetModelResponse()
            {
                Status = "Error",
                Message = "Image does not exist or database is error."
            };
        return new ImageGetModelResponse()
        {
            Status = "Success",
            Message = "Image retrieved successfully.",
            ImageUrl = imageUrlResponse
        };
    }

    public async Task<ImageDeleteModelResponse> DeleteImageAsync(ImageDeleteModelRequest request)
    {
        var userDeleteImage = await _userRepository.FindByIdAsync(request.UserId.ToString());
        if (!string.IsNullOrEmpty(userDeleteImage?.Image))
        {
            var oldImage = userDeleteImage.Image.Replace("/images/", "");
            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images", oldImage);
            if (File.Exists(oldImagePath))
            {
                File.Delete(oldImagePath);
            }
        }
        if (!await _imageRepository.DeleteImageAsync(userDeleteImage!))
        {
            return new ImageDeleteModelResponse()
            {
                Status = "Error",
                Message = "Image does not exist or database is error."
            };
        }
        
        return new ImageDeleteModelResponse()
        {
            Status = "Success",
            Message = "Image deleted successfully."
        };
    }
}