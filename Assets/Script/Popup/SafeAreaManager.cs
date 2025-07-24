using System;
using UnityEngine;

public sealed class SafeAreaManager : MonoBehaviour
{
    //#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    //#if UNITY_IOS || UNITY_IPHONE
    private static Vector2 anchorMin, anchorMax;
    private static event Action OnSafeAreaChanged;
    private static Rect lastKnownSafeArea;

    public static bool ShouldCalculateSafeArea { get; private set; } = false;

    private void Awake()
    {
#if UNITY_EDITOR
        ShouldCalculateSafeArea = false;
#else
        ShouldCalculateSafeArea = lastKnownSafeArea.size != new Vector2(Screen.width, Screen.height);
#endif

        Initialize();
        DontDestroyOnLoad(gameObject);
    }

#if !UNITY_EDITOR
    private void Update()
    {

        if (lastKnownSafeArea.Equals(Screen.safeArea))
            return;
        Initialize();
        OnSafeAreaChanged?.Invoke();
    }
#endif

    public static void Initialize()
    {
        lastKnownSafeArea = Screen.safeArea;

#if UNITY_EDITOR
        if (ShouldCalculateSafeArea)
        {
            anchorMin = lastKnownSafeArea.position + new Vector2(100, 0); //left
            anchorMax = lastKnownSafeArea.position + Screen.safeArea.size - new Vector2(100, 0); //right
        }
        else
        {
            anchorMin = lastKnownSafeArea.position;
            anchorMax = lastKnownSafeArea.position + Screen.safeArea.size;
        }
#else
        anchorMin = lastKnownSafeArea.position;
        anchorMax = lastKnownSafeArea.position + lastKnownSafeArea.size;
#endif
        //=======Manoj.k
        /*anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;*/

        //=======Manoj.k

        //HARDCODE
        /*ShouldCalculateSafeArea = true;
        anchorMin = lastKnownSafeArea.position + new Vector2(100,0);
        anchorMax = lastKnownSafeArea.position + Screen.safeArea.size - new Vector2(150, 0);*/
    }

    public static void ApplySafeArea(Canvas canvas, RectTransform safeAreaTransform)
    {
        if (ShouldCalculateSafeArea)
        {
            safeAreaTransform.anchorMin = new Vector2(anchorMin.x / canvas.pixelRect.width, 0);
            safeAreaTransform.anchorMax = new Vector2(anchorMax.x / canvas.pixelRect.width, anchorMax.y / canvas.pixelRect.height);
        }
    }

    public static void ApplySafeArea(Canvas canvas, RectTransform[] safeAreaTransforms)
    {
        if (ShouldCalculateSafeArea)
        {
            Vector2 _anchorMin = new Vector2(anchorMin.x / canvas.pixelRect.width, 0);
            Vector2 _anchorMax = new Vector2(anchorMax.x / canvas.pixelRect.width, anchorMax.y / canvas.pixelRect.height);

            for (int i = 0; i < safeAreaTransforms.Length; i++)
            {
                safeAreaTransforms[i].anchorMin = _anchorMin;
                safeAreaTransforms[i].anchorMax = _anchorMax;
            }
        }
    }

    public static void ApplySafeArea(RectTransform[] safeAreaTransforms)
    {
        lastKnownSafeArea = Screen.safeArea;
#if UNITY_EDITOR
		
		anchorMin = lastKnownSafeArea.position;
		anchorMax = lastKnownSafeArea.position + lastKnownSafeArea.size;// - Vector2.up * 100f;
#else
        anchorMin = lastKnownSafeArea.position;
        anchorMax = lastKnownSafeArea.position + lastKnownSafeArea.size;
#endif

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;
        if (safeAreaTransforms != null)
        {
            for (byte i = 0; i < safeAreaTransforms.Length; i++)
            {
                
                safeAreaTransforms[i].anchorMin = anchorMin;
                safeAreaTransforms[i].anchorMax = anchorMax;
            }
        }
    }

    public static void ApplySafeArea(RectTransform safeAreaTransforms)
    {
        lastKnownSafeArea = Screen.safeArea;
#if UNITY_EDITOR
		
		anchorMin = lastKnownSafeArea.position;
		anchorMax = lastKnownSafeArea.position + lastKnownSafeArea.size;// - Vector2.up * 100f;
#else

        anchorMin = lastKnownSafeArea.position;
        anchorMax = lastKnownSafeArea.position + lastKnownSafeArea.size;
#endif

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        if (safeAreaTransforms != null)
        {
            safeAreaTransforms.anchorMin = anchorMin;
            safeAreaTransforms.anchorMax = anchorMax;
        }
    }


    /*public static void ApplySafeArea(RectTransform safeAreaTransform)
    {
        Vector2 _anchorMin = new Vector2(anchorMin.x / Screen.width, 0);
        Vector2 _anchorMax = new Vector2(anchorMax.x / Screen.width, anchorMax.y / Screen.height);
        safeAreaTransform.anchorMin = _anchorMin;
        safeAreaTransform.anchorMax = _anchorMax;
    }*/

    public static void RegisterAndSetForSafeArea(Action callback)
    {
        OnSafeAreaChanged += callback;
        callback?.Invoke();
    }

    public static void UnregisterForSafeArea(Action callback) => OnSafeAreaChanged -= callback;
    //#endif
}
