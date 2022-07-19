using System;

public sealed class SavegameLoadFailedEventArgs : EventArgs
{
    public string Message { get; }

    public SavegameLoadFailedEventArgs(string message)
    {
        Message = message;
    }
}
