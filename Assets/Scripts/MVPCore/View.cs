using UnityEngine;

public class View : MonoBehaviour, IView
{
    private bool _disposed;
    ~View()
    {
        if (!_disposed)
            Dispose(false);
    }

    public void Dispose()
    {
        if (!_disposed)
            Dispose(true);
    }


    /// <summary>
    /// Dispose method for inheritors
    /// Note that _disposed boolean should force
    /// implementors to call base dispose at the end
    /// This should reduce potential errors
    /// </summary>
    /// <param name="disposing">True when called from user code, False when called from destructor</param>
    protected virtual void Dispose(bool disposing)
    {
        // As unity manages objects in it's own manner we will call Destroy only on user code
        // If we will call it from destructor - gameObject sended to Destroy will be null and Unity will throw exception
        if(disposing)
            Destroy(gameObject);
        _disposed = true;
    }
}
