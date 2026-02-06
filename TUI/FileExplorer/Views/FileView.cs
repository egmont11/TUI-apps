using FileExplorer.Models;
using Terminal.Gui.Views;

namespace FileExplorer;

public class FileView
{
    private readonly Window _window;
    public string CurrentPath { get; }

    public FileView()
    {
        
    }
    
    public void Show()
    {
        
    }

    public void Refresh()
    {
        throw new NotImplementedException();
    }
    
    public void GoToParentDirectory()
    {
        throw new NotImplementedException();
    }
    
    public void GoToDirectory(string path)
    {
        throw new NotImplementedException();
    }
    
    public void OpenFile(string path)
    {
        throw new NotImplementedException();
    }

    private static List<ExplorerItem> GetDirectoryContent(string path)
    {
        List<ExplorerItem> directoryContent = [];
        var directory = new DirectoryInfo(path);

        directoryContent
            .AddRange(directory.GetFiles()
            .Select(file => new ExplorerItem()
            {
                Name = file.Name,
                Path = path + file.Name,
                IsDirectory = false,
                Size = file.Length,
                LastModified = file.LastWriteTime,
                Created = file.CreationTime
            }
        ));
        
        directoryContent
            .AddRange(directory.GetDirectories()
            .Select(file => new ExplorerItem()
            {
                Name = file.Name,
                Path = path + file.Name,
                IsDirectory = true,
                Size = null,
                LastModified = file.LastWriteTime,
                Created = file.CreationTime
            }
        ));

        return directoryContent;
    }
}