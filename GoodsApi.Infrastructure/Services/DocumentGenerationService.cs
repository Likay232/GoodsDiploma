using GoodsApi.Infrastructure.Models.Enums;
using GoodsApi.Infrastructure.Models.Strategies;

namespace GoodsApi.Infrastructure.Services;

public class DocumentGenerationService(IEnumerable<IDocumentGenerationStrategy> strategies, string contentRootPath)
{
    private Dictionary<DocumentType, IDocumentGenerationStrategy> Strategies => strategies.ToDictionary(s => s.DocumentType, s => s);
    
    public async Task<string?> GenerateDoc(DocumentType documentType, Dictionary<string, string> fieldValues)
    {
        if (!Strategies.TryGetValue(documentType, out var strategy))
            return null;

        var outputName = GenerateOutputFile();
        
        if (strategy.Generate(fieldValues, outputName)) return outputName;
        
        return null;
    }

    private string GenerateOutputFile()
    {
        var projectRoot = Directory.GetParent(AppContext.BaseDirectory)?.Parent?.Parent?.Parent?.FullName;

        if (projectRoot == null) throw new Exception("File storage not found.");
        
        var filesDirectory = Path.Combine(projectRoot, "Files");
        
        if (!Directory.Exists(filesDirectory))
        {
            Directory.CreateDirectory(filesDirectory);
        }

        string fileName = $"Документ от {DateTime.Now:yyyy-MM-dd HH-mm-ss}.docx";
    
        string fullPath = Path.Combine(filesDirectory, fileName);
    
        if (File.Exists(fullPath))
        {
            string uniqueFileName = $"Документ от {DateTime.Now:yyyy-MM-dd HH-mm-ss}-{Guid.NewGuid():N}.docx";
            fullPath = Path.Combine(projectRoot, uniqueFileName);
        }
        
        return fullPath;
    }
}