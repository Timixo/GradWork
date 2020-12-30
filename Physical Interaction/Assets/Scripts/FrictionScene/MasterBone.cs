using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterBone : MonoBehaviour
{
    [SerializeField]
    private OVRHand.HandFinger _finger = OVRHand.HandFinger.Thumb;

    public OVRHand.HandFinger _Finger
    {
        get
        {
            return _finger;
        }
    }

    private ProxyHand _proxyHand = null;

    private Transform _lastStableTransform = null;

    public Transform _LastStableTransform
    {
        get
        {
            return _lastStableTransform;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        _lastStableTransform = transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_proxyHand)
            return;

        if (!_proxyHand._Hand.IsTracked || (_proxyHand._Hand.HandConfidence == OVRHand.TrackingConfidence.Low))
            return;

        if (_finger != OVRHand.HandFinger.Max)
        {
            if (_proxyHand._Hand.GetFingerConfidence(_finger) == OVRHand.TrackingConfidence.Low)
                return;
        }

        _lastStableTransform = transform;
    }

    public void SetProxyHand(ProxyHand proxyHand)
    {
        if (!_proxyHand)
            _proxyHand = proxyHand;
    }
}
