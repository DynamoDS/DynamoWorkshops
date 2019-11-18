using System;

namespace DynamoDev
{
    /// <summary>
    /// Represent an individual (human) that is a member of the space provider.
    /// </summary>
    public class Person : Identifiable
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
        /// The unique identifier of the desk assigned to this person.
        /// </summary>
        public Guid AssignedDesk { get; set; }

        /// <summary>
        /// The space provider membership the person has access to.
        /// </summary>
        public Membership Membership { get; set; }

        #region Constructors

        public Person() : base()
        {
            this.Name = "John Doe";
            this.Membership = new Membership();
        }

        /// <summary>
        /// Create a new Person given a name.
        /// </summary>
        /// <param name="name">The name of the Person to create.</param>
        public Person(string name) : this()
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        /// <summary>
        /// Create a new person given a name, email and their membership.
        /// </summary>
        /// <param name="name">The name of the Person to create.</param>
        /// <param name="email">The email of the Person.</param>
        /// <param name="membership">The membership of the person.</param>
        /// <returns>A new person.</returns>
        public static Person ByNameEmailMembership(string name, string email, Membership membership)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            if (membership is null)
            {
                throw new ArgumentNullException(nameof(membership));
            }

            var person = new Person(name)
            {
                Email = email,
                Membership = membership
            };
            return person;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update the assigned desk of the person.
        /// </summary>
        /// <param name="desk">The desk to assign to the person.</param>
        public void AssignDesk(Desk desk)
        {
            this.AssignedDesk = desk.Id;
            desk.Occupier = this;
        }

        /// <summary>
        /// Change the membership of the person to a new one.
        /// </summary>
        /// <param name="newMembership">The membership to assign to the person.</param>
        /// <returns>True if the new membership was assigned, false otherwise.</returns>
        public bool ChangeMembership(Membership newMembership)
        {
            try
            {
                this.Membership = newMembership ?? throw new ArgumentNullException(nameof(newMembership));
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
