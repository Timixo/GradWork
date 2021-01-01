using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public struct ProxyHandBones
{
    public MasterBone _master;
    public SlaveBone _slave;
}

public class ProxyHand : MonoBehaviour
{
    [SerializeField]
    private OVRSkeleton _skeletonData = null;

    [SerializeField]
    private OVRHand _hand = null;

    private List<GameObject> _children = new List<GameObject>();

    public OVRHand _Hand
    {
        get
        {
            return _hand;
        }
    }

    [SerializeField]
    private Collider _palmCollider = null;

    [SerializeField]
    private ProxyHandBones[] _proxyHandBones = new ProxyHandBones[24];

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            _children.Add(child.gameObject);
        }

        for (int bone = 0; bone < _proxyHandBones.Length; bone++)
        {
            if (_proxyHandBones[bone]._slave && _proxyHandBones[bone]._master)
            {
                _proxyHandBones[bone]._slave._Master = _proxyHandBones[bone]._master;
                _proxyHandBones[bone]._master.SetProxyHand(this);
            }
        }

        Collider[] colliders = _proxyHandBones[0]._slave.GetComponentsInChildren<Collider>();
        for (int c = 0; c < colliders.Length; c++)
        {
            Physics.IgnoreCollision(_palmCollider, colliders[c], true);
        }
    }

    void Update()
    {
        if (_skeletonData.Bones.Count != _proxyHandBones.Length)
            return;

        if (_hand.IsDataValid && _hand.IsDataHighConfidence)
        {
            _proxyHandBones[0]._master.transform.position = _skeletonData.Bones[0].Transform.position;
            _proxyHandBones[0]._master.transform.rotation = _skeletonData.Bones[0].Transform.rotation;
        }
        else
            return;

        for (int bone = 1; bone < _skeletonData.Bones.Count; bone++)
        {
            if (_proxyHandBones[bone]._master && (_hand.GetFingerConfidence(_proxyHandBones[bone]._master._Finger) == OVRHand.TrackingConfidence.High))
                _proxyHandBones[bone]._master.transform.rotation = _skeletonData.Bones[bone].Transform.rotation;
        }
    }
}
