using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogiPark.Core;

namespace LogiPark.MVVM.ViewModel
{


    class AdminParkEditViewModel : ObservableObject
    {
        public AdminParkEditViewModel ParkVm { get; set; }


        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public AdminParkEditViewModel()
        {
            CurrentView = new LogiPark.MVVM.View.ParkEditViewPage();
        }
    }
}
