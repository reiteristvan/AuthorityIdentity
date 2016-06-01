using System;
using Authority.DomainModel;
using Authority.EntityFramework;

namespace Authority.Operations.Errors
{
    public sealed class RecordError : OperationWithReturnValue<Guid>
    {
        private readonly string _type;
        private readonly string _message;
        private readonly string _stacktrace;

        public RecordError(IAuthorityContext AuthorityContext, string type, string message, string stacktrace)
            : base(AuthorityContext)
        {
            _type = type;
            _message = message;
            _stacktrace = stacktrace;
        }

        public override Guid Do()
        {
            Error error = new Error
            {
                Message = _message,
                StackTrace = _stacktrace,
                Type = _type,
                Date = DateTime.UtcNow
            };

            Context.Errors.Add(error);

            return error.Id;
        }
    }
}
