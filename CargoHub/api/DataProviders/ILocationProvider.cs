public interface ILocationProvider
{
    public List<Location> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Location[] Get();
    public void Add(Location client);
    public void Delete(int id);
}