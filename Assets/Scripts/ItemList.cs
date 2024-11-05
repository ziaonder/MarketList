using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    private List<string> list = new List<string>();
    public static event Action OnListChange;
    [SerializeField] private GameObject itemPrefab, itemParent;

    private void Awake()
    {
        if (FindObjectsOfType<ItemList>().Length > 1)
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(string item)
    {
        // No duplicates
        foreach(string listItem in list)
        {
            if (listItem == item)
            {
                return;
            }
        }
        
        list.Add(item);
        Vector3 pos = new Vector3(Screen.width / 2, Screen.height - 200 - 130 / 2 - 250, 0f);
        Instantiate(itemPrefab, pos, Quaternion.identity, itemParent.transform);
        OnListChange?.Invoke();
    }

    public void RemoveItem(string item)
    {
        list.Remove(item);
        OnListChange?.Invoke();
    }

    public List<string> GetList()
    {
        return list;
    }
}
