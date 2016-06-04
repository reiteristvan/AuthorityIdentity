namespace Authority.DomainModel
{
    public sealed class AuthorityClaim : EntityBase
    {
        public string FriendlyName { get; set; }

        public string Issuer { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
