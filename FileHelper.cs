using System.IO;
using System.Threading.Tasks;

public static class FileHelper
{
    public static async Task<string> ReadFileAsync(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public static async Task WriteFileAsync(string filePath, string content)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            await writer.WriteAsync(content);
        }
    }
}
