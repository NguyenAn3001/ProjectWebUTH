// using MentorBooking.Repository.Data;
// using MentorBooking.Repository.Entities;
// using MentorBooking.Service.DTOs.Response;
// using MentorBooking.Service.Enum;
// using MentorBooking.Service.Interfaces;
//
// namespace MentorBooking.Service.Services
// {
//     public class MentorServices : IMentorServices
//     {
//         private readonly ApplicationDbContext _db;
//         public MentorServices(ApplicationDbContext mentorDbcontext)
//         {
//             _db = mentorDbcontext;
//         }
//         // private MentorSearchingResponse ConvertMentorToMentorSearchingResponse(Mentor mentor)
//         // {
//         //     MentorSearchingResponse searchMentorRespone = mentor.ToMentorSearchingResponse();
//         //     foreach (var skill in mentor.Skills)
//         //     {
//         //         foreach (var skillname in skill.Name)
//         //         {
//         //             searchMentorRespone.SkillName?.Add(skillname.ToString());
//         //         }
//         //     }
//         //     return searchMentorRespone;
//         // }
//         // public List<MentorSearchingResponse> GetAllMentors()
//         // {
//         //     return _db.Mentors.ToList()
//         //          .Select(temp => ConvertMentorToMentorSearchingResponse(temp)).ToList();
//         // }
//
//         public List<MentorSearchingResponse> GetMentorBySearchText(string? searchText)
//         {
//             List<MentorSearchingResponse> allMentors = GetAllMentors();
//             List<MentorSearchingResponse> matchingMentors = allMentors;
//             if (string.IsNullOrEmpty(searchText)) return matchingMentors;
//
//             matchingMentors = allMentors
//                 .Where(temp => (
//                     !string.IsNullOrEmpty(temp.FirstName) ?
//                     temp.FirstName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true) ||
//                     (!string.IsNullOrEmpty(temp.LastName) ?
//                     temp.LastName.Contains(searchText, StringComparison.OrdinalIgnoreCase) : true)).ToList();
//
//             return matchingMentors;
//         }
//
//         public List<MentorSearchingResponse> GetSortMentor(List<MentorSearchingResponse> allMentors, SortOptions sortType)
//         {
//             allMentors = GetAllMentors();
//             List<MentorSearchingResponse> sortMentor = (sortType) switch
//             {
//                 (SortOptions.ASC) => allMentors.OrderBy(temp => temp.FirstName, StringComparer.OrdinalIgnoreCase).ToList(),
//                 (SortOptions.DESC) => allMentors.OrderByDescending(temp => temp.FirstName, StringComparer.OrdinalIgnoreCase).ToList(),
//                 _ => allMentors
//             };
//             return sortMentor;
//         }
//     }
// }
