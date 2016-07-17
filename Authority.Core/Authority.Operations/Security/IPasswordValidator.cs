namespace Authority.Operations.Security
{
    public interface IPasswordValidator
    {
        bool Validate(string password);
    }
}
