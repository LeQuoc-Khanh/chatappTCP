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
        ObservableCollection<StatusDataModel> statusThumbsCollection;
        ViewModel()
        {
            statusThumbsCollection = new ObservableCollection<StatusDataModel>()
            {
                new StatusDataModel
                {
                    ContactName="",
                    ContactPhoto="",
                    StatusImage="",
                    IsMeAddSatus="",
                },
            };
        }
    }
}
