using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class SoundController : MonoBehaviour
{
	protected AudioSource BgSource;
	protected AudioSource CrowdSource;
	protected AudioSource source;
	protected AudioSource BatSource;
	protected AudioSource BowlSource;
	//protected AudioSource CommentarySource;
	//protected AudioSource CSKWhistleSource;
	protected AudioSource postBattingCrowdSoundsource;

	protected AudioClip TowardsBoundaryCrowdSound;
	
	
	protected int frameFreq = 60;
	protected bool StopBgSound = false;
	protected bool StartBgSound = false;
	protected bool StopCrowdSound = false;
	protected bool StartCrowdSound = false;


	[Header("AUDIO FILES")]
	public AudioClip BGM;
	public AudioClip[] BallMissing;
	public AudioClip[] BatHit;
	public AudioClip BowledCrowd;
	public AudioClip Bowledsound;
	public AudioClip Buttonsound;
	public AudioClip CatchCrowd;
	public AudioClip CountDown;
	public AudioClip CrowdLoop;
	public AudioClip FourRuncrowd;
	public AudioClip LBWpadsound;
	public AudioClip LevelCompleted;
	public AudioClip LevelFailed;
	public AudioClip MissingCrowd;
	public AudioClip RunAppear;
	public AudioClip SixRunCrowd;
	public AudioClip celebrationsfx;

	const float CROWD_MAX_SOUND = 0.4f;
	void Awake ()
	{
		DontDestroyOnLoad(this.gameObject);
		
		BgSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
		BgSource.playOnAwake = false;
		BgSource.loop = true;
		
		CrowdSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
		CrowdSource.playOnAwake = false;
		CrowdSource.loop = true;
		
		source = this.gameObject.AddComponent<AudioSource>() as AudioSource;
		source.playOnAwake = false;
		
		BatSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
		BatSource.playOnAwake = false;

		BowlSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
		BowlSource.playOnAwake = false;
		
		postBattingCrowdSoundsource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
		postBattingCrowdSoundsource.playOnAwake = false;
	}
	
	// Use this for initialization
	void Start ()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		CrowdSource.clip = CrowdLoop;
		CrowdSource.volume = 0;
		CrowdSource.Play();
		source.clip = Buttonsound;
		LoadBgSound ();
	}
	private void OnDestroy()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
	
	int CurrentSceneIndex;
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		CurrentSceneIndex = scene.buildIndex;
	}
	public void LoadBgSound ()
	{
		BgSource.clip = BGM;
		BgSource.volume = 0;
		if(CONTROLLER.BGMusicVal == 1)
		{
			BgSource.Play();
			StartBgSound = true;
		}
	}
	
	public void MuteAudio (int boolean)
	{
		if (boolean == 0)
		{
			AudioListener.volume = 0;
		}
		else
		{
			AudioListener.volume = 1; 
		}
	}
	
	public void CallGarbageCollection ()
	{
		System.GC.Collect ();
	}
	
	void Update ()
	{
		if(StopBgSound == true)
		{
			StartBgSound = false;
			if(BgSource.volume > 0)
			{				
				BgSource.volume -= Time.deltaTime * 0.5f;
			}
			else if(BgSource.volume <= 0.1f && CONTROLLER.BGMusicVal == 1)//27march
			{				
				BgSource.volume = 0.1f;//27march
				StopBgSound = false;
			}
		}
		if (StopCrowdSound == true)
		{
			if(CrowdSource.volume > 0)
			{
				CrowdSource.volume -= Time.deltaTime * 0.2f;
			}
			else if(CrowdSource.volume <= CROWD_MAX_SOUND)
			{
				CrowdSource.volume = CROWD_MAX_SOUND;
				StopCrowdSound = false;
			}
		}
		if (StartBgSound == true && CONTROLLER.BGMusicVal == 1)
		{
			StopBgSound = false;//shankar 08April
			if (CurrentSceneIndex == 1)
			{
				if(BgSource.volume < 1)
				{
					BgSource.volume += Time.deltaTime * 0.5f;
				}
				else if(BgSource.volume >= 1)
				{
					BgSource.volume = 1;
					StartBgSound = false;
				}
			}//shankar 08April
			else if (CurrentSceneIndex == 2)
			{
				BgSource.volume = 0.1f;
				StartBgSound = false;
			}
		}
		if (StartCrowdSound == true && CONTROLLER.GameMusicVal == 1)
		{
			StopCrowdSound = false;
			if(CrowdSource.volume < CROWD_MAX_SOUND)
			{
				CrowdSource.volume += Time.deltaTime * 0.5f;
			}
			else if(CrowdSource.volume >= CROWD_MAX_SOUND)
			{
				CrowdSource.volume = CROWD_MAX_SOUND;
				StartCrowdSound = false;
			}
        }
		if (CurrentSceneIndex == 1 || CONTROLLER.GameMusicVal == 0)
		{
			CrowdSource.volume = 0;
		}
	}
	
	public void PlayButtonSnd ()
	{
		if (CONTROLLER.GameMusicVal == 1)
		{
			source.volume = 1;
			source.PlayOneShot(Buttonsound);
        }
	}
	
	public void PlayBatSnd ()
	{
		if (CONTROLLER.GameMusicVal == 1)
		{
			BatSource.volume = 1;
			int rand = UnityEngine.Random.Range(0, 100);
			if(PlayerPrefs.GetInt("loft")  == 1)
			{
				if(rand<30)
					BatSource.PlayOneShot(BatHit[0]);
				else if(rand<60)
					BatSource.PlayOneShot(BatHit[2]);
				else
					BatSource.PlayOneShot(BatHit[4]);
			}
			else
			{
				if (rand < 50)
					BatSource.PlayOneShot(BatHit[1]);
				else
					BatSource.PlayOneShot(BatHit[3]);
			}
		}
	}
	
	public void PlayBowledSnd ()
	{
		if (CONTROLLER.GameMusicVal == 1)
		{
			BowlSource.volume = 1;
			BowlSource.PlayOneShot(Bowledsound);
		}
	}


	public void PlayGameSnd (string SoundType)
	{
		if(CONTROLLER.GameMusicVal == 1)
		{
			if(SoundType== "Six Run Crowd")
			{
				source.clip = SixRunCrowd;
				source.PlayOneShot(SixRunCrowd);
			}
			else if (SoundType == "Four Run Crowd")
			{
				source.clip = FourRuncrowd;
				source.PlayOneShot(FourRuncrowd);
			}
			else if(SoundType == "LBW Pad Sound")
			{
				source.clip = LBWpadsound;
				source.PlayOneShot(LBWpadsound);
			}
			else if (SoundType == "Bowled")
			{
				source.clip = Bowledsound;
				source.PlayOneShot(Bowledsound);
			}
			else if (SoundType == "Catch Crowd")
			{
				source.clip = CatchCrowd;
				source.PlayOneShot(CatchCrowd);
			}
			else if (SoundType == "Bowled Crowd")
			{
				source.clip = BowledCrowd;
				source.PlayOneShot(BowledCrowd);
			}
			else if (SoundType == "Countdown")
			{
				source.clip = CountDown;
				source.PlayOneShot(CountDown);
			}
			else if (SoundType == "Missing Crowd")
			{
				source.clip = MissingCrowd;
				source.PlayOneShot(MissingCrowd);
			}
			else if (SoundType == "Run appear")
			{
				source.clip = RunAppear;
				source.PlayOneShot(RunAppear);
			}
			else if (SoundType == "Level Completed")
			{
				source.clip = LevelCompleted;
				source.PlayOneShot(LevelCompleted);
			}
			else if (SoundType == "Level Failed")
			{
				source.clip = LevelFailed;
				source.PlayOneShot(LevelFailed);
			}
			else if (SoundType == "Ball Missing")
			{
				if (PlayerPrefs.GetInt("loft") == 0)
				{
					source.clip = BallMissing[0];
					source.PlayOneShot(BallMissing[0]);
				}
				else
				{
					source.clip = BallMissing[1];
					source.PlayOneShot(BallMissing[1]);
				}
			}
			else if (SoundType == "celebration")
			{
				source.clip = celebrationsfx;
				source.PlayOneShot(celebrationsfx);
			}

		}
	}
	
	public void BGMusicToggle ()
	{		
		if(CONTROLLER.BGMusicVal == 0)
		{
			BgSource.volume = 0;
		}
		else
		{
			if (CurrentSceneIndex == 1)
			{
				BgSource.volume = 1;
				StopBgSound = false;	//gopi v1.1
			}
			else if (CurrentSceneIndex == 2)
			{
				BgSource.volume = 0.1f;
			}
		}
		
		if(CONTROLLER.GameMusicVal == 1)
		{
			if ( CurrentSceneIndex == 2)
			{
				CrowdSource.volume = CROWD_MAX_SOUND;
			}
			source.volume = 1;
			BatSource.volume = 1;
        }
		else
		{
			CrowdSource.volume = 0;
			source.volume = 0;
			BatSource.volume = 0;
        }
	}

	public void RemoveGameSounds ()
	{
		StartBgSound = true;
		StopCrowdSound = true;
	}
	
	public void ToggleBgSound (bool StopSound)
	{		
		StopBgSound = StopSound;		
		if (StopBgSound == false)
		{
			StopCrowdSound = true;
			if (CONTROLLER.BGMusicVal == 1)
			{
				BgSource.volume =0.1f;
			}
		}
	}
	
	public void ToggleCrowdSound ()
	{
		StartCrowdSound = true;
	}

	#region FADE IN FADE OUT
	public Coroutine FadeInOut;
	public void StopFadeInOutCoRoutine()
	{
		if (FadeInOut != null)
			StopCoroutine(FadeInOut);
	}

	public void FadeOutBGM(bool isFade)
	{
		if (isFade)
		{
			StartCoroutine(FadeOutSound(BgSource, 0.2f));
		}
		else
		{
			StartCoroutine(FadeInSound(BgSource, CONTROLLER.BGMusicVal));
		}
	}
	public static IEnumerator FadeInSound(AudioSource audioSource, float endValue)
	{
		while (audioSource.volume < endValue)
		{
			audioSource.volume += (0.2f * Time.deltaTime);
			yield return null;
		}

		yield break;
	}
	public static IEnumerator FadeOutSound(AudioSource audioSource, float EndValue, float stepvalue = 0.2f)
	{
		while (audioSource.volume > EndValue)
		{
			audioSource.volume -= (stepvalue * Time.deltaTime);
			yield return null;
		}
		yield break;
	}
	#endregion
}