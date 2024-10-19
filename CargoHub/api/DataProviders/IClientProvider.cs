public interface IClientProvider
{
    public List<Client> context { get; set; }
    public string path { get; set; }
    public Task Save();
    public Client[] Get();
    public void Add(Client client);
}
