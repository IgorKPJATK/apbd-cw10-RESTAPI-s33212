using RESTAPI.DTOs;
using RESTAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace RESTAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PcsController : ControllerBase
{
    private readonly IPcService _pcService;

    public PcsController(IPcService pcService)
    {
        _pcService = pcService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPcs()
    {
        var pcs = await _pcService.GetAllPcsAsync();
        return Ok(pcs);
    }

    [HttpGet("{id}/components")]
    public async Task<IActionResult> GetPcComponents(int id)
    {
        var components = await _pcService.GetPcComponentsAsync(id);
        if (components == null)
        {
            return NotFound();
        }
        return Ok(components);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePc([FromBody] PcRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdPc = await _pcService.CreatePcAsync(requestDto);
        return Created($"/api/pcs/{createdPc.Id}", createdPc);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePc(int id, [FromBody] PcRequestDto requestDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updated = await _pcService.UpdatePcAsync(id, requestDto);
        if (!updated)
        {
            return NotFound();
        }

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePc(int id)
    {
        var deleted = await _pcService.DeletePcAsync(id);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}