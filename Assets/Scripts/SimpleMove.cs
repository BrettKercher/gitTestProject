﻿using UnityEditor;
using UnityEngine;

public class SimpleMove : MonoBehaviour
{
    public CharacterController Controller;
    public float Speed = 11;

    // Update is called once per frame
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        
        Debug.Log(BuildTarget.GameCoreXboxSeries);

        Controller.SimpleMove(new Vector3(h * Speed, 0, v * Speed));
    }
}