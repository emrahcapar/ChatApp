using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppRecord.ViewModel
{
    public class ChatPageViewModel : BaseViewModel
    {
        ObservableCollection<MessageModel> messageList = new ObservableCollection<MessageModel>();

        public SignalRClient SignalRClient = new SignalRClient("http://xamarinchatapp.azurewebsites.net/");


        public ObservableCollection<MessageModel> MessageList
        {
            get { return messageList; }
            set
            {
                messageList = value;
                OnPropertyChanged();
            }
        }



        private Command _loadItemsCommand;

        public Command LoadItemsCommand
        {
            get
            {
                return _loadItemsCommand ?? (_loadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand()));
            }
        }

        private async Task ExecuteLoadItemsCommand()

        {
            await SignalRClient.Start();

            Device.StartTimer(TimeSpan.FromSeconds(10), () =>
            {
                if (!SignalRClient.IsConnectedOrConnecting)
                    SignalRClient.Start();

                return true;
            });
            SignalRClient.OnMessageReceived += (username, message) =>
            {
                MessageList.Insert(0, new AppRecord.MessageModel { UserName = username, Text = message });
            };

        }





        public void SentMessage(string userName, string userMsg)

        {
            SignalRClient.SendMessage(userName, userMsg);
        }
    }
}
