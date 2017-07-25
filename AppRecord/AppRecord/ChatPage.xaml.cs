using AppRecord.ViewModel;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace AppRecord
{
    public partial class ChatPage : ContentPage
    {
        private ChatPageViewModel ViewModel
        {
            get { return BindingContext as ChatPageViewModel; }
        }
        public ChatPage()
        {
            InitializeComponent();
            BindingContext = new ChatPageViewModel();
            btnSendMessage.Clicked += BtnSendMessage_Clicked;
            ViewModel.LoadItemsCommand.Execute(null); 
        }
        
        private void BtnSendMessage_Clicked(object sender, EventArgs e)
        {
            ViewModel.SentMessage(txtUserName.Text, txtUserMsg.Text);
 
        }
    }
}
