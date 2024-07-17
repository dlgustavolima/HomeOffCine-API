namespace HomeOffCine.Business.Interfaces.Service;

public interface IUser
{
    string GetUserName();

    Guid GetUserId();

    bool IsAuthenticated();

    bool IsInRole(string role);
}
