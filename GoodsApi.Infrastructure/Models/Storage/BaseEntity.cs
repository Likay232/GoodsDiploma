using System.ComponentModel.DataAnnotations;

namespace GoodsApi.Infrastructure.Models.Storage;

public class BaseEntity : IEntity
{
    [Key]
    public int Id { get; set; }
}