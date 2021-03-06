﻿using System;
using System.Collections.Generic;
using System.Linq;
using Authority.IntegrationTests;
using AuthorityIdentity.Configuration;
using AuthorityIdentity.Observers;
using AuthorityIdentity.DomainModel;
using AuthorityIdentity.EntityFramework;

namespace AuthorityIdentity.IntegrationTests
{
    public sealed class AuthorityTestContext : IDisposable
    {
        static AuthorityTestContext()
        {
            AuthorityConfiguration configuration = new AuthorityConfiguration
            {
                DomainMode = DomainMode.Multi,
                Logger = new TestLogger(),
                Observers = new List<IAccountObserver> {  new LoggingObserver() },
                TwoFactorAuthenticationEnabled = true,
                TwoFactorMode = TwoFactorMode.Optional,
                TwoFactorService = new TestTwoFactorService()
            };

            Authority.Init(configuration);
            Authority.RegisterExternalIdentityProvider(new TestExternalProvider());
        }

        public AuthorityTestContext()
        {
            Context = new AuthorityContext();
            Guid domainId = Authority.Domains.Create(RandomData.RandomString()).Result;
            Domain = Authority.Domains.All().First(d => d.Id == domainId);
        }

        public AuthorityContext Context { get; set; }
        public Domain Domain { get; set; }

        public void Dispose()
        {
            // in this case 'forceReload' needs to set to True as tests does not use the service interface
            foreach (Domain domain in Authority.Domains.All(true))
            {
                Authority.Domains.Delete(domain.Id).Wait();
            }
        }
    }
}
