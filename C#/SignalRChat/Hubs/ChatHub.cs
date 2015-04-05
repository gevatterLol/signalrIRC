using System;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRChat.Classes;
using System.Threading;


namespace SignalRChat
{
    public class ChatHub : Hub
    {


        public ChatHub()
        {

        }

        public void foo()
        {
            foreach (var item in Implement.Logs)
            {
                Clients.All.addNewMessageToPage(DateTime.Now.ToString() + "    " + "Logs", item);
            }
        }


        public void WriteIrcLog(string name, string message)
        {

            Clients.All.addNewMessageToPage(DateTime.Now.ToString() + "    " + name, message);
        }


        public void Send(string name, string message)
        {
            Clients.All.addNewMessageToPage(DateTime.Now.ToString() + "    " + name, message);
            try
            {

                new Thread(delegate()
                {
                    Implement.SendMessage(message);
                }).Start();
                Implement.ChatHub = this;
            }
            catch (Exception ex)
            {             
                Clients.All.addNewMessageToPage( DateTime.Now.ToString() + "    " + name, ex.ToString());
            }
        }
    }
}