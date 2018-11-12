    /// <summary>
    /// Delete all text notes?
    /// </summary>
    public bool DeleteTextNotes { get; set; } = true;

    /// <summary>
    /// Text note prefixes that should be ignored.
    /// </summary>
    public string IgnoreTextNotePrefixes { get; set; } = "";








      // Identify all text notes to keep/delete
      if (DeleteTextNotes)
      {
        // Make sure that no text notes are currently selected
        GeneralUtils.ClearSelection();
        // Create a whitelist of prefixes for text note content from what was entered by the user in the UI
        var textNoteIgnoreList = IgnoreTextNotePrefixes.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        // Cycle through all text notes in the graph
        foreach (var note in viewModel.Model.CurrentWorkspace.Notes)
        {
          // Cycle through the whitelist
          foreach (var ignoreTerm in textNoteIgnoreList)
          {
            // Identify keepers
            if (note.Text.StartsWith(ignoreTerm) && !stuffToKeep.Contains(note.GUID.ToString()))
            {
              stuffToKeep.Add(note.GUID.ToString());
            }
          }
          // Add all obsolete text notes to selection
          if (!stuffToKeep.Contains(note.GUID.ToString())) { viewModel.AddToSelectionCommand.Execute(note); }
        }
        // Delete all obsolete text notes
        viewModel.DeleteCommand.Execute(null);
      }