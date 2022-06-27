using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using System;

public sealed class SavingSystem : GlobalInstance<SavingSystem>, ISavable
{
    private const string SaveFileName = "Space deliverance";
    private const string SaveFileExtension = ".save";

    private const float MinSavingPeriod = 60f;
    private const float MaxSavingPeriod = 600f;
    private const float DefaultSavingPeriod = 300f;
    private const float SavingDelay = 10f;

    public event EventHandler SavingStartup;
    public event EventHandler SavingStarted;
    public event EventHandler SavingCompleted;

    private readonly Dictionary<string, SavableEntity> _registeredEntities = new();
    private Dictionary<string, object> _states = new();

    private float _savingPeriod = DefaultSavingPeriod;
    public float SavingPeriod => _savingPeriod;

    private string SaveFileFullName => SaveFileName + SaveFileExtension;
    private string SavePath => Path.Combine(Application.persistentDataPath, SaveFileFullName);

    protected override void Awake()
    {
        base.Awake();

        TryLoad();
    }

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(SavingPeriod);

            StartCoroutine(SaveDelayed());
        }
    }

    private IEnumerator SaveDelayed()
    {
        SavingStartup?.Invoke(this, new SavingStartupEventArgs(SavingDelay));

        yield return new WaitForSecondsRealtime(SavingDelay);

        SavingStarted?.Invoke(this, EventArgs.Empty);
        Save();
    }

    private void TryLoad()
    {
        if (Directory.Exists(SavePath))
        {
            var serializer = new DataContractSerializer(typeof(SavableData));
            using var stream = new FileStream(SavePath, FileMode.Open);

            var data = (SavableData)serializer.ReadObject(stream);
            _states = data.Content;
        }
    }

    public void Save()
    {
        var data = new SavableData(_states);
        var serializer = new DataContractSerializer(typeof(SavableData));
        using var stream = new FileStream(SavePath, FileMode.OpenOrCreate);

        serializer.WriteObject(stream, data);
        SavingCompleted?.Invoke(this, EventArgs.Empty);
    }

    public void RegisterEntity(string id, SavableEntity entity)
    {
        if (!_registeredEntities.ContainsKey(id))
        {
            _registeredEntities.Add(id, entity);

            if (_states.TryGetValue(id, out var state))
            {
                entity.SetState(state);
            }
        }
    }

    public void DeregisterEntity(string id)
    {
        if (_registeredEntities.TryGetValue(id, out var entity))
        {
            _states.Add(id, entity.GetState());
            _registeredEntities.Remove(id);
        }
    }

    public void SetSavingPeriod(float value) => _savingPeriod = Mathf.Clamp(value, MinSavingPeriod, MaxSavingPeriod);

    public void SetDefaultSavingPeriod() => SetSavingPeriod(DefaultSavingPeriod);

    public object CaptureState() => _savingPeriod;

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

        _savingPeriod = (float)state;
    }
}
