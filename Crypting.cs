using System;
using System.Threading;
using System.Threading.Tasks;

public class Cripting
{
    private static Random rand = new Random();

    public static (char, char) CriptareChar(char ch, int index)
    {
        char randomChar = (char)(rand.Next(128));
        char encryptedChar = (char)(ch ^ randomChar);
        char keyChar = (char)(randomChar ^ (128 - index));
        return (encryptedChar, keyChar);
    }

    public static char DecriptareChar(char encryptedChar, char keyChar, int index)
    {
        char randomChar = (char)(keyChar ^ (128 - index));
        return (char)(encryptedChar ^ randomChar);
    }

    public static async Task<string> CriptareAsync(string text, CancellationToken ct, IProgress<int> progress)
    {
        return await Task.Run(() =>
        {
            string textCriptat = "";
            string key = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }

                var (encryptedChar, keyChar) = Cripting.CriptareChar(text[i], i);
                textCriptat += encryptedChar;
                key += keyChar;

                int progressPercentage = (i + 1) * 100 / text.Length;
                progress?.Report(progressPercentage);

                Thread.Sleep(100); // Simulate work
            }
            return textCriptat + "|" + key; // Return both encrypted text and key
        }, ct);
    }

    public static async Task<string> DecriptareAsync(string text, string key, CancellationToken ct, IProgress<int> progress)
    {
        return await Task.Run(() =>
        {
            string textDecriptat = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (ct.IsCancellationRequested)
                {
                    ct.ThrowIfCancellationRequested();
                }

                char decryptedChar = Cripting.DecriptareChar(text[i], key[i], i);
                textDecriptat += decryptedChar;

                int progressPercentage = (i + 1) * 100 / text.Length;
                progress?.Report(progressPercentage);

                Thread.Sleep(100); // Simulate work
            }
            return textDecriptat;
        }, ct);
    }
}
