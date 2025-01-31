using System;
using System.IO;

public class TokenManager
{
    private readonly string tokenPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "token.txt");

    public void SaveToken(string token)
    {
        File.WriteAllText(tokenPath, token);
    }

    public string GetToken()
    {
        return File.Exists(tokenPath) ? File.ReadAllText(tokenPath) : null;
    }

    public void ClearToken()
    {
        if (File.Exists(tokenPath)) File.Delete(tokenPath);
    }
}
