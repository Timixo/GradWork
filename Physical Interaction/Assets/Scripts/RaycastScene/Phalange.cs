﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Phalange : MonoBehaviour
{
    private static float _deltaOrigin = 0.0001f;

    [Serializable]
    private struct Contact
    {
        public RaycastHit _hitInfo;
        public Vector3 _phalanxLocalPoint;
        public GameObject _object;
        public Collider _objectCollider;
    }

    [Tooltip("Leave empty if the rigidbody is on the same object")]
    [SerializeField]
    private Rigidbody _rigidbody = null;

    private Collider _collider = null;

    [SerializeField]
    private List<Contact> _contacts = new List<Contact>();

    private LineRenderer _lineRenderer = null;

    private Transform _actualPhalangeTransform = null;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        for (int i = _contacts.Count - 1; i >= 0; i--)
        {
            Contact contact = _contacts[i];
            Vector3 rayOrigin = contact._hitInfo.point + _deltaOrigin * contact._hitInfo.normal;
            Vector3 phalanxPos = _actualPhalangeTransform.localToWorldMatrix * contact._phalanxLocalPoint;
            Vector3 direction = phalanxPos - rayOrigin;
            float maxDistance = direction.magnitude;
            direction = direction.normalized;
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, direction, out hitInfo, maxDistance))
            {
                if (contact._object == hitInfo.collider.gameObject)
                {

                }
                else
                {
                    _contacts.Remove(contact);
                }
            }
            else
            {
                _contacts.Remove(contact);
            }
        }
        if (_contacts.Count == 0)
        {
            _collider.isTrigger = false;
            _lineRenderer.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 contactPoint = new Vector3();
        foreach (ContactPoint point in collision.contacts)
            contactPoint += point.point;
        contactPoint /= collision.contactCount;

        Vector3 objectCenter = collision.collider.bounds.center;
        Vector3 direction = objectCenter - contactPoint;
        float maxDistance = direction.magnitude;
        direction = direction.normalized;
        RaycastHit hitInfo;

        if (Physics.Raycast(contactPoint, direction, out hitInfo, maxDistance))
        {
            Contact contact = new Contact();
            contact._hitInfo = hitInfo;
            contact._object = hitInfo.collider.gameObject;
            contact._phalanxLocalPoint = _actualPhalangeTransform.worldToLocalMatrix * hitInfo.point;
            contact._objectCollider = hitInfo.collider;

            _contacts.Add(contact);

            _collider.isTrigger = true;

            _lineRenderer.enabled = true;
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPositions(new Vector3[] { hitInfo.point, hitInfo.point + 10 * direction });
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (!other.CompareTag("Object"))
    //        return;

    //    GameObject gameObject = other.gameObject;
    //    for (int contact = 0; contact < _contacts.Count; contact++)
    //    {
    //        if (gameObject == _contacts[contact]._object)
    //        {
    //            Debug.Log("Object exited");
    //            _lineRenderer.positionCount = 0;
    //            _lineRenderer.SetPositions(new Vector3[] { });
    //            _contacts.RemoveAt(contact);
    //        }
    //    }
    //}

    public void SetPhalange(Transform phalange)
    {
        _actualPhalangeTransform = phalange;
    }
}
