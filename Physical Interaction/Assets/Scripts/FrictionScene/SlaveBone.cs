using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConfigurableJoint))]
public class SlaveBone : MonoBehaviour
{
    [SerializeField]
    private bool _followPosition = false;
    [SerializeField]
    private bool _followRotation = true;

    private ConfigurableJoint _joint = null;

    [SerializeField]
    private MasterBone _master = null;

    private Quaternion _jointRotation;
    private Quaternion _initLocalRotation;
    private Quaternion _initGlobalRotation;

    public MasterBone _Master
    { 
        get
        {
            return _master;
        }
        
        set
        {
            if (!_master)
                _master = value;
        }
    
    }

    void Start()
    {
        if (!_joint)
            _joint = GetComponent<ConfigurableJoint>();

        Vector3 xAxis = _joint.axis;
        Vector3 zAxis = Vector3.Cross(_joint.axis, _joint.secondaryAxis).normalized;
        Vector3 yAxis = Vector3.Cross(zAxis, xAxis).normalized;

        _jointRotation = Quaternion.LookRotation(zAxis, yAxis);

        if (_joint.connectedBody != null && !_joint.configuredInWorldSpace)
        {
            _initLocalRotation = _joint.connectedBody.transform.localRotation;
        }
        else
        {
            _initGlobalRotation = _joint.transform.rotation;
        }
    }

    void FixedUpdate()
    {
        if (!_master)
            return;

        if (_followPosition)
        {
            if (_joint.connectedBody != null && !_joint.configuredInWorldSpace)
            {
                //The joint is not the wrist, may not follow the position, only rotation
            }
            else
            {
                // these steps are to change the anchor, the pivot point around what the hand will rotate (needs to be your wrist, not the original position)
                if (_joint.autoConfigureConnectedAnchor)
                    _joint.autoConfigureConnectedAnchor = false;

                if (_joint.targetPosition != Vector3.zero)
                    _joint.targetPosition = Vector3.zero;

                _joint.connectedAnchor = _master._LastStableTransform.position;
            }
        }

        if (_followRotation)
        {
            Quaternion result;
            if (_joint.connectedBody != null && !_joint.configuredInWorldSpace)
            {
                result = Quaternion.Inverse(_jointRotation);
                result *= _initLocalRotation;
                result *= Quaternion.Inverse(_master._LastStableTransform.localRotation);
                result *= _jointRotation;

                _joint.targetRotation = result;
            }
            else
            {
                result = Quaternion.Inverse(_jointRotation);
                result *= _initGlobalRotation;
                result *= Quaternion.Inverse(_master._LastStableTransform.rotation);
                result *= _jointRotation;

                _joint.targetRotation = result;
            }
        }
    }
}
