using Autodesk.DesignScript.Geometry;
using DynamoDev.Business;
using System;
using System.Collections.Generic;

namespace DynamoDev.Layout
{
    public class DeskArrangement : Identifiable
    {
        private const double DEFAULT_EFFICIENCY_TARGET = 0.8;
        private readonly double sourceSurfaceArea;

        /// <summary>
        /// The desks included in the arrangement.
        /// </summary>
        public List<Desk> Desks { get; }
        public DateTime CreatedAt { get; }
        /// <summary>
        /// The number of desks in the arrangement.
        /// </summary>
        public int DeskCount => this.Desks is null ? 0 : this.Desks.Count;

        /// <summary>
        /// Returns the actual yield of the desk arrangement, 
        /// expressed as the number of desks divided by the surface area they are meant to occupy.
        /// </summary>
        public double ActualYield => CalculateYield();

        /// <summary>
        /// The target yield, as a percentage.
        /// </summary>
        public double TargetYield { get; set; }

        /// <summary>
        /// Expresses the yield efficiency of actual yield divided by target yield, as a percentage.
        /// </summary>
        public double Efficiency => CalculateEfficiency();

        public double Revenue { get; set; }

        public DeskArrangement(List<Desk> desks, Surface surface) : base()
        {
            if (surface is null)
                throw new ArgumentNullException(nameof(surface));

            this.Desks = desks ?? throw new ArgumentNullException(nameof(desks));

            this.sourceSurfaceArea = surface.Area;
            this.TargetYield = DEFAULT_EFFICIENCY_TARGET * this.sourceSurfaceArea;
        }

        private double CalculateYield()
        {
            return this.DeskCount / this.sourceSurfaceArea;
        }

        private double CalculateEfficiency()
        {
            if (this.TargetYield == 0) throw new DivideByZeroException("Target yield cannot be 0.");
            return this.ActualYield / this.TargetYield * 100;
        }
    }
}
