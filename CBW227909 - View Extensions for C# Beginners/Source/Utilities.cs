using Dynamo.Models;
using System.Reflection;

namespace Unfancify
{
    /// <summary>
    /// Shared general utility functions
    /// </summary>
    static class GeneralUtils
    {
        public static void ClearSelection()
        {
            var dynamoSelection = typeof(DynamoModel).Assembly.GetType("Dynamo.Selection.DynamoSelection");
            var selectionInstance = dynamoSelection.GetProperty("Instance", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            var clearMethod = dynamoSelection.GetMethod("ClearSelection", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
            clearMethod.Invoke(selectionInstance.GetValue(null), null);
        }
    }
}
