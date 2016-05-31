namespace Authority.Core
{
    public sealed class ClientApplication : EntityBase
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
        public string ApplicationId { get; set; }
        public string ApplicationSecret { get; set; }
        public string RedirectUrl { get; set; }
        public bool IsPublic { get; set; }
        public Product Product { get; set; }
    }
}
