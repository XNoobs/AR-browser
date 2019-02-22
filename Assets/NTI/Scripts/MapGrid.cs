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
        private uint height = 10;
        private uint width = 10;
        private readonly Color _activeCellColor = Color.red;
        private InputField _inputHeight;
        private InputField _inputWidth;
        private GameObject[,] _grid;
        private GameObject menuBtn;


        private void CreateMenuButton()
        {
            square.transform.localScale = new Vector3(5, 5, 0);
            var position = new Vector3(0, 0, 0);
            menuBtn = Instantiate(square, position, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
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

            var position = new Vector3(0, 0, padding);
            menuBtn.transform.position = position;
            menuBtn.SetActive(true);
        }

        private void DrawGrid()
        {
            UInt32.TryParse(_inputHeight.text, out height);
            UInt32.TryParse(_inputWidth.text, out width);

            float movementX;
            float movementZ;

            _grid = new GameObject[height, width];
            if (height % 2 == 0)
            {
                movementX = Convert.ToSingle((height / 2 - 0.5) * squareSize);
            }
            else
            {
                movementX = height / 2 * squareSize;
            }

            if (width % 2 == 0)
            {
                movementZ = Convert.ToSingle((width / 2 - 0.5) * squareSize);
            }
            else
            {
                movementZ = width / 2 * squareSize;
            }


            square.transform.localScale = new Vector3(5, 5, 0);

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var position = new Vector3(i * squareSize - movementX, 0, j * squareSize - movementZ);
                    var current = Instantiate(square, position, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
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
                    Destroy(_grid[i, j]);
                }
            }
        }


        private void Start()
        {
            userInterface.enabled = true;
            CreateMenuButton();
            DrawMenuButton(true);
            //caching objects refs to increase performance
            _inputHeight = GameObject.Find("size_x").GetComponent<InputField>();
            _inputWidth = GameObject.Find("size_z").GetComponent<InputField>();
        }

        private void Update()
        {
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                var raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;

                if (Physics.Raycast(raycast, out raycastHit))
                {
                    if (userInterface.enabled == false)
                    {
                        if (raycastHit.collider.name != "menu")
                        {
                            var name = raycastHit.collider.name.Split('_');
                            int[] coordinates = {Convert.ToInt16(name[0]), Convert.ToInt16(name[1])};
                            var sprite = _grid[coordinates[0], coordinates[1]].GetComponent<SpriteRenderer>();
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

                    if (raycastHit.collider.name == "menu")
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
                }
            }
        }
    }
}