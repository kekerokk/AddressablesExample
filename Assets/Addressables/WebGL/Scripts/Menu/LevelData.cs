using System;
using UnityEngine;
[Serializable]
public struct LevelData {
    [field: SerializeField] public string name { get; private set; }
    [field: SerializeField] public string sceneAddressName { get; private set; }
}
