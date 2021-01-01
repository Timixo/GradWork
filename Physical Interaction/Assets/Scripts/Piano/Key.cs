using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private float _angleDifference = 0f;
    private float _deltaAngle = 1f;

    private HingeJoint _hingeJoint = null;

    private AudioSource _audioSource = null;

    private void Awake()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _audioSource = GetComponent<AudioSource>();

        _angleDifference = _hingeJoint.limits.max - _hingeJoint.limits.min;
        _deltaAngle = _angleDifference / 8;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hingeJoint.angle >= (_angleDifference / 2))
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
        if (_hingeJoint.angle <= _deltaAngle)
        {
            _audioSource.Stop();
        }
    }
}
