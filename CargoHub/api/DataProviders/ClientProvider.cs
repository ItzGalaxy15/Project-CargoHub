public class ClientProvider : BaseProvider<Client>, IClientProvider
{
    public ClientProvider() : base("data/clients.json") {}

    public Client[] Get(){
        return context.ToArray();
    }

    public async void Add(Client client){
        context.Add(client);
        //await Save();
    }
}
