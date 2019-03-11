using Sample;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;


namespace NTI.Scripts
{
    public class TargetSearcherMap : MonoBehaviour
    {
        private ARGlobalSetupBehaviour _setupBehaviour;
        public Vector3 CenterPosition;
        public float Height;
        public float Width;

        [SerializeField] private Button menuButton;
        [SerializeField] private Canvas canvas;
        private MapGrid _gridHandler;
    
        // Start is called before the first frame update
        private void Start()
        {
            _setupBehaviour = GameObject.Find("EasyAR_Startup").GetComponent<ARGlobalSetupBehaviour>();
            canvas.enabled = false;
            _gridHandler = GameObject.Find("Grid").GetComponent<MapGrid>();
            
            InvokeRepeating(nameof(SetupMap), 1f, 5f);
            menuButton.onClick.AddListener(EnableCanvas);
        }

        private void EnableCanvas()
        {
            canvas.enabled = true;
            _gridHandler.DeleteGrid();
        }
        
        private void SetupMap()
        {
            if (_setupBehaviour.Targets.Count >= 3)
            {
                canvas.enabled = true;
                
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
            Debug.Log(">>>>>>>>>> " + _setupBehaviour.Targets.Count);
        }
    }
}