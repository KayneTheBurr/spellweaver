using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Header("Ability Info")]
    public List<AbilityData> mySpells = new List<AbilityData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayerCamera.instance.player = this;
        PlayerInputManager.instance.player = this;
    }

    void Update()
    {
        PlayerCamera.instance.HandleAllCameraAction();
    }

    public void AssignAbility (int spellListIndex, AbilityData ability)
    {

    }
}
