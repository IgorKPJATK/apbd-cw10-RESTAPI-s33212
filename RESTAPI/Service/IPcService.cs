using RESTAPI.DTOs;

namespace RESTAPI.Services;

public interface IPcService
{
    Task<IEnumerable<PcResponseDto>> GetAllPcsAsync();
    Task<IEnumerable<PcComponentResponseDto>> GetPcComponentsAsync(int id);
    Task<PcResponseDto> CreatePcAsync(PcRequestDto dto);
    Task<bool> UpdatePcAsync(int id, PcRequestDto dto);
    Task<bool> DeletePcAsync(int id);
}