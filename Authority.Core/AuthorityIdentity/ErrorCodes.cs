namespace AuthorityIdentity
{
    public sealed class ErrorCodes
    {
        // Accounts
        public const int DomainNotAvailable = 1000;
        public const int EmailAlreadyExists = 1001;
        public const int UsernameNotAvailable = 1002;
        public const int FailedActivation = 1003;
        public const int UserNotFound = 1004;
        public const int InviteNotFound = 1005;
        public const int InviteExpired = 1006;
        public const int InviteAlreadyAccepted = 1007;
        public const int PasswordInvalid = 1008;

        // Claims
        public const int ClaimNameNotAvailable = 2001;
        public const int ClaimNotFound = 2002;

        // Domains
        public const int DomainNotFound = 3001;
        public const int DomainNotExists = 3002;
        public const int DomainNameNotAvailable = 3003;
        public const int DomainMismatch = 3004;

        // Groups
        public const int GroupNameNotAvailable = 4001;
        public const int DefaultGroupAlreadyExists = 4002;
        public const int GroupNotFound = 4004;

        // Policies
        public const int DefaultPolicyAlreadyExists = 5002;
        public const int PolicyNotFound = 5003;

        // 2FA
        public const int TwoFactorNotEnabled = 6001;
    }
}
