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

        //Bindable properties of ViewModel
        public ObservableCollection<ShortcutVM> Shortcuts { get; set; }
        public RelayCommand OpenRepoCommand { get; set; }
        public string SavedFilePath
        {
            get
            {
                return RapidFire.SaveFilePath;
            }
        }

        public RapidFireVM(DynamoViewModel dynVM, RapidFire rapidFire)
        {
            DynamoVM = dynVM;
            RapidFire = rapidFire;

            OpenRepoCommand = new RelayCommand(OpenRepo);

            InitializeShortcutList(); 
        }

        private void OpenRepo()
        {
            System.Diagnostics.Process.Start("https://github.com/DynamoDS/DeveloperWorkshop/tree/RapidFire/CBW227910%20-%20Build%20Custom%20User%20Interfaces%20and%20Interactions%20for%20Dynamo");
        }

        /// <summary>
        /// Using the DynamoViewModel to access all of the node names that exist and 
        /// the RapidFire Model's lilst of existing shortcuts create ShortCutVMs for each shortcut
        /// </summary>
        private void InitializeShortcutList()
        {
            var newList = new ObservableCollection<ShortcutVM>();
            foreach (var nodeEntry in DynamoVM.SearchViewModel.Model.SearchEntries)
            {
                var foundShortcut = RapidFire.Shortcuts.FirstOrDefault(s => nodeEntry.CreationName == s.NodeName);
                string keys = foundShortcut != null ? foundShortcut.Keys : "";

                Shortcut sCut = new Shortcut(keys, nodeEntry.CreationName);
                ShortcutVM sCutVM = new ShortcutVM(sCut);

                newList.Add(sCutVM);
            }
            Shortcuts = newList;
        }


    }
}
