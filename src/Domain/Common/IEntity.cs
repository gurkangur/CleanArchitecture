namespace Domain.Common
{
    public interface IEntity<T>
    {
        T Id { get; set; }
    }
}