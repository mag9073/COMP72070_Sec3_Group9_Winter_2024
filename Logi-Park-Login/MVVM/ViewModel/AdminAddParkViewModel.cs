﻿using LogiPark.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogiPark.MVVM.ViewModel
{
    public class AdminAddParkViewModel : ObservableObject
    {
        public AdminAddParkViewModel ParkVm { get; set; }


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

        public AdminAddParkViewModel()
        {
            CurrentView = new LogiPark.MVVM.View.AdminAddParkViewPage();
        }
    }
}
