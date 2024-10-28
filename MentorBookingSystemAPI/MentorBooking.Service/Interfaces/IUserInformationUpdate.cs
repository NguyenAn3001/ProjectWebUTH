using MentorBooking.Repository.Entities;

namespace MentorBooking.Service.Interfaces;

public interface IUserInformationUpdate
{
    Task<bool> UpdateInformationUser(Users user, object request);
}