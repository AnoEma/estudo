using Calculadora.Data.VO;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Calculadora.Service
{
    public interface IFileService
    {
        public byte[] GetFile(string filename);
        public Task<FileDetailsVO> SaveFileToDisk(IFormFile file);
        public Task<List<FileDetailsVO>> SaveFilesToDisk(IList<IFormFile> file);
    }
}