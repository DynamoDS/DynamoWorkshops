using Autodesk.Revit.DB;
using DynamoDev.Business;
using DynamoDev.Layout;
using System.Collections.Generic;
using System.Linq;

namespace ZeroTouchNodes.FamilyType
{
    /// <summary>
    /// A helper class with methods used to set the properties of FamilySymbol.
    /// </summary>
    class FamilyTypeCreator
    {
        /// <summary>
        /// Duplicates the given FamilySymbol with the deskTypeName and sets the Width and Depth with the given values.
        /// </summary>
        internal static FamilySymbol Desk(FamilySymbol deskType, DeskArrangement deskArrangement, string deskTypeName)
        {
            FamilySymbol newDeskType = (FamilySymbol)deskType.Duplicate(deskTypeName);

            List<string> paramNameLookUp = new List<string> {"Width", "Depth"};

            Desk firstDesk = deskArrangement.Desks.First();
            List<double> dimensions = new List<double> { firstDesk.Width, firstDesk.Length };

            for (int i = 0; i < paramNameLookUp.Count; i++)
            {
                Parameter parameter = newDeskType.LookupParameter(paramNameLookUp[i]);

                // convert the dimension into Revits internal units.
                double dimensionInFt = UnitUtils.ConvertToInternalUnits(dimensions[i], DisplayUnitType.DUT_MILLIMETERS);

                parameter.Set(dimensionInFt);
            }

            return newDeskType;
        }
    }
}
