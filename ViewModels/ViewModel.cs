using chatapp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chatapp.ViewModels
{
    public class ViewModel
    {
        public ObservableCollection<StatusDataModel> statusThumbsCollection;
        public ViewModel()
        {
            statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            {
                new StatusDataModel
                {
                    IsMeAddSatus=true
                },

                 new StatusDataModel
                 {
                    ContactName="Duy",
                    ContactPhoto=new Uri("/assets/1.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddSatus=false
                 },

                  new StatusDataModel
                 {
                    ContactName="Duy",
                    ContactPhoto=new Uri("/assets/1.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddSatus=false
                 },

                   new StatusDataModel
                 {
                    ContactName="Duy",
                    ContactPhoto=new Uri("/assets/1.png", UriKind.RelativeOrAbsolute),
                    StatusImage=new Uri("/assets/5.jpg", UriKind.RelativeOrAbsolute),
                    IsMeAddSatus=false
                 },
            };
        }
    }
}
