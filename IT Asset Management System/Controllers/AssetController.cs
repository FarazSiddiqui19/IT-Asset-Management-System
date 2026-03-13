using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IT_Asset_Management_System.Services.Interfaces;
using IT_Asset_Management_System.DTOs.Asset;
using IT_Asset_Management_System.Common;

namespace IT_Asset_Management_System.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AssetController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpPost("List")]
        public async Task<IActionResult> GetAll([FromBody] AssetFilter filter)
        {
            var assets = await _assetService.GetAllAsync(filter);
            return Ok(assets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var asset = await _assetService.GetByIdAsync(id);
            return Ok(asset);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAssetDto dto)
        {
            var asset = await _assetService.AddAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = asset.AssetId }, asset);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAssetDto dto)
        {
            await _assetService.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _assetService.DeleteAsync(id);
            return NoContent();
        }
    }
}
