    /// <summary>
    /// Group prefixes that should be ignored.
    /// </summary>
    public string IgnoreGroupPrefixes { get; set; } = "";



      // Create a list for storing guids of groups, nodes and text notes that we want to keep
      var stuffToKeep = new List<string>();



using System.Collections.Generic;



      // Identify all groups to keep/ungroup
      if (UngroupAll)
      {
        // Make sure that no groups are currently selected
        GeneralUtils.ClearSelection();
        // Create a whitelist of prefixes for group titles from what was entered by the user in the UI
        var groupIgnoreList = IgnoreGroupPrefixes.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Cycle through all groups in the graph
        foreach (var anno in viewModel.CurrentSpaceViewModel.Annotations)
        {
          // Cycle through the whitelist
          foreach (var ignoreTerm in groupIgnoreList)
          {
            // Identify keepers (group and its contents)
            if (anno.AnnotationText.StartsWith(ignoreTerm) && !stuffToKeep.Contains(anno.AnnotationModel.GUID.ToString()))
            {
              stuffToKeep.Add(anno.AnnotationModel.GUID.ToString());
              // Identify all nodes and text notes within those groups
              foreach (var element in anno.Nodes) { stuffToKeep.Add(element.GUID.ToString()); }
            }
          }
          // Add all obsolete groups to selection
          if (!stuffToKeep.Contains(anno.AnnotationModel.GUID.ToString())) { viewModel.AddToSelectionCommand.Execute(anno.AnnotationModel); }
        }
        // Ungroup all obsolete groups
        viewModel.UngroupAnnotationCommand.Execute(null);
      }

      foreach (var node in viewModel.Model.CurrentWorkspace.Nodes)
      {
        // We're not interested in keepers (i.e. nodes that will not be run through node-to-code)
        if (!stuffToKeep.Contains(node.GUID.ToString()))
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
      }