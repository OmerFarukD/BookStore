using Entities.LinkModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "v1")]
public class RootController :ControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public RootController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [HttpGet("getroot",Name="GetRoot")]
    public async Task<IActionResult> GetRoot([FromHeader(Name = "Accept")]string mediatype)
    {
        if (mediatype.Contains("application/vnd.aib.apiroot"))
        {
            var list = new List<Link>()
            {
                new Link()
                {
                    Href = _linkGenerator.GetUriByName(HttpContext,nameof(GetRoot),new {}),
                    Rel = "_self",
                    Method = "GET"
                },
                new Link()
                {
                    Href = _linkGenerator.GetUriByName(HttpContext,nameof(BooksController.GetAll),new {}),
                    Rel = "_self",
                    Method = "GET"
                },
                new Link()
                {
                    Href = _linkGenerator.GetUriByName(HttpContext,nameof(BooksController.CreateOneBook),new {}),
                    Rel = "_self",
                    Method = "POST"
                }
            };
            
            return Ok(list);
        }
        return NoContent();
    }
}