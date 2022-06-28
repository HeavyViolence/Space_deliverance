using System;
using System.Collections.Generic;

public sealed class RangedFloat : IEquatable<RangedFloat>, IComparable<RangedFloat>, IComparer<RangedFloat>
{
    private readonly float _min, _max;

    private float LowestRangeValue => PivotValue - RangeValue;
    private float HighestRangeValue => PivotValue + RangeValue;

    public float PivotValue { get; }
    public float RangeValue { get; }
    public float MinValue => LowestRangeValue < _min ? _min : LowestRangeValue;
    public float MaxValue => HighestRangeValue > _max ? _max : HighestRangeValue;
    public float RandomValue => UnityEngine.Random.Range(MinValue, MaxValue + 1);

    public RangedFloat(float pivotValue, float rangeValue, float minValue = float.MinValue, float maxValue = float.MaxValue)
    {
        _min = minValue;
        _max = maxValue;

        PivotValue = pivotValue;
        RangeValue = AuxMath.EnsurePositiveValue(rangeValue);
    }

    public bool Equals(RangedFloat other)
    {
        if (other == null) return false;

        return other.PivotValue.Equals(PivotValue) &&
               other.MinValue.Equals(MinValue) &&
               other.MaxValue.Equals(MaxValue);
    }

    public int CompareTo(RangedFloat other)
    {
        if (MaxValue < other.MaxValue) return -1;
        else if (MaxValue > other.MaxValue) return 1;
        else return 0;
    }

    public int Compare(RangedFloat x, RangedFloat y)
    {
        if (x.MaxValue < y.MaxValue) return -1;
        else if (x.MaxValue > y.MaxValue) return 1;
        else return 0;
    }

    public override int GetHashCode() => PivotValue.GetHashCode() ^ MinValue.GetHashCode() ^ MaxValue.GetHashCode();

    public override string ToString() => $"Pivot = {PivotValue}; min = {MinValue}; max = {MaxValue}";
}