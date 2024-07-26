namespace SportHub.Services.Data.CrawlerStorageDb.Interfaces;

using SportHub.Data.Models.DbEntities.Crawlers;

public interface IGroupsService
{
    Task AddOrUpdateGroupAsync(Group group);

    Task<Group> GetGroupAsync(Guid identifier);

    Task<Group> GetGroupAsync(int crawlerId, string name);

    Task AddGroupAsync(Group group);

    Task UpdateGroupAsync(Group newGroup, Group oldGroup);

    Task<IList<string>> GetGroupNamesAsync(int crawlerId);
}