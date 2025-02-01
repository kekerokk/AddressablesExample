using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "Scriptable Objects/LevelsData")]
public class LevelsData : ScriptableObject {
    public List<LevelData> levels;
}