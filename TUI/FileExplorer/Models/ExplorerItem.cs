namespace FileExplorer.Models;

public class ExplorerItem
{
    public string Name { get; set; }
    public string Path { get; set; }
    public bool IsDirectory { get; set; }

    public long? Size { get; set; }
    public DateTime LastModified { get; set; }
    public DateTime Created { get; set; }
    
    public override string ToString()
    {
        return IsDirectory ? $"{Name}/" : Name;
    }
}