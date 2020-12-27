using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ProxyHandBones
{
    public Transform _master;
    public SlaveBone _slave;
}

public class ProxyHand : MonoBehaviour
{
    [SerializeField]
    private OVRSkeleton _handData = null;

    [SerializeField]
    private ProxyHandBones[] _proxyHandBones = new ProxyHandBones[24];

    private void Awake()
    {
        for (int bone = 0; bone < _proxyHandBones.Length; bone++)
        {
            if (_proxyHandBones[bone]._slave && _proxyHandBones[bone]._master)
                _proxyHandBones[bone]._slave._Master = _proxyHandBones[bone]._master;
        }
    }

    void Update()
    {
        if (_handData.Bones.Count != _proxyHandBones.Length)
            return;

        _proxyHandBones[0]._master.position = _handData.Bones[0].Transform.position;
        _proxyHandBones[0]._master.rotation = _handData.Bones[0].Transform.rotation;

        for (int bone = 1; bone < _handData.Bones.Count; bone++)
        {
            _proxyHandBones[bone]._master.rotation = _handData.Bones[bone].Transform.rotation;
        }
    }
}
