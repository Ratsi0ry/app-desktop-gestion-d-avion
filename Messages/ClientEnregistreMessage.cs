using Gestion_avion.Models;

namespace Gestion_avion.Messages;

public class ClientEnregistreMessage
{
    public ClientModel Value { get; }
    public ClientEnregistreMessage(ClientModel value) => Value = value;
}