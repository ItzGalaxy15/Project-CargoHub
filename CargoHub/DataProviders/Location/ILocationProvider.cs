public interface ILocationProvider
{
    public List<Location> context { get; set; }
    public string? path { get; set; }
    public Task Save();
    public Location[] Get();
    public void Add(Location location);
    public void Delete(Location location);
    public void Update(Location location, int locationId);

}