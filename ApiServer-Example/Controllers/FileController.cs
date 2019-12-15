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
    [Authorize]
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
        public async Task<IEnumerable<FileDto>>ListAllFiles()
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var clientId = authorizationHeader.GetClientId();

            var files = await _fileRepository.GetAllUserFiles(Guid.Parse(clientId));

            return _mapper.Map<IEnumerable<FileDto>>(files);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewFile([FromForm]FileCreateDto fileCreateDto)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var clientId = authorizationHeader.GetClientId();

            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(clientId));
            var userPath =  Path.Combine(Directory.GetCurrentDirectory(), user.Id.ToString());
            var filePath = Path.Combine(userPath,fileCreateDto.FileName + fileCreateDto.FileType);

            await using (var stream = System.IO.File.Create(filePath))
            {
                await fileCreateDto.FormFile.CopyToAsync(stream);
            }

            if (!System.IO.File.Exists(filePath))
                return BadRequest(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status400BadRequest.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "IOError",new[]{"Błąd podczas zapisywania pliku na serwerze!"}}
                    }
                });

            var fileModel = _mapper.Map<FileModel>(fileCreateDto);
            fileModel.UserId = Guid.Parse(clientId);

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

            return Ok(_mapper.Map<FileDto>(createdFileModel));
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();
            var clientId = authorizationHeader.GetClientId();

            var user = await _userRepository.GetUserByIdAsync(Guid.Parse(clientId));
            var file = await _fileRepository.GetFileByIdAsync(Guid.Parse(fileId));

            if(file.UserId != user.Id)
                return Unauthorized(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status401Unauthorized.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "Unauthorized",new[]{"Nie jesteś wlascicielem tego pliku! Nie mozesz go usunąć!"}}
                    }
                });

            if(!await _fileRepository.DeleteFileAsync(Guid.Parse(fileId)))
                return StatusCode(StatusCodes.Status500InternalServerError,new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status500InternalServerError.ToString(),
                    Errors = new Dictionary<string, string[]>
                    {
                        { "ServerError",new[]{"Błąd podczas usuwania pliku z serwera!"}}
                    }
                });

            var userPath =  Path.Combine(Directory.GetCurrentDirectory(), user.Id.ToString());
            var filePath = Path.Combine(userPath,file.FileName + file.FileType);

            if (!System.IO.File.Exists(filePath))
                return BadRequest(new ValidationErrorDto
                {
                    TraceId = HttpContext.TraceIdentifier,
                    Status = StatusCodes.Status400BadRequest.ToString(),
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