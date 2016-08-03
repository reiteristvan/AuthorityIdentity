namespace Authority.Operations.Policies
{
    public static class PolicyErrorCodes
    {
        public const int UnAuthorizedAccess = 4000;
        public const int DomainNotFound = 4001;
        public const int DefaultAlreadyExists = 4002;
        public const int PolicyNotFound = 4003;
        public const int ClaimNotExists = 4004;
    }
}
