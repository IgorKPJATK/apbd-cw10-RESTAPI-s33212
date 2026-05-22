using RESTAPI.Data;
using RESTAPI.DTOs;
using RESTAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RESTAPI.Services;

public class PcService : IPcService
{
    private readonly AppDbContext _context;

    public PcService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PcResponseDto>> GetAllPcsAsync()
    {
        return await _context.PCs
            .Select(pc => new PcResponseDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            }).ToListAsync();
    }

    public async Task<IEnumerable<PcComponentResponseDto>> GetPcComponentsAsync(int id)
    {
        var pcExists = await _context.PCs.AnyAsync(p => p.Id == id);
        if (!pcExists) return null!;

        return await _context.PCComponents
            .Where(pcc => pcc.PCId == id)
            .Include(pcc => pcc.Component)
            .Select(pcc => new PcComponentResponseDto
            {
                ComponentCode = pcc.ComponentCode,
                ComponentName = pcc.Component.Name,
                Amount = pcc.Amount
            }).ToListAsync();
    }

    public async Task<PcResponseDto> CreatePcAsync(PcRequestDto dto)
    {
        var pc = new PC
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.PCs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcResponseDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task<bool> UpdatePcAsync(int id, PcRequestDto dto)
    {
        var pc = await _context.PCs.FirstOrDefaultAsync(p => p.Id == id);
        if (pc == null) return false;

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePcAsync(int id)
    {
        var pc = await _context.PCs.FirstOrDefaultAsync(p => p.Id == id);
        if (pc == null) return false;

        _context.PCs.Remove(pc);
        await _context.SaveChangesAsync();
        return true;
    }
}