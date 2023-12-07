using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CannonObjectPart : ObjectPart
{
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _bullet;

    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _bulletSpeed = 10f;

    private void Start()
    {
        _animator.SetBool("IsOn", false);
    }


    void Update()
    {
        if (IsActive)
            _animator.SetBool("IsOn", true);
    }

    public void Shoot()
    {
        GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.identity);

        bullet.GetComponent<Rigidbody>().AddForce(_bulletSpawn.forward * _bulletSpeed, ForceMode.Impulse);
    }
}