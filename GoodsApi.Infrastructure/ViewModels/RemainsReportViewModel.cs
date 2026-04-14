using System.ComponentModel.DataAnnotations;
using GoodsApi.Infrastructure.Models.DTO;
using GoodsApi.Infrastructure.Models.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GoodsApi.Infrastructure.ViewModels;

public class RemainsReportViewModel
{
    [StringLength(100)]
    public string? Category { get; set; }
    [StringLength(100)]
    public string? Brand { get; set; }
    public RemainStatus? StatusFilter { get; set; }

    public List<SelectListItem> Categories { get; set; } = [];
    public List<SelectListItem> Brands { get; set; } = [];
    public List<SelectListItem> Statuses { get; set; } = [];

    public List<RemainsReportEntry> RemainReport { get; set; } = [];
    
}