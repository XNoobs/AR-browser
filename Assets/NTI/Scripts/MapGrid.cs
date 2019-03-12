using UnityEngine;
using UnityEngine.Events;

using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using EasyAR;
// using Eas


namespace NTI.Scripts
{

    // public class CustomImageTracker : 


    public class MapGrid : MonoBehaviour
    {
        [SerializeField] private GameObject square;
        [SerializeField] private Canvas userInterface;
        [SerializeField] private GameObject objectToPlace;
        private GameObject[,] _grid;
        private bool[,] _cellsVacated;
        private GameObject menuBtn;
        private AppConfigHandler _configHandler;
        private TargetSearcherScript targetSearcher;


        private static async Task Fetch()
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync("http://192.168.43.238/ar/status/5");
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    // if responseBody == "\"{}\\n\"" => all files are up-to-date
                    // ToDo: create success response

                    if (responseBody != "\"{}\\n\"")
                    {
                        Dictionary<int, string[]> dict = JsonConvert.DeserializeObject<Dictionary<int, string[]>>(responseBody);
                        Console.WriteLine(new string('-', 20));
                        Console.WriteLine(dict);
                        Console.WriteLine(dict.ToString());

                        for (int i = 0; i < dict.Count; i++)
                        {
                            File.WriteAllText(String.Format(@"C:\Users\Skufler\source\repos\HttpClientTest\HttpClientTest\map__{0}.obj", i), dict[i][0]);
                            File.WriteAllText(String.Format(@"C:\Users\Skufler\source\repos\HttpClientTest\HttpClientTest\map__{0}.mtl", i), dict[i][1]);
                            try
                            {
                                var x = Base64Encode(dict[i][2]).Remove(0, 2);
                                x = x.Remove(x.Length - 1);
                                File.WriteAllText(String.Format(@"C:\Users\Skufler\source\repos\HttpClientTest\HttpClientTest\map__{0}.jpg", i), x);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("GAVNO");
                    }

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException Caught!");
                    Console.WriteLine("Message :{0} ", e.Message);
                }
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public void DrawGrid()
        {
            square.transform.localScale = new Vector3(_configHandler.scale.x, _configHandler.scale.z, 0);
            this.transform.position = targetSearcher.CenterPosition;
            _grid = new GameObject[_configHandler.height, _configHandler.width];
            _cellsVacated = new bool[_configHandler.height, _configHandler.width];

            for (var i = 0; i < _configHandler.height; i++)
            {
                for (var j = 0; j < _configHandler.width; j++)
                {
                    PlaceObject(square, new Tuple<int, int>(i, j), Quaternion.Euler(90f, 0f, 0f), false);
                }
            }

            Debug.Log("Grid created");
        }

        public void DeleteGrid()
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
            var position = new Vector3(coordinates.Item1 *  _configHandler.squareRenderer.x  + _configHandler.movementX, 0, coordinates.Item2  * _configHandler.squareRenderer.z + _configHandler.movementZ);
            var current = Instantiate(objectToPlace, position, rotation) as GameObject;
            
            if (vacation)
            {
                var sizeObj = current.GetComponent<MeshRenderer>().bounds.size;
                var sizeCell = square.GetComponent<SpriteRenderer>().bounds.size;
                var maxBound = sizeObj.x > sizeObj.z ? sizeObj.x : sizeObj.z;
                var scaleFactor = sizeCell.x / maxBound;
                
                current.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }
            
            current.transform.SetParent(this.transform);
            var currentBoxCollider = current.AddComponent<BoxCollider>();
            currentBoxCollider.name = coordinates.Item1.ToString() + '_' + coordinates.Item2;
            current.SetActive(true);
            
            _grid[coordinates.Item1, coordinates.Item2] = current;
            _cellsVacated[coordinates.Item1, coordinates.Item2] = vacation;
            Debug.Log("Cell placed");
        }
        

        private void Start()
        {
            _configHandler = GameObject.Find("MainPanel").GetComponent<AppConfigHandler>();
            userInterface.enabled = true;
            var squareSize = square.GetComponent<SpriteRenderer>().bounds.size;
            targetSearcher = GameObject.Find("Canvas").GetComponent<TargetSearcherScript>();
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
                }
            }
        }
    }
}