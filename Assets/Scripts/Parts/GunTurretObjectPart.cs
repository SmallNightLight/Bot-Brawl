using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTurretObjectPart : ObjectPart
{
    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _bullet;

    [SerializeField] private Transform _bulletSpawn1;
    [SerializeField] private Transform _bulletSpawn2;
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

    bool left = true;

    public void Shoot()
    {
        left = !left;

        if (left)
        {
            GameObject bullet1 = Instantiate(_bullet, _bulletSpawn1.position, Quaternion.identity);
            bullet1.GetComponent<Rigidbody>().AddForce(_bulletSpawn1.forward * _bulletSpeed, ForceMode.Impulse);
            Destroy(bullet1, 5);
        }
        else
        {
            GameObject bullet2 = Instantiate(_bullet, _bulletSpawn2.position, Quaternion.identity);
            bullet2.GetComponent<Rigidbody>().AddForce(_bulletSpawn2.forward * _bulletSpeed, ForceMode.Impulse);
            Destroy(bullet2, 5);
        }
    }
}