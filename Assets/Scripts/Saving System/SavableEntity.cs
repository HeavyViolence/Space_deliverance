using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class SavableEntity : MonoBehaviour, IEquatable<SavableEntity>
{
    [SerializeField] private string _id;

    private readonly List<ISavable> _savables = new();
    private bool _registered = false;

    private bool SavingRequired => _savables.Count > 0;

    public string ID => _id;

    private void Awake()
    {
        FindSavableComponents();
    }

    private void Start()
    {
        StartCoroutine(RegisterItself());
    }

    private void OnDisable()
    {
        DeregisterItself();
    }

    private void FindSavableComponents()
    {
        foreach (var savable in transform.root.gameObject.GetComponentsInChildren<ISavable>())
        {
            _savables.Add(savable);
        }
    }

    private IEnumerator RegisterItself()
    {
        yield return SavingSystem.Instance != null;

        if (SavingRequired)
        {
            _registered = SavingSystem.Instance.Register(this);
        }
    }

    private void DeregisterItself()
    {
        if (_registered)
        {
            _registered = !SavingSystem.Instance.Deregister(this);
        }
    }

    public object GetState()
    {
        List<object> states = new(_savables.Count);

        foreach (var savable in _savables)
        {
            states.Add(savable.CaptureState());
        }

        return states;
    }

    public void SetState(object state)
    {
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state), "Attempted to set an invalid state!");
        }

        var capturedStates = (List<object>)state;

        for (int i = 0; i < _savables.Count; i++)
        {
            _savables[i].RestoreState(capturedStates[i]);
        }
    }

    public bool Equals(SavableEntity other)
    {
        if (other == null) return false;

        return other.ID.Equals(ID);
    }

    public override int GetHashCode() => ID.GetHashCode();

    public override string ToString() => $"{nameof(SavableEntity)} ID: {ID}";
}