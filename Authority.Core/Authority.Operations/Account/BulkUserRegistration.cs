using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authority.EntityFramework;
using Authority.Operations.Security;

namespace Authority.Operations.Account
{
    public sealed class BulkRegistrationData
    {
        public Guid DomainId { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }


    /// <summary>
    /// This is a long running process due to the 20.000 iteration for password hashing
    /// Results for one batch (100 user) preparation/insertion
    /// Time ellapsed preparing a batch: 30468 ms
    //  Time ellapsed inserting batch: 53 ms
    /// </summary>
    public sealed class BulkUserRegistration : SqlOperation
    {
        private const string InsertSql = @"insert into Authority.Users 
                (DomainId, Email, Username, PasswordHash, Salt, IsPending, PendingRegistrationId, IsActive, Id, IsExternal) 
                values 
                ('{0}', '{1}', '{2}', '{3}', '{4}', {5}, '{6}', {7}, '{8}', 0)";

        private readonly List<BulkRegistrationData> _registrationData;
        private readonly bool _shouldActivate;
        private readonly PasswordService _passwordService;

        public BulkUserRegistration(IAuthorityContext context, List<BulkRegistrationData> registrationData, bool shouldActivate = false) 
            : base(context)
        {
            _registrationData = registrationData;
            _shouldActivate = shouldActivate;
            _passwordService = new PasswordService();
        }

        protected override async Task Do()
        {
            //Stopwatch sw = new Stopwatch();

            const int BatchSize = 100;

            int skip = 0;
            StringBuilder sqlBuilder = new StringBuilder();

            while (skip < _registrationData.Count)
            {
                //sw.Start();

                IEnumerable<BulkRegistrationData> batch = _registrationData.Skip(skip).Take(BatchSize);

                foreach (BulkRegistrationData user in batch)
                {
                    string password = string.IsNullOrEmpty(user.Password)
                        ? _passwordService.GeneratePassword(12, true)
                        : user.Password;

                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] saltBytes = _passwordService.CreateSalt();
                    byte[] hashBytes = _passwordService.CreateHash(passwordBytes, saltBytes);

                    string insert = string.Format(
                        InsertSql,
                        user.DomainId,
                        user.Email,
                        user.Username,
                        Convert.ToBase64String(hashBytes),
                        Convert.ToBase64String(saltBytes),
                        _shouldActivate ? 0 : 1,
                        _shouldActivate ? Guid.NewGuid() : Guid.Empty,
                        1,
                        Guid.NewGuid());

                    sqlBuilder.AppendLine(insert);
                }

                //sw.Stop();

                //Debug.WriteLine("Time ellapsed preparing a batch: {0} ms", sw.ElapsedMilliseconds);

                //sw.Reset();
                //sw.Start();

                await Context.Database.ExecuteSqlCommandAsync(sqlBuilder.ToString());

                //sw.Stop();

                //Debug.WriteLine("Time ellapsed inserting batch: {0} ms", sw.ElapsedMilliseconds);

                //sw.Reset();

                skip = skip + BatchSize;
                sqlBuilder.Clear();
            }
        }
    }
}
