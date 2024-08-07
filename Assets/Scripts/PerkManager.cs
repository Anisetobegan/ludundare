using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkManager : MonoBehaviour
{
    [SerializeField] List<PerkData> dataList;
    public List<Perks> perkList;

    public List<PerkData> PerkDataList { get { return dataList; } }

    public int PerkDataCount {  get { return dataList.Count; } }
    public static PerkManager Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
