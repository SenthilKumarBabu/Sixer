using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatsmanScript : MonoBehaviour
{
	public Animator batsman_Animator;
	public int action = -1;
	public string status = "idle";
	public float initalBatsmanSpeed = 0.0f; 
	public GameObject targetObj;
	
	public float batsmanTurnAngle = 0.0f;
	public float targetTurnAngle = 0.0f;
	public float batsmanSpeed = 0.0f;
	public bool isMirror = false;
	public GameObject bat_R,bat_L;
	
	public int _idleRandom = 0;
	public string shotPlayed = string.Empty;
	private float rotateSpeed = 10.0f;
	
	public GroundController GroundControllerScript;
	
	public bool isCancelRun = false;
	public bool isBatsmanDiving = false;

	public bool replayMode = false;
	public bool isRunOut = false;
	Coroutine setBatsmanIdleLoop;
	Vector3 _moveTowards = Vector3.zero;
	public string currentAnimPlaying ="";
	void Awake ()
	{
		batsman_Animator = this.GetComponent<Animator>();

	}
	
	void Update()
	{
		switch(action)
		{
			case 1:
				moveBatsman();
			break;
			case 2:
				turnBatsman();
			break;
			case 3:
				walkORRun();
			break;
			case 4:
				moveTowards();
			break;
			case 5:
				isEndofAnimation();
			break;
			case 7:
				//setBatsmanIdleLoop = StartCoroutine(waitforTime());
				break;
            case 8:
               // isEndofBattingShot();
                //setBatsmanIdleLoop = StartCoroutine(waitforTime());
                break;
            default:
			break;
		}
	}

	private void incDecSpeed()
	{
        if (initalBatsmanSpeed < batsmanSpeed)
		{
			initalBatsmanSpeed += 0.45f;
			if(initalBatsmanSpeed >= batsmanSpeed)
			{
				initalBatsmanSpeed = batsmanSpeed;
			}
		}
		else if(initalBatsmanSpeed > batsmanSpeed)
		{
			initalBatsmanSpeed -= 0.45f;
			if(initalBatsmanSpeed <= batsmanSpeed)
			{
				initalBatsmanSpeed = batsmanSpeed;
			}
		}
	}
	public void returnToCrease(GameObject target) 
	{
        if (isBatsmanDiving == false)
        {
            targetObj = target;
            action = 4;
            initalBatsmanSpeed = 3.0f;
            setSpeedAndDirection(initalBatsmanSpeed, 0.0f);
            _fadeAnimation("WalkAndRun");
        }
	}
	
	public void _cancelRun()
	{
		isCancelRun = true;
		action = 5;
	}

	public void setSpeedForRunOut()
	{
		if(replayMode == true)
		{
			if(isRunOut == true)
			{
                initalBatsmanSpeed = 6.25f;
                setAnimationSpeed(1.1f);
				setSpeedAndDirection(initalBatsmanSpeed, 0.0f);
			}
			else
            {
                initalBatsmanSpeed = 7.2f;
                setAnimationSpeed(1.4f);
				setSpeedAndDirection(initalBatsmanSpeed, 0.0f);
			}
		}
	}


	private void isEndofAnimation()
	{
		if(isCancelRunAnimationEnded())
		{
			action = 4;
			setSpeedForRunOut();
			isCancelRun = false;
			status = "run";
		}
	}
	
	/*public float isDRSAnimationIsPlaying ()
	{
		//float _time = anim.GetLayer(0).GetStateByName(animName).clip.length;
		AnimatorStateInfo currInfo = batsman_Animator.GetCurrentAnimatorStateInfo(0);
		float _time = currInfo.normalizedTime;
		return _time;
	}*/
	
	public bool isCancelRunAnimationEnded()
	{
		bool isClipEnd = false;
		float cancelRunCurveValue = batsman_Animator.GetFloat("cancelRunEndCurve");
		if(cancelRunCurveValue >= 0.1f)
		{
			isClipEnd = true;
		}
		return isClipEnd;
	}
	public void _run()
	{
		if(isBatsmanDiving == true)
		{
			return;
		}
        setSpeedForRunOut();
		rotateSpeed = 5.0f;
		action = 3;
		playAnimationByLayer("Run2",1);
		_crossFade("WalkAndRun",0.1f);
	}
	public void _slowDown()
	{
		initalBatsmanSpeed = 2.5f;
		setSpeedAndDirection(initalBatsmanSpeed, 0.0f);
		rotateSpeed = 2.0f;
		action = 4;
		playAnimationByLayer("Walk",1);
		crossFadeAnimation("WalkAndRun");
	}
	
	private void moveTowards()
	{
		//incDecSpeed();
		//Vector3 inversePos = this.transform.InverseTransformPoint(targetObj.transform.position);
		if(this.transform.position.x > 0)
		{
			_moveTowards = new Vector3(targetObj.transform.position.x + 0.3f,targetObj.transform.position.y
			,targetObj.transform.position.z);
		}
		else
		{
			_moveTowards = new Vector3(targetObj.transform.position.x - 0.3f,targetObj.transform.position.y
			,targetObj.transform.position.z);
		}
		Vector3 inversePos = this.transform.InverseTransformPoint(_moveTowards);
		float angle = Mathf.Atan2(inversePos.x, inversePos.z) * 180/Mathf.PI;
		this.transform.Rotate (0, angle * Time.deltaTime * 2.0f, 0);
		batsmanTurnAngle = angle;
		//initalBatsmanSpeed = 2.5f;
		batsman_Animator.SetFloat("Speed",initalBatsmanSpeed);
		batsman_Animator.SetFloat("Direction",batsmanTurnAngle);
		
		float dis = Vector3.Distance(this.transform.position , _moveTowards);
		//float dis = Vector3.Distance(this.transform.position , targetObj.transform.position);

		if(dis <= 1.0f)
		{
			action = -1;
			resetLayerVal ();
			initalBatsmanSpeed = 0.0f;
			status = "moveTowards";
			batsman_Animator.SetFloat("Speed",initalBatsmanSpeed);
			_idleRandom = Random.Range(1,4);			
			
			//_playAnimation("BatsmanIdle_PostShot0"+_idleRandom);
			_crossFade("BatsmanIdle_PostShot0"+_idleRandom, 0.1f);
		}
	}
	
	private void walkORRun()
	{
		incDecSpeed();
		Vector3 inversePos = this.transform.InverseTransformPoint(targetObj.transform.position);
		float angle = Mathf.Atan2(inversePos.x, inversePos.z) * 180/Mathf.PI;
		this.transform.Rotate (0, angle * Time.deltaTime * rotateSpeed, 0);
		batsmanTurnAngle = angle;
		batsman_Animator.SetFloat("Direction",batsmanTurnAngle);
		batsman_Animator.SetFloat("Speed",initalBatsmanSpeed);
	}
	private void moveBatsman()
	{
		incDecSpeed();
		if(initalBatsmanSpeed <= 0.0f)
		{
			action = -1;
			_playAnimation("Standing");
			resetSpeed(0.0f,0.0f);
		}
	}
	public void turnBatsman()
	{
		Vector3 inversePos = this.transform.InverseTransformPoint(targetObj.transform.position);
		float angle = Mathf.Atan2(inversePos.x, inversePos.z) * 180/Mathf.PI;
		this.transform.Rotate (0, angle * Time.deltaTime * 10.0f, 0);
		batsmanTurnAngle = angle;
		batsman_Animator.SetFloat("TurnDirection",batsmanTurnAngle);
	}
	
	public void setSpeedAndDirection(float speed, float dir)
	{
		batsmanSpeed = speed;
		batsman_Animator.SetFloat("Direction",dir);
	}
	
	public float getClipLength(string aniName)
	{
		float time = 0.0f;
		RuntimeAnimatorController ac = batsman_Animator.runtimeAnimatorController; 

		for(int i = 0; i<ac.animationClips.Length; i++)
		{
			if(ac.animationClips[i].name == aniName)
			{
				time = ac.animationClips[i].length;
				break;
			}
		}
		return time;
	}
	public void highlightBowlerAnimation(float normalizedTime,string animName)
	{
		
		batsman_Animator.Play (animName,0,normalizedTime);
		batsman_Animator.speed = 0.0001f;
//		batsman_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime = normalizedTime;

//		 AnimationState clipState = animation.GetClip( "idle" );
//		 clipState.normalizedTime = Mathf.Random( 0.0f, 1.0f );
//		 animation.Play();
//		 animation.Sample(); //forces the pose to be calculated
//		 animation.Stop(); // ac

	}
	public bool getcurrentAnimName()
	{
		string shot = GroundControllerScript.shotPlayed;
		return batsman_Animator.GetCurrentAnimatorStateInfo(0).IsName(shot);
	}
	public float isAnimPlaying(string aniName)
	{
		AnimatorStateInfo currInfo = batsman_Animator.GetCurrentAnimatorStateInfo(0);
		float _time = currInfo.normalizedTime;
		return _time;
	}
	public void resetSpeed(float speed, float dir)
	{
		batsmanSpeed = speed;
		batsman_Animator.SetFloat("Speed",speed);
		batsman_Animator.SetFloat("Direction",dir);
		reset();
	}
	public void setTurn180(bool status)
	{
		batsman_Animator.SetBool("turn180",status);
	}
	
	public void setMirror (bool status)
	{
		isMirror = status;
		if(status == true)
		{
			bat_R.SetActive(false);
			bat_L.SetActive(true);
		}
		else 
		{
			bat_R.SetActive(true);
			bat_L.SetActive(false);
		}
		batsman_Animator.SetBool("canMirror", status);
	}

	public void setMirrorBool(bool status)
	{
		batsman_Animator.SetBool("canMirror", status);
	}
	public void setTurnInPlace(bool status)
	{
		batsman_Animator.SetBool("TurnInPlace",status);
	}
	
	public void reset()
	{
		action = -1;
	}
	public void resetBatsmanAction()
	{
		action = -1;
	}
	public float getAnimationSpeed()
	{
		return batsman_Animator.speed;
	}
	public void setAnimationSpeed(float _speed)
	{
		batsman_Animator.speed = _speed;
	}
	
	public void _playAnimation(string aniName)
	{
		currentAnimPlaying = aniName;
		batsman_Animator.Play(aniName,0,0.0f);
	}
	
	public void playAnimationByLayer(string aniName, int layer)
	{
		currentAnimPlaying = aniName;
		batsman_Animator.Play(aniName, layer);
	}
	public void setLayerWeight(int layer,float layerWeight)
	{
		
		batsman_Animator.SetLayerWeight(layer, layerWeight);
	}
	public void _fadeAnimation(string aniName)
	{
		currentAnimPlaying = aniName;
		batsman_Animator.Play(aniName,0,0.0f);
	}
	public void _crossFade (string aniName, float time)
	{
		
		batsman_Animator.CrossFade(aniName,time,-1,0.01f);
		currentAnimPlaying = aniName;
	}
	public void crossFadeAnimation(string aniName)
	{
		currentAnimPlaying = aniName;
		if(aniName == "2_PreIdle" || aniName == "4_PreIdle" || aniName == "1_PreIdle" || aniName == "3_PreIdle")
		{
            batsman_Animator.CrossFade(aniName, 0.05f, -1, 0.01f);
		}
		else
		{
			batsman_Animator.CrossFade(aniName,0.25f,-1,0.01f);
		}
	}
    
    public void TempBatSwap()
    {
        if (bat_L.activeInHierarchy == true)
        {
            bat_R.SetActive(true);
            bat_L.SetActive(false);
        }
    }

	
	public void resetLayerVal()
	{
		setLayerWeight(1,0);
		status = "idle";
		initalBatsmanSpeed = 0.0f; 
		batsman_Animator.SetFloat("Speed",initalBatsmanSpeed);
		isBatsmanDiving = false;
		playAnimationByLayer("Run2",1);
	}

    #region STEPOUT
    public void stumpingCrossFade(string aniName, float normailzeTime)
    {
        batsman_Animator.CrossFade(aniName, 0.01f, -1, normailzeTime);
        currentAnimPlaying = aniName;
    }
    //StepoutAnimation-Gopi
    public string GetCurrentPlayingAnimationName(int layerIndex = 0)
    {
        AnimatorClipInfo[] m_CurrentClipInfo = batsman_Animator.GetCurrentAnimatorClipInfo(layerIndex);
        if (m_CurrentClipInfo.Length > 0)
            return m_CurrentClipInfo[0].clip.name;
        else
            return string.Empty;
    }
    #endregion
}