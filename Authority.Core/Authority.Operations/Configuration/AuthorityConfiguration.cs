namespace Authority.Operations.Configuration
{
    public enum DomainMode
    {
        Single,
        Multi
    }

    public sealed class AuthorityConfiguration
    {
        public static AuthorityConfiguration Default
        {
            get
            {
                return new AuthorityConfiguration
                {
                    DomainMode = DomainMode.Single
                };
            }
        }

        public DomainMode DomainMode { get; set; }
    }
}
