using System;

public interface IModel<T> : IModel where T:class, IModel<T>
{
    event Action<T> OnModelChanged;
}
public interface IModel
{
    string ModelKey { set; }
}
