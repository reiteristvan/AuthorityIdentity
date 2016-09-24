namespace Authority.Security
{
    public interface IPasswordValidator
    {
        bool Validate(string password);
    }
}
