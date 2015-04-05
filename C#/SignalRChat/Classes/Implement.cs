using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Meebey.SmartIrc4net;

namespace SignalRChat.Classes
{

    public static class Implement
    {

        public static List<string> Logs = new List<string>();
        public static IrcClient Client = new IrcClient();
        public static ChatHub ChatHub = null;

        public static void Starter()
        {



            Client.OnRawMessage += new IrcEventHandler(OnRawMessage);
            Client.ActiveChannelSyncing = true;
            Client.SendDelay = 200;

            // the server we want to connect to, could be also a simple string
            string serverlist =  "irc.nerdlife.de" ;
            int port = 6667;
            string channel = "#rumkugel";
            try
            {
                // here we try to connect to the server and exceptions get handled
                Client.Connect(serverlist, port);
            }
            catch (ConnectionException e)
            {
                // something went wrong, the reason will be shown
                if (ChatHub != null)
                {
                    Logs.Add(e.Message);
                    ChatHub.Send(DateTime.Now + "   " +  "INTERNAL", "oh noes errorz! couldn't connect! Reason: " + e.Message);
                }

            }

            try
            {
                // here we logon and register our nickname and so on 
                Client.Login("docKatze", "doclolTestBot");
                // join the channel
                Client.RfcJoin(channel);

                //client.SendMessage(SendType.Message, channel, "test message (" + i.ToString() + ")");

                // spawn a new thread to read the stdin of the console, this we use
                // for reading IRC commands from the keyboard while the IRC connection
                // stays in its own thread


                //new Thread(new ThreadStart(ReadCommands)).Start();

                // here we tell the IRC API to go into a receive mode, all events
                // will be triggered by _this_ thread (main thread in this case)
                // Listen() blocks by default, you can also use ListenOnce() if you
                // need that does one IRC operation and then returns, so you need then 
                // an own loop 
                Logs.Add("Started");
                Client.SendMessage(SendType.Message, "#rumkugel", "i got started");
                Client.Listen();

                // when Listen() returns our IRC session is over, to be sure we call
                // disconnect manually
                //irc.Disconnect();
            }
            catch (ConnectionException)
            {
                // this exception is handled because Disconnect() can throw a not
                // connected exception

            }
            catch (Exception e)
            {
                // this should not happen by just in case we handle it nicely
                //System.Console.WriteLine("Error occurred! Message: "+e.Message);
                //System.Console.WriteLine("Exception: "+e.StackTrace);

            }



        }

        private static void OnRawMessage(object sender, IrcEventArgs e)
        {
            if (ChatHub != null && !string.IsNullOrEmpty(e.Data.Message) && !string.IsNullOrEmpty(e.Data.Nick) && e.Data.Type == ReceiveType.ChannelMessage)
            {
                Logs.Add(e.Data.Nick + "     " + e.Data.Message);
                ChatHub.WriteIrcLog(e.Data.Nick,e.Data.Message);
            }
        }


        public static void SendMessage(string message)
        {
            Logs.Add(Client.Nickname + "     " + message);
            Client.SendMessage(SendType.Message, "#rumkugel", message);
        }



    }


}