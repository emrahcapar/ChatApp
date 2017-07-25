using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppRecord
{
    public class SignalRClient : INotifyPropertyChanged
    {
        private HubConnection Connection;
        private IHubProxy ChatHubProxy;

        public delegate void MessageReceived(string username, string message);
        public event MessageReceived OnMessageReceived;

        public SignalRClient(string url)
        {
            Connection = new HubConnection(url);

            Connection.StateChanged += (StateChange obj) =>
            {
                OnPropertyChanged("ConnectionState");
            };

            ChatHubProxy = Connection.CreateHubProxy("ChatHub");
            ChatHubProxy.On<string, string>("SendMessage", (username, text) =>
            {
                OnMessageReceived?.Invoke(username, text);
            });
        }

        public void SendMessage(string username, string text)
        {
            ChatHubProxy.Invoke("Send", username, text);
        }

        public Task Start()
        {
            return Connection.Start();
        }

        public bool IsConnectedOrConnecting
        {
            get
            {
                return Connection.State != ConnectionState.Disconnected;
            }
        }

        public ConnectionState ConnectionState { get { return Connection.State; } }

        public static async Task<SignalRClient> CreateAndStart(string url)
        {
            var client = new SignalRClient(url);
            await client.Start();
            return client;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
