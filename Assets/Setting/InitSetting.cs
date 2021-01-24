using UnityEngine;
using Assets.Core.Settings;


public class InitSetting
{
    [SerializeField]
    public GameObject player;

    void Start()
    {
        GlobalSetting.targetReaction = player;
    }
}

