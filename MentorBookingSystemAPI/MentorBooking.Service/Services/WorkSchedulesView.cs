using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Repository.Repositories;
using MentorBooking.Service.DTOs.Response;
using MentorBooking.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MentorBooking.Service.Services
{
    public class WorkSchedulesView : IWorkSchedulesView
    {
        private readonly IMentorWorkScheduleRepository _mentorWorkScheduleRepository;
        private readonly ISchedulesAvailableRepository _schedulesAvailableRepository;
        private readonly IMentorSupportSessionRepository _mentorSupportSessionRepository;
        private readonly IStudentGroupRepository _studentGroupRepository;
        public WorkSchedulesView(IMentorWorkScheduleRepository mentorWorkScheduleRepository, ISchedulesAvailableRepository schedulesAvailableRepository,
            IMentorSupportSessionRepository mentorSupportSessionRepository, IStudentGroupRepository studentGroupRepository)
        {
            _mentorWorkScheduleRepository = mentorWorkScheduleRepository;
            _schedulesAvailableRepository = schedulesAvailableRepository;
            _mentorSupportSessionRepository = mentorSupportSessionRepository;
            _studentGroupRepository = studentGroupRepository;
        }
        public List<ApiResponse> MentorWorkScheulesViews(Guid MentorId)
        {
            var existMentorSchedules = _schedulesAvailableRepository.GetAllSchedulesAvailable(MentorId);
            List<WorkScheulesViewResponse> ScheduleViews = new List<WorkScheulesViewResponse>();
            List<ApiResponse> results = new List<ApiResponse>();
            var response = new ApiResponse()
            {
                Status="Success",
                Message="No Schedules Now"
            };
            foreach(var item in existMentorSchedules)
            {
                var existWorkSchedule =_mentorWorkScheduleRepository.GetMentorWorkSchedulesView(item.ScheduleAvailableId);
                if(existWorkSchedule!=null && existWorkSchedule.UnavailableDate )
                {
                    var schdedule = new WorkScheulesViewResponse()
                    {
                        ScheduleId=existWorkSchedule.ScheduleId,
                        UnAvailableScheduleId=existWorkSchedule.ScheduleAvailableId,
                        SessionId=existWorkSchedule.SessionId,
                        IsActive=existWorkSchedule.UnavailableDate,
                        Date = item.FreeDay,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                    };
                    ScheduleViews.Add(schdedule);
                }    
            }
            if(ScheduleViews.Count > 0)
            {
                foreach(var item in ScheduleViews)
                {
                    var result = new ApiResponse()
                    {
                        Status = "Success",
                        Message = "Found",
                        Data = new WorkScheulesViewResponse()
                        {
                            ScheduleId = item.ScheduleId,
                            UnAvailableScheduleId = item.UnAvailableScheduleId,
                            SessionId = item.SessionId,
                            IsActive = item.IsActive,
                            Date = item.Date,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                        }
                    };
                    results.Add(result);
                }
                return results;
            }
            results.Add(response);
            return results;
        }

        public async Task<List<ApiResponse>> SchedulesForBooking(Guid MentorId)
        {
            List<WorkScheulesViewResponse> ScheduleViews = new List<WorkScheulesViewResponse>();
            var existMentorSchedules = _schedulesAvailableRepository.GetAllSchedulesAvailable(MentorId);
            List<ApiResponse>? results = new List<ApiResponse>();
            if(existMentorSchedules.Count()>0)
            {
                foreach (var item in existMentorSchedules)
                {
                    var existWorkSchedule = _mentorWorkScheduleRepository.GetMentorWorkSchedulesView(item.ScheduleAvailableId);
                    if (existWorkSchedule == null)
                    {
                        var schdedule = new WorkScheulesViewResponse()
                        {
                            UnAvailableScheduleId = item.ScheduleAvailableId,
                            Date = item.FreeDay,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                        };
                        ScheduleViews.Add(schdedule);
                    }
                }
            }    
            foreach (var item in ScheduleViews)
            {
                var result = new ApiResponse()
                {
                    Status = "Success",
                    Message = "Found",
                    Data = new WorkScheulesViewResponse()
                    {
                        UnAvailableScheduleId = item.UnAvailableScheduleId,
                        Date = item.Date,
                        StartTime = item.StartTime,
                        EndTime = item.EndTime,
                    }
                };
                results.Add(result);
            }
            return results;
        }

        public async Task<List<ApiResponse>> StudentWorkScheulesViews(Guid StudentId)
        {
            var listGroup = _studentGroupRepository.GetListStudentInGroup(StudentId);
            var listSchedule = new List<WorkScheulesViewResponse>();
            var listStudentWork = new List<MentorWorkSchedule>();
            var listSession = new List<MentorSupportSession>();
            var results = new List<ApiResponse>();
            if (listGroup != null)
            {
                foreach (var item in listGroup)
                {
                    var result = await _mentorSupportSessionRepository.GetMentorSupportSessionByGroupIdAsync(item.GroupId);
                    if (result != null)
                    {
                        listSession.Add(result);
                    }
                }
            }
            foreach(var item in listSession)
            {
                var result = _mentorWorkScheduleRepository.GetMentorWorkSchedule(item.SessionId);
                foreach(var workschedule in result)
                {
                    if(workschedule != null&& workschedule.UnavailableDate)
                    {
                        listStudentWork.Add(workschedule);
                    }    
                }    
            }
            foreach( var item in listStudentWork)
            {
                var existWorkSchedule = await _schedulesAvailableRepository.GetSchedulesAvailableAsync(item.ScheduleAvailableId);
                if(existWorkSchedule != null)
                {
                    var result = new WorkScheulesViewResponse()
                    {
                        ScheduleId = item.ScheduleId,
                        UnAvailableScheduleId = existWorkSchedule.ScheduleAvailableId,
                        SessionId = item.SessionId,
                        Date = existWorkSchedule.FreeDay,
                        StartTime = existWorkSchedule.StartTime,
                        EndTime = existWorkSchedule.EndTime,
                        IsActive = item.UnavailableDate
                    };
                    listSchedule.Add(result);
                }    
            }
            if(listSchedule.Count()>0)
            {
                foreach( var item in listSchedule)
                {
                    var result = new ApiResponse()
                    {
                        Status = "Success",
                        Message = "Found",
                        Data = new WorkScheulesViewResponse()
                        {
                            ScheduleId = item.ScheduleId,
                            UnAvailableScheduleId = item.UnAvailableScheduleId,
                            SessionId = item.SessionId,
                            Date = item.Date,
                            StartTime = item.StartTime,
                            EndTime = item.EndTime,
                            IsActive = item.IsActive
                        }
                    };
                    results.Add(result);
                }    
            }
            return results;
        }
    }
}
