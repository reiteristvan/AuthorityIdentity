using System;
using System.Threading.Tasks;
using Xunit;

namespace Authority.IntegrationTests.Common
{
    public static class AssertExtensions
    {
        public static async Task ThrowAsync<TException>(Func<Task> task)
        {
            Type expected = typeof(TException);
            Type actual = null;

            try
            {
                await task();
            }
            catch (Exception e)
            {
                actual = e.GetType();
            }

            Assert.Equal(expected, actual);
        }
    }
}
