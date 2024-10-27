using MentorBooking.Repository.Data;
using MentorBooking.Repository.Interfaces;
using MentorBooking.Service.Interfaces;

namespace MentorBooking.Service.Services;

public class UserInformationFactory : IUserInformationFactory
{
    private readonly IMentorRepository _mentorRepository;
    private readonly ISkillRepository _skillRepository;
    private readonly IMentorSkillRepository _mentorSkillRepository;
    private readonly IUserPointRepository _userPointRepository;
    private readonly ApplicationDbContext _dbContext;
    private readonly IStudentRepository _studentRepository;

    public UserInformationFactory(IMentorRepository mentorRepository,
        ISkillRepository skillRepository, IMentorSkillRepository mentorSkillRepository,
        IUserPointRepository userPointRepository, ApplicationDbContext dbContext, IStudentRepository studentRepository)
    {
        _mentorRepository = mentorRepository;
        _skillRepository = skillRepository;
        _mentorSkillRepository = mentorSkillRepository;
        _userPointRepository = userPointRepository;
        _dbContext = dbContext;
        _studentRepository = studentRepository;
    }
    public IUserInformationUpdate CreateUserInformation(string role)
    {
        return role switch
        {
            "Mentor" => new MentorInfoUpdateService(_mentorRepository, _skillRepository, _mentorSkillRepository, _userPointRepository),
            "Student" => new StudentInfoUpdateService(_studentRepository, _userPointRepository),
            _ => throw new ArgumentException("Invalid role")
        };
    }
}