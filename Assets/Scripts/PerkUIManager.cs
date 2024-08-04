using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUIManager : MonoBehaviour
{
    [SerializeField] GameObject perkPrefab;

    IEnumerator enumerator = null;

    GameObject selectedPerk = null;

    public enum Type
    {
        ExtraSummon,
        AddHealth,
        AddSummonHealth
    }
    Type type;

    public static PerkUIManager Instance
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

    private void Start()
    {
        
    }

    private void Update()
    {
        if (enumerator == null)
        {
            enumerator = PerkSelection();
            StartCoroutine(enumerator);
        }
    }

    IEnumerator PerkSelection()
    {
        List<PerkData> newDataList = new List<PerkData>(PerkManager.Instance.PerkDataList); //Declares a List of PerkData to generate one at random

        for (int i = 0; i < PerkManager.Instance.PerkDataCount; i++)
        {
            GameObject newPerkPrefab = null;
            int randomIndex = Random.Range(0, newDataList.Count); //Generates random number between 0 and the count of total perks available

            newPerkPrefab = Instantiate(perkPrefab, transform.position, transform.rotation); //Instantiates the perk UI GameObject
            newPerkPrefab.transform.SetParent(this.transform);

            Perks newPerk = CreatePerkType(newDataList[randomIndex].type); //Generates a new Perk
            newPerk.Data = newDataList[randomIndex]; //Recieves the data of the perk randomized

            newPerk.Player = GameManager.Instance.PlayerGet;
            
            newPerkPrefab.GetComponent<PerkUI>().FeedDataToUI(newPerk); //Sends the generated Perk to the UIManager to fill data
            newDataList.RemoveAt(randomIndex); //Removes the data from the DataList to avoid repeated perks
            
        }

        while (selectedPerk == null)
        {
            yield return null;
        }

        enumerator = null;

    }

    Perks CreatePerkType(PerkData.Type type)
    {
        Perks newPerk = null;
        switch (type)
        {
            case PerkData.Type.ExtraSummon:
                newPerk = new ExtraSummon();
                break;

            case PerkData.Type.AddHealth:
                newPerk = new PlayerExtraHealth();
                break;
            case PerkData.Type.AddSummonHealth:
                newPerk = new SummonExtraHealth();
                break;
            default:
                break;
        }

        return newPerk;

    }
}
