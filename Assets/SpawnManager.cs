using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public Spawner spawner;

    public int NoOfPhonesToCollect = 6;
    public int CurrentPhones = 0;

    public TextMeshProUGUI phoneText;
    private void Awake()
    {
        phoneText.text = "0/" + NoOfPhonesToCollect.ToString();
        if (instance == null)
        {
            instance = this;
        }
    }
    public void UpdatePhones()
    {
        CurrentPhones += 1;
        if(phoneText != null)
        {
            phoneText.text = CurrentPhones.ToString() + "/" + NoOfPhonesToCollect.ToString();
        }
    }
    private void Update()
    {
        if(CurrentPhones >= NoOfPhonesToCollect)
        {
            spawner.enabled = false;

        }
    }


}
