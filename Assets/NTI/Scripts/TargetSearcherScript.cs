using Sample;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


namespace NTI.Scripts
{
    public class TargetSearcherScript : MonoBehaviour
    {
        private ARGlobalSetupBehaviour _setupBehaviour;
        public Vector3 CenterPosition;
        public float Height;
        public float Width;
        private bool _canvasState = true; 

        [SerializeField] private Button menuButton;
        [SerializeField] private Canvas canvas;
        private MapGrid _gridHandler;
    
        // Start is called before the first frame update
        private void Start()
        {
            _setupBehaviour = GameObject.Find("EasyAR_Startup").GetComponent<ARGlobalSetupBehaviour>();
            _gridHandler = GameObject.Find("Grid").GetComponent<MapGrid>();
            
            InvokeRepeating(nameof(SetupMap), 1f, 5f);
            menuButton.onClick.AddListener(EnableGrid);
        }

        private void EnableGrid()
        {
            canvas.enabled = false;
            _canvasState = false;
            _gridHandler.DrawGrid();
        }
        
        private void SetupMap()
        {
            if (_setupBehaviour.Targets.Count >= 3)
            {
                if (_canvasState)
                {
                    canvas.enabled = true;
                }

                var xValues = new List<float>();
                var zValues = new List<float>();
                
                Debug.Log("------------- START -------------");
                
                foreach (var targetsTransform in _setupBehaviour.TargetsTransforms)
                {
                    Debug.Log(targetsTransform.Value.transform.position.x + " " + targetsTransform.Value.transform.position.z);

                    xValues.Add(targetsTransform.Value.position.x);
                    zValues.Add(targetsTransform.Value.position.z);
                }
                
                Width = xValues.Max() - xValues.Min();
                Height = zValues.Max() - zValues.Min();
                
                CenterPosition = new Vector3(xValues.Min() + Width / 2, 0, zValues.Min() + Height / 2);

                transform.position = CenterPosition;
                transform.rotation = Quaternion.Euler(90,0,90);
                
                Debug.Log("------------- END -------------");
            }
            else
            {
                canvas.enabled = false;
            }
            Debug.Log(">>>>>>>>>> " + _setupBehaviour.Targets.Count);
        }
    }
}