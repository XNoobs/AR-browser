using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using UnityEngine.Experimental.UIElements;
using UnityEngine.UI;

namespace NTI.Scripts
{
    public class MapGrid : MonoBehaviour
    {
        [SerializeField] private GameObject square;
        [SerializeField] private Canvas userInterface;
        [SerializeField] private GameObject objectToPlace;
        private GameObject[,] _grid;
        private bool[,] _cellsVacated;
        private GameObject menuBtn;
        private AppConfigHandler _configHandler;


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
                buttonPos = -(_configHandler.width / 2 + 1) * _configHandler.padding;
            }

            var position = new Vector3(0, 0, buttonPos);
            menuBtn.transform.position = position;
            menuBtn.SetActive(true);
        }

        private void DrawGrid()
        {
            square.transform.localScale = new Vector3(_configHandler.padding-1, _configHandler.padding-1, 0);

            _grid = new GameObject[_configHandler.height, _configHandler.width];
            _cellsVacated = new bool[_configHandler.height, _configHandler.width];

            for (var i = 0; i < _configHandler.height; i++)
            {
                for (var j = 0; j < _configHandler.width; j++)
                {
                    var position = new Vector3(i * _configHandler.padding - _configHandler.movementX, 0, j * _configHandler.padding - _configHandler.movementZ);
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
            for (var i = 0; i < _configHandler.height; i++)
            {
                for (var j = 0; j < _configHandler.width; j++)
                {
                    Destroy(_grid[i, j]);
                }
            }
        }

        private void PlaceObject(GameObject objectToPlace, Tuple<int, int> coordinates, Quaternion rotation, bool vacation)
        {
            var position = new Vector3(coordinates.Item1 * _configHandler.padding - _configHandler.movementX, 0, coordinates.Item2 * _configHandler.padding - _configHandler.movementZ);
            var current = Instantiate(objectToPlace, position, rotation) as GameObject;
            current.SetActive(true);
            current.transform.SetParent(this.transform);
            var currentBoxCollider = current.AddComponent<BoxCollider>();
            currentBoxCollider.name = coordinates.Item1.ToString() + '_' + coordinates.Item2.ToString();
            _grid[coordinates.Item1, coordinates.Item2] = current;
            _cellsVacated[coordinates.Item1, coordinates.Item2] = vacation;
        }
        

        private void Start()
        {
            _configHandler = GameObject.Find("MainPanel").GetComponent<AppConfigHandler>();
            square.transform.localScale = new Vector3(5, 5, 0);
            userInterface.enabled = true;
            var squareSize = square.GetComponent<SpriteRenderer>().bounds.size;
            CreateMenuButton();
            DrawMenuButton(true);
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
                            Tuple<int, int> coordinates =
                                new Tuple<int, int>(Convert.ToInt16(name[0]), Convert.ToInt16(name[1]));
                            var cellHitted = _grid[coordinates.Item1, coordinates.Item2];
                            if (_cellsVacated[coordinates.Item1,coordinates.Item2] == false)
                            {
                                PlaceObject(objectToPlace, coordinates, Quaternion.Euler(0f,0f,0f), true);
                                Destroy(cellHitted);
                            }
                            else
                            {
                                PlaceObject(square,coordinates, Quaternion.Euler(90f,0f,0f), false);
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