namespace Authority.DomainModel
{
    public sealed class AuthorityClaim : EntityBase
    {
        public const string TableName = "Authority.Claims";

        public string FriendlyName { get; set; }

        public string Issuer { get; set; }

        public string Type { get; set; }

        public string Value { get; set; }
    }
}
