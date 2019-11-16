using System.Collections.Generic;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Analysis;
using Revit.GeometryConversion;
using DynamoElement = Revit.Elements.Element;
using DynamoCurve = Autodesk.DesignScript.Geometry.Curve;

namespace ZeroTouchNodes.EgressCalculations
{
    /// <summary>
    /// A class storing the results of an egress test. 
    /// </summary>
    public class EgressResult
    {
        /// <summary>
        /// The desk Family Instance.
        /// </summary>
        public DynamoElement Instance { get; }

        /// <summary>
        /// A list of curves defining the path of egress.
        /// </summary>
        public List<DynamoCurve> Path { get; }

        /// <summary>
        /// Returns true if the length of egress is within the max egress distance.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Constructs a new EgressResult.
        /// </summary>
        internal EgressResult(DynamoElement instance, PathOfTravel pathOfTravel, double maxEgressDistance)
        {
            this.Instance = instance;

            double totalPathLength = 0.0;

            this.Path = new List<DynamoCurve>();
            foreach (Curve curve in pathOfTravel.GetCurves())
            {
                totalPathLength += curve.Length;
                this.Path.Add(curve.ToProtoType());
            }

            this.IsValid = totalPathLength < maxEgressDistance;
        }
    }
}
