﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogiPark.Core;

namespace LogiPark.MVVM.ViewModel
{


    class ParkViewModel : ObservableObject
    {
        public ParkViewModel ParkVm { get; set; }


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

        public ParkViewModel()
        {
            CurrentView = new LogiPark.MVVM.View.ParkViewPage();
        }
    }
}
