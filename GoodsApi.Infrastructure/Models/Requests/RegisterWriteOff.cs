namespace GoodsApi.Infrastructure.Models.Requests;

public class RegisterWriteOff
{
    public int ProductInfoId { get; set; }
    public int Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime WriteOffDate { get; set; }
}