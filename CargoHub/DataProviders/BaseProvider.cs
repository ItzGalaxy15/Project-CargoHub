using System.Text.Json;

public abstract class BaseProvider<T>
{
    public List<T> context { get; set; }

    public string? path { get; set; }

    public BaseProvider(List<T> initialContext)
    {
        this.context = initialContext;
        this.path = null;
    }

    public BaseProvider(string _path)
    {
        this.path = _path;
        this.context = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(this.path)) ?? new List<T>();
    }

    public async Task Save()
    {
        if (!string.IsNullOrEmpty(this.path))
        {
            string json = JsonSerializer.Serialize(this.context);
            await File.WriteAllTextAsync(this.path, json);
        }
    }
}
