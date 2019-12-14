using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.Models;

namespace ApiServer_Example.Services
{
    public interface IFileRepository
    {
        Task<FileModel> CreateFileAsync(FileModel fileModel);

        Task<bool> DeleteFileAsync(Guid id);

        Task<IEnumerable<FileModel>> GetAllUserFiles(Guid userId);

        Task<FileModel> GetFileByIdAsync(Guid id);
    }
}
