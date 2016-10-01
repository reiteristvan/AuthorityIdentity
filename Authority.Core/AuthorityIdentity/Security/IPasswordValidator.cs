namespace AuthorityIdentity.Security
{
    public interface IPasswordValidator
    {
        bool Validate(string password);
    }
}
