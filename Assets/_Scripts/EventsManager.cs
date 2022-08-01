using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class EventsManager : MonoBehaviour
{
    public event Action ShowButton;
    public void OnShowButton() => ShowButton?.Invoke();

    public event Action HideButton;
    public void OnHideButton() => HideButton?.Invoke();

    public event Action StartGame;
    public void OnGameStart() => StartGame?.Invoke();
}
