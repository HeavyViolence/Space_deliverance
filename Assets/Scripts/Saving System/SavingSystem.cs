using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public sealed class SavingSystem : GlobalInstance<SavingSystem>, ISavable
{
    private const string SavegameFileName = "Space deliverance";
    private const string SavegameFileExtension = ".save";
    private const string PublicKey = "Ldr7uh20";
    private const string PrivateKey = "xmjG8i13";

    private const float MinSavegamePeriod = 60f;
    private const float MaxSavegamePeriod = 600f;
    private const float DefaultSavegamePeriod = 300f;
    private const float SavegameDelay = 10f;

    private readonly Type[] _knownSavableTypes = { typeof(object[]),
                                                   typeof(AudioPlayerSavableData) };

    public event EventHandler SavegameAboutToStart;
    public event EventHandler SavegameCompleted;

    private readonly HashSet<SavableEntity> _registeredEntities = new();
    private Dictionary<string, IEnumerable<object>> _storedStates = new();

    private string SavegameFileFullName => SavegameFileName + SavegameFileExtension;
    private string SavegamePath => Path.Combine(Application.persistentDataPath, SavegameFileFullName);

    public float SavegamePeriod { get; private set; } = DefaultSavegamePeriod;
    public bool SavegameLoaded { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();

        SavegameLoaded = Load();
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

            SavegameAboutToStart?.Invoke(this, new SavegameAboutToStartEventArgs(SavegameDelay));

            yield return new WaitForSeconds(SavegameDelay);

            Save();
        }
    }

    public void Save()
    {
        PrepareSavableData();

        using StreamWriter writer = new(SavegamePath);

        string rawData = AuxMath.SerializeObjectToString(_storedStates);
        string encodedData = AuxMath.Encode(rawData, PublicKey, PrivateKey);

        writer.Write(encodedData);
        writer.Close();

        SavegameCompleted?.Invoke(this, EventArgs.Empty);
    }

    private bool Load()
    {
        if (File.Exists(SavegamePath))
        {
            try
            {
                using StreamReader reader = new(SavegamePath);

                string encodedData = reader.ReadToEnd();
                string decodedData = AuxMath.Decode(encodedData, PublicKey, PrivateKey);

                _storedStates = AuxMath.DeserializeStringToObject<Dictionary<string, IEnumerable<object>>>(decodedData, _knownSavableTypes);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }

    private void PrepareSavableData()
    {
        foreach (var entity in _registeredEntities)
        {
            _storedStates[entity.ID] = entity.GetState();
        }
    }

    public bool Register(SavableEntity entity)
    {
        if (entity == null)
        {
            return false;
        }

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
        if (entity == null)
        {
            return false;
        }

        if (_registeredEntities.Contains(entity))
        {
            _storedStates[entity.ID] = entity.GetState();
            _registeredEntities.Remove(entity);

            return true;
        }

        return false;
    }

    public void SetSavegamePeriod(float value) => SavegamePeriod = Mathf.Clamp(value, MinSavegamePeriod, MaxSavegamePeriod);

    public void SetDefaultSavegamePeriod() => SetSavegamePeriod(DefaultSavegamePeriod);

    public object CaptureState() => SavegamePeriod;

    public void RestoreState(object state)
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state), $"Passed state is null!");
        }

        if (state is float value)
        {
            SavegamePeriod = value;
        }
        else
        {
            throw new ArgumentException($"Passed state must be of a {typeof(float)} type!", nameof(state));
        }
    }
}
