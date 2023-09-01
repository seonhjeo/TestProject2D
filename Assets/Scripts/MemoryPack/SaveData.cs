using MemoryPack;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 저장 데이터
/// </summary>
[MemoryPackable]
[Serializable]
public partial class SaveData
{
    /// <summary>
    /// ID
    /// </summary>
    /// <value></value>
    [MemoryPackOrder(0)]
    public int Id { get; protected set; } = default;

    /// <summary>
    /// 저장 일시시
    /// </summary>
    /// <value></value>
    [MemoryPackOrder(1)]
    public string Message { get; protected set; } = null;

    [MemoryPackOrder(2)]
    public Dictionary<int, string> KeyValue { get; protected set; } = null;
    
    [MemoryPackOrder(3)]
    public Vector3 VectorData { get; protected set; } = default;

    /// <summary>
    /// 빈 데이터 플래그
    /// </summary>
    [MemoryPackIgnore]
    public bool IsEmpty => this.Id == default;

    public SaveData()
    {
    }

    [MemoryPackConstructor]
    public SaveData(int id, string message, Dictionary<int, string> keyValue, Vector3 vectorData)
    {
        this.Id = id;
        this.Message = message;
        KeyValue = keyValue;
        VectorData = vectorData;
    }
}
