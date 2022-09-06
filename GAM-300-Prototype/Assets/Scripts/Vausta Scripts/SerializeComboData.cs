using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SerializeComboData : MonoBehaviour
{
    public void Start()
    {
        LoadFromJSON();
    }
    public void LoadFromJSON()
    {
        string json = File.ReadAllText(Application.dataPath + "/JSON_Data/SavedComboData.json");
        ComboData comboData = JsonUtility.FromJson<ComboData>(json);
        LoadComboData(comboData);
    }
    private void LoadComboData(ComboData comboData)
    {
        comboData.slot1 = new List<string>();
        comboData.slot2 = new List<string>();
        comboData.slot3 = new List<string>();
        comboData.slot4 = new List<string>();
        foreach (string i  in comboData.slot1)
        {
            int count = 0;
            comboData.slot1[count] = comboData.slot1[count];
            count++;
        }
        
        foreach (string i in comboData.slot2)
        {
            int count = 0;
            comboData.slot2[count] = comboData.slot2[count];
            count++;
        }
        foreach (string i in comboData.slot3)
        {
            int count = 0;
            comboData.slot3[count] = comboData.slot3[count];
            count++;
        }
        foreach (string i in comboData.slot4)
        {
            int count = 0;
            comboData.slot4[count] = comboData.slot4[count];
            count++;
        }
    }
}
