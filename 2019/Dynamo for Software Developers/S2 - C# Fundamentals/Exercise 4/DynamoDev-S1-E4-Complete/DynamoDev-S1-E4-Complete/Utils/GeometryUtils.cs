using Autodesk.DesignScript.Geometry;
using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDev.Utils
{
    public static class GeometryUtils
    {
        public const int DEFAULT_NUMBER_OF_POINTS = 10;

        /// <summary>
        /// Generate a grid of equally spaced points on a given surface, flattened to a simple list.
        /// </summary>
        /// <param name="surface">The surface to generate the grid on.</param>
        /// <param name="numberOfPoints">The number of points in both X and Y coordinates, default to 10.</param>
        /// <returns>A flat list of points that cover the surface area.</returns>
        internal static List<Point> GeneratePointGridForSurface(Surface surface, int numberOfPoints = DEFAULT_NUMBER_OF_POINTS)
        {
            if (surface is null)
                throw new ArgumentNullException(nameof(surface));

            var listOfPoints = new List<Point>();

            double step = 1 / numberOfPoints;
            for (double i = 0; i < 1; i += step)
            {
                for (double j = 0; j < 1; j += step)
                {
                    listOfPoints.Add(surface.PointAtParameter(i, j));
                }
            }
            return listOfPoints;
        }

    }
}
