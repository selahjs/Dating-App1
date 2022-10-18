using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    //these methods track who is online and disconnected
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = 
            new Dictionary<string, List<string>>();

        public Task UserConnected(string username, string connectionId)
        {
            // bool isOnline = false;
            lock(OnlineUsers)
            {   // a single user can have multiple connection Ids, stating he has connected throgh different/multiple devices
                //if there is already a connection id created with the same user then, add the next connection id
                if(OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, new List<string>{ connectionId });
                    // isOnline = true;
                }
            }
            return Task.CompletedTask;
        }

        public Task UserDisconnected(string username, string connectionId)
        {
            // bool isOffline = false;
            lock(OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;
                
                //
                OnlineUsers[username].Remove(connectionId);

                //remove the hall element if there are no more connection id associated with the specific username
                if(OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                    // isOffline = true;
                }
            }
            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;

            lock(OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);  
        }
        
        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;
            lock(OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}