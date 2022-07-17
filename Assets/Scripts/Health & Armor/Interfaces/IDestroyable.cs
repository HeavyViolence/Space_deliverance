using System;

public interface IDestroyable
{
    public event EventHandler<DestroyedEventArgs> Destroyed;
}
