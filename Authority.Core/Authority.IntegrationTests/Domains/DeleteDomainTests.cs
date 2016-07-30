using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Authority.DomainModel;
using Authority.IntegrationTests.Common;
using Authority.Operations.Products;
using Xunit;

namespace Authority.IntegrationTests.Domains
{
    public sealed class DeleteDomainTests
    {
        [Fact]
        public async Task DeleteDomainShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                CreateDomain createOperation = new CreateDomain(testContext.Context, RandomData.RandomString());
                Guid domainId = await createOperation.Do();
                await createOperation.CommitAsync();

                DeleteDomain deleteOperation = new DeleteDomain(testContext.Context, domainId);
                await deleteOperation.Execute();

                Domain domain = await testContext.Context.Domains.FirstOrDefaultAsync(d => d.Id == domainId);

                Assert.Null(domain);
            }
        }

        [Fact]
        public async Task DeleteDomainWithUsersShouldSucceed()
        {
            using (AuthorityTestContext testContext = new AuthorityTestContext())
            {
                CreateDomain createOperation = new CreateDomain(testContext.Context, RandomData.RandomString());
                Guid domainId = await createOperation.Do();
                await createOperation.CommitAsync();

                // TODO: bulk register
                for (int i = 0; i < 100; ++i)
                {
                    await
                        TestOperations.RegisterAndActivateUser(testContext.Context, domainId,
                            RandomData.RandomString(12, true));
                }

                DeleteDomain deleteOperation = new DeleteDomain(testContext.Context, domainId);
                await deleteOperation.Execute();

                Domain domain = await testContext.Context.Domains.FirstOrDefaultAsync(d => d.Id == domainId);

                Assert.Null(domain);

                List<User> users = testContext.Context.Users
                    .Where(u => u.DomainId == domainId)
                    .ToList();

                Assert.False(users.Any());
            }
        }
    }
}
