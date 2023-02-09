using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Sprite_Swap : MonoBehaviour
{
    public Sprite[]  Spritecolor;
    public Image[] objects;
    public static Sprite_Swap Instance;

    private void Awake()
    {
        Instance = this;
    }

}

