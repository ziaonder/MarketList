using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ItemData
{
    public string name;
    public bool toggle;
    public string quantity;
}

public class ItemController : MonoBehaviour, IMovable
{
    private float distancebetweenItems = 250f;
    public enum ItemState { Moving, Idle }
    private ItemState itemState = ItemState.Idle;
    private float startingPos, targetPos, moveTime = 0, anchorPos;
    private ItemList itemList;
    private string itemName;
    private int distanceMultiplier = 0;
    private ItemData itemData;
    private Button removeButton;

    private void Awake()
    {
        itemList = FindObjectOfType<ItemList>();
        itemName = itemList.GetList()[itemList.GetList().Count - 1];
        anchorPos = Screen.height - 200 - 130 / 2;
        removeButton = transform.Find("RemoveButton").GetComponent<Button>();
    }

    private void Start()
    {
        itemData = new ItemData();
        transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = itemName;
        OnDataChange();
    }

    private void OnEnable()
    {
        ItemList.OnListChange += HandleIndexChange;
        removeButton.onClick.AddListener(OnRemoveButtonClicked);
    }

    private void OnDisable()
    {
        ItemList.OnListChange -= HandleIndexChange;
        removeButton.onClick.RemoveListener(OnRemoveButtonClicked);
    }

    private void Update()
    {
        if(itemState == ItemState.Moving)
        {
            moveTime += Time.deltaTime * 8;
            if(moveTime > 1)
                moveTime = 1;
            transform.position = new Vector3(transform.position.x,
                Mathf.Lerp(startingPos, targetPos, moveTime), transform.position.z); 
            if(moveTime >= 1)
            {
                moveTime = 0;
                itemState = ItemState.Idle;
            }
        }
    }

    public void OnDataChange()
    {
        itemData.name = transform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text;
        itemData.toggle = transform.Find("Toggle").GetComponent<Toggle>().isOn;
        itemData.quantity = transform.Find("Quantity").Find("Text Area").Find("Text").
            GetComponent<TMPro.TextMeshProUGUI>().text;
    }

    public void OnRemoveButtonClicked()
    {
        itemList.RemoveItem(itemName);
        Destroy(gameObject);
    }

    private void HandleIndexChange()
    {
        int index = -1;
        for(int i = 0; i < itemList.GetList().Count; i++)
        {
            if (itemList.GetList()[i] == itemName)
            {
                index = i;
            }
        }
        
        if (index == -1)
            return;

        if (distanceMultiplier == itemList.GetList().Count - index)
            return;
        else
        {
            distanceMultiplier = itemList.GetList().Count - index;
            Move();
        }
    }

    public void Move()
    {
        startingPos = transform.position.y;
        targetPos = anchorPos - distanceMultiplier * distancebetweenItems;
        itemState = ItemState.Moving;
    }
}
