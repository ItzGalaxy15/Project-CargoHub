public interface IClientProvider
{
    public List<Client> context { get; set; }

    public string? path { get; set; }

    public Task Save();

    public Client[] Get();

    public void Add(Client client);

    public void Delete(Client client);

    public void Update(Client client, int clientId);
}
