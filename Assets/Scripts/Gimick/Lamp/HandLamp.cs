using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lumiere.Gimick.Lamp
{
    public class HandLamp : MonoBehaviour
    {
        Transform transformCache;
        [SerializeField] Transform parmDirTransform;
        //-------------------------------------------------
        //  プロパティ
        //-------------------------------------------------
        //public bool IsDraw { get { return true; } }
        //public Vector3 DrawPosition { get { return transform.position; } }
        //public bool IsActive    { get { return true; } }
        //=================================================
        void Start()
        {
            transformCache = transform;
        }
        void Update()
        {
            transformCache.position = parmDirTransform.position;
            transformCache.rotation = parmDirTransform.rotation;
            transformCache.localEulerAngles = Vector3.Scale(transformCache.localEulerAngles, Vector3.up);
        }
        //-------------------------------------------------
        //
        //-------------------------------------------------
    }
}