namespace MentorBooking.Service.Interfaces;

public interface IUserInformationFactory
{
    IUserInformationUpdate CreateUserInformation(string role);
}