using chatappTCP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace chatappTCP.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region MainWindow

        #region Properties
        public string ContactName { get; set; }
        public Uri ContactPhoto { get; set; }
        public string LastSeen { get; set; }
        #endregion

        #endregion

        #region Status Thumbs
        #region Properties

        public ObservableCollection<StatusDataModel> statusThumbsCollection { get; set; }
        #endregion
        #region Logics
        void LoadStatusThumbs()
        {
            statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            {
                new StatusDataModel
                {
                    IsMeAddStatus=true
                },

                 new StatusDataModel
                 {
                    ContactName="Mike",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/6.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                 },

                 new StatusDataModel
                 {
                    ContactName="Steve",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/2.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                 },

                 new StatusDataModel
                 {
                    ContactName="Will",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                 },
                 
            };
            OnPropertyChanged("statusThumbsCollection");
        }
        #endregion

        #endregion

        #region Chat List
        #region Properties
        public ObservableCollection<ChatListData> Chats { get; set; }
        #endregion

        #region Logics
        void LoadChats()
        {
            Chats = new ObservableCollection<ChatListData>()
            {
                new ChatListData
                {
                    ContactName = "Mike",
                    ContactPhoto = new Uri("/assets/6.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                    ChatIsSelected=true
                },
                new ChatListData
                {
                    ContactName = "Steve",
                    ContactPhoto = new Uri("/assets/2.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                },
                new ChatListData
                {
                    ContactName = "Will",
                    ContactPhoto = new Uri("/assets/3.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                },
                new ChatListData
                {
                    ContactName = "John",
                    ContactPhoto = new Uri("/assets/4.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                },
            };
            OnPropertyChanged();
        }
        #endregion
        #endregion

        #region Conversations
        #region Properties
        public ObservableCollection<ChatConversation> Conversations;
        #endregion

        #region Logics
        void LoadChatConversation()
        {
            
        }
        #endregion
        #endregion

        public ViewModel()
        {
           LoadStatusThumbs();
           LoadChats();
           LoadChatConversation();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
