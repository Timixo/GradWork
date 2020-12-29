using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksCollider : MonoBehaviour
{
    [Serializable]
    private struct RotationDirections
    {
        public bool x;
        public bool y;
        public bool z;
    }

    [SerializeField]
    private RotationDirections _rotationDirections = new RotationDirections();

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        SmallCube cube = other.GetComponent<SmallCube>();
        if (cube)
        {
            SmallCube.Rotations rotations = new SmallCube.Rotations();
            if (_rotationDirections.x) rotations.x = 1;
            else rotations.x = 0;
            if (_rotationDirections.y) rotations.y = 1;
            else rotations.y = 0;
            if (_rotationDirections.z) rotations.z = 1;
            else rotations.z = 0;

            cube.AddRotations(rotations);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SmallCube cube = other.GetComponent<SmallCube>();
        if (cube)
        {
            SmallCube.Rotations rotations = new SmallCube.Rotations();
            if (_rotationDirections.x) rotations.x = -1;
            else rotations.x = 0;
            if (_rotationDirections.y) rotations.y = -1;
            else rotations.y = 0;
            if (_rotationDirections.z) rotations.z = -1;
            else rotations.z = 0;
        }
    }
}
