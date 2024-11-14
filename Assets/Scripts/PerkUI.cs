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

    [SerializeField] AudioClip selectClip;
    [SerializeField] AudioClip pointerEnterClip;

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

        AudioManager.Instance.PlaySFX(selectClip);

        PerkUIManager.Instance.PlayPerkOutAnimation();
    }    

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightBorder.SetActive(true);
        AudioManager.Instance.PlaySFX(pointerEnterClip);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightBorder.SetActive(false);
    }
}
