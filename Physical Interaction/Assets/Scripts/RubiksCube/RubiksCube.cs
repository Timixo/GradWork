using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RubiksCube : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        BoxCollider fullCubeCollider = GetComponent<BoxCollider>();
        List<BoxCollider> cubeColliders = GetComponentsInChildren<BoxCollider>().ToList();

        cubeColliders.ForEach(cubeCollider => Physics.IgnoreCollision(fullCubeCollider, cubeCollider));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
