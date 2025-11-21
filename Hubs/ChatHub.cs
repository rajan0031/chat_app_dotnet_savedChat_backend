using ChatApp.Data;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatDbContext _context;

        public ChatHub(ChatDbContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            var chatMessage = new ChatMessage
            {
                UserName = user,
                MessageText = message,
                SentAt = DateTime.Now
            };

            _context.ChatMessages.Add(chatMessage);
            await _context.SaveChangesAsync();

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public override async Task OnConnectedAsync()
        {
            var messages = _context.ChatMessages
                .OrderBy(m => m.SentAt)
                .ToList();

            await Clients.Caller.SendAsync("LoadChatHistory", messages);

            await base.OnConnectedAsync();
        }
    }
}
