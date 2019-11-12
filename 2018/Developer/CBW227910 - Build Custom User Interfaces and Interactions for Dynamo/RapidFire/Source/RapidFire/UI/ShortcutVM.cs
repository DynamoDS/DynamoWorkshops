using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RapidFire.UI
{
    public class ShortcutVM : ViewModelBase
    {
        private Shortcut _shortcut;

        public ShortcutVM(Shortcut shortcut)
        {
            _shortcut = shortcut;
        }

        public string NodeName
        {
            get
            {
                return _shortcut.NodeName;
            }
        }

        public string ShortcutKey
        {
            get
            {
                return _shortcut.Keys;
            }
            set
            {
                if(value.Length > 2)
                {
                    _shortcut.Keys = string.Join("", value.Substring(0,2)).ToUpper();
                }
                else
                {
                    _shortcut.Keys = value.ToUpper();

                }
                RaisePropertyChanged();
            }
        }

        public Shortcut GetShortcut()
        {
            return _shortcut;
        }
    }
}
