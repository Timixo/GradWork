using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallCube : MonoBehaviour
{
    public struct Rotations
    {
        public int x;
        public int y;
        public int z;
    }

    private Rigidbody _rigidBody = null;

    private Rotations _rotations = new Rotations();

    // Start is called before the first frame update
    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        RigidbodyConstraints constraints = RigidbodyConstraints.FreezePosition;
        if (_rotations.x == 0) constraints |= RigidbodyConstraints.FreezeRotationX;
        if (_rotations.y == 0) constraints |= RigidbodyConstraints.FreezeRotationY;
        if (_rotations.z == 0) constraints |= RigidbodyConstraints.FreezeRotationZ;
        _rigidBody.constraints = constraints;
    }

    public void AddRotations(Rotations rotations)
    {
        _rotations.x += rotations.x;
        if (_rotations.x < 0) _rotations.x = 0;
        _rotations.y += rotations.y;
        if (_rotations.y < 0) _rotations.y = 0;
        _rotations.z += rotations.z;
        if (_rotations.z < 0) _rotations.z = 0;
    }
}
