using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiServer_Example.Domains.DTO;
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
        private readonly IMapper _mapper;

        public FileController(IFileRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository ?? throw new ArgumentNullException(nameof(fileRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IEnumerable<FileDto>>ListAllFiles(Guid userId)
        {
            var files = await _fileRepository.GetAllUserFiles(userId);

            return _mapper.Map<IEnumerable<FileDto>>(files);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewFile(FileCreateDto fileCreateDto)
        {
            throw new NotImplementedException();
        }
    }
}