using Feature.Api.Business;
using Feature.Api.Entities;
using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using System.Text.Json;

namespace Feature.Api.Hub
{
    public class CustomHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private AdivinaQuienBusiness _adivinaQuienBusiness { get; set; }
        JsonSerializerOptions serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public CustomHub(AdivinaQuienBusiness adivinaQuienBusiness)
        {
            _adivinaQuienBusiness = adivinaQuienBusiness;
        }
        public async Task<string> AddToGroup(Player player)
        {
            // Unir al canal de usuario, su nombre es el ID del Usuario
            if(string.IsNullOrEmpty(player.Code))
            {
                player.Code = Guid.NewGuid().ToString();
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, player.Code);
            var message = new Message()
            {
                Player = player,
                Text = $"{player.Name} a ingresado a la sala."
            };
            await SendMessage(message);
            return JsonSerializer.Serialize(player, serializeOptions);
        }
        public async Task SendMessage(Message message)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, message.Player.Code);

            await Clients.Group(message.Player.Code).SendAsync("recibirMensaje", message);
        }
        public void RemoveFromGroup(string groupName)
        {
            Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
        public async Task SendMessageToClient(string connectionId, string message)
        {
            await Clients.Client(connectionId).SendAsync("recibirMensaje", message);
        }
        public async Task SendMessageToGroup(string groupName, string message)
        {
            await Clients.Group(groupName).SendAsync("recibirMensaje", message);
        }
    }
}
