using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShotVariables : MonoBehaviour
{
	public static Dictionary<string, float> optimalShotTable;

	public static void InitShotVariables()
	{
		optimalShotTable = new Dictionary<string, float>();

		optimalShotTable.Add("Lofted3rdManGlanceOptimalShotLength", 18.2f);
		optimalShotTable.Add("Lofted3rdManGlanceOptimalShotFrame", 10.0f);

		optimalShotTable.Add("UpperCutOptimalShotLength", 18.0f);
		optimalShotTable.Add("UpperCutOptimalShotFrame", 10.0f);

		optimalShotTable.Add("LateCutOptimalShotLength", 18.0f);
		optimalShotTable.Add("LateCutOptimalShotFrame", 12.0f);

		optimalShotTable.Add("LateCut2OptimalShotLength", 16.9f);
		optimalShotTable.Add("LateCut2OptimalShotFrame", 22.0f);

		optimalShotTable.Add("LoftedReverseSweepOptimalShotLength", 16.2f);
		optimalShotTable.Add("LoftedReverseSweepOptimalShotFrame", 17.0f);

		optimalShotTable.Add("ReverseSweep2OptimalShotLength", 16.5f);
		optimalShotTable.Add("ReverseSweep2OptimalShotFrame", 13.0f);

		optimalShotTable.Add("LegGlanceOptimalShotLength", 16.8f);
		optimalShotTable.Add("LegGlanceOptimalShotFrame", 8.0f);

		optimalShotTable.Add("PullShotOptimalShotLength", 17.1f);
		optimalShotTable.Add("PullShotOptimalShotFrame", 6.0f);

		optimalShotTable.Add("MidWicketPushOptimalShotLength", 17.0f);
		optimalShotTable.Add("MidWicketPushOptimalShotFrame", 7.0f);

		optimalShotTable.Add("MidWicketPush2OptimalShotLength", 16.6f);
		optimalShotTable.Add("MidWicketPush2OptimalShotFrame", 8.0f);

		optimalShotTable.Add("MidWicketSlogOptimalShotLength", 17.1f);
		optimalShotTable.Add("MidWicketSlogOptimalShotFrame", 6.0f);

		optimalShotTable.Add("LoftedSweepShotOptimalShotLength", 16.5f);
		optimalShotTable.Add("LoftedSweepShotOptimalShotFrame", 6.0f);

		optimalShotTable.Add("LoftedPullShotOptimalShotLength", 17.0f);
		optimalShotTable.Add("LoftedPullShotOptimalShotFrame", 5.5f);

		optimalShotTable.Add("HookShotOptimalShotLength", 17.0f);
		optimalShotTable.Add("HookShotOptimalShotFrame", 7.0f);

		optimalShotTable.Add("HookShot2OptimalShotLength", 16.7f); // new...
		optimalShotTable.Add("HookShot2OptimalShotFrame", 8.0f);

		optimalShotTable.Add("OnSidePushOptimalShotLength", 17.5f);
		optimalShotTable.Add("OnSidePushOptimalShotFrame", 6.0f);

		optimalShotTable.Add("OnSideSlogOptimalShotLength", 16.9f);
		optimalShotTable.Add("OnSideSlogOptimalShotFrame", 8.0f);

		optimalShotTable.Add("StraightDriveOptimalShotLength", 16.7f);
		optimalShotTable.Add("StraightDriveOptimalShotFrame", 7.0f);

		optimalShotTable.Add("StraightSlogOptimalShotLength", 16.6f); // 16.5
		optimalShotTable.Add("StraightSlogOptimalShotFrame", 7.0f); // 8.0f

		optimalShotTable.Add("BackFootStraightDriveOptimalShotLength", 18.0f); //17.6f
		optimalShotTable.Add("BackFootStraightDriveOptimalShotFrame", 8.0f); // 9.0f

		optimalShotTable.Add("OnDriveOptimalShotLength", 17.0f); // new
		optimalShotTable.Add("OnDriveOptimalShotFrame", 9.0f);

		optimalShotTable.Add("OnDrive2OptimalShotLength", 16.6f); // new
		optimalShotTable.Add("OnDrive2OptimalShotFrame", 10.0f);

		optimalShotTable.Add("LoftedStraightDriveOptimalShotLength", 17.2f);
		optimalShotTable.Add("LoftedStraightDriveOptimalShotFrame", 7.0f);

		optimalShotTable.Add("HelicoptorShotOptimalShotLength", 17.3f);
		optimalShotTable.Add("HelicoptorShotOptimalShotFrame", 8.0f);

		optimalShotTable.Add("FrontFootPushOptimalShotLength", 16.5f);
		optimalShotTable.Add("FrontFootPushOptimalShotFrame", 7.0f);

		optimalShotTable.Add("BackFootPushOptimalShotLength", 17.6f);
		optimalShotTable.Add("BackFootPushOptimalShotFrame", 7.0f);

		optimalShotTable.Add("LoftedOnDriveOptimalShotLength", 16.4f); // new
		optimalShotTable.Add("LoftedOnDriveOptimalShotFrame", 8.0f);

		optimalShotTable.Add("DilsonScoopOptimalShotLength", 16.4f);
		optimalShotTable.Add("DilsonScoopOptimalShotFrame", 10.0f);

		optimalShotTable.Add("SweepShotOptimalShotLength", 16.1f);
		optimalShotTable.Add("SweepShotOptimalShotFrame", 9.0f);

		optimalShotTable.Add("SweepShot2OptimalShotLength", 16.9f);
		optimalShotTable.Add("SweepShot2OptimalShotFrame", 7.0f);

		optimalShotTable.Add("BackFootDriveOptimalShotLength", 17.7f);
		optimalShotTable.Add("BackFootDriveOptimalShotFrame", 7.0f);

		optimalShotTable.Add("LoftedOffDriveOptimalShotLength", 16.7f);
		optimalShotTable.Add("LoftedOffDriveOptimalShotFrame", 9.0f);

		optimalShotTable.Add("SquareCutOptimalShotLength", 18.0f);
		optimalShotTable.Add("SquareCutOptimalShotFrame", 12.0f);

		optimalShotTable.Add("SquarePushOptimalShotLength", 17.5f);
		optimalShotTable.Add("SquarePushOptimalShotFrame", 7.0f);

		optimalShotTable.Add("CoverDrive2OptimalShotLength", 16.7f);
		optimalShotTable.Add("CoverDrive2OptimalShotFrame", 10.0f);

		optimalShotTable.Add("SquareCut2OptimalShotLength", 17.5f);
		optimalShotTable.Add("SquareCut2OptimalShotFrame", 9.0f);

		optimalShotTable.Add("SquareSlogOptimalShotLength", 17.6f);
		optimalShotTable.Add("SquareSlogOptimalShotFrame", 15.0f);

		optimalShotTable.Add("LoftedSquareCutOptimalShotLength", 18.1f);
		optimalShotTable.Add("LoftedSquareCutOptimalShotFrame", 9.0f);

		optimalShotTable.Add("LoftedCoverDriveOptimalShotLength", 17.3f);
		optimalShotTable.Add("LoftedCoverDriveOptimalShotFrame", 7.5f);

		optimalShotTable.Add("WideHitLowOptimalShotLength", 17.9f);
		optimalShotTable.Add("WideHitLowOptimalShotFrame", 15.0f);

		optimalShotTable.Add("CoverSlogOptimalShotLength", 16.5f);
		optimalShotTable.Add("CoverSlogOptimalShotFrame", 9.0f);

		optimalShotTable.Add("3rdManGlanceOptimalShotLength", 18.0f);
		optimalShotTable.Add("3rdManGlanceOptimalShotFrame", 12.0f);

		optimalShotTable.Add("LateCut3OptimalShotLength", 18.0f);
		optimalShotTable.Add("LateCut3OptimalShotFrame", 15.0f);
		// new shots...

		optimalShotTable.Add("SquareCut11OptimalShotLength", 17.6f);
		optimalShotTable.Add("SquareCut11OptimalShotFrame", 13.0f);

		optimalShotTable.Add("SquareCut12OptimalShotLength", 17.6f);
		optimalShotTable.Add("SquareCut12OptimalShotFrame", 11.0f);

		optimalShotTable.Add("WideSquareCutOptimalShotLength", 17.6f);
		optimalShotTable.Add("WideSquareCutOptimalShotFrame", 16.0f);

		// Original OffDrive
		optimalShotTable.Add("OffDrive3OptimalShotLength", 17.4f);
		optimalShotTable.Add("OffDrive3OptimalShotFrame", 9.0f);

		optimalShotTable.Add("LoftedSquareCut2OptimalShotLength", 17.6f);
		optimalShotTable.Add("LoftedSquareCut2OptimalShotFrame", 13.0f);

		optimalShotTable.Add("BackPushOptimalShotLength", 17.6f);
		optimalShotTable.Add("BackPushOptimalShotFrame", 11.0f);

		optimalShotTable.Add("LateCut4OptimalShotLength", 18.0f);
		optimalShotTable.Add("LateCut4OptimalShotFrame", 13.0f);

		// new shots...

		optimalShotTable.Add("YorkerBall1OptimalShotLength", 17.7f);
		optimalShotTable.Add("YorkerBall1OptimalShotFrame", 20.0f);

		optimalShotTable.Add("YorkerBall2OptimalShotLength", 17.7f);
		optimalShotTable.Add("YorkerBall2OptimalShotFrame", 15.0f);

		//Defense		
		optimalShotTable.Add("BackFootDefenseOptimalShotLength", 17.7f);
		optimalShotTable.Add("BackFootDefenseOptimalShotFrame", 12.0f);

		optimalShotTable.Add("FrontFootDefenseOptimalShotLength", 16.6f);
		optimalShotTable.Add("FrontFootDefenseOptimalShotFrame", 12.0f);

		//Nijanthan
		optimalShotTable.Add("Bowled_Expression01OptimalShotLength", 18f);
		optimalShotTable.Add("Bowled_Expression01OptimalShotFrame", 14f);

		optimalShotTable.Add("Bowled_Expression02OptimalShotLength", 18f);
		optimalShotTable.Add("Bowled_Expression02OptimalShotFrame", 20f);

		optimalShotTable.Add("Bowled_Expression03OptimalShotLength", 17.5f);
		optimalShotTable.Add("Bowled_Expression03OptimalShotFrame", 18f);

		optimalShotTable.Add("Bowled_Expression04OptimalShotLength", 16.5f);
		optimalShotTable.Add("Bowled_Expression04OptimalShotFrame", 20f);

		//Well-left Defence
		optimalShotTable.Add("Well_Left_Pace01OptimalShotLength", 18f);
		optimalShotTable.Add("Well_Left_Pace01OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Pace02OptimalShotLength", 17f);
		optimalShotTable.Add("Well_Left_Pace02OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Pace03OptimalShotLength", 18f);
		optimalShotTable.Add("Well_Left_Pace03OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Pace04OptimalShotLength", 19f);
		optimalShotTable.Add("Well_Left_Pace04OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Pace05OptimalShotLength", 19f);
		optimalShotTable.Add("Well_Left_Pace05OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Pace06OptimalShotLength", 17f);
		optimalShotTable.Add("Well_Left_Pace06OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Spin01OptimalShotLength", 20f);
		optimalShotTable.Add("Well_Left_Spin01OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Spin02OptimalShotLength", 15f);
		optimalShotTable.Add("Well_Left_Spin02OptimalShotFrame", 17.8f);

		optimalShotTable.Add("Well_Left_Spin03OptimalShotLength", 16f);
		optimalShotTable.Add("Well_Left_Spin03OptimalShotFrame", 17f);

		optimalShotTable.Add("Well_Left_Spin04OptimalShotLength", 19f);
		optimalShotTable.Add("Well_Left_Spin04OptimalShotFrame", 17f);

		//Nijanthan Defense
		optimalShotTable.Add("BackFootDefense01OptimalShotLength", 17.7f);
		optimalShotTable.Add("BackFootDefense01OptimalShotFrame", 15.0f);

		optimalShotTable.Add("BackFootDefense02OptimalShotLength", 16.0f);//AI Improve
		optimalShotTable.Add("BackFootDefense02OptimalShotFrame", 17.0f);

		optimalShotTable.Add("BackFootDefense03OptimalShotLength", 17.7f);
		optimalShotTable.Add("BackFootDefense03OptimalShotFrame", 17.0f);

		optimalShotTable.Add("FrontFootDefense01OptimalShotLength", 16.6f);
		optimalShotTable.Add("FrontFootDefense01OptimalShotFrame", 16.0f);

		optimalShotTable.Add("FrontFootDefense02OptimalShotLength", 16.6f);
		optimalShotTable.Add("FrontFootDefense02OptimalShotFrame", 15.0f);

		optimalShotTable.Add("FrontFootDefense03OptimalShotLength", 16.6f);
		optimalShotTable.Add("FrontFootDefense03OptimalShotFrame", 15.0f);

		optimalShotTable.Add("FrontFootDefense04OptimalShotLength", 16.6f);
		optimalShotTable.Add("FrontFootDefense04OptimalShotFrame", 15.0f);

		optimalShotTable.Add("FrontFootDefense05OptimalShotLength", 16.3f);
		optimalShotTable.Add("FrontFootDefense05OptimalShotFrame", 18.0f);

		optimalShotTable.Add("OffDriveOptimalShotLength", 17.2f);
		optimalShotTable.Add("OffDriveOptimalShotFrame", 7.0f);

		//New batting shots - First 10
		optimalShotTable.Add("UnOrthodoxShot1OptimalShotLength", 17.2f);
		optimalShotTable.Add("UnOrthodoxShot1OptimalShotFrame", 16.0f);

		optimalShotTable.Add("UnOrthodoxShot2OptimalShotLength", 17.5f);
		optimalShotTable.Add("UnOrthodoxShot2OptimalShotFrame", 17.0f);

		optimalShotTable.Add("ExtraCoverDriveOptimalShotLength", 17.5f);
		optimalShotTable.Add("ExtraCoverDriveOptimalShotFrame", 14.0f);

		optimalShotTable.Add("PaddleSweepOptimalShotLength", 16.2f);
		optimalShotTable.Add("PaddleSweepOptimalShotFrame", 10.0f);

		optimalShotTable.Add("SweepShotSpinOptimalShotLength", 16.2f); // 16.3
		optimalShotTable.Add("SweepShotSpinOptimalShotFrame", 14.0f); // 15.0

		optimalShotTable.Add("DownTheTrackStraightSlogOptimalShotLength", 16.5f);
		optimalShotTable.Add("DownTheTrackStraightSlogOptimalShotFrame", 16.0f);

		optimalShotTable.Add("DownTheTrackOffSideLoftOptimalShotLength", 16.7f);
		optimalShotTable.Add("DownTheTrackOffSideLoftOptimalShotFrame", 13.0f);

		optimalShotTable.Add("LoftedMidWicketOptimalShotLength", 16.8f);
		optimalShotTable.Add("LoftedMidWicketOptimalShotFrame", 7.0f);

		optimalShotTable.Add("LoftedReverseSweep2OptimalShotLength", 16.5f);
		optimalShotTable.Add("LoftedReverseSweep2OptimalShotFrame", 15.0f);

		optimalShotTable.Add("PointSlogOptimalShotLength", 17.5f); // 16.8
		optimalShotTable.Add("PointSlogOptimalShotFrame", 9.0f); // 20

		//New batting shots - Remaining 15
		optimalShotTable.Add("DeepMidWicketShotOptimalShotLength", 17.5f);
		optimalShotTable.Add("DeepMidWicketShotOptimalShotFrame", 16.0f);

		optimalShotTable.Add("PullShotChest01OptimalShotLength", 17.4f);
		optimalShotTable.Add("PullShotChest01OptimalShotFrame", 9.0f);

		optimalShotTable.Add("PullShotHip01OptimalShotLength", 16.5f);
		optimalShotTable.Add("PullShotHip01OptimalShotFrame", 12.0f);

		optimalShotTable.Add("DownTheTrackDefensiveShotOptimalShotLength", 15.7f); // 16.2f
		optimalShotTable.Add("DownTheTrackDefensiveShotOptimalShotFrame", 13.0f);


		optimalShotTable.Add("DownTheTrackHittingLegSideOptimalShotLength", 16.4f); // 16.5
		optimalShotTable.Add("DownTheTrackHittingLegSideOptimalShotFrame", 16.0f);


		optimalShotTable.Add("LoftedDeepCoverShotOptimalShotLength", 17.1f);
		optimalShotTable.Add("LoftedDeepCoverShotOptimalShotFrame", 16.0f);

		optimalShotTable.Add("ThirdManFlickOptimalShotLength", 17.9f);
		optimalShotTable.Add("ThirdManFlickOptimalShotFrame", 19.0f);

		optimalShotTable.Add("PullShotChestOptimalShotLength", 16.8f);
		optimalShotTable.Add("PullShotChestOptimalShotFrame", 10f);// 11.0f);

		optimalShotTable.Add("PullShotHipOptimalShotLength", 16.5f);
		optimalShotTable.Add("PullShotHipOptimalShotFrame", 9f);		//	12.0f);

		optimalShotTable.Add("ReverseSweepSpinOptimalShotLength", 16.3f);//15.8f
		optimalShotTable.Add("ReverseSweepSpinOptimalShotFrame", 13.0f);

		optimalShotTable.Add("StepOutOffSlogOptimalShotLength", 17.4f);
		optimalShotTable.Add("StepOutOffSlogOptimalShotFrame", 15.0f);

		optimalShotTable.Add("UpperCut2OptimalShotLength", 17.4f); // 17.3f
		optimalShotTable.Add("UpperCut2OptimalShotFrame", 15.0f); // 18.0f


		optimalShotTable.Add("DownTheTrackMidOffOptimalShotLength", 16.0f);
		optimalShotTable.Add("DownTheTrackMidOffOptimalShotFrame", 13.0f);

		optimalShotTable.Add("DownTheTrackMidOnOptimalShotLength", 15.8f);
		optimalShotTable.Add("DownTheTrackMidOnOptimalShotFrame", 14.0f);

		optimalShotTable.Add("PointSlogHipOptimalShotLength", 16.8f); // 17.0f
		optimalShotTable.Add("PointSlogHipOptimalShotFrame", 10.0f); // 20.0f

		//nijanthan new Shots length-dist, frame-animation frame
		optimalShotTable.Add("BackfootPunchOptimalShotLength", 18.0f);
		optimalShotTable.Add("BackfootPunchOptimalShotFrame", 14.0f);

		optimalShotTable.Add("DownTheTrackSpin_StraightDriveOptimalShotLength", 15.8f); // 15.0f
		optimalShotTable.Add("DownTheTrackSpin_StraightDriveOptimalShotFrame", 16.0f); // 17.0f


		optimalShotTable.Add("LegGlance_FrontfootOptimalShotLength", 17.0f);
		optimalShotTable.Add("LegGlance_FrontfootOptimalShotFrame", 9.0f);

		optimalShotTable.Add("McCullum_UnorthodoxShotOptimalShotLength", 16.0f);
		optimalShotTable.Add("McCullum_UnorthodoxShotOptimalShotFrame", 14.0f);

		optimalShotTable.Add("MidOnSlogOptimalShotLength", 16.0f);//
		optimalShotTable.Add("MidOnSlogOptimalShotFrame", 15.0f);

		optimalShotTable.Add("Spin_BackwardPointOptimalShotLength", 18.0f);
		optimalShotTable.Add("Spin_BackwardPointOptimalShotFrame", 13.0f);

		optimalShotTable.Add("Spin_CoverPointOptimalShotLength", 17.6f);
		optimalShotTable.Add("Spin_CoverPointOptimalShotFrame", 14.0f);

		optimalShotTable.Add("Spin_CutShotOptimalShotLength", 17.8f);//issue
		optimalShotTable.Add("Spin_CutShotOptimalShotFrame", 13.0f);

		optimalShotTable.Add("Spin_DeepExtraCoverOptimalShotLength", 16.6f);//
		optimalShotTable.Add("Spin_DeepExtraCoverOptimalShotFrame", 13.0f);

		optimalShotTable.Add("Spin_DeepMidWicketOptimalShotLength", 16.0f);//issue
		optimalShotTable.Add("Spin_DeepMidWicketOptimalShotFrame", 15.0f);

		optimalShotTable.Add("Spin_LegGlance_01OptimalShotLength", 16.8f);
		optimalShotTable.Add("Spin_LegGlance_01OptimalShotFrame", 15.0f);

		optimalShotTable.Add("Spin_LegGlance_02OptimalShotLength", 17.5f);//issue
		optimalShotTable.Add("Spin_LegGlance_02OptimalShotFrame", 15.0f);

		optimalShotTable.Add("Spin_MidwicketPushOptimalShotLength", 16.5f);//
		optimalShotTable.Add("Spin_MidwicketPushOptimalShotFrame", 14.0f);

		optimalShotTable.Add("SquareCut_NewOptimalShotLength", 18.2f);//issue
		optimalShotTable.Add("SquareCut_NewOptimalShotFrame", 15.0f);

		optimalShotTable.Add("SwitchHit_To_SweepOptimalShotLength", 16.0f);//issue
		optimalShotTable.Add("SwitchHit_To_SweepOptimalShotFrame", 17.0f);



		optimalShotTable.Add("DavidWarner_Switch_HitOptimalShotLength", 17.0f);
		optimalShotTable.Add("DavidWarner_Switch_HitOptimalShotFrame", 16.0f);

		optimalShotTable.Add("Unorthodox_ThirdmanFlickOptimalShotLength", 17.3f);
		optimalShotTable.Add("Unorthodox_ThirdmanFlickOptimalShotFrame", 14.0f);

		optimalShotTable.Add("StraightDrive_PaceOptimalShotLength", 16.7f);
		optimalShotTable.Add("StraightDrive_PaceOptimalShotFrame", 11.0f);




		optimalShotTable.Add("BouncerNew_01OptimalShotLength", 17.2f);//issue
		optimalShotTable.Add("BouncerNew_01OptimalShotFrame", 9.0f);

		optimalShotTable.Add("BouncerNew_02OptimalShotLength", 17.0f);//issue
		optimalShotTable.Add("BouncerNew_02OptimalShotFrame", 13.0f);

		optimalShotTable.Add("MidOffDriveOptimalShotLength", 17.2f);
		optimalShotTable.Add("MidOffDriveOptimalShotFrame", 9.0f);


		//Big Bash Shots

		optimalShotTable.Add("AaronFinch_BouncerFlickOptimalShotLength", 17.2f);
		optimalShotTable.Add("AaronFinch_BouncerFlickOptimalShotFrame", 8.0f);


		optimalShotTable.Add("AaronFinch_FineLegFlickOptimalShotLength", 17.2f);
		optimalShotTable.Add("AaronFinch_FineLegFlickOptimalShotFrame", 8.0f);

		optimalShotTable.Add("ChrisLynn_OnSideHeaveOptimalShotLength", 16f);
		optimalShotTable.Add("ChrisLynn_OnSideHeaveOptimalShotFrame", 6.0f);

		optimalShotTable.Add("DArcyShort_LongOnSlogOptimalShotLength", 16f);
		optimalShotTable.Add("DArcyShort_LongOnSlogOptimalShotFrame", 10.0f);


		optimalShotTable.Add("GeorgeBailey_DeepMidWicketSlogOptimalShotLength", 16f);
		optimalShotTable.Add("GeorgeBailey_DeepMidWicketSlogOptimalShotFrame", 9.0f);

		optimalShotTable.Add("GeorgeBailey_PullSlogOptimalShotLength", 17f);
		optimalShotTable.Add("GeorgeBailey_PullSlogOptimalShotFrame", 6.0f);

		optimalShotTable.Add("GlennMaxwell_SwitchHitOptimalShotLength", 16.5f);
		optimalShotTable.Add("GlennMaxwell_SwitchHitOptimalShotFrame", 17.0f);

		optimalShotTable.Add("GlennMaxwell_SwitchHit2OptimalShotLength", 16.5f);
		optimalShotTable.Add("GlennMaxwell_SwitchHit2OptimalShotFrame", 17.0f);

		optimalShotTable.Add("MichaelKlinger_LongOffSlogOptimalShotLength", 16.5f);
		optimalShotTable.Add("MichaelKlinger_LongOffSlogOptimalShotFrame", 10.0f);

		optimalShotTable.Add("MoisesHenriques_DownTheTrackOptimalShotLength", 16.5f);
		optimalShotTable.Add("MoisesHenriques_DownTheTrackOptimalShotFrame", 14.0f);

		optimalShotTable.Add("ShaneWatson_PullShotOptimalShotLength", 16.5f);
		optimalShotTable.Add("ShaneWatson_PullShotOptimalShotFrame", 16.0f);

		optimalShotTable.Add("DhoniHeliCopter_NewOptimalShotLength", 17.3f);
		optimalShotTable.Add("DhoniHeliCopter_NewOptimalShotFrame", 10.0f);


		optimalShotTable.Add("ScoopShotOptimalShotLength", 16.4f);
		optimalShotTable.Add("ScoopShotOptimalShotFrame", 10.0f);

		optimalShotTable.Add("Thirdman_UpperCutOptimalShotLength", 18.0f);
		optimalShotTable.Add("Thirdman_UpperCutOptimalShotFrame", 12.0f);

		optimalShotTable.Add("ShortPitch_LongOnPullShotOptimalShotLength", 17.1f);
		optimalShotTable.Add("ShortPitch_LongOnPullShotOptimalShotFrame", 13.0f);

		optimalShotTable.Add("DeepSquareLeg_FlickOptimalShotLength", 16.5f);
		optimalShotTable.Add("DeepSquareLeg_FlickOptimalShotFrame", 13.0f);

		optimalShotTable.Add("ShortPitch_ LongOnSlogOptimalShotLength", 16f);
		optimalShotTable.Add("ShortPitch_ LongOnSlogOptimalShotFrame", 11.0f);

		optimalShotTable.Add("DownTheTrackSpin_LongOnOptimalShotLength", 15.5f);
		optimalShotTable.Add("DownTheTrackSpin_LongOnOptimalShotFrame", 17.0f);

		optimalShotTable.Add("TravisHead_SlapShotOptimalShotLength", 17.5f);
		optimalShotTable.Add("TravisHead_SlapShotOptimalShotFrame", 17.0f);

		optimalShotTable.Add("MidOnStrikeOptimalShotLength", 17.0f);
		optimalShotTable.Add("MidOnStrikeOptimalShotFrame", 13.0f);

		optimalShotTable.Add("ThirdManGlanceOptimalShotLength", 18.3f);
		optimalShotTable.Add("ThirdManGlanceOptimalShotFrame", 13.0f);
	}
}