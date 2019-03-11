using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using NTI.Scripts;

public class AppConfigHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TargetSearcherScript targetSearcher;
    [SerializeField] private GameObject square;
    private InputField _inputHeight;
    private InputField _inputWidth;
    public float movementX;
    public float movementZ;
    public uint height = 10;
    public uint width = 10;
    public uint padding = 1;
    public Vector3 scale;
    public float scaleX;
    public float scaleZ;
    
    void Start()
    {
        var canvas = GameObject.Find("Canvas");
        targetSearcher = canvas.GetComponent<TargetSearcherScript>();
        
        
        _inputHeight = GameObject.Find("size_x").GetComponent<InputField>();
        _inputWidth = GameObject.Find("size_z").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        uint currentHeight;
        uint currentWidth;
        
        UInt32.TryParse(_inputHeight.text, out currentHeight);
        UInt32.TryParse(_inputWidth.text, out currentWidth);
        height = currentHeight;
        width = currentWidth;

        scaleX = targetSearcher.Width / width / square.transform.localScale.x;
        scaleZ = targetSearcher.Height / height / square.transform.localScale.y;
        
        movementX = (targetSearcher.Width + scaleX) / 2;
        movementZ = (targetSearcher.Height + scaleZ) / 2;
        movementX += targetSearcher.CenterPosition.x;
        movementZ += targetSearcher.CenterPosition.z;
        scale = new Vector3(scaleX, 0, scaleZ); 
    }
}
