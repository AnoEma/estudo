using Calculadora.Data.VO;
using Calculadora.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Calculadora.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class FileController : Controller
    {

        private readonly IFileService _service;

        public FileController(IFileService service)
        {
            _service = service;
        }

        [HttpGet("downloadFile/{fileName}")]
        [ProducesResponseType((200), Type = typeof(byte[]))]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]
        public async Task<IActionResult> GetFileAsync(string fileName)
        {
            byte[] buffer = _service.GetFile(fileName);

            if(buffer != null)
            {
                HttpContext.Response.ContentType = 
                    $"applicatioon/{Path.GetExtension(fileName).Replace(".", "")}";

                HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
                await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }

            return new ContentResult();
        }

        [HttpPost("uploadFile")]
        [ProducesResponseType((200), Type = typeof(FileDetailsVO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            FileDetailsVO detail = await _service.SaveFileToDisk(file);

            return new OkObjectResult(detail);
        }

        [HttpPost("uploadMultipleFile")]
        [ProducesResponseType((200), Type = typeof(List<FileDetailsVO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<IActionResult> UploadManyFile([FromForm] List<IFormFile> files)
        {
            List<FileDetailsVO> detail = await _service.SaveFilesToDisk(files);

            return new OkObjectResult(detail);
        }
    }
}