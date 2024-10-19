public class ClientProvider : BaseProvider<Client>, IClientProvider
{
    public ClientProvider() : base("data/clients.json") {}

    public Client[] Get(){
        return context.ToArray();
    }

    public void Add(Client client){
        context.Add(client);
    }

    public void Delete(int id){
        context.RemoveAt(id);
    }
}
