using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PerkUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{    
    [SerializeField] TextMeshProUGUI titleTextMeshPro;
    [SerializeField] Image perkIcon;
    [SerializeField] TextMeshProUGUI descriptionTextMeshPro;
    [SerializeField] GameObject highlightBorder;

    [SerializeField] Button button;

    IEnumerator enumerator = null;

    PerkData.Type type;

    Perks selectedPerk;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void FeedDataToUI(Perks perk)
    {
        titleTextMeshPro.text = perk.Data.title;
        perkIcon = perk.Data.icon;
        descriptionTextMeshPro.text = perk.Data.description;
        type = perk.Data.type;

        selectedPerk = perk;
    }

    void OnButtonClick()
    {
        selectedPerk.Apply();

        StartCoroutine(PerkIsSelected());
    }

    IEnumerator PerkIsSelected()
    {
        PerkUIManager.Instance.DestroyPerkPrefabs();

        if (PerkUIManager.Instance.CheckEmptyQueue() == true)
        {
            GameManager.Instance.ClosePerkSelectionScreen();
        }

        yield return null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightBorder.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightBorder.SetActive(false);
    }
}
