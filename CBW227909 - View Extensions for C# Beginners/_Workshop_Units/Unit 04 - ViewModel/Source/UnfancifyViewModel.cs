using System;
using System.Windows;
using Dynamo.Core;
using Dynamo.Extensions;
using Dynamo.ViewModels;

namespace Unfancify
{
  /// <summary>
  /// The view model of our tool.
  /// </summary>
  class UnfancifyViewModel : NotificationObject, IDisposable
  {
    private ReadyParams readyParams;
    private DynamoViewModel viewModel;
    private Window dynWindow;

    /// <summary>
    /// The constructor of our view model.
    /// </summary>
    /// <param name="p">ReadyParams: Application-level handles provided to an extension when Dynamo has started and is ready for interaction</param>
    /// <param name="vm">The Dynamo view model: We'll need this for most of what we do below</param>
    /// <param name="dw">The Dynamo window: We'll need this for auto-layout to work properly</param>
    public UnfancifyViewModel(ReadyParams p, DynamoViewModel vm, Window dw)
    {
      // Hold references to the constructor arguments to be used later
      readyParams = p;
      viewModel = vm;
      dynWindow = dw;
    }

    /// <summary>
    /// Method that is called for freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose() { }
  }
}
