using System;

namespace DynamoDev
{
    public class Membership
    {
        public int Number { get; set; }
        public MembershipType Type { get; set; }

        public Membership()
        {
            this.Number = Guid.NewGuid().GetHashCode();
            this.Type = MembershipType.None;
        }
    }

    public enum MembershipType
    {
        None,
        Coworking,
        DedicatedDesk,
        PrivateOffice,
        Enterprise
    }

}