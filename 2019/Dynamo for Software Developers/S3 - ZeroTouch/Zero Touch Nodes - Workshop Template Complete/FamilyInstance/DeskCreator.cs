using Autodesk.Revit.DB;
using DynamoDev.Business;
using DynamoDev.Layout;
using RevitServices.Persistence;
using RevitServices.Transactions;
using System.Collections.Generic;
using Revit.Elements;
using Revit.GeometryConversion;
using ZeroTouchNodes.FamilyType;
using DynamoElement = Revit.Elements.Element;
using DynamoFamilyType = Revit.Elements.FamilyType;
using DynamoLevel = Revit.Elements.Level;
using StructuralType = Autodesk.Revit.DB.Structure.StructuralType;

namespace ZeroTouchNodes.FamilyInstance
{
    /// <summary>
    /// A public class of methods which consume the <see cref="Desk"/>'s stored in the <see cref="DeskArrangement"/>
    /// object. This class is public and includes public methods which can be imported into Dynamo via zero touch import.
    /// </summary>
    public class DeskCreator
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        private DeskCreator()
        {

        }

        /// <summary>
        /// Returns a list of desk Revit family instances using the Desk stored in the DeskArrangement.
        /// </summary>
        /// <param name="deskArrangement"> The DeskArrangement object.</param>
        /// <param name="deskType"> The desk type.</param>
        /// <param name="hostLevel"> The host hostLevel for the new desks.</param>
        public static List<DynamoElement> Generate(DeskArrangement deskArrangement, DynamoFamilyType deskType, DynamoLevel hostLevel)
        {
            Document doc = DocumentManager.Instance.CurrentDBDocument;

            TransactionManager.Instance.EnsureInTransaction(doc);

            FamilySymbol newDeskType = FamilyTypeCreator.Desk((FamilySymbol)deskType.InternalElement, deskArrangement, "AU 2019 Vegas Desk");

            // Get the Revit Document factory object from the Autodesk.Revit.DB.Creation namespace. 
            // This object is used to instantiate most Revit Elements.
            var revitFactory = doc.Create;

            var revitDesks = new List<DynamoElement>();
            foreach (var desk in deskArrangement.Desks)
            {
                XYZ location = desk.Origin.ToXyz();
                var familyInstance = revitFactory.NewFamilyInstance(location, newDeskType, hostLevel.InternalElement, StructuralType.NonStructural);

                // Wrap and bind the new desk instance.
                revitDesks.Add(familyInstance.ToDSType(false));
            }

            TransactionManager.Instance.TransactionTaskDone();

            return revitDesks;
        }
    }
}
