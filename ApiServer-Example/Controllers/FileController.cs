using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.DTO;
using ApiServer_Example.Domains.Models;
using ApiServer_Example.Helpers;
using ApiServer_Example.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer_Example.Controllers
{
    [Route("api/file")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileRepository _fileRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FileController(IFileRepository fileRepository,IUserRepository userRepository, IMapper mapper)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<FileDto>>ListAllFiles()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var clientId = authorizationHeader.GetClientId();

            var files = await _fileRepository.GetAllUserFiles(clientId);

            return _mapper.Map<IEnumerable<FileDto>>(files);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateNewFile([FromForm]FileCreateDto fileCreateDto)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var clientId = authorizationHeader.GetClientId();

            var user = await _userRepository.GetUserByIdAsync(clientId);
            var userPath =  Path.Combine(Directory.GetCurrentDirectory(), user.Id.ToString());
            var filePath = Path.Combine(userPath,fileCreateDto.FileName + fileCreateDto.FileType);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await fileCreateDto.FormFile.CopyToAsync(stream);
            }

            if (!System.IO.File.Exists(filePath))
                return StatusCode(StatusCodes.Status500InternalServerError, new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "IOError",new[]{"Błąd podczas zapisywania pliku na serwerze!"}}
                    }
                });

            FileDto result;
            if (await _fileRepository.CheckIfFileExist(fileCreateDto.FileName + fileCreateDto.FileType))
            {
                var file = await _fileRepository.GetFileByNameAsync(fileCreateDto.FileName + fileCreateDto.FileType);
                _mapper.Map(fileCreateDto, file);
                var updatedFileModel = await _fileRepository.UpdateFile(file);
                result = _mapper.Map<FileDto>(updatedFileModel);
                result.ShareLink = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/file/{result.Id}";
            }
            else
            {
                var fileModel = _mapper.Map<FileModel>(fileCreateDto);
                fileModel.UserId = clientId;

                var createdFileModel = await _fileRepository.CreateFileAsync(fileModel);
                if(createdFileModel == null)
                    return StatusCode(StatusCodes.Status500InternalServerError,new ValidationErrorDto
                    {
                        TraceId = HttpContext.TraceIdentifier,
                        Status = StatusCodes.Status500InternalServerError.ToString(),
                        Errors = new Dictionary<string, string[]>
                        {
                            { "ServerError",new[]{"Błąd podczas zapisywania pliku na serwerze!"}}
                        }
                    });

                result = _mapper.Map<FileDto>(createdFileModel);
                result.ShareLink = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/file/{result.Id}";   
            }

            return Ok(result);
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> DownloadFile(Guid fileId)
        {
            var file = await _fileRepository.GetFileByIdAsync(fileId);
            if(file == null)
                return NotFound(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "NotFound",new[]{"Plik nie istnieje!"}}
                    }
                });


            if (!file.IsShared)
            {
                var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
                var clientId = authorizationHeader.GetClientId();
                if(file.UserId != clientId)
                    return Unauthorized(new ValidationErrorDto
                    {
                        TraceId = HttpContext.TraceIdentifier,
                        Status = StatusCodes.Status401Unauthorized.ToString(),
                        Errors = new Dictionary<string, string[]>
                        {
                            { "Unauthorized",new[]{"Nie masz dostępu do żądanego pliku!"}}
                        }
                    });
            }

            var userPath =  Path.Combine(Directory.GetCurrentDirectory(), file.UserId.ToString());
            var filePath = Path.Combine(userPath,file.FileName + file.FileType);
            var stream = System.IO.File.OpenRead(filePath);
            if(stream == null)
                return NotFound(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status404NotFound.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "NotFound",new[]{"Żądany plik nie znajduje sie na serwerze!"}}
                    }
                });

            return File(stream, "application/octet-stream");
        }

        [HttpDelete("{fileId}")]
        [Authorize]
        public async Task<IActionResult> DeleteFile(Guid fileId)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var clientId = authorizationHeader.GetClientId();

            var file = await _fileRepository.GetFileByIdAsync(fileId);

            if(file.UserId != clientId)
                return Unauthorized(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status401Unauthorized.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Unauthorized",new[]{"Nie jesteś wlascicielem tego pliku! Nie mozesz go usunąć!"}}
                    }
                });

            if(!await _fileRepository.DeleteFileAsync(fileId))
                return StatusCode(StatusCodes.Status500InternalServerError,new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "ServerError",new[]{"Błąd podczas usuwania pliku z serwera!"}}
                    }
                });

            var userPath =  Path.Combine(Directory.GetCurrentDirectory(), clientId.ToString());
            var filePath = Path.Combine(userPath,file.FileName + file.FileType);

            if (!System.IO.File.Exists(filePath))
                return StatusCode(StatusCodes.Status500InternalServerError, new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "IOError",new[]{"Błąd podczas usuwania pliku z serwera!"}}
                    }
                });

            System.IO.File.Delete(filePath);

            return NoContent();
        }
    }
}