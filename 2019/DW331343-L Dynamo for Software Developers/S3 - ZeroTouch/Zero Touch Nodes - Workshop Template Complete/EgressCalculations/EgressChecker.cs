using Autodesk.Revit.DB.Analysis;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Revit.GeometryConversion;
using RevitServices.Persistence;
using RevitServices.Transactions;
using DynamoElement = Revit.Elements.Element;
using DynamoView = Revit.Elements.Views.View;
using ProtoPoint = Autodesk.DesignScript.Geometry.Point;

namespace ZeroTouchNodes.EgressCalculations
{
    /// <summary>
    /// A processor class which computes the egress results of the given desks relative to an escape door.
    /// </summary>
    public class EgressChecker
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        private EgressChecker()
        {

        }

        /// <summary>
        /// Calculates the path of egress from each desk instance to the escapeLocation and returns a list of EgressResult objects.
        /// </summary>
        /// <param name="deskInstances"> A list of Revit desk family instances.</param>
        /// <param name="escapeLocation"> The point where the exit is, typically from a door location.</param>
        /// <param name="view"> The view to perform the egress calculation.</param>
        /// <param name="maxEgressDistance"> The maximum distance which is permitted for escape.</param>
        public static List<EgressResult> GetResults(List<DynamoElement> deskInstances, ProtoPoint escapeLocation, DynamoView view, double maxEgressDistance)
        {
            double maxEgressDistanceInFt =
                UnitUtils.ConvertToInternalUnits(maxEgressDistance, DisplayUnitType.DUT_MILLIMETERS);

            Document doc = DocumentManager.Instance.CurrentDBDocument;

            TransactionManager.Instance.EnsureInTransaction(doc);

            XYZ escapeLocationXyz = escapeLocation.ToXyz();

            List<EgressResult> egressResults = new List<EgressResult>();
            foreach (var deskInstance in deskInstances)
            {
                LocationPoint deskLocation = (LocationPoint)deskInstance.InternalElement.Location;

                PathOfTravel pathOfTravel = PathOfTravel.Create((View)view.InternalElement, deskLocation.Point, escapeLocationXyz);
                
                EgressResult egressResult = new EgressResult(deskInstance, pathOfTravel, maxEgressDistanceInFt);

                egressResults.Add(egressResult);
            }

            TransactionManager.Instance.TransactionTaskDone();

            return egressResults;
        }
    }
}
