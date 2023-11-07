public interface IModel<T> : IModel where T : class, IModel<T>
{
    IGameEvent<T> OnModelChanged { get; }
}
public interface IModel
{
    string ModelKey { set; }
    void Save();
}
