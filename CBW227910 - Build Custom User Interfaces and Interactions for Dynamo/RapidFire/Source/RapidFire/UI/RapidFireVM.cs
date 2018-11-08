using Dynamo.ViewModels;
using GalaSoft.MvvmLight;
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

        public ObservableCollection<ShortcutVM> Shortcuts { get; set; }

        public RapidFireVM(DynamoViewModel dynVM, RapidFire rapidFire)
        {
            DynamoVM = dynVM;
            RapidFire = rapidFire;

            InitializeShortcutList(); 
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
