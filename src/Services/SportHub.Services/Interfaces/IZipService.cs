namespace SportHub.Services.Interfaces;

using SportHub.Data.Models.Entities.Crawlers;
using SportHub.Data.Models.Zip;

public interface IZipService
{
    byte[] ZipGroup(Group group);

    List<ZipModel> UnzipGroup(byte[] data);
}