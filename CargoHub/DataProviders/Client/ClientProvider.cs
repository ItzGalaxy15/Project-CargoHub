public class ClientProvider : BaseProvider<Client>, IClientProvider
{
    public ClientProvider(List<Client> mockData) : base(mockData) { }
    public ClientProvider() : base("test_data/clients.json") {}

    public Client[] Get(){
        return context.ToArray();
    }

    public void Add(Client client){
        context.Add(client);
    }

    public void Delete(Client client){
        context.Remove(client);
    }

    public void Update(Client client, int clientId)
    {
        client.Id = clientId;
        int index = context.FindIndex(c => c.Id == clientId);
        context[index] = client;
    }
}
