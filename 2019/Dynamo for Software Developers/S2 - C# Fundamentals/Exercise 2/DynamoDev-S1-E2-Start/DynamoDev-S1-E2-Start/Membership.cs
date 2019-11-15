namespace DynamoDev
{
    public class Membership
    {
        public int Number { get; set; }
        public MembershipType Type { get; set; }

    }

    public enum MembershipType
    {
        Coworking,
        DedicatedDesk,
        PrivateOffice,
        Enterprise
    }

}