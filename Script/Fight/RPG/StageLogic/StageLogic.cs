using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class WaveInfo
{
    [SerializeField]
    public List<string> NPCs = new List<string>();
}

public class StageLogic : MonoBehaviour
{
    [SerializeField]
    public List<WaveInfo> _Waves = new List<WaveInfo>();
}
