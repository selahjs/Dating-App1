using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize] //only authorized users should access our hub. 
    //also SinglaR/ Websockets can not send authentication header so we have to use query strings( in identity service ext..)
    public class PresenceHub : Hub  
    {
        private readonly PresenceTracker _tracker;
        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        //called when a new connection is stablished with a hub
        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(Context.User.GetUsername(),Context.ConnectionId); 

            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUsername());

            var currentUsers = _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _tracker.UserDisconnected(Context.User.GetUsername(), Context.ConnectionId);
            
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUsername());

            var currentUsers = _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);

            await base.OnDisconnectedAsync(exception);
        }
        
    }
}