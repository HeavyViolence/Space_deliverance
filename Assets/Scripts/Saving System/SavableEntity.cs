using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public sealed class SavableEntity : MonoBehaviour, IEquatable<SavableEntity>
{
    [SerializeField] private string _id;

    private readonly List<ISavable> _savables = new();

    private bool SavingRequired => _savables.Count > 0;

    public string ID => _id;

    private void Awake()
    {
        FindSavableComponents();
    }

    private IEnumerator Start()
    {
        yield return SavingSystem.Instance != null;

        if (SavingRequired)
        {
            SavingSystem.Instance.RegisterEntity(_id, this);
        }
    }

    private void OnDisable()
    {
        if (SavingRequired)
        {
            SavingSystem.Instance.DeregisterEntity(_id);
        }
    }

    private void FindSavableComponents()
    {
        foreach (var savable in transform.root.gameObject.GetComponentsInChildren<ISavable>())
        {
            _savables.Add(savable);
        }
    }

    public object GetState()
    {
        var states = new List<object>(_savables.Count);

        foreach (var savable in _savables)
        {
            states.Add(savable.CaptureState());
        }

        return states;
    }

    public void SetState(object state)
    {
        var capturedStates = (List<object>)state;

        for (int i = 0; i < _savables.Count; i++)
        {
            _savables[i].RestoreState(capturedStates[i]);
        }
    }

    public bool Equals(SavableEntity other)
    {
        if (other == null)
        {
            return false;
        }
        else
        {
            return other.ID.Equals(ID);
        }
    }
}