using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CharAnimator : MonoBehaviour
{
    [SerializeField] private Transform[] _blocks;
    [SerializeField] private TextMeshProUGUI[] _textChar;
    [SerializeField] private GameObject startButton;
    public EventsManager EventsManager;
    private void OnEnable()
    {
        EventsManager.ShowButton += ActivateStart;
        EventsManager.HideButton += DeActivateStart;
    }

    private void OnDisable()
    {
        EventsManager.ShowButton -= ActivateStart;
        EventsManager.HideButton -= DeActivateStart;

    }

    private void ActivateStart()
    {
        startButton.SetActive(true);
        startButton.transform.DOPunchScale(new Vector3(1, 1, 0), 1f, 1, 0.2f).SetEase(Ease.Flash);
    }

    private void DeActivateStart()
    {
        startButton.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.Flash).OnComplete((StartGame)
            );
        
    }

    private void StartGame()
    {
        startButton.SetActive(false);
        WipeAnimation();
        
    }

    private void WipeAnimation()
    {
        foreach (var block in _blocks)
        {
            block.DOPunchRotation(new Vector3(0, 0, 360), 0.8f, 1, 0.5f).SetEase(Ease.InBack);
        }

        foreach (var tmpText in _textChar)
        {
            tmpText.text = "";
        }
    }
    private void Awake()
    {
        EventsManager = FindObjectOfType<EventsManager>();
        startButton.SetActive(false);
    }

    private void Start()
    {
        var sequence = DOTween.Sequence();
        foreach (var block in _blocks)
        {
            
           sequence.Append(block.DOPunchPosition(new Vector3(0, 30, 0), 0.5f, 2, 0.2f));
           sequence.Append(block.DOPunchScale(new Vector3(1, 1, 0), 0.2f, 2, 0.2f).SetEase(Ease.Flash));

        }

        sequence.OnComplete(EventsManager.OnShowButton);

        

    }

    
    
}
