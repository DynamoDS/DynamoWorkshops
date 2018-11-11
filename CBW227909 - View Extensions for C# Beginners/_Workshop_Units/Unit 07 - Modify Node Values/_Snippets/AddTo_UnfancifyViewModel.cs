      // Process nodes before we call node to code
      // We need this part to circumnavigate two minor node-to-code bugs
      // So this is actually a good example for how you can fix Dynamo issues for yourself without having to touch DynamoCore code
      foreach (var node in viewModel.Model.CurrentWorkspace.Nodes)
      {
        // Pre-processing of string input nodes only
        // Temporary fix for https://github.com/DynamoDS/Dynamo/issues/9117 (Escape backslashes in string nodes)
        // Temporary fix for https://github.com/DynamoDS/Dynamo/issues/9120 (Escape double quotes in string nodes)
        if (node.GetType() == typeof(StringInput))
        {
          // Cast NodeModel to StringInput
          var inputNode = (StringInput)node;
          // Get the current value of the input node
          var nodeVal = inputNode.Value;
          // Escape backslahes and double quotes
          nodeVal = nodeVal.Replace("\\", "\\\\").Replace("\"", "\\\"");
          // Update the input node's value
          var updateVal = new UpdateValueParams("Value", nodeVal);
          node.UpdateValue(updateVal);
        }
        // Add each node to the current selection
        viewModel.AddToSelectionCommand.Execute(node);
      }


using CoreNodeModels.Input;
using Dynamo.Graph;