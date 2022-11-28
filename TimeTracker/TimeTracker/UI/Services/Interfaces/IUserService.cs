namespace UI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<Tuple<string, string>>> GetUserList();
    }
}