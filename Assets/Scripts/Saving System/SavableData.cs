using System.Collections.Generic;
using System.Runtime.Serialization;

[DataContract]
public sealed class SavableData
{
    [DataMember] public Dictionary<string, object> Content { get; }

    public SavableData(Dictionary<string, object> data)
    {
        Content = data;
    }
}
