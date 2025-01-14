public class ClientProvider : BaseProvider<Client>, IClientProvider
{
    public ClientProvider(List<Client> mockData)
        : base(mockData)
    {
    }

    public ClientProvider()
        : base("test_data/clients.json")
    {
    }

    public Client[] Get()
    {
        return this.context.ToArray();
    }

    public void Add(Client client)
    {
        this.context.Add(client);
    }

    public void Delete(Client client)
    {
        client.IsDeleted = true;
        client.UpdatedAt = client.GetTimeStamp();
    }

    public void Update(Client client, int clientId)
    {
        client.Id = clientId;
        int index = this.context.FindIndex(c => c.Id == clientId);
        this.context[index] = client;
    }
}
