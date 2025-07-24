using System.Collections;
using UnityEngine;
public class MoveBannerTexture : MonoBehaviour
{

    [SerializeField]
    private Material matToMove;

    [SerializeField]
    private int noOfSections = 8;
    [SerializeField]
    float delay = 0.5f;
    [SerializeField]
    float moveSpeed = 0.75f;

    private float blockOffset, movementIncrement;
    sbyte direction = 1, targetCount = 1;

    private WaitForSeconds waitInstruction;
    void OnEnable()
    {
        waitInstruction = new WaitForSeconds(delay);
        blockOffset = 1.0f / noOfSections;
        movementIncrement = moveSpeed * blockOffset * Time.deltaTime;
        Reset();

    }

    IEnumerator MoveBanner()
    {
        float currentOffset;
        bool flag = false;
        do
        {
            currentOffset = matToMove.mainTextureOffset.y;
            currentOffset += movementIncrement * direction;
            if (direction == 1)
            {
                if (currentOffset > targetCount * blockOffset)
                {
                    currentOffset = targetCount * blockOffset;
                    flag = true;
                }
            }
            else
            {
                if (currentOffset < (noOfSections - targetCount) * blockOffset)
                {
                    currentOffset = (noOfSections - targetCount) * blockOffset;
                    flag = true;
                }
            }
            matToMove.mainTextureOffset = new Vector2(0, currentOffset);
            yield return null;
        } while (!flag);
        StartCoroutine(SwitchBanner());
    }

    IEnumerator SwitchBanner()
    {
        yield return waitInstruction;
        if (targetCount >= noOfSections)
        {
            targetCount = 1;
            direction *= -1;
        }
        else
            targetCount++;
        StartCoroutine(MoveBanner());
    }

    public void Reset()
    {
        StopAllCoroutines();
        matToMove.mainTextureOffset = Vector2.zero;
        targetCount = 1;
        direction = 1;
        StartCoroutine(MoveBanner());
    }
}


