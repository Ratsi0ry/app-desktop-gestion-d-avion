using CommunityToolkit.Mvvm.Messaging.Messages;
using Gestion_avion.Models;

namespace Gestion_avion.Messages;

// Ce message transportera le ClientModel sélectionné
public class DemandeModificationClientMessage : ValueChangedMessage<ClientModel>
{
    public DemandeModificationClientMessage(ClientModel client) : base(client) { }
}