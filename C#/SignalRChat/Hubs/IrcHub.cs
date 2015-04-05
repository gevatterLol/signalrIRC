using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRChat.Classes;

namespace SignalRChat.Hubs
{
    public class IrcHub : Hub
    {


        public IrcHub()
        {
          
        }

        public void Send(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            //IrcFactory.GetIrc().irc.WriteLine("foo");
        }
    }
}