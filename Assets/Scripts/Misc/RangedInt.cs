using System;
using System.Collections.Generic;

public sealed class RangedInt : IEquatable<RangedInt>, IComparable<RangedInt>, IComparer<RangedInt>
{
    private readonly int _min, _max;

    private int LowestRangeValue => PivotValue - RangeValue;
    private int HighestRangeValue => PivotValue + RangeValue;

    public static RangedInt Zero => new(0, 0, 0, 0);
    public int PivotValue { get; }
    public int RangeValue { get; }
    public int MinValue => LowestRangeValue < _min ? _min : LowestRangeValue;
    public int MaxValue => HighestRangeValue > _max ? _max : HighestRangeValue;
    public int RandomValue => UnityEngine.Random.Range(MinValue, MaxValue + 1);

    public RangedInt(int pivotValue, int rangeValue, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        _min = minValue;
        _max = maxValue;

        PivotValue = pivotValue;
        RangeValue = Math.Abs(rangeValue);
    }

    public bool Equals(RangedInt other)
    {
        if (other == null) return false;

        return other.PivotValue.Equals(PivotValue) &&
               other.MinValue.Equals(MinValue) &&
               other.MaxValue.Equals(MaxValue);
    }

    public int CompareTo(RangedInt other)
    {
        if (MaxValue < other.MaxValue) return -1;
        else if (MaxValue > other.MaxValue) return 1;
        else return 0;
    }

    public int Compare(RangedInt x, RangedInt y)
    {
        if (x.MaxValue < y.MaxValue) return -1;
        else if (x.MaxValue > y.MaxValue) return 1;
        else return 0;
    }

    public override int GetHashCode() => PivotValue.GetHashCode() ^ MinValue.GetHashCode() ^ MaxValue.GetHashCode();

    public override string ToString() => $"Pivot = {PivotValue}; min = {MinValue}; max = {MaxValue}";
}