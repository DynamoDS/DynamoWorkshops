using System;

namespace DynamoDev
{
    /// <summary>
    /// Represent an individual (human) that is a member of the space provider.
    /// </summary>
    public class Person
    {
        /// <summary>
        /// The name of the person.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The email the person registered with.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The unique identifier of the person.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique identifier of the desk assigned to this person.
        /// </summary>
        public Guid AssignedDesk { get; set; }

        /// <summary>
        /// The space provider membership the person has access to.
        /// </summary>
        public Membership Membership { get; set; }
    }
}
