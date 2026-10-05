namespace Towl.Core.Utils;

public static class FileUtils
{
    public static void WriteFileSafe(string filePath, string text, string? backupFilePath = null, string? tempFilePath = null)
    {
        tempFilePath ??= filePath + "." + Guid.NewGuid().ToString() + ".tmp";
        File.WriteAllText(tempFilePath, text);

        try
        {
            if (File.Exists(filePath) && backupFilePath is not null)
                File.Replace(tempFilePath, filePath, backupFilePath);
            else
                File.Move(tempFilePath, filePath, overwrite: true);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            try
            {
                if (File.Exists(tempFilePath))
                    File.Delete(tempFilePath);
            }
            catch { /* Ignore */ }
        }
    }
}
