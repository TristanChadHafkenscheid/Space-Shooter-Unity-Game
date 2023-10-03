﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUp")]
public class PowerUp : ScriptableObject
{
    public string powerUpName;
    public string description;

    public Sprite artwork;
}