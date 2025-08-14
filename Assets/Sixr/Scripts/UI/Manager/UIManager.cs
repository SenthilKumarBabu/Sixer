using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("References")]
    [SerializeField] private Transform pageRoot;
    [SerializeField] private Transform popupRoot;

    private Dictionary<string, UIPage> _pages = new();
    private Stack<UIPage> pageStack = new();

    private Dictionary<string, UIPopup> popups = new();
    private Stack<UIPopup> popupStack = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;

        RegisterUIElements();
        
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
        if (pageStack.Count > 0 && !pageStack.Peek().isOverlay)
        {
            pageStack.Peek().OnHide();
            pageStack.Peek().gameObject.SetActive(false);
        }

        string key = typeof(T).Name;
        if (!_pages.TryGetValue(key, out var page)) { Debug.LogError($"Page {key} not found"); return; }

        page.OnShow(data);
        page.gameObject.SetActive(true);
        pageStack.Push(page);
    }

    private void ShowCurrentPage()
    {
        var currentTop = pageStack.Peek();
        currentTop.OnShow();
        currentTop.gameObject.SetActive(true);
    }

    public void CloseCurrentPage(bool showCurrentPage = true)
    {
        if (pageStack.Count == 0) return;

        var top = pageStack.Pop();
        top.OnHide();
        top.gameObject.SetActive(false);

        if (showCurrentPage)
            ShowCurrentPage();
    }

    public void OpenPopup<T>(object data = null) where T : UIPopup
    {
        string key = typeof(T).Name;
        if (!popups.TryGetValue(key, out var popup)) { Debug.LogError($"Popup {key} not found"); return; }

        popup.OnShow(data);
        popup.gameObject.SetActive(true);
        popupStack.Push(popup);
    }

    public void CloseTopPopup()
    {
        if (popupStack.Count == 0) return;

        var top = popupStack.Pop();
        top.OnHide();
        top.gameObject.SetActive(false);
    }

    public void CloseAllPopups()
    {
        while (popupStack.Count > 0)
            CloseTopPopup();
    }
}
