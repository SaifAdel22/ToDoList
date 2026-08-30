using Microsoft.AspNetCore.Http;
using ToDoList.Helper.ToDoList.Services;

namespace ToDoList.Services
{
    public interface IFileUpload
    {
        string GenerateFileName(string fileName);
        string? GenerateFullPath(FileType fileType, string fileName);
        bool UploadFileLocally(string path, IFormFile file);
        bool DeleteFileLocally(string path);

        string? SaveFile(IFormFile? file, FileType fileType);
        string? UpdateFile(IFormFile? newFile, string? oldRelativePath, FileType fileType);
    }
}