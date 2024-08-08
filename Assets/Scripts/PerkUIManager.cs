using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkUIManager : MonoBehaviour
{
    [SerializeField] GameObject perkPrefab;

    int perksToChoose = 3;

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

    private void OnEnable()
    {
        Actions.OnLevelUp += PerkSelection;
        Actions.OnWaveWon += PerkSelection;
    }

    private void OnDisable()
    {
        Actions.OnLevelUp -= PerkSelection;
        Actions.OnWaveWon -= PerkSelection;
    }

    void PerkSelection()
    {
        List<PerkData> newDataList = new List<PerkData>(PerkManager.Instance.PerkDataList); //Declares a List of PerkData to generate one at random

        for (int i = 0; i < perksToChoose; i++)
        {
            GameObject newPerkPrefab = null;
            int randomIndex = Random.Range(0, newDataList.Count); //Generates random number between 0 and the count of total perks available

            newPerkPrefab = Instantiate(perkPrefab, transform.position, transform.rotation); //Instantiates the perk UI GameObject
            newPerkPrefab.transform.SetParent(this.transform);

            Perks newPerk = CreatePerkType(newDataList[randomIndex].type); //Generates a new Perk
            newPerk.Data = newDataList[randomIndex]; //Recieves the data of the perk randomized

            
            
            newPerkPrefab.GetComponent<PerkUI>().FeedDataToUI(newPerk); //Sends the generated Perk to the UIManager to fill data
            newDataList.RemoveAt(randomIndex); //Removes the data from the DataList to avoid repeated perks
            
        }
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

            case PerkData.Type.ExtraMoveSpeed:
                newPerk = new ExtraMoveSpeed();
                break;

            case PerkData.Type.FasterCast:
                newPerk = new FasterCast();
                break;

            case PerkData.Type.CastCoolDown:
                newPerk = new CastCoolDown();
                break;

            default:
                break;
        }

        return newPerk;

    }

    public void DestroyPerkPrefabs()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        }
    }
}
