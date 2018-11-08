using System.Windows.Input;
using Dynamo.UI.Commands;



    private string unfancifyMsg = "";




    public ICommand UnfancifyCurrentGraph { get; set; }



      // The Unfancify Current Graph button is bound to this command
      UnfancifyCurrentGraph = new DelegateCommand(OnUnfancifyCurrentClicked);



    /// <summary>
    /// Text message that appears below the buttons.
    /// It is updated by some of the methods in this view model.
    /// </summary>
    public string UnfancifyMsg
    {
      get { return unfancifyMsg; }
      set
      {
        unfancifyMsg = value;
        // Notify the UI that the value has changed
        RaisePropertyChanged("UnfancifyMsg");
      }
    }