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
        private uint _padding;
        [SerializeField] private GameObject square;
        [SerializeField] private Canvas userInterface;
        [SerializeField] private GameObject objectToPlace;
        private uint height = 10;
        private uint width = 10;
        private float movementX;
        private float movementZ;
        private InputField _inputHeight;
        private InputField _inputWidth;
        private InputField _inputSquareSize;
        private GameObject[,] _grid;
        private bool[,] _cellsVacated;
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
            var buttonPos = 0f;
            if (UIOn)
            {
                buttonPos = -28;
            }
            else
            {
                buttonPos = -(width / 2 + 1) * _padding;
            }

            var position = new Vector3(0, 0, buttonPos);
            menuBtn.transform.position = position;
            menuBtn.SetActive(true);
        }

        private void DrawGrid()
        {
            UInt32.TryParse(_inputHeight.text, out height);
            UInt32.TryParse(_inputWidth.text, out width);

            uint tmp;
            UInt32.TryParse(_inputSquareSize.text, out tmp);
            square.transform.localScale = new Vector3(tmp, tmp, 0);
            _padding = tmp + 1;

            _grid = new GameObject[height, width];
            _cellsVacated = new bool[height, width];
            
            if (height % 2 == 0)
            {
                movementX = Convert.ToSingle((height / 2 - 0.5) * _padding);
            }
            else
            {
                movementX = height / 2 * _padding;
            }

            if (width % 2 == 0)
            {
                movementZ = Convert.ToSingle((width / 2 - 0.5) * _padding);
            }
            else
            {
                movementZ = width / 2 * _padding;
            }

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    var position = new Vector3(i * _padding - movementX, 0, j * _padding - movementZ);
                    var current = Instantiate(square, position, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
                    current.SetActive(true);
                    current.transform.SetParent(this.transform);
                    var currentBoxCollider = current.AddComponent<BoxCollider>();
                    currentBoxCollider.name = i.ToString() + '_' + j.ToString();
                    _grid[i, j] = current;
                    _cellsVacated[i, j] = false;
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

        private void placeObject(GameObject objectToPlace, int[] coordinates, Quaternion rotation, bool vacation)
        {
            var position = new Vector3(coordinates[0] * _padding - movementX, 0, coordinates[1] * _padding - movementZ);
            var current = Instantiate(objectToPlace, position, rotation) as GameObject;
            current.SetActive(true);
            current.transform.SetParent(this.transform);
            var currentBoxCollider = current.AddComponent<BoxCollider>();
            currentBoxCollider.name = coordinates[0].ToString() + '_' + coordinates[1].ToString();
            _grid[coordinates[0], coordinates[1]] = current;
            _cellsVacated[coordinates[0], coordinates[1]] = vacation;
        }
        

        private void Start()
        {
            square.transform.localScale = new Vector3(5, 5, 0);
            userInterface.enabled = true;
            var squareSize = square.GetComponent<SpriteRenderer>().bounds.size;
            _padding = Convert.ToUInt16(squareSize.x+1);
            CreateMenuButton();
            DrawMenuButton(true);
            
            //caching objects refs to increase performance
            _inputHeight = GameObject.Find("size_x").GetComponent<InputField>();
            _inputWidth = GameObject.Find("size_z").GetComponent<InputField>();
            _inputSquareSize = GameObject.Find("squareSize").GetComponent<InputField>();
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
                            var cellHitted = _grid[coordinates[0], coordinates[1]];
                            if (_cellsVacated[coordinates[0],coordinates[1]] == false)
                            {
                                placeObject(objectToPlace, coordinates, Quaternion.Euler(0f,0f,0f), true);
                                Destroy(cellHitted);
                            }
                            else
                            {
                                placeObject(square,coordinates, Quaternion.Euler(90f,0f,0f), false);
                                Destroy(cellHitted);
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