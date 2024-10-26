public class ClientProvider : BaseProvider<Client>, IClientProvider
{
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

    public bool Update(Client client, int clientId)
    {
        int index = context.FindIndex(c => c.Id == clientId);
        if (index == -1) return false;
        context[index] = client;
        return true;
    }
}
