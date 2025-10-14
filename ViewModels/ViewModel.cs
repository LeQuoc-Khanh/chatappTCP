using chatapp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace chatapp.ViewModels
{
    public class ViewModel : INotifyPropertyChanged
    {
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

        #endregion
        public ViewModel()
        {
           LoadStatusThumbs();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
