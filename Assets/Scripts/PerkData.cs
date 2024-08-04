using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PerkData", menuName = "Perk Data")]
public class PerkData : ScriptableObject
{
    public string title;
    public Image icon;
    public string description;
    public enum Type
    {
        ExtraSummon,
        AddHealth,
        AddSummonHealth
    }
    public Type type;
}
