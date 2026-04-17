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

    public bool Generate(List<string> fieldValues, string outputPath)
    {
        if (fieldValues.Count != FieldValues.Count)
            return false;

        var keys = FieldValues.Keys.ToList();
        for (int i = 0; i < fieldValues.Count; i++)
        {
            FieldValues[keys[i]] = fieldValues[i];
        }
        
        var engine = new Engine();
        var errors = engine.Merge(TemplatePath, FieldValues, outputPath);

        foreach (var error in errors)
        {
            Console.WriteLine(error.Accept(new ErrorToRussianString()));
        }
        
        return true;
    }}