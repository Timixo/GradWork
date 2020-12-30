using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Phalange : MonoBehaviour
{
    [Serializable]
    private struct Contact
    {
        public Vector3 _point;
        public Vector3 _phalanxLocalPoint;
        public GameObject _object;
    }

    [Tooltip("Leave empty if the rigidbody is on the same object")]
    [SerializeField]
    private Rigidbody _rigidbody = null;

    private Collider _collider = null;

    [SerializeField]
    private List<Contact> _contacts = new List<Contact>();

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        if (_contacts.Count == 0) _collider.isTrigger = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 contactPoint = new Vector3();
        foreach (ContactPoint point in collision.contacts)
            contactPoint += point.point;
        contactPoint /= collision.contactCount;

        Vector3 direction = collision.collider.transform.position - contactPoint;
        RaycastHit hitInfo;

        if (Physics.Raycast(contactPoint, direction, out hitInfo))
        {
            Contact contact = new Contact();
            contact._point = hitInfo.point;
            contact._object = hitInfo.collider.gameObject;
            contact._phalanxLocalPoint = transform.worldToLocalMatrix * hitInfo.point;

            _contacts.Add(contact);

            _collider.isTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Object"))
            return;

        GameObject gameObject = other.gameObject;
        for (int contact = 0; contact < _contacts.Count; contact++)
        {
            if (gameObject == _contacts[contact]._object)
            {
                _contacts.RemoveAt(contact);
            }
        }
    }
}
