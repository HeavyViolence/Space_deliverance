using System;

public sealed class SavegameAboutToStartEventArgs : EventArgs
{
    public float SavegameDelay { get; }

    public SavegameAboutToStartEventArgs(float savegameDelay)
    {
        SavegameDelay = savegameDelay;
    }
}
