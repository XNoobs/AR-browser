using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Security.Cryptography;

public class AppConfigHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private InputField _inputHeight;
    private InputField _inputWidth;
    private InputField _inputSquareSize;
    public float movementX;
    public float movementZ;
    public uint height=10;
    public uint width=10;
    public uint padding=6;
    
    void Start()
    {
        _inputHeight = GameObject.Find("size_x").GetComponent<InputField>();
        _inputWidth = GameObject.Find("size_z").GetComponent<InputField>();
        _inputSquareSize = GameObject.Find("squareSize").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        uint currentHeight;
        uint currentWidth;
        uint currentPadding;
        
        UInt32.TryParse(_inputHeight.text, out currentHeight);
        UInt32.TryParse(_inputWidth.text, out currentWidth);
        UInt32.TryParse(_inputSquareSize.text, out currentPadding);
        currentPadding += 1; //padding 1 pixel bigger than square size
        if (currentHeight != height || currentWidth != width || currentPadding != padding)
        {
            height = currentHeight;
            width = currentWidth;
            padding = currentPadding;
            
            if (height % 2 == 0)
            {
                movementX = Convert.ToSingle((height / 2 - 0.5) * padding);
            }
            else
            {
                movementX = height / 2 * padding;
            }

            if (width % 2 == 0)
            {
                movementZ = Convert.ToSingle((width / 2 - 0.5) * padding);
            }
            else
            {
                movementZ = width / 2 * padding;
            }  
        }
        
        
    }
}
