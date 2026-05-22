namespace RESTAPI.DTOs;

public class PcComponentResponseDto
{
    public string ComponentCode { get; set; } = string.Empty;
    public string ComponentName { get; set; } = string.Empty;
    public int Amount { get; set; }
}