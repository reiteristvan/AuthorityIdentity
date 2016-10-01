using System.Collections.Generic;
using AuthorityIdentity.DomainModel;

namespace AuthorityIdentity.IntegrationTests.Common
{
    public sealed class TestDomain
    {
        public List<User> Users { get; set; }
        public List<Policy> Policies { get; set; }
        public List<AuthorityClaim> Claims { get; set; }   
    }
}
