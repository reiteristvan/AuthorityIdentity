using System;
using System.Threading.Tasks;
using Xunit;

namespace AuthorityIdentity.IntegrationTests.Common
{
    public static class AssertExtensions
    {
        public static async Task ThrowAsync<TException>(Func<Task> task, Func<TException, bool> exceptionCondition = null)
            where TException : Exception
        {
            Type expected = typeof(TException);
            Type actual = null;
            TException exception = null;

            try
            {
                await task();
            }
            catch (Exception e)
            {
                actual = e.GetType();
                exception = e as TException;
            }

            Assert.Equal(expected, actual);

            if (exceptionCondition != null)
            {
                Assert.True(exceptionCondition(exception));
            }
        }
    }
}
