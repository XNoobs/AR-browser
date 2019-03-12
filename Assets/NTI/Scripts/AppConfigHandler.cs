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
    public uint height = 4;
    public uint width = 4;
    public Vector3 scale;
    public float scaleX;
    public float scaleZ;
    public Vector3 squareRenderer;
    
    void Start()
    {
        var canvas = GameObject.Find("Canvas");
        targetSearcher = canvas.GetComponent<TargetSearcherScript>();
        squareRenderer = square.GetComponent<SpriteRenderer>().bounds.size;
        
        _inputHeight = GameObject.Find("size_x").GetComponent<InputField>();
        _inputWidth = GameObject.Find("size_z").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        uint currentHeight=4;
        uint currentWidth=4;
        
        UInt32.TryParse(_inputHeight.text, out currentHeight);
        UInt32.TryParse(_inputWidth.text, out currentWidth);
        height = currentHeight;
        width = currentWidth;
        scaleX = (targetSearcher.Width / width) / squareRenderer.x;
        scaleZ = (targetSearcher.Height / height) / squareRenderer.z;

        movementX = targetSearcher.CenterPosition.x - targetSearcher.Width / 2;
        movementZ = targetSearcher.CenterPosition.z - targetSearcher.Height / 2;
        scale = new Vector3(scaleX, 0, scaleZ); 
    }
}
