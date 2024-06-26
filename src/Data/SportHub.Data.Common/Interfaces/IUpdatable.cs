namespace SportHub.Data.Common.Interfaces;

public interface IUpdatable<T>
{
    bool IsUpdated(T other);
}