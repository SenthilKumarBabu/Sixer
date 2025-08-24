using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePopupAnimation : MonoBehaviour
{
    [Header("UI Targets")]
    public RectTransform[] UserScoreTarget;
    public RectTransform[] OppScoreTarget;
    public GameObject parentObject;              

    [Header("Prefab")]
    public GameObject scorePrefab; 


    public void ShowScore(string scoreText, bool isUser)
    {
        GameObject scoreObj = Instantiate(scorePrefab, parentObject.transform);
        Text scoreUI = scoreObj.GetComponent<Text>(); 
        //TMP_Text scoreUI = scoreObj.GetComponent<TMP_Text>();
        RectTransform scoreRect = scoreObj.GetComponent<RectTransform>();
        scoreUI.text = scoreText;
        scoreUI.color = new Color(scoreUI.color.r, scoreUI.color.g, scoreUI.color.b, 1f);
        //scoreRect.anchoredPosition = Vector2.zero;
        //scoreRect.localPosition = Vector3.zero;
        
        Vector2 startOffset = isUser ? new Vector2(-50f, 0f) : new Vector2(50f, 0f);
        scoreRect.anchoredPosition = startOffset;

        RectTransform target = isUser ? UserScoreTarget[MultiplayerManager.Instance.userBallIndex] : OppScoreTarget[MultiplayerManager.Instance.oppBallIndex];

        Sequence seq = DOTween.Sequence();
        seq.Insert(0f, scoreRect.DOPunchScale(Vector3.one * 0.6f, 0.35f, 5, 0.8f));
        //  seq.Insert( 0.35f, scoreRect.DOMove(target.position, 1.35f).SetEase(Ease.InOutQuad));

        Vector3 startPos = scoreRect.position;
        Vector3 endPos = target.position;
        float arcOffsetX = isUser ? -100f : 100f;

        seq.Insert(0.35f, DOTween.To(
            () => 0f,
            t =>
            {
                Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                // Add curved X offset (parabola style)
                float curve = Mathf.Sin(t * Mathf.PI) * arcOffsetX;
                pos.x += curve;

                scoreRect.position = pos;
            },
            1f, 1.35f
        ).SetEase(Ease.Linear));


        seq.Insert(1f, scoreRect.DOScale(0.25f, 0.85f).SetEase(Ease.InOutQuad));
       // seq.Insert(1.1f, scoreUI.DOFade(0, 0.7f));
        seq.OnComplete(() => Destroy(scoreObj));

    }

}
