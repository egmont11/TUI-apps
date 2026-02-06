using FileExplorer.Models;
using Terminal.Gui.Views;

namespace FileExplorer;

public class FileView
{
    private readonly Window _window;
    private string _currentPath;
    public string CurrentPath => _currentPath;
    private List<ExplorerItem> _directoryContent;

    public FileView(Window window)
    {
        _window = window;
        _currentPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
    }
    
    public void Show()
    {
        var line = new Line()
        {
            X = 0, Y = 1
        };
        var pathLabel = new Label()
        {
            Text = $"Path: {_currentPath}",
            X = 0, Y = 0
        };
        
        Refresh();
        var lineCount = _directoryContent.Count;
        
        for (int i = 0; i < lineCount; i++)
        {
            var name = _directoryContent[i].IsDirectory ? $"{_directoryContent[i].Name}/" : _directoryContent[i].Name;
            var sizeString = _directoryContent[i].Size is null ? "" : $"{_directoryContent[i].Size} bytes";
            var showItem = new Label()
            {
                Text = $"{name} {sizeString}",
                X = 0, Y = i + 2
            };
            
            _window.Add(showItem);
        }
        
        _window.Add(line);
        _window.Add(pathLabel);
    }

    private void Refresh()
    {
        _directoryContent = GetDirectoryContent(CurrentPath);
    }
    
    public void GoToParentDirectory()
    {
        var parent = Directory.GetParent(_currentPath);
        if (parent is null) return;
        
        _currentPath = parent.FullName;
        Refresh();
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
                Path = Path.Combine(path, file.Name),
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
                Path = Path.Combine(path, file.Name),
                IsDirectory = true,
                Size = null,
                LastModified = file.LastWriteTime,
                Created = file.CreationTime
            }
        ));

        return directoryContent;
    }
}