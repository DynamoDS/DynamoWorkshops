using Dynamo.Controls;
using Dynamo.ViewModels;
using Dynamo.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Dynamo.Models.DynamoModel;

namespace RapidFire
{
    public class RapidFire 
    {
        public HashSet<Shortcut> Shortcuts = new HashSet<Shortcut> { new Shortcut("DT", "Date Time") };
        public DynamoView View;
        public string SaveFilePath = Serialization.ShortcutFilePath;

        public RapidFire(DynamoView view)
        {
            View = view;
            LoadShortcuts();
        }

        private void LoadShortcuts()
        {
            var stcts = Serialization.ReadFile(SaveFilePath);
            if (stcts != null)
            {
                Shortcuts = new HashSet<Shortcut>(stcts);
            }
        }

        internal void SaveShortCuts(IEnumerable<Shortcut> shortcutModels = null)
        {
            if(shortcutModels != null)
            {
                Shortcuts = new HashSet<Shortcut>(shortcutModels);
            }
            Serialization.WriteShortcutsToFile(SaveFilePath, Shortcuts.ToList());
            LoadShortcuts();
        }

        static char? lastChar = null;

        /// <summary>
        /// if a key is lifted, we can forget any 
        /// previously stored key.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void View_KeyUp(object sender, KeyEventArgs e)
        {
            lastChar = null;
        }

        /// <summary>
        /// On Key down, one of two things will happen: 
        /// 1) This is the first key being held down and we store it
        /// 2) This is the second key being pressed, and we initiate 
        ///    adding a node to the graph
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void View_KeyDown(object sender, KeyEventArgs e)
        {
            if (lastChar == null)
            {
                char[] chars = e.Key.ToString().ToCharArray();
                if (chars.Length == 1)
                {
                    lastChar = chars[0];
                }
            }
            else
            {
                WorkspaceView wsV = View.ChildOfType<WorkspaceView>();
                WorkspaceViewModel wsVM = wsV.ViewModel as WorkspaceViewModel;
              
                var sel = wsVM.Model.CurrentSelection;
                var childrenNodeViews = View.ChildrenOfType<NodeView>();

                char[] chars = e.Key.ToString().ToCharArray();
                if (chars.Length == 1)
                {
                    string key = lastChar.ToString() + chars[0].ToString();

                    TryAndPlaceNode(key);
                    lastChar = null;
                }
            }
        }


        /// <summary>
        /// find the Node Name which matches the shortcut
        /// key that is entered.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string GetNodeNameFromKeysEntered(string key)
        {
            Shortcut target = Shortcuts.FirstOrDefault(s => s.Keys.Equals(key.ToUpper()));
            return target?.NodeName;
        }

        /// <summary>
        /// Given a given shortcut key, try and place the node.
        /// </summary>
        /// <param name="key"></param>
        private void TryAndPlaceNode(string key)
        {
            string nodeName = GetNodeNameFromKeysEntered(key);

            if (nodeName != null)
            {
                DynamoViewModel vm = View.DataContext as DynamoViewModel;
                System.Windows.Point pnt;
                System.Windows.Point adjusted = new System.Windows.Point(0,0);
                try
                {
                    //todo playl with how to apply the scale and x/y values.  x/y kind works
                    WorkspaceView wsV = View.ChildOfType<WorkspaceView>();
                    pnt = Mouse.GetPosition(wsV);
                    double scale = wsV.ViewModel.Zoom;
                    double X = wsV.ViewModel.X;
                    double Y = wsV.ViewModel.Y;
                    adjusted = new System.Windows.Point( pnt.X / scale - X/scale, pnt.Y/scale - Y/scale);
                }
                catch (Exception)
                {
                    pnt = new System.Windows.Point(0,0);
                }
                try
                {
                    vm.Model.ExecuteCommand(new CreateNodeCommand(Guid.NewGuid().ToString(), nodeName, adjusted.X, adjusted.Y, false, false));
                }
                catch { }
            }
        }
    }
}
