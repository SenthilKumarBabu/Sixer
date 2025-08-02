using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance = null;


    [SerializeField] private AudioHolder audioHolder;
    [SerializeField] private AudioHolder CommentaryHolder;

    private AudioSource BGMAudioSource;
    private AudioSource SoundSource;
    private AudioSource CrowdAmbience;
    private AudioSource CrowdSource;
    private AudioSource BatSource;
    private AudioSource BowlSource;
    private AudioSource postBattingCrowdSoundsource;
    private AudioSource introSource;
    private AudioSource TravelSource;
    private AudioSource CommentarySource;

    protected bool StopBgSound = false;
    protected bool StartBgSound = false;
    protected bool StopCrowdSound = false;
    protected bool StartCrowdSound = false;
    const float CROWD_MAX_SOUND = 0.4f;

    int introSoundIdx;
    List<int> introSoundList;

    private List<string> FourCom = new List<string> { "F1", "F2", "F3" ,"F4","F5","F6","F7", "F8", "F9", "F10", "F11", "F12", "F13", "F14", "F15", "F16", "F17", "F18", "F19", "F20", "F21", "F22", "F23", "F24", "F25", "F26", "F27", "F28", "F29", "F30", "F31", "F32", "F33", "F34", "F35", "F36", "F37", "F38", "F39", "F40", "F41", "F42", "F43", "F44" };
    private List<string> SixCom = new List<string> { "S1", "S2", "S3", "S4","S5","S6","S7", "S8", "S9", "S10", "S11", "S12", "S13", "S14", "S15", "S16", "S17", "S18", "S19", "S20", "S21", "S22", "S23", "S24", "S25", "S26", "S27", "S28", "S29", "S30", "S31", "S32", "S33", "S34", "S35", "S36", "S37", "S38", "S39", "S40", "S41", "S42", "S43", "S44", "S45", "S46", "S47", "S48", "S49", "S50", "S51", "S52", "S53", "S54", "S55", "S56", "S57", "S58", "S59", "S60", "S61", "S62", "S63", "S64", "S65", "S66", "S67", "S68", "S69", "S70", "S71" };
    int fourComIdx;
    int sixComIdx;

    [SerializeField]
    private FadeInOutExtensions BgmFadeInOut, IntroFadeInOut, CrowdAmbienceFadeInOut;

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }

       /* BGMAudioSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        BGMAudioSource.playOnAwake = false;
        BGMAudioSource.loop = true;
        BGMAudioSource.volume = 0f;

        CrowdAmbience = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        CrowdAmbience.playOnAwake = false;
        CrowdAmbience.loop = true;
        CrowdAmbience.volume = 0;

        CrowdSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        CrowdSource.playOnAwake = false;
        CrowdSource.loop = false;
        CrowdSource.volume = 0;

        SoundSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        SoundSource.playOnAwake = false;
        SoundSource.loop = false;

        BatSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        BatSource.playOnAwake = false;
        BatSource.loop = false;

        BowlSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        BowlSource.playOnAwake = false;
        BowlSource.loop = false;

        postBattingCrowdSoundsource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        postBattingCrowdSoundsource.playOnAwake = false;
        postBattingCrowdSoundsource.loop = false;

        introSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        introSource.playOnAwake = false;
        introSource.loop = false;
        introSoundIdx = 0;
        introSoundList = new List<int> { 1, 2, 3, 4, 5 };
        introSoundList.Shuffle();

        TravelSource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        TravelSource.playOnAwake = false;
        TravelSource.clip = audioHolder.GetClip("sixcrowd");
        TravelSource.loop = false;
        TravelSource.volume = 0f;

        CommentarySource = this.gameObject.AddComponent<AudioSource>() as AudioSource;
        CommentarySource.playOnAwake = false;
        CommentarySource.loop = false;

        FourCom.Shuffle();
        fourComIdx = 0;
        SixCom.Shuffle();
        sixComIdx = 0;*/
    }
    private void Start()
    {
       // PlayBGMFromPreload();
    }

    #region BAT BOWL SOUND
    public void PlayBatSnd()
    {
        return;
        if (CONTROLLER.GameMusicVal == 1)
        {
            BatSource.volume = 1;
            int rand = UnityEngine.Random.Range(0, 100);
            if (PlayerPrefs.GetInt("loft") == 1)
            {
                if (rand < 30)
                    BatSource.PlayOneShot(audioHolder.GetClip("Bathit"));
                else if (rand < 60)
                    BatSource.PlayOneShot(audioHolder.GetClip("bathit2"));
                else
                    BatSource.PlayOneShot(audioHolder.GetClip("bathit4"));
            }
            else
            {
                if (rand < 50)
                    BatSource.PlayOneShot(audioHolder.GetClip("bathit1"));
                else
                    BatSource.PlayOneShot(audioHolder.GetClip("bathit3"));
            }
        }
    }

    public void PlayBowledSnd()
    {
        return;
        if (CONTROLLER.GameMusicVal == 1)
        {
            BowlSource.volume = 1;
            BowlSource.PlayOneShot(audioHolder.GetClip("bowledsound"));
        }
    }
    #endregion

    #region SOUND
    public void PlayButtonSnd()
    {
        return;
        PlayTheSound("button");
    }

    public void PlayGameSnd(string SoundType)
    {
        return;

        if (SoundType == "BallMissing")
        {
            if (PlayerPrefs.GetInt("loft") == 0)
            {
                SoundType = "Ballmissing1";
            }
            else
            {
                SoundType = "Ballmissing2";
            }
        }
        PlayTheSound(SoundType);
    }

    private string LastPlayedSoundClip;
    private void PlayTheSound(string name)
    {
        return; //dont use sounds of existing project
        if (CONTROLLER.GameMusicVal == 0)
            return;
        if (audioHolder.GetClip(name) == null)
            return;

        if(SoundSource!=null)
        {
            SoundSource.volume = CONTROLLER.GameMusicVal;
            LastPlayedSoundClip = name;
            SoundSource.PlayOneShot(audioHolder.GetClip(name));
        }        
    }

    #endregion

    #region CROWD SOUND
    public void PlayTheCrowdSound(string name)
    {
        return;

        if (CONTROLLER.GameMusicVal == 0)
            return;
        if (audioHolder.GetClip(name) == null)
            return;

        if (CrowdSource != null)
        {
            CrowdSource.volume = 0.6f;
            //CrowdSource.PlayOneShot(audioHolder.GetClip(name));
            CrowdSource.clip = audioHolder.GetClip(name);
            CrowdSource.Play();
        }
    }

    public void StopCrowdNoiseSound()
    {
        return;

        if (CrowdSource!=null)
            CrowdSource.Stop();
    }
    #endregion

    #region MISC
    public string GetLastPlayedsoundClip()
    {
        return LastPlayedSoundClip;
    }

    public bool GetSoundSourceState()
    {
        return SoundSource.isPlaying;
    }

    public void StopAllAudioSource()
    {
        return;

        if (BGMAudioSource != null)
            BGMAudioSource.Stop();

        if (SoundSource != null)
            SoundSource.Stop();

        if (CrowdAmbience != null)
            CrowdAmbience.Stop();

        if (CrowdSource != null)
            CrowdSource.Stop();

        if (BatSource != null)
            BatSource.Stop();

        if (BowlSource != null)
            BowlSource.Stop();

        if (postBattingCrowdSoundsource != null)
            postBattingCrowdSoundsource.Stop();

        if (introSource != null)
            introSource.Stop();
    }
    public void MuteAudio(int boolean)
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

    public void CallGarbageCollection()
    {
        System.GC.Collect();
    }

    public void PlayLandingPageIntoGameSFX(bool flag)
    {
        return;

        if (flag)
        {
            PlayOrStop_BGM(false);
            if (CONTROLLER.GameMusicVal == 1)
            {
                ToggleInGameSounds(true);
                PlayTheIntroSound();
            }
        }
        else
        {
            PlayOrStop_BGM(true);
            ToggleInGameSounds(false);
            StopIntroSound(0.75f);
        }
    }
    #endregion


    #region INTRO SOUND
    public void PlayTheIntroSound()
    {
        return;

        if (CONTROLLER.GameMusicVal == 0)
            return;

        if (introSource != null)
        {
            int temp = introSoundList[introSoundIdx];
            introSoundIdx++;
            if (introSoundIdx >= introSoundList.Count)
                introSoundIdx = 0;
            
            if (audioHolder.GetClip("intro" + temp) == null)
                return;

            introSource.clip= audioHolder.GetClip("intro"+temp);
            introSource.volume = 0;
            introSource.Play();
            introSource.loop = true;

            if (IntroFadeInOut.FadeInOut != null)
                StopCoroutine(IntroFadeInOut.FadeInOut);

            IntroFadeInOut.FadeInOut = StartCoroutine(IntroFadeInOut.FadeInSound(introSource, 1,0.7f));
        }
    }

    public void StopIntroSound(float step = 0.2f)
    {
        return;

        if (IntroFadeInOut.FadeInOut != null)
        {
            StopCoroutine(IntroFadeInOut.FadeInOut);
            IntroFadeInOut.FadeInOut = null;
        }
        if (introSource.volume > 0)
        {
            IntroFadeInOut.FadeInOut = StartCoroutine(IntroFadeInOut.FadeOutSound(introSource, 0, step));
        }
    }

    #endregion

    #region BALL TRAVEL SOUND
    public void PlayBallTravelSound()
    {
        return;

        if (CONTROLLER.GameMusicVal == 0)
            return;

        if (TravelSource != null)
        {
            TravelSource.volume = 0.6f;    
            TravelSource.Play();
        }
    }
    public void StopBallTravelSound(float fadeValue=0.85f)
    {
        return;

        if (IntroFadeInOut.FadeInOut != null)
            StopCoroutine(IntroFadeInOut.FadeInOut);
        IntroFadeInOut.FadeInOut = StartCoroutine(IntroFadeInOut.FadeOutSound(TravelSource, 0, fadeValue));
    }

    #endregion

    #region COMMENTARY
    public void playFourSixCommentary(int num)
    {
        return;

        string temp;
        if (num == 4)
        {
            temp = FourCom[fourComIdx];
            fourComIdx++;
            if (fourComIdx >= FourCom.Count)
                fourComIdx = 0;
        }
        else
        {
            temp = SixCom[sixComIdx];
            sixComIdx++;
            if (sixComIdx >= SixCom.Count)
                sixComIdx = 0;
        }
        PlayTheCommentary(temp);
    }
    private void PlayTheCommentary(string name)
    {
        return;

        if (CONTROLLER.GameMusicVal == 0)
            return;
        if (CommentaryHolder.GetClip(name) == null)
            return;

        if (CommentarySource != null)
        {
            CommentarySource.volume = 1f;
            CommentarySource.PlayOneShot(CommentaryHolder.GetClip(name));
        }
    }
    public void StopCommentary()
    {
        return;

        CommentarySource.volume = 0f;
        CommentarySource.Stop();
    }
    #endregion

    #region BGM & CROWD NOISE
    private void PlayBGMFromPreload()
    {
        return;

        if (BGMAudioSource != null)
        {
            BGMAudioSource.loop = true;
            BGMAudioSource.clip = audioHolder.GetClip("MenuBG");
            BGMAudioSource.volume = 0f;
            BGMAudioSource.Play();

            CrowdAmbience.clip = audioHolder.GetClip("crowdloop");
            CrowdAmbience.volume = 0;
            CrowdAmbience.Play();
        }
    }

    public void PlayOrStop_BGM(bool flag)
    {
        return;

        if (CONTROLLER.BGMusicVal == 1 && flag)
        {
            BGMFadeInOut(0);
        }
        else
        {
            BGMFadeInOut(1);
        }
    }

    public void BGMFadeInOut(int type)
    {
        return;

        if (BgmFadeInOut.FadeInOut != null)
            StopCoroutine(BgmFadeInOut.FadeInOut);

        if (type == 0)//fade in
        {
            BgmFadeInOut.FadeInOut = StartCoroutine(BgmFadeInOut.FadeInSound(BGMAudioSource, 1, 0.7f));
        }
        else
        {
            BgmFadeInOut.FadeInOut = StartCoroutine(BgmFadeInOut.FadeOutSound(BGMAudioSource, 0, 0.85f));
        }
    }

    public void ToggleInGameSounds(bool flag)
    {
        return;

        if (flag)
        {
            CrowdAmbience.volume = CROWD_MAX_SOUND;
            SoundSource.volume = 1;
            BatSource.volume = 1;
            introSource.volume = 1;
            CrowdSource.volume = 1;
            TravelSource.volume = 1;
        }
        else
        {
            CrowdAmbience.volume = 0;
            SoundSource.volume = 0;
            BatSource.volume = 0;
            introSource.volume = 0;
            CrowdSource.volume = 0;
            TravelSource.volume = 0;
        }
    }
    #endregion
}
