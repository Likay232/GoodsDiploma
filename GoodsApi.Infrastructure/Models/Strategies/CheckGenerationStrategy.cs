using EasyDox;
using GoodsApi.Infrastructure.Models.Enums;

namespace GoodsApi.Infrastructure.Models.Strategies;

public class CheckGenerationStrategy(string rootPath) : IDocumentGenerationStrategy
{
    public DocumentType DocumentType { get; } = DocumentType.Check;

    public string TemplatePath { get; set; } = Path.Combine(Directory.GetParent(rootPath)?.FullName!, "GoodsApi.Infrastructure/Templates/CheckTemplate.docx");

    private Dictionary<string, string> FieldValues { get; set; } =
        new()
        {
            { "check-", "" },
            { "date", "" },
            { "product-name", "" },
            { "amount", "" },
            { "price-for-unit", "" },
            { "total-sum", "" },
            { "rubles", "" },
            { "pennies", "" },
            { "manager-name", "" },
        };

    public bool Generate(Dictionary<string, string> fieldValues, string outputPath)
    {
        var engine = new Engine();
        var errors = engine.Merge(TemplatePath, fieldValues, outputPath);

        foreach (var error in errors)
        {
            Console.WriteLine(error.Accept(new ErrorToRussianString()));
        }
        
        return true;
    }}