using Autodesk.DesignScript.Geometry;
using DynamoDev.Business;
using DynamoDev.Utils;
using System;
using System.Collections.Generic;

namespace DynamoDev.Layout
{
    /// <summary>
    /// There can only be one desk architect.
    /// Lays out a grid of desks on a given surface.
    /// </summary>
    public static class DeskArchitect
    {
        public static List<Desk> LayoutDesksOnSurface(Surface surface)
        {
            if (surface is null)
                throw new ArgumentNullException(nameof(surface));

            // initialise a list that will hold the created desks
            var desks = new List<Desk>();

            // make a grid of points for our surface
            var points = GeometryUtils.GeneratePointGridForSurface(surface);

            // make a new Desk instance for each point
            foreach (var point in points)
            {
                var desk = new Desk();
                desk.Origin = point;
            }

            return desks;
        }
    }
}
