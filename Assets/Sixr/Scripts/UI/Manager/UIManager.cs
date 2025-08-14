using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    [SerializeField] private Transform pageRoot;
    [SerializeField] private Transform popupRoot;

    private Dictionary<string, UIPage> _pages = new();
    [ShowInInspector] private Stack<UIPage> pageStack = new();

    private Dictionary<string, UIPopup> popups = new();
    [ShowInInspector] private Stack<UIPopup> popupStack = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        RegisterUIElements();
    }

    private void Start()
    {
        OpenPage<Sixer.UI.SplashScreenPage>();  
    }

    private void RegisterUIElements()
    {
        foreach (var page in pageRoot.GetComponentsInChildren<UIPage>(true))
        {
            _pages[page.GetType().Name] = page;
            page.gameObject.SetActive(false);
        }

        foreach (var popup in popupRoot.GetComponentsInChildren<UIPopup>(true))
        {
            popups[popup.GetType().Name] = popup;
            popup.gameObject.SetActive(false);
        }
    }

    public void OpenPage<T>(object data = null) where T : UIPage
    {
        string key = typeof(T).Name;
        if (!_pages.TryGetValue(key, out var page)) { Debug.LogError($"Page {key} not found"); return; }
        
        if (pageStack.Count > 0 && !page.isOverlay)
        {
            pageStack.Peek().OnHide();
            pageStack.Peek().gameObject.SetActive(false);
        }

        pageStack.Push(page);
        page.gameObject.SetActive(true);
        page.OnShow(data);
    }

    private void ShowCurrentPage()
    {
        var currentTop = pageStack.Peek();
        currentTop.gameObject.SetActive(true);
        currentTop.OnShow();
    }

    public void CloseCurrentPage()
    {
        if (pageStack.Count == 0) return;

        var top = pageStack.Pop();
        top.gameObject.SetActive(false);
        top.OnHide();

        if (!top.isOverlay)
            ShowCurrentPage();
    }

    public void OpenPopup<T>(object data = null) where T : UIPopup
    {
        string key = typeof(T).Name;
        if (!popups.TryGetValue(key, out var popup)) { Debug.LogError($"Popup {key} not found"); return; }

        popupStack.Push(popup);
        popup.gameObject.SetActive(true);
        popup.OnShow(data);
    }

    public void CloseTopPopup()
    {
        if (popupStack.Count == 0) return;

        var top = popupStack.Pop();
        top.gameObject.SetActive(false);
        top.OnHide();
    }

    public void CloseAllPopups()
    {
        while (popupStack.Count > 0)
            CloseTopPopup();
    }
}
