using System;
using System.Linq;
using Authority.DomainModel;

namespace Authority.Operations.Services
{
    public static class Common
    {
        /// <summary>
        /// This function retrieves the FIRST domain in the list if there was no domain id provided to the function
        /// It can be used in single-domain scenarios
        /// Do not use it in multi-domain scenarios
        /// </summary>
        /// <returns>The id of the first domain in the list</returns>
        internal static Guid GetDomainId()
        {
            if (!Authority.Domains.All().Any())
            {
                throw new InvalidOperationException("No domain found");
            }

            Domain domain = Authority.Domains.All().First();
            return domain.Id;
        }
    }
}
