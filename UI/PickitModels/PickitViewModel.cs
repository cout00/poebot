using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PoeItemObjectModelLib;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace UI.Parts {

    

    public class PickitFilterViewModel :ViewModelBase {

        public int Order { get; set; }
        public string Title { get; set; }


        private bool _isActive;

        public bool IsActive {
            get { return _isActive; }
            set {
                if (value != _isActive) {
                    _isActive = value;
                    OnPropertyChanged();
                }
            }
        }
    }
    
    public class PickitViewModel :ViewModelBase {

        public ObservableCollection<PickitFilterViewModel> PickitFilters { get; }
       
        public ICommand AddCommand { get; set; }

        public ICommand DownCommand { get; set; }
        public ICommand UpCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }       
        public ICommand DeleteCommand { get; set; }


        public PickitViewModel() {
            AddCommand = CreateCommand(CanAdd, DoAdd);
            DownCommand = CreateCommand<PickitFilterViewModel>(CanDown, DoDown);
            UpCommand = CreateCommand<PickitFilterViewModel>(CanUp, DoUp);
            SaveCommand = CreateCommand(CanSave, DoSave);
            CancelCommand = CreateCommand(CanCancel, DoCancel);
            DeleteCommand = CreateCommand<PickitFilterViewModel>(CanDelete, DoDelete);

            PickitFilters = new ObservableCollection<PickitFilterViewModel>();
            PickitFilters.Add(new PickitFilterViewModel());
            PickitFilters.Add(new PickitFilterViewModel());
            PickitFilters.Add(new PickitFilterViewModel());
        }

        bool CanDelete(PickitFilterViewModel model) {
            return PickitFilters.Count > 1;
        }

        bool CanDown(PickitFilterViewModel model) {
            return true;
        }

        bool CanUp(PickitFilterViewModel model) {
            return true;
        }
        
        bool CanSave() {
            return true;
        }

        bool CanCancel() {
            return true;
        }

        bool CanAdd() {
            return true;
        }

        public void DoAdd() {
            
        }

        public void DoCancel() {

        }

        public void DoSave() {

        }

        public void DoUp(PickitFilterViewModel model) {

        }

        public void DoDown(PickitFilterViewModel model) {

        }

        public void DoDelete(PickitFilterViewModel model) {
            PickitFilters.Remove(model);
        }

        
    }
}
