using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private Button okButton;
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
        ItemList.OnListChange += AdjustContentSize;
    }

    private void OnDisable()
    {
        okButton.onClick.RemoveListener(OnOkButtonClicked);
        ItemList.OnListChange -= AdjustContentSize;
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

    private void AdjustContentSize()
    {
        Vector3 viewportPosition;
        contentTransform.offsetMin = new Vector2(0f, -400f);

        for (int i = 0; i < contentTransform.childCount; i++)
        {
            RectTransform child = contentTransform.GetChild(i).GetComponent<RectTransform>();
            viewportPosition = Camera.main.WorldToViewportPoint(child.position);

            if (viewportPosition.y < 0)
            {
                contentTransform.offsetMin -= new Vector2(0f, 250f);
            }
        }
    }
}
