using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.Strategies;

public interface IDocumentGenerationStrategy
{
    public DocumentType DocumentType { get; }
    public string TemplatePath { get; }
    public bool Generate(List<string> fieldValues, string outputPath);
}