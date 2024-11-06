using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    private ItemList itemList;

    public class Wrapper
    {
        public List<string> items;
    }

    private void Awake()
    {
        if(FindObjectsOfType<SaveSystem>().Length > 1)
            Destroy(gameObject);

        itemList = FindObjectOfType<ItemList>();
    }

    private void OnEnable()
    {
        ItemList.OnListChange += Save;
    }

    private void OnDisable()
    {
        ItemList.OnListChange -= Save;
    }

    private void Start()
    {
        Load();
    }

    public void Save()
    {
        Wrapper wrapper = new Wrapper();
        wrapper.items = itemList.GetList();
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/save.json"))
        {
            string json = File.ReadAllText(Application.persistentDataPath + "/save.json");
            Wrapper wrapper = JsonUtility.FromJson<Wrapper>(json);
            wrapper.items.ForEach(item => itemList.AddItem(item));
        }
    }
}
