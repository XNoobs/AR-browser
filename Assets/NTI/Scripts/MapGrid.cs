using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NTI.Scripts
{
    public class MapGrid : MonoBehaviour
    {
        [SerializeField] private uint height = 10;
        [SerializeField] private uint width = 10;
        [SerializeField] private uint squareSize = 7;
        [SerializeField] private GameObject square;
        [SerializeField] private Color activeCellColor = Color.red;
        private Vector3 _position;

       private List<List<GameObject>> _grid = new List<List<GameObject>>();

        private void Start()
        {
            square.transform.localScale = new Vector3(5, 5, 0);
            for (var i = 0; i < height; i++)
            {
                _grid.Add(new List<GameObject>());
            }

            for (var i = 0; i < height; i++)
            {

                for (var j = 0; j < width; j++)
                {
                    //TODO Add colliders to game objects
                    _position = new Vector3((i - (height / 2)) * squareSize, 0, (j - (width / 2)) * squareSize);
                    var current = Instantiate(square, _position, Quaternion.Euler(90f, 0f, 0f)) as GameObject;
                    current.SetActive(true);
                    current.transform.SetParent(this.transform);
                    var currentBoxCollider = current.AddComponent<BoxCollider>();
                    currentBoxCollider.name = i.ToString() + '_' + j.ToString();
                    _grid[i].Add(current);
                }
            }
        }

        private void Update()
        {

            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                for (var i = 0; i < height; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                        RaycastHit raycastHit;
                        if (Physics.Raycast(raycast, out raycastHit))
                        {
                            if (raycastHit.collider.name == i.ToString()+'_'+j.ToString())
                            {
                                var sprite = _grid[i][j].GetComponent<SpriteRenderer>();
                                if (sprite.color != activeCellColor)
                                {
                                    sprite.color = activeCellColor;
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