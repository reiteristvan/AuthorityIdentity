namespace Authority.Operations.Account
{
    // 1XXX
    public static class AccountErrorCodes
    {
        public static int DomainNotAvailable = 1000;
        public static int EmailAlreadyExists = 1001;
        public static int UsernameNotAvailable = 1002;
        public static int FailedActivation = 1003;
        public static int UserNotFound = 1004;
        public static int InviteNotFound = 1005;
        public static int InviteExpired = 1006;
        public static int InviteAlreadyAccepted = 1007;
    }
}
