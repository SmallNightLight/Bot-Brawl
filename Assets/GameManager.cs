using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Bot _bot1;
    [SerializeField] private Bot _bot2;

    [SerializeField] private Camera _gateCamera;
    [SerializeField] private List<Camera> _cameras = new List<Camera>();
    private int _currentCamera;

    [SerializeField] private List<Animator> _gateAnimators = new List<Animator>();

    private bool _inFight;

    private void Start()
    {
        _bot1.SetBotData(DataManager.Instance.CurrentBotData);
        _bot2.SetBotData(DataManager.Instance.OpponentBotData);

        StartCoroutine(GameStart());
    }

    private void Update()
    {
        if (_inFight && Input.GetKeyDown(KeyCode.Space))
        {
            _currentCamera++;

            if (_currentCamera == _cameras.Count)
                _currentCamera = 0;

            SetCamera(_currentCamera);
        }
    }

    private void SetCamera(int index)
    {
        _gateCamera.gameObject.SetActive(false);

        for (int i = 0; i < _cameras.Count; i++)
            _cameras[i].gameObject.SetActive(i == index);
    }

    private void SetCamera(Camera camera)
    {
        _gateCamera.gameObject.SetActive(false);

        for (int i = 0; i < _cameras.Count; i++)
            _cameras[i].gameObject.SetActive(false);

        camera.gameObject.SetActive(true);
    }

    IEnumerator GameStart()
    {
        _inFight = false;
        _bot1.IsInFight = false;
        _bot2.IsInFight = false;

        SetCamera(_gateCamera);

        yield return new WaitForSeconds(0.5f);

        foreach (var animator in _gateAnimators)
            animator.SetTrigger("Open");

        yield return new WaitForSeconds(3);

        _bot1.IsMovingOut = true;
        _bot2.IsMovingOut = true;

        yield return new WaitForSeconds(3);

        _currentCamera = 0;
        SetCamera(_currentCamera);

        _bot1.IsMovingOut = false;
        _bot2.IsMovingOut = false;

        yield return new WaitForSeconds(4);

        _inFight = true;
        _bot1.IsInFight = true;
        _bot2.IsInFight = true;
    }
}