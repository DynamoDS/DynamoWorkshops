using Autodesk.DesignScript.Geometry;
using System;

namespace DynamoDev.Business
{
    /// <summary>
    /// A work surface that can be used and occupied by one person at a time.
    /// </summary>
    public class Desk
    {
        /// <summary>
        /// The unique identifier of the desk
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// The length of the desk, expressed in mm.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// The height of the desk, expressed in mm.
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// The width of the desk, expressed in mm.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The person who is currently occupying this desk.
        /// </summary>
        public Person Occupier { get; set; }

        /// <summary>
        /// The price of the desk per day, expressed in currency units.
        /// </summary>
        public double PricePerDay { get; }

        /// <summary>
        /// The origin point in space for this desk.
        /// </summary>
        public Point Origin { get; set; }

        public Desk()
        {
            this.Id = Guid.NewGuid();
            this.PricePerDay = 35;
            this.Origin = Point.ByCoordinates(0,0,0);
        }

        #region Computed methods

        public double Volume()
        {
            return this.Width * this.Length * this.Height;
        }

        public double Area()
        {
            return this.Width * this.Length;
        }

        public double PriceForNumberOfDays(int days)
        {
            return days * this.PricePerDay;
        }

        #endregion
    }
}
