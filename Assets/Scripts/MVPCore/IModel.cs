using System;

public interface IModel<T> where T:IModel<T>
{
    event Action<T> OnModelChanged;
}
