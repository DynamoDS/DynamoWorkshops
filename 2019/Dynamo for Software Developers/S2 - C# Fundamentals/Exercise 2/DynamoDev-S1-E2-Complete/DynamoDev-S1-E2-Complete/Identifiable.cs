using System;
using System.Collections.Generic;
using System.Text;

namespace DynamoDev
{
    public class Identifiable
    {
        /// <summary>
        /// The unique identifier of the person.
        /// </summary>
        public Guid Id { get; set; }

        public Identifiable()
        {
            this.Id = Guid.NewGuid();
        }
    }
}
