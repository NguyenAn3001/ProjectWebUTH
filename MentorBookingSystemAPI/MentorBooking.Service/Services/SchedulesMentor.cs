using AutoMapper;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class SchedulesMentor : ISchedulesMentor
{
    private readonly ISchedulesAvailableRepository _schedulesAvailableRepository;
    private readonly IMapper _mapper;

    public SchedulesMentor(ISchedulesAvailableRepository schedulesAvailableRepository, IMapper mapper)
    {
        _schedulesAvailableRepository = schedulesAvailableRepository;
        _mapper = mapper;
    }
    public async Task<ApiResponse> AddSchedulesAsync(Guid mentorId, List<SchedulesAvailableModelRequest> schedulesModels)
    {
        var schedules = new List<SchedulesAvailable>();
        foreach (SchedulesAvailableModelRequest schedulesModel in schedulesModels)
        {
            if (TimeOnly.Parse(schedulesModel.StartTime) >= TimeOnly.Parse(schedulesModel.EndTime))
            {
                return new ApiResponse()
                {
                    Status = "Error",
                    Message = "Schedules start time cannot be greater than the end time."
                };
            }

            schedules.Add(new SchedulesAvailable()
            {
                ScheduleAvailableId = Guid.NewGuid(),
                MentorId = mentorId,
                StartTime = TimeOnly.Parse(schedulesModel.StartTime),
                EndTime = TimeOnly.Parse(schedulesModel.EndTime),
                FreeDay = schedulesModel.FreeDay
            });
        }

        if (!await _schedulesAvailableRepository.AddSchedulesAvailableAsync(schedules))
        {
            return new ApiResponse()
            {
                Status = "ServerError",
                Message = "Add schedules failed, please try again later."
            };
        }
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Schedules successfully added.",
            Data = _mapper.Map<List<SchedulesAvailableModelResponse>>(schedules)
        };
    }

    public ApiResponse GetSchedules(Guid mentorId)
    {
        var schedules = _schedulesAvailableRepository.GetAllSchedulesAvailable(mentorId);
        if (schedules == null)
            return new ApiResponse()
            {
                Status = "NotFound",
                Message = "Schedules not found."
            };
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Schedules successfully retrieved.",
            Data = _mapper.Map<List<SchedulesAvailableModelResponse>>(schedules)
        };
    }

    public async Task<ApiResponse> UpdateScheduleAsync(Guid mentorId, Guid scheduleAvailableId, SchedulesAvailableModelRequest scheduleModels)
    {
        var scheduleToUpdate = new SchedulesAvailable()
        {
            ScheduleAvailableId = scheduleAvailableId,
            MentorId = mentorId,
            FreeDay = scheduleModels.FreeDay,
            StartTime = TimeOnly.Parse(scheduleModels.StartTime),
            EndTime = TimeOnly.Parse(scheduleModels.EndTime)
        };
        var schedulesAvailableExist = _schedulesAvailableRepository.GetAllSchedulesAvailable(mentorId);
        var scheduleOverlap = schedulesAvailableExist?.SingleOrDefault(x => x.FreeDay == scheduleToUpdate.FreeDay && x.StartTime == scheduleToUpdate.StartTime && x.EndTime == scheduleToUpdate.EndTime);
        if (scheduleOverlap != null)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Schedules already exist."
            };
        
        var updateScheduleResponse = await _schedulesAvailableRepository.UpdateSchedulesAvailableAsync(scheduleAvailableId, scheduleToUpdate);
        if (!updateScheduleResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Schedules update failed, please try again later."
            };
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Schedules successfully updated.",
            Data = scheduleToUpdate
        };
    }

    public async Task<ApiResponse> DeleteScheduleAsync(Guid scheduleId)
    {
        var scheduleExisting = await _schedulesAvailableRepository.GetSchedulesAvailableAsync(scheduleId);
        if (scheduleExisting == null)
            return new ApiResponse()
            {
                Status = "NotFound",
                Message = "Schedules not found."
            };
        
        var deleteScheduleResponse = await _schedulesAvailableRepository.DeleteSchedulesAvailableAsync(scheduleId);
        if (!deleteScheduleResponse)
            return new ApiResponse()
            {
                Status = "Error",
                Message = "Schedules missing or deleted."
            };
        return new ApiResponse()
        {
            Status = "Success",
            Message = "Schedules successfully deleted."
        };
    }
}