using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowHandData : MonoBehaviour
{
    [SerializeField]
    private OVRSkeleton _skeletonData = null;

    [SerializeField]
    private Transform[] _bones = new Transform[24];

    private Rigidbody[] _rigidbodies = new Rigidbody[24];

    // Start is called before the first frame update
    void Awake()
    {
        for (int bone = 0; bone < _bones.Length; bone++)
        {
            _rigidbodies[bone] = _bones[bone].GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((_skeletonData.Bones.Count != _bones.Length) || !_skeletonData.IsDataHighConfidence)
            return;

        _bones[0].position = _skeletonData.Bones[0].Transform.position;
        _bones[0].rotation = _skeletonData.Bones[0].Transform.rotation;

        for (int bone = 1; bone < _skeletonData.Bones.Count; bone++)
        {
            _bones[bone].rotation = _skeletonData.Bones[bone].Transform.rotation;
        }
    }
}
