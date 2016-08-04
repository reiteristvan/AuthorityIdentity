using System.Collections.Generic;
using Authority.DomainModel;

namespace Authority.IntegrationTests.Common
{
    public sealed class TestDomain
    {
        public List<User> Users { get; set; }
        public List<Policy> Policies { get; set; }
        public List<AuthorityClaim> Claims { get; set; }   
    }
}
