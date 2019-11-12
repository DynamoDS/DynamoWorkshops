using Dynamo.ViewModels;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidFire.UI  
{
    public class RapidFireVM : GalaSoft.MvvmLight.ViewModelBase
    {
        DynamoViewModel DynamoVM;
        RapidFire RapidFire;
        private List<ShortcutVM> _shortcuts;

        #region Bindable Properties

        //TODO Bindable Observable Collection to show only the shortcuts meeting some filter
        public ObservableCollection<ShortcutVM> FilteredShortcuts
        {
            get
            {
                return new ObservableCollection<ShortcutVM>(_shortcuts.Where(s=>s.NodeName.ToLower().Contains(SearchString.ToLower())));
            }
        }

        //TODO RelayCommand to invoke opening the github repo
        public RelayCommand OpenRepoCommand { get; set; }

        //TODO string  property to allow searching the code names
        private string _searchString = "";
        public string SearchString
        {
            get
            {
                return _searchString;
            }

            set
            {
                _searchString = value;
                RaisePropertyChanged("FilteredShortcuts");
            }
        }

        //TODO string property to display the current location of the saved shortcuts
        public string SavedFilePath
        {
            get
            {
                return RapidFire.SaveFilePath;
            }
        }

        #endregion

        public RapidFireVM(DynamoViewModel dynVM, RapidFire rapidFire)
        {
            DynamoVM = dynVM;
            RapidFire = rapidFire;

            //todo create the relay command
            OpenRepoCommand = new RelayCommand(OpenRepo);

            _shortcuts = InitializeShortcutList();
        }

        /// <summary>
        /// UsefulProperty for returning all of the shortcut Models from this View Model
        /// </summary>
        public IEnumerable<Shortcut> ShortcutModels
        {
            get
            {
                return _shortcuts.Select(s=>s.GetShortcut());
            }
        }

        /// <summary>
        /// Method for showing a link to the RapidFire AU2018 dynamo workshop github link in the default browser
        /// </summary>
        private void OpenRepo()
        {
            System.Diagnostics.Process.Start("https://github.com/DynamoDS/DeveloperWorkshop/tree/RapidFire/CBW227910%20-%20Build%20Custom%20User%20Interfaces%20and%20Interactions%20for%20Dynamo");
        }

        /// <summary>
        /// Using the DynamoViewModel to access all of the node names that exist and 
        /// the RapidFire Model's lilst of existing shortcuts create ShortCutVMs for each shortcut
        /// </summary>
        private List<ShortcutVM> InitializeShortcutList()
        {
            var newList = new List<ShortcutVM>();
            foreach (var nodeEntry in DynamoVM.SearchViewModel.Model.SearchEntries)
            {
                var foundShortcut = RapidFire.Shortcuts.FirstOrDefault(s => nodeEntry.CreationName == s.NodeName);

                ShortcutVM sCutVM = null;
                if(foundShortcut != null)
                {
                    sCutVM = new ShortcutVM(foundShortcut);
                }
                else
                {
                    sCutVM = new ShortcutVM(new Shortcut("", nodeEntry.CreationName));
                }

                newList.Add(sCutVM);
            }
            return newList;
        }


    }
}
