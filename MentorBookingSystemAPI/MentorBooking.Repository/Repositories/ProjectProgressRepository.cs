using MentorBooking.Repository.Data;
using MentorBooking.Repository.Entities;
using MentorBooking.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MentorBooking.Repository.Repositories;

public class ProjectProgressRepository : IProjectProgressRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectProgressRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<bool> CreateProjectProgressAsync(ProjectProgress projectProgress)
    {
        try
        {
            _dbContext.ProjectProgresses.Add(projectProgress);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> UpdateProjectProgressAsync(ProjectProgress projectProgress)
    {
        try
        {
            var progressToUpdate = await _dbContext.ProjectProgresses.SingleOrDefaultAsync(pg => pg.ProgressId == projectProgress.ProgressId);
            if (progressToUpdate == null)
                return false;
            progressToUpdate.Description = projectProgress.Description;
            progressToUpdate.UpdateAt = projectProgress.UpdateAt;
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> DeleteProjectProgressAsync(ProjectProgress projectProgress)
    {
        try
        {
            _dbContext.ProjectProgresses.Remove(projectProgress);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<ProjectProgress?> GetProjectProgressAsync(Guid progressId)
    {
        try
        {
            var projectProgress = await _dbContext.ProjectProgresses.SingleOrDefaultAsync(pg => pg.ProgressId == progressId);
            if (projectProgress == null)
                return null;
            return projectProgress;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<List<ProjectProgress>?> GetAllProjectProgressAsync(Guid sessionId)
    {
        try
        {
            var projectProgresses = await _dbContext.ProjectProgresses.Where(pg => pg.SessionId == sessionId).ToListAsync();
            if (projectProgresses.Count == 0)
                return null;
            return projectProgresses;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<string> GetMentorIdProjectProgressesAsync(Guid sessionId)
    {
        var x = await _dbContext.ProjectProgresses.Include(x => x.MentorSupportSession)
            .FirstOrDefaultAsync(x => x.MentorSupportSession.SessionId == sessionId);
        return x!.MentorSupportSession.MentorId.ToString();
    }

    public Task<ProjectGroup> GetGroupAsync(Guid progressId)
    {
        var groupId = _dbContext.ProjectProgresses.Include(x => x.MentorSupportSession)
            .FirstOrDefault(x => x.ProgressId == progressId)?.MentorSupportSession.GroupId;
        if (groupId == null)
            return Task.FromResult<ProjectGroup>(null!);
        return _dbContext.ProjectGroups.FirstOrDefaultAsync(g => g.GroupId == groupId)!;
    }
}