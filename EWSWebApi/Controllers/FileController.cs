using ApplicationEF.Services;
using Microsoft.AspNetCore.Mvc;

namespace EWSWebApi.Controllers;

[ApiController]
[Route("file")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;
    public FileController(FileService fileService)
    {
        _fileService = fileService;
    }
    
    
}