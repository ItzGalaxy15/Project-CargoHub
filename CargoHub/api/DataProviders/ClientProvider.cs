public class ClientProvider : BaseProvider<Client>, IClientProvider
{
    public ClientProvider() : base("data/clients.json") {}
}
