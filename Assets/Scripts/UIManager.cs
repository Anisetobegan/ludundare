
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{

    [SerializeField] private List<Sprite> spriteList;

    private Dictionary<string, Sprite> spriteDictionary = new Dictionary<string, Sprite>();

    [SerializeField] private GameObject summonIndicator;

    [SerializeField] private List<GameObject> summonIndicatorList;

    [SerializeField] private GameObject panelUI;

    public static UIManager Instance
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
        spriteDictionary.Add("Ghost", spriteList[0]);
        spriteDictionary.Add("Skeleton", spriteList[1]);
        spriteDictionary.Add("Zombie", spriteList[2]);
    }

    public void UpdateSummon(Summon selectedSummon)
    {

        GameObject newSummonIndicator = Instantiate(summonIndicator, transform.position, transform.rotation);
        newSummonIndicator.transform.SetParent(panelUI.transform);

        summonIndicatorList.Add(newSummonIndicator);

        UpdateSummonImage(selectedSummon.GetSummonName(), newSummonIndicator.gameObject.GetComponentInChildren<Image>());
        UpdateSummonName(selectedSummon.GetSummonName(), newSummonIndicator.gameObject.GetComponentInChildren<TextMeshProUGUI>());
    }

    private void UpdateSummonImage(string summonName, Image instantiatedImage)
    {
        instantiatedImage.sprite = spriteDictionary[summonName];        
    }

    private void UpdateSummonName(string summonName, TextMeshProUGUI summonTextMesh)
    {
        summonTextMesh.text = summonName;
    }

    public void ClearSelectedSummons()
    {
        if (summonIndicatorList.Count > 0)
        {
            for (int i = 0; i < summonIndicatorList.Count; i++)
            {
                GameObject.Destroy(summonIndicatorList[i]);
            }
            summonIndicatorList.Clear();                    
        }
    }

    

}
