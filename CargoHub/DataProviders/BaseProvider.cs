using System.Text.Json;


    public abstract class BaseProvider<T> 
    {
        public List<T> context { get; set; }
        public string? path { get; set; }

        public BaseProvider(List<T> initialContext)
        {
            context = initialContext;
            path = null;
        }

        public BaseProvider(string _path)
        {
            path = _path;
            context = JsonSerializer.Deserialize<List<T>>(File.ReadAllText(path)) ?? new List<T>();
        }

        public async Task Save()
        {
            if (!string.IsNullOrEmpty(path))
            {
                string json = JsonSerializer.Serialize(context);
                await File.WriteAllTextAsync(path, json);
            }
        }
    }
