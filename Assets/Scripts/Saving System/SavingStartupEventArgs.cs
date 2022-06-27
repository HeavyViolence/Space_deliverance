using System;

public sealed class SavingStartupEventArgs : EventArgs
{
    public float SavingDelay { get; }

    public SavingStartupEventArgs(float savingDelay)
    {
        SavingDelay = savingDelay;
    }
}
