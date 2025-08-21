using UnityEngine;

public class BotController : MonoBehaviour
{
    [HideInInspector]public bool isBot = false;
    public void InitAsBot()
    {
        isBot = true;
    }
}
