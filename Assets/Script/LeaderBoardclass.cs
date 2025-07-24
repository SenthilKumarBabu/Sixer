using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class LeaderBoardclass : Singleton<LeaderBoardclass> 
{
	public GameObject Holder;
	public Image[] Buttons  ;
	public Sprite selectedTab, defaultTab;
	public LeaderBoardDetails[] LBDetail;
	public LeaderBoardDetails LBDetail_User;
	public Text NoData;
	public Sprite UserBG, OthersBG;


	#region ANIMATION
	[Header("ANIMATION")]
	public RectTransform shineRect;
	void startLogoAnim()
	{
		shineRect.DOLocalMoveX(-300f, 0f);
		Sequence seq = DOTween.Sequence();
		seq.Insert(0f, shineRect.DOLocalMoveX(300f, 4f));
		seq.SetLoops(-1, LoopType.Restart);
	}
	Sequence barSeq,shineSeq;
	void DoBarAnimation(int idx)
	{
		if(barSeq!=null)
			barSeq.Kill();
		if (shineSeq != null)
			shineSeq.Kill();

		LBDetail[idx].Shine.gameObject.SetActive(true);
		LBDetail[idx].Shine.transform.DOLocalMoveX(-310f, 1f);

		barSeq = DOTween.Sequence();
		barSeq.Insert(0f, LBDetail[idx].holder.transform.DOScale(1.05f, 1f));
		barSeq.SetLoops(-1, LoopType.Yoyo);
		barSeq.SetUpdate(true);

		shineSeq = DOTween.Sequence();
		shineSeq.Insert(0f, LBDetail[idx].Shine.transform.DOLocalMoveX(310f, 4f));
		shineSeq.SetLoops(-1, LoopType.Restart);
		shineSeq.SetUpdate(true);
	}
	private void OnDisable()
	{
		barSeq.Kill();
		shineSeq.Kill();
	}
	#endregion

	void OnEnable() 
	{
		Buttons [0].sprite = selectedTab;
		Buttons [1].sprite = defaultTab;
		Buttons [2].sprite = defaultTab;
		canplaysound = false;
		DisplayDetails (0);
		startLogoAnim();
		GameModeSelector._instance.ShowLandingPage(false);
	}
	bool canplaysound = false;
	public void DisplayDetails (int index)
	{
		if(canplaysound)
			AudioPlayer.instance.PlayButtonSnd();
		canplaysound = true;
		ResetLeaderBoard ();	
		if (index == 0)
		{
			ListUserNames ("weekly");
            Buttons [0].sprite = selectedTab;
			Buttons [1].sprite = defaultTab;
			Buttons [2].sprite = defaultTab;
		}
		else if (index == 1)
		{
			ListUserNames ("monthly");
            Buttons [1].sprite = selectedTab;
			Buttons [0].sprite = defaultTab;
			Buttons [2].sprite = defaultTab;
		}
		else if (index == 2)
		{
			ListUserNames ("alltime");
            Buttons [2].sprite = selectedTab;
			Buttons [1].sprite = defaultTab;
			Buttons [0].sprite = defaultTab;
		}
	}

	public void  ResetLeaderBoard ()
	{
		int  i ;
		for(i = 0; i < LBDetail.Length; i++)
		{
			LBDetail[i].Name.text = "";
			LBDetail[i].Points.text = "";
			LBDetail [i].Position .text ="";
		}
	}

	private void HideAllContents()
	{
		if (barSeq != null)
			barSeq.Kill();
		if (shineSeq != null)
			shineSeq.Kill();

		for (int i = 0; i < LBDetail.Length; i++)
		{
			LBDetail[i].gameObject.SetActive(false);
			LBDetail[i].gameObject.transform.localScale = Vector3.one;
			LBDetail[i].Shine.gameObject.SetActive(false);
		}

		LBDetail_User.gameObject.SetActive(false);

        NoData.enabled = false;

    }

    public void  ListUserNames (string  LBtype)
	{
		 int i = 0;

		if(LBtype == "weekly" && CONTROLLER.WeeklyList != null)
		{
			HideAllContents();
			int cnt = CONTROLLER.WeeklyList.Length;
			if (cnt > 10)
				cnt = 10;
			if ( cnt!=0)
			{
				for (i=0; i<cnt ; i++)
				{		
					LBDetail[i].gameObject.SetActive(true);

					LBDetail[i].holder.sprite = OthersBG;
					LBDetail[i].Name.text = CONTROLLER.UppercaseFirst( CONTROLLER.WeeklyList[i].UserName);
					LBDetail[i].Points.text = CONTROLLER.WeeklyList[i].UserPoints;
					LBDetail[i].Position.text = AddZeroToRank(CONTROLLER.WeeklyList[i].UserRank);
				}

				LBDetail_User.gameObject.SetActive(true);
				LBDetail_User.Name.text = CONTROLLER.UppercaseFirst(CONTROLLER.myRankWeekly.UserName);
				LBDetail_User.Points.text = CONTROLLER.myRankWeekly.UserPoints;
				LBDetail_User.Position.text = AddZeroToRank(CONTROLLER.myRankWeekly.UserRank);

				int n;
				bool isNumeric = int.TryParse(CONTROLLER.myRankWeekly.UserRank, out n);
				if (isNumeric && n != 0 && n > 0 && n <= 10)
				{
					LBDetail[n - 1].holder.sprite = UserBG;
					DoBarAnimation(n - 1);
				}
			
				//if(!isNumeric || n ==0 )
				//	LBDetail_User.Position.text = "-";
			}
			else
			{
				NoData.enabled = true;
				NoData.text = "No data available for this week.";
			}
		}
		else if(LBtype == "monthly" && CONTROLLER.MonthlyList != null)
		{
			HideAllContents();
			int cnt = CONTROLLER.MonthlyList.Length;
			if (cnt > 10)
				cnt = 10;
			if (cnt !=0)
			{
                for (i=0; i<cnt ; i++)
				{
					LBDetail[i].gameObject.SetActive(true);
					
					LBDetail[i].holder.sprite = OthersBG;
					LBDetail[i].Name.text = CONTROLLER.UppercaseFirst(CONTROLLER.MonthlyList[i].UserName);
					LBDetail[i].Points.text =  CONTROLLER.MonthlyList[i].UserPoints;
					LBDetail[i].Position .text = AddZeroToRank(CONTROLLER.MonthlyList[i].UserRank) ;
				}
				
				LBDetail_User.gameObject.SetActive(true);
				LBDetail_User.Name.text = CONTROLLER.UppercaseFirst(CONTROLLER.myRankMonthly.UserName);
				LBDetail_User.Points.text = CONTROLLER.myRankMonthly.UserPoints;
				LBDetail_User.Position.text = AddZeroToRank(CONTROLLER.myRankMonthly.UserRank) ;

				int n;
				bool isNumeric = int.TryParse(CONTROLLER.myRankMonthly.UserRank, out n);

				if (isNumeric && n!=0 && n > 0 && n<=10)
				{		
					LBDetail[n-1].holder.sprite = UserBG;
					DoBarAnimation(n - 1);
				}

				//if (!isNumeric || n == 0)
				//	LBDetail_User.Position.text = "-";
			}
			else
			{
				NoData.enabled = true;
				NoData.text = " No data available for this month.";
			}

		}
		else if(LBtype == "alltime" && CONTROLLER.AllTimeList != null)
		{
			HideAllContents();
			int cnt = CONTROLLER.AllTimeList.Length;
			if (cnt > 10)
				cnt = 10;
			if (cnt != 0)
			{
				for (i = 0; i < cnt; i++)
				{
					LBDetail[i].gameObject.SetActive(true);
					LBDetail[i].holder.sprite = OthersBG;
					LBDetail[i].Name.text = CONTROLLER.UppercaseFirst(CONTROLLER.UppercaseFirst(CONTROLLER.AllTimeList[i].UserName));
					LBDetail[i].Points.text = CONTROLLER.AllTimeList[i].UserPoints;
					LBDetail[i].Position.text = AddZeroToRank(CONTROLLER.AllTimeList[i].UserRank);
				}

				LBDetail_User.gameObject.SetActive(true);
				LBDetail_User.Name.text = CONTROLLER.UppercaseFirst(CONTROLLER.myRankAllTime.UserName);
				LBDetail_User.Points.text = CONTROLLER.myRankAllTime.UserPoints;
				LBDetail_User.Position.text = AddZeroToRank(CONTROLLER.myRankAllTime.UserRank);



				int n;
				bool isNumeric = int.TryParse(CONTROLLER.myRankAllTime.UserRank, out n);

				if ( isNumeric && n != 0 && n > 0 && n <= 10)
				{
					LBDetail[n - 1].holder.sprite = UserBG;
					DoBarAnimation(n - 1);
				}

				//if (!isNumeric || n == 0)
				//	LBDetail_User.Position.text = "-";
			}
			else
			{
				NoData.enabled = true;
				NoData.text = "No data available for all time.";
			}
		}

	}

	string AddZeroToRank(string rankText)
	{
        int n;
        bool isNumeric = int.TryParse(rankText, out n);

		if (isNumeric && n != 0 && n > 0 && n <= 9)
		{
			return "0" + rankText;
		}
		else
			return rankText;
	}

	public void  HideMe ()
	{
		Holder.SetActive (false);
	}
}


