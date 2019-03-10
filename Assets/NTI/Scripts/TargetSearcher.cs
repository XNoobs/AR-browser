using Sample;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;


namespace NTI.Scripts
{
    public class TargetSearcher : MonoBehaviour
    {
        private ARGlobalSetupBehaviour _setupBehaviour;
        private MeshRenderer _meshRenderer;
        private float _height;
        private float _width;
        public Vector3 CenterCoordinates;
    
    
        // Start is called before the first frame update
        private void Start()
        {
            _setupBehaviour = GameObject.Find("EasyAR_Startup").GetComponent<ARGlobalSetupBehaviour>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
            
            InvokeRepeating(nameof(SetupMap), 1f, 5f);
        }

        private void SetupMap()
        {
            if (_setupBehaviour.Targets.Count >= 3)
            {
                _meshRenderer.enabled = true;
                
                var xValues = new List<float>();
                var zValues = new List<float>();
                
                Debug.Log("------------- START -------------");
                
                foreach (var targetsTransform in _setupBehaviour.TargetsTransforms)
                {
                    Debug.Log(targetsTransform.Value.transform.position.x + " " + targetsTransform.Value.transform.position.z);

                    xValues.Add(targetsTransform.Value.position.x);
                    zValues.Add(targetsTransform.Value.position.z);
                }
                
                _width = xValues.Max() - xValues.Min();
                _height = zValues.Max() - zValues.Min();
                
                CenterCoordinates = new Vector3(xValues.Min() + _width / 2, 0, zValues.Min() + _height / 2);
                transform.position = CenterCoordinates;
                
                transform.rotation = Quaternion.identity;
                
                Debug.Log("------------- END -------------");
            }
            else
            {
                _meshRenderer.enabled = false;
            }
            Debug.Log(">>>>>>>>>> " + _setupBehaviour.Targets.Count);
        }
    }
}