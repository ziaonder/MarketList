using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private Button okButton, exportButton;
    private ItemList itemList;
    [SerializeField] private Transform itemParent;
    [SerializeField] private Transform inputField;
    [SerializeField] private RectTransform contentTransform;

    public Transform InputField { get => inputField; }

    private void Awake()
    {
        if(FindObjectsOfType<UIManager>().Length > 1)
            Destroy(gameObject);
    }

    private void OnEnable()
    {
        okButton.onClick.AddListener(OnOkButtonClicked);
        exportButton.onClick.AddListener(OnShareButtonClicked);
    }

    private void OnDisable()
    {
        okButton.onClick.RemoveListener(OnOkButtonClicked);
        exportButton.onClick.RemoveListener(OnShareButtonClicked);
    }

    private void Start()
    {
        itemList = FindObjectOfType<ItemList>();
    }

    public void OnOkButtonClicked()
    {
        if(inputText.text == string.Empty)
            return;

        itemList.AddItem(inputText.text);
        inputText.text = string.Empty;
    }

    public void OnShareButtonClicked()
    {
        string shareText = "";
        ItemController[] itemconts = FindObjectsOfType<ItemController>();
        foreach(ItemController item in itemconts)
        {
            if(item.itemData.toggle)
                shareText += item.itemData.name + " " + item.itemData.quantity + "\n";
        }

        new NativeShare().AddTarget("com.whatsapp").SetText(shareText).Share();
    }
}
