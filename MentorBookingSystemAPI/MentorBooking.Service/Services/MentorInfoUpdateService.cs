using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.DTOs.Request;
using MentorBooking.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Service.Services;

public class MentorInfoUpdateService : IUserInformationUpdate
{
    private readonly IMentorRepository _mentorRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IMentorSkillRepository _mentorSkillRepository;
    private readonly IUserPointRepository _userPointRepository;
    private const int ReturnAddSkillFailed = -1;
    private const int PointDefaultForMentor = 0;

    public MentorInfoUpdateService(IMentorRepository mentorRepository,
        ISkillRepository skillRepository, IMentorSkillRepository mentorSkillRepository,
        IUserPointRepository userPointRepository)
    {
        _mentorRepository = mentorRepository;
        _skillRepository = skillRepository;
        _mentorSkillRepository = mentorSkillRepository;
        _userPointRepository = userPointRepository;
    }

    public async Task<bool> UpdateInformationUser(Users user, object request)
    {
        MentorInformationModelRequest? mentorInfor = request as MentorInformationModelRequest;

        user.FirstName = mentorInfor?.FirstName;
        user.LastName = mentorInfor?.LastName;
        user.PhoneNumber = mentorInfor?.Phone;

        var mentor = new Mentor()
        {
            MentorId = user.Id,
            UserId = user.Id,
            ExperienceYears = (byte)mentorInfor.ExperienceYears,
            MentorDescription = mentorInfor.MentorDescription,
            CreateAt = DateTime.Now
        };
        if (!await _mentorRepository.AddInformationMentorAsync(mentor))
        {
            return false;
        }

        List<MentorSkill> mentorSkills = new List<MentorSkill>();
        foreach (var item in mentorInfor.Skills!)
        {
            // try
            // {
            //     var existingSkill = _dbContext.Skills.FirstOrDefault(x => x.Name == item);
            //     if (existingSkill == null)
            //     {
            //         existingSkill = new Skill()
            //         {
            //             Name = item
            //         };
            //         await _dbContext.Skills.AddAsync(existingSkill);
            //         await _dbContext.SaveChangesAsync();
            //     }
            //     mentorSkills.Add(new MentorSkill()
            //     {
            //         MentorId = mentor.MentorId,
            //         SkillId = existingSkill.SkillId
            //     });
            // }
            // catch (Exception ex)
            // {
            //     Console.WriteLine($"Error: {ex.Message}");
            //     return false;
            // }
            var addSkillResponse = await _skillRepository.AddSkillAsync(item);
            if (addSkillResponse == ReturnAddSkillFailed)
            {
                return false;
            }

            mentorSkills.Add(new MentorSkill()
            {
                MentorId = mentor.MentorId,
                SkillId = addSkillResponse
            });
        }

        foreach (var mentorSkill in mentorSkills)
        {
            // var existingMentorSkill = await _dbContext.MentorSkills.FirstOrDefaultAsync(x => x.MentorId == mentorSkill.MentorId && x.SkillId == mentorSkill.SkillId);
            // if (existingMentorSkill == null)
            // {
            //     await _dbContext.MentorSkills.AddAsync(mentorSkill);
            //     await _dbContext.SaveChangesAsync();
            // }
            var addMentorSkillResponse = await _mentorSkillRepository.AddMentorSkillAsync(mentorSkill);
            if (!addMentorSkillResponse) return false;
        }

        // UserPoint initPoint = new UserPoint()
        // {
        //     UserId = user.Id,
        //     PointsBalance = 0
        // };
        // if (_dbContext.UserPoints.FirstOrDefault(x => x.UserId == user.Id) == null)
        // {
        //     await _dbContext.UserPoints.AddAsync(initPoint);
        //     await _dbContext.SaveChangesAsync();
        // }
        if (!await _userPointRepository.SetUserPoint(mentor.MentorId, PointDefaultForMentor))
            return false;
        return true;
    }
}