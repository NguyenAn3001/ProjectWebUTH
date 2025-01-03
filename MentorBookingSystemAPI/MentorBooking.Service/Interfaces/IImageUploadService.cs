﻿using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;

namespace MentorBooking.Service.Interfaces;

public interface IImageUploadService
{
    Task<ImageUploadModelResponse> UploadImageAsync(Guid userId, ImageUploadModelRequest request);
    Task<ImageGetModelResponse> GetImageByIdAsync(ImageGetModelRequest request);
    Task<ImageDeleteModelResponse> DeleteImageAsync(ImageDeleteModelRequest request);
}