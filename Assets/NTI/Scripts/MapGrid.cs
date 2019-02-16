using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.UI;

namespace NTI.Scripts
{
    public class MapGrid : MonoBehaviour
    {
        [SerializeField] private uint squareSize = 7;
        [SerializeField] private GameObject square;
        [SerializeField] private Canvas userInterface;
        private Vector3 _position;
        private uint height = 10;
        private uint width = 10;
        private float movement_x = 0;
        private float movement_z = 0;
        private readonly Color _activeCellColor = Color.red;
        private GameObject[,] _grid;
        private GameObject menuBtn;


        private void CreateMenuButton()
        {
            square.transform.localScale = new Vector3(5, 5, 0);
            var position = new Vector3(0,0,0);
            menuBtn = Instantiate(square, _position, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
            menuBtn.transform.SetParent(this.transform);
            var menuBtnBoxCollider = menuBtn.AddComponent<BoxCollider>();
            menuBtnBoxCollider.name = "menu";
            
        }
        private void DrawMenuButton(bool UIOn)
        {
            var padding = 0f;
            if (UIOn)
            {
                padding = -28;
            }
            else
            {
                padding = -(width / 2 + 1) * squareSize;

            }
            
            _position = new Vector3(0, 0, padding);
            menuBtn.transform.position = _position;
            menuBtn.SetActive(true);
        }

        private void DrawGrid()
        {
            var inputHeight = GameObject.Find("size_x").GetComponent<InputField>();
            var inputWidth = GameObject.Find("size_z").GetComponent<InputField>();
            UInt32.TryParse(inputHeight.text, out height);
            UInt32.TryParse(inputWidth.text, out width);
            
            _grid = new GameObject[height,width];
            if (height % 2 == 0)
            {
                movement_x = Convert.ToSingle((height / 2 - 0.5) * squareSize);
            }
            else
            {
                movement_x = height / 2 * squareSize;
            }

            if (width % 2 == 0)
            {
                movement_z = Convert.ToSingle((width / 2 - 0.5) * squareSize);
            }
            else
            {
                movement_z = width / 2 * squareSize;
            }


            square.transform.localScale = new Vector3(5, 5, 0);

            for (var i = 0; i < height; i++)
            {

                for (var j = 0; j < width; j++)
                {

                    _position = new Vector3(i * squareSize - movement_x, 0, j * squareSize - movement_z);
                    var current = Instantiate(square, _position, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
                    current.SetActive(true);
                    current.transform.SetParent(this.transform);
                    var currentBoxCollider = current.AddComponent<BoxCollider>();
                    currentBoxCollider.name = i.ToString() + '_' + j.ToString();
                    _grid[i, j] = current;
                }
            }

        }

        private void DeleteGrid()
        {
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    Destroy(_grid[i,j]);
                }
            }
        }


        private void Start()
        {
            userInterface.enabled = true;
            CreateMenuButton();
            DrawMenuButton(true);
            
        }

        private void Update()
        {

            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                var raycastMenu = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHitMenu;
                if (Physics.Raycast(raycastMenu, out raycastHitMenu))
                {
                    
                    if (raycastHitMenu.collider.name == "menu")
                    {
                        if (userInterface.enabled == false)
                        {
                            DeleteGrid();
                            userInterface.enabled = true;
                            DrawMenuButton(true);

                        }
                        else
                        {
                            userInterface.enabled = false;
                            DrawGrid();
                            DrawMenuButton(false);

                        }
                    }

                    if (userInterface.enabled == false)
                    {
                        for (var i = 0; i < height; i++)
                        {
                            for (var j = 0; j < width; j++)
                            {
                                var raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                                RaycastHit raycastHit;
                                if (Physics.Raycast(raycast, out raycastHit))
                                {
                                    if (raycastHit.collider.name == i.ToString() + '_' + j.ToString())
                                    {
                                        var sprite = _grid[i, j].GetComponent<SpriteRenderer>();
                                        if (sprite.color != _activeCellColor)
                                        {
                                            sprite.color = _activeCellColor;
                                        }
                                        else
                                        {
                                            var tmpColor = square.GetComponent<SpriteRenderer>().color;
                                            sprite.color = tmpColor;
                                        }

                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
    }
}