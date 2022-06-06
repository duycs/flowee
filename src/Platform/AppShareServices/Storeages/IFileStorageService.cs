using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppShareServices.Storeages
{
    public interface IFileStorageService
    {
        bool UploadStream(string key, Stream stream);
        Stream DownloadAsStream(string key);
        bool CopyFile(string fromPath, string toPath);
        bool MoveFile(string fromPath, string toPath);
        string GeneratePreSignedURL(string key, bool isDirectDownload);
        string GetPresignUrl(string key, bool isDirectDownload);
        string GetPublicUrl(string key, Dictionary<string, string> customParameters = null);
        bool IsExist(string path);
    }
}
