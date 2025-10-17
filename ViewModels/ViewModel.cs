using chatappTCP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
                    ContactName="Khanh",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/6.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                 },

                 new StatusDataModel
                 {
                    ContactName="Khoa",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/2.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                 },

                 new StatusDataModel
                 {
                    ContactName="Linh",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/3.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                 },
                 new StatusDataModel
                 {
                    ContactName="Kiet",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/4.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddStatus=false
                 },
                 new StatusDataModel
                 {
                    ContactName="Huy",
                    ContactPhoto=new Uri("/assets/ok.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
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
                    ContactName = "Khanh",
                    ContactPhoto = new Uri("/assets/6.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                    ChatIsSelected=true
                },
                new ChatListData
                {
                    ContactName = "Khoa",
                    ContactPhoto = new Uri("/assets/2.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                },
                new ChatListData
                {
                    ContactName = "Linh",
                    ContactPhoto = new Uri("/assets/3.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                },
                new ChatListData
                {
                    ContactName = "Kiet",
                    ContactPhoto = new Uri("/assets/4.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                },
                new ChatListData
                {
                    ContactName = "Huy",
                    ContactPhoto = new Uri("/assets/5.jpg",UriKind.RelativeOrAbsolute),
                    Message="Hello",
                    LastMessageTime="Tue, 12:58 PM",
                }
            };
            OnPropertyChanged();
        }
        #endregion
        #endregion
        public ViewModel()
        {
           LoadStatusThumbs();
            LoadChats();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
