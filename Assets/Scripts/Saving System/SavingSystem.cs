using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using System;

public sealed class SavingSystem : GlobalInstance<SavingSystem>, ISavable
{
    private const string SavegameFileName = "Space deliverance";
    private const string SavegameFileExtension = ".save";

    private const float MinSavegamePeriod = 60f;
    private const float MaxSavegamePeriod = 600f;
    private const float DefaultSavegamePeriod = 300f;
    private const float SavegameDelay = 10f;

    public event EventHandler SavegameAboutToStart;
    public event EventHandler SavegameStarted;
    public event EventHandler SavegameCompleted;

    private readonly HashSet<SavableEntity> _registeredEntities = new();
    private Dictionary<string, object> _storedStates = new();

    private string SavegameFileFullName => SavegameFileName + SavegameFileExtension;
    private string SavegamePath => Path.Combine(Application.persistentDataPath, SavegameFileFullName);

    public float SavegamePeriod { get; private set; } = DefaultSavegamePeriod;
    public bool SavegameExists { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        SavegameExists = Load();
    }

    private void Start()
    {
        StartCoroutine(SavePeriodically());
    }

    private IEnumerator SavePeriodically()
    {
        while (true)
        {
            yield return new WaitForSeconds(SavegamePeriod);

            StartCoroutine(DelayedSave());
        }
    }

    private IEnumerator DelayedSave()
    {
        SavegameAboutToStart?.Invoke(this, new SavingStartupEventArgs(SavegameDelay));

        yield return new WaitForSecondsRealtime(SavegameDelay);

        SavegameStarted?.Invoke(this, EventArgs.Empty);
        Save();
    }

    public void Save()
    {
        DataContractSerializer serializer = new(typeof(KeyValuePair<string, object>));
        using FileStream stream = new(SavegamePath, FileMode.OpenOrCreate);

        serializer.WriteObject(stream, _storedStates);
        SavegameCompleted?.Invoke(this, EventArgs.Empty);
    }

    private bool Load()
    {
        if (Directory.Exists(SavegamePath))
        {
            DataContractSerializer serializer = new(typeof(KeyValuePair<string, object>));
            using FileStream stream = new(SavegamePath, FileMode.Open);

            _storedStates = (Dictionary<string, object>)serializer.ReadObject(stream);

            return true;
        }

        return false;
    }

    public bool Register(SavableEntity entity)
    {
        if (!_registeredEntities.Contains(entity))
        {
            _registeredEntities.Add(entity);

            if (_storedStates.TryGetValue(entity.ID, out var state))
            {
                entity.SetState(state);
            }

            return true;
        }

        return false;
    }

    public bool Deregister(SavableEntity entity)
    {
        if (_registeredEntities.Contains(entity))
        {
            _storedStates[entity.ID] = entity.GetState();
            _registeredEntities.Remove(entity);

            return true;
        }

        return false;
    }

    public void SetSavingPeriod(float value) => SavegamePeriod = Mathf.Clamp(value, MinSavegamePeriod, MaxSavegamePeriod);

    public void SetDefaultSavingPeriod() => SetSavingPeriod(DefaultSavegamePeriod);

    public object CaptureState() => SavegamePeriod;

    public void RestoreState(object state)
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state), $"Transmitted object state must not be null!");
        }

        if (!state.GetType().Equals(typeof(float)))
        {
            throw new ArgumentException(nameof(state), $"Transmitted object state must be of type {typeof(float)}!");
        }

        SavegamePeriod = (float)state;
    }
}
