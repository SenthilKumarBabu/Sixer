/*========This file  was ported from Js to Cs file by Gopinath on March2018*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System .Linq ;
using UnityEngine .UI;

public class GroundController : MonoBehaviour 
{
	public MoveBannerTexture moveBannerTextureScript;

	private float shotAngle;
	public static bool loft = false;
	public Button loftBtn, loftBtn2;
	public Image loftText, loftText2;
	public Text loftDesc, loftDesc2;
	public Sprite[] loftSprite;
	// Constants...
	private float DEG2RAD = Mathf.PI / 180;
	private float RAD2DEG = 180 / Mathf.PI;
	private bool playIntro = false;
	private string battingBy = "user";
	// "user" || "computer"
	private string bowlingBy;
	// "user" || "computer"

	// ball variables
	private GameObject ball;
	protected GameObject ballSkin; 
	protected Transform ballRot;
	protected Transform ballSkinTransform;
	protected Rigidbody ballSkinRigidBody;
	protected Renderer ballSkinRenderer;
	protected Collider ballSphereCollider;


	private TrailRenderer ballTrail;
	private Transform ballTransform;
	private GameObject ballRayCastReferenceGO;
	private Transform ballRayCastReferenceGOTransform;
	private GameObject ballOriginGO;
	private GameObject ballTimingOrigin;
	private Vector3 ballInitPosition;
	private float ballRadius = 0.05f;
	// lift the ball by ball's radius height above the ground terrain...
	private int ballNoOfBounce = 0;
	private float ballProjectileAngle = 270.0f;
	// top - semi-angle....
	private float ballProjectileAnglePerSecond;
	//110;//180.0; // dynamic calculation...
	private float ballProjectileHeight = 2.3f;
	private float horizontalSpeed = 22;
	//22;//22.0; // meters per second.. // dynamic speed from user interface...
	private float ballAngle;

	private string ballStatus = "";
	private float ballBatMeetingHeight;
	private float ballTimingFirstBounceDistance;
	private GameObject ballTimingFirstBounce;
	private GameObject ballCatchingSpot;
	private float ballPreCatchingDistance;
	private bool pauseTheBall = false;
	private string ballResult = "";
	private bool applyBallFiction = false;
	private int currentBallNoOfRuns = 0;
	private float timeBetweenBalls = 0;

	private float stayStartTime = 0;
	private GameObject ballSpotAtCreaseLine;
	private GameObject ballSpotAtStump;
	private float shortestBallPickupDistance;
	// to enable computer batsman to take runs...
	public bool ballReleased = false;
	public bool isTimeToShowAd = true;
	private bool  hattrickBall = false;
	private float throwingFirstBounceDistance = 0;


	// batsman variables
	public  GameObject batsman;
	private GameObject batsmanUniform;
	private GameObject dummyBall;

	private GameObject batCollider;
	private GameObject batColliderHolder;
	private GameObject batCollider_Left;
	private GameObject batColliderHolder_Left;
	private GameObject batCollider_Right;
	private GameObject batColliderHolder_Right;

	private GameObject leftLowerLegObject;
	private GameObject rightLowerLegObject;
	private GameObject leftUpperLegObject;
	private GameObject rightUpperLegObject;
	private GameObject upperBodyObject;
	private GameObject headObject;

	private Collider batColliderComponent;
	private Collider batColliderComponent_Left;
	private Collider batColliderComponent_Right;

	private GameObject stump1Collider;

	private GameObject boardCollider ;
	private GameObject billBoardCollider ;
	private MeshCollider stadiumCollider;

	private GameObject RHBatsmanMaxBowlLimitGO;
	private GameObject RHBatsmanMinBowlLimitGO;
	private GameObject LHBatsmanMaxBowlLimitGO;
	private GameObject LHBatsmanMinBowlLimitGO;
	private bool lbwAppeal = false;
	private bool LBW = false;
	private float batsmanInitXPos;

	private Vector3 RHBatsmanInitPosition;
	private Vector3 LHBatsmanInitPosition;
	private GameObject LHBatsmanInitSpot;
	public string shotPlayed = "";
	private bool squareLegGlance = false;
	private string batsmanAnimation = "";
	// shot animation...
	private GameObject shotActivationMinLimit;
	private GameObject shotActivationMaxLimit;
	private bool canMakeShot = false;
	private bool batsmanTriggeredShot = false;
	private bool batsmanMadeShot = false;
	private bool batsmanCanMoveLeftRight = false;
	private bool batsmanOnLeftRightMovement = false;

	private float batsmanLeftRightMovementDuring = 2.0f;

	private GameObject RHBatsmanBackwardLimit;
	private GameObject RHBatsmanForwardLimit;
	private GameObject LHBatsmanBackwardLimit;
	private GameObject LHBatsmanForwardLimit;

	private GameObject RHBMaxWideLimit  ;
	private GameObject RHBMinWideLimit ;
	private GameObject LHBMaxWideLimit ;
	private GameObject LHBMinWideLimit ;
	public	string currentBatsmanHand = CONTROLLER.BatsmanHand;
	//"right";

	// accurate timing of ball
	private float animationFPS = 25;
	// Fixed...

	private float optimalShotTime;
	private float ballReleasedTime  ;
	private float batReachingTimeForOptimalShotLength;
	private float optimalShotActivationTime;

	// wicket keeper
	private GameObject wicketKeeper;
	private GameObject wicketKeeperSkin ;
	private GameObject wicketKeeperBall;

	private Vector3 wicketKeeperInitPosition4RHBFast;
	private Vector3 wicketKeeperInitPosition4LHBFast;
	private Vector3 wicketKeeperInitPosition4RHBSpin;
	private Vector3 wicketKeeperInitPosition4LHBSpin;

	private GameObject wicketKeeperStraightBallStumping;
	private GameObject wicketKeeperLegSideBallStumping;
	private GameObject wicketKeeperOffSideBallStumping;
	private float distanceBtwBallAndCollectingPlayerWhileThrowing;
	private string postBattingWicketKeeperDirection = "";

	// bowler variables
	private GameObject bowler  ;
	private GameObject bowlerSkin ;
	private GameObject bowlerBall ;
	private GameObject fastBowler ;
	private GameObject fastBowlerSkin ;
	private GameObject fastBowlerBall ;
	private GameObject spinBowler ;
	private GameObject spinBowlerSkin ;
	private GameObject spinBowlerBall ;
	private float spinValue   = 0;
	private GameObject hideBowlingInterfaceSpot ;
	private bool  hideBowlingInterface = false;
	private bool  userBowlerCanMoveBowlingSpot = false;
	private Vector3 suggestedBowlingSpot  ;
	private bool  userBowlingSpotSelected = false;
	private GameObject userBowlingMinLimit ;
	private GameObject userBowlingMaxLimit ;

	private GameObject fielder10  ;
	private GameObject fielder10Skin ;
	private GameObject fielder10Ball ;
	private GameObject bowlingSpotGO ;
	private GameObject fielder10FastInit ;
	private GameObject fielder10SpinInit ;
	private BowlingSpot bowlingSpotScript  ;

	private float ballSpotLength;

	private float bowlerRunningSpeed  = 5;
	private string  currentBowlerType ="fast";
	private int  currentBowlerSpinType = 2; // 0=fast, 1=off, 2= leg
	private string  currentBowlerHand = "right";//"right";
	private float  bowlerRunupTime = 4.84f; //seconds

	private bool canActivateBowler = false;
	private string fielder10Action = "";
	private GameObject fielderStraightBallStumping;
	private GameObject fielderLegSideBallStumping;
	private GameObject fielderOffSideBallStumping;
	private string postBattingStumpFielderDirection;

	// fielder variables
	public bool fieldRestriction = false;
	private int noOfFielders = 9;
	private GameObject[] fielder = new GameObject[10];
	private GameObject[] fielderBall = new GameObject[10];
	private GameObject[] fielderSkin = new GameObject[11];
	private Vector3[] fielderInitPosition = new Vector3 [10];
	// for 1 to 9 fielders..., so length is 10...

	//fielder Team color variables
	private Color32[] teamUniformColor = new Color32[16];
	private int oppTeamColorIndex = 0;

	private Vector3[] fieldRestriction1FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction2FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction3FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction4FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction5FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction6FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction7FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction8FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction9FielderPosition = new Vector3 [10];
	private Vector3[] fieldRestriction10FielderPosition = new Vector3 [10];

	private float[] fielderAngle = new float[10];
	private float[] fielderDistance = new float[10];
	private float[] fielderBallDiffInAngle = new float[10];
	private GameObject[] fielderChasePoint = new GameObject[10];
	private bool stopTheFielders = false;
	private float fielderThrowElapsedTime;
	private bool slipFielderWarmUpAction = false;
	private bool slipFielder2WarmUpAction = false;

	private GameObject slipFielderSpotForRHBSpin ;
	private GameObject slipFielder2SpotForRHBSpin ;
	private GameObject mainUmpire;
	private GameObject runner;
	private GameObject runnerSkin;

	private GameObject RHBNonStickerRunningSpot;
	private GameObject RHBStickerRunningSpot;
	private GameObject runnerStickerRunningSpot;
	private GameObject runnerNonStickerRunningSpot;
	private GameObject nonStickerNearCreaseSpot;
	private GameObject stickerNearCreaseSpot;
	private float batsmanRunningAngle;
	private float runnerRunningAngle;
	private Vector3 runnerInitPosition;
	private GameObject nonStickerReachSpot;
	private GameObject stickerReachSpot;

	// game-play variables
	public int action;
	// idle state...
	private Camera mainCamera;
	// pitch camera

	private Vector3 mainCameraPitchPosition = new Vector3(-0.4f,  2f, 14.5f);//Vector3(0,  2, 15) //2013
	private Vector3 mainCameraInitRotation =new  Vector3 (0f, 178f, 0f);//Vector3 (0, 180, 0) //2013

	private GameObject stump1  ;
	private GameObject stump2 ;
	private GameObject stump1Crease ;
	private GameObject stump2Crease ;

	private GameObject stump1Spot;
	private GameObject stump2Spot;
	private GameObject outOfPitch;

	private bool mainCameraOnTopDownView = false;
	private float groundRadius = 68.2f;
	protected float groundBannerRadius = 70.2f; // 70.4f
	public static bool ballOnboundaryLine = false;

	public Camera rightSideCamera;
	public Camera leftSideCamera;
	public Camera straightCamera;
	public Camera sixDistanceCamera;
	public Camera ultraMotionCamera;
	public Camera introCamera;
	public Camera blastEffectCamera;
	public Camera BowledReplayCamera;

	private GameObject introCameraPivot ;
	public bool sideCameraSelected = false;

	private GameObject groundCenterPoint;

	private GameObject crowdsGO ;
	private GameObject exteriorGO ;
	private GameObject fenceGO ;
	private GameObject stadiumGO ;
	private GameObject stadium180GO ;
	private int  stadiumRotationAngle = 0;
	private GameObject cameraFlashReferences ;
	private GameObject skyboxGO ;

	// key controllers...
	private bool upArrowKeyDown = false;
	private bool downArrowKeyDown = false;
	private bool leftArrowKeyDown = false;
	private bool rightArrowKeyDown = false;

	private bool powerKeyDown  = false;
	private bool powerShot  = false;
	private bool takeRun  = false;
	private bool canTakeRun  = false;

	// interface GUI
	public GameModel GameModelScript;
	private bool waitForCommentary = false;
	private GameObject batsmanExit;

	private bool  StrikerOnFocus = true;
	public static GroundController instance;

	public  bool  showShadows = true;
	private GameObject[] FielderShadowRefGO   = new GameObject[9];
	private List<GameObject> ShadowsArray = new List<GameObject> ();
	private List<GameObject> ShadowRefArray = new List<GameObject> ();

	// Confidence Level
	private Transform batsmanRefPoint;
	// Confidence Level

	private bool LateAttempt = false;
	private string battingTeam;
	private string bowlingTeam;
	private float bowlingSpeed  ;

	private List<int> activeFielderNumber = new List<int> ();
	//private Array [] activeFielderNumber = new Array [10];
	private List<string> activeFielderAction = new List<string> ();
	//private Array [] activeFielderAction = new Array [10];

	private float fielderSpeed = 7;

	private GameObject fielderToCatchTheBall;
	private float ballSpinningSpeedInX = 0;
	private float ballSpinningSpeedInZ = 0;

	private float wicketKeeperAdjacentLength = 0.0f;
	private float wicketKeeperHypotenuse = 0.0f;
	private float wicketKeeperOppositeLength = 0.0f;
	private float wicketKeeperThetaBtwAdjAndHyp = 0.0f;
	private bool wicketKeeperIsActive = false;
	private string wicketKeeperStatus = "";
	private bool wicketKeeperCatchingAnimationSelected = false;
	private float wicketKeeperCatchingFrame;
	private	string wicketKeeperAnimationClip;
	private float wicketKeeprMaxCatchingDistance = 1.75f;
	private bool wideBall = false;
	private bool noBall = false;
	private bool freeHit = false;
	private bool wideBallChecked = false;
	private float shotExecutionTime;

	private float shotCompletionDuration = 1.0f;
	private bool  computerBatsmanNewRunAttempt = false;

	private float topDownViewStartTime = 0.0f;
	private float topDownViewZoomingSecs = 0.1f;//1.0f//0.5f//0.75f;
	private float lookAtBallTime = 0.5f;
	private bool cameraToKeeper = false;

	private bool runOut;
	private bool takingRun;
	private bool isRunOut;

	private float stickerSpeed;
	private float nonStickerSpeed;
	private string stickerStatus;
	private string nonStickerStatus;
	private GameObject sticker;
	private GameObject nonSticker;
	private float stickerRunningAngle;
	private float nonStickerRunningAngle;
	private float introRotationSpeed = 5;
	private float batInCurrFrame;
	private int bufferFrame = 3;

	private float batToBallConnectLength = 1.5f;

	private float animSpeed = 0.2f;
	private bool playedUltraSlowMotion = false;
	//private var rotateAngle : int = 0;
	private bool touchDeviceShotInput = false;

	private bool  bowlerIsWaiting = false;


	private string bowlerSide = "right";
	private float swingProjectileAnglePerSecond;
	private float swingProjectileAngle;
	private float swingValue;

	private float batsmanWaitSeconds = 0.2f;
	private float bowlerWaitSeconds = 0.5f;
	private float currentBallStartTime = 0.0f;
	private bool slipShot = false;

	private bool  ballToFineLeg = false;

	private string pickingupAnimationToPlay = "";
	private bool ballOverTheFence = false;
	private float boundaryFenceHeight = 1.2f;
	// 6 meters...
	private string boundaryAction = "";
	private bool ballBoundaryReflection = false;

	//public var BallPitchEffect : Transform;
	public Transform BallHitEffect;
	//public var glowEffect : GameObject;
	private GameObject fielder10FocusGObjToCollectTheBall;
	public GameObject ballConnectToTheBatGO;
	private int canBe4or6 = 6;
	private bool playingInTouchDevice = false;
	public WeaponTrail leftSwipe;
//	protected AnimationController animationController;

	private float ballToBatsmanDistance;
	private bool perfectShot = false;
	private float batsmanStep = 0.3f;
	private Vector2 prevMousePos;
	private float BallPickTime;
	private float BallHitTime;
	private bool ballInline;
	private GameObject ninjaSlice;
	private bool mouseDownD = false;
	private bool playShotCommentary = false;
	private bool swingingBall = false;

	//private string[] coverDriveCommentary  = new string[] {"CoverDrive/A brilliant drive through", "CoverDrive/Placed well through", "CoverDrive/That was a beautiful", "CoverDrive/That was a superb drive"};
	//private string[] squareCommentary = new string[] {"Square/A superb square", "Square/Excellent back"};
	//private string[] catchCommentary  = new string[] {"CatchOut/A brilliant catch", "CatchOut/A difficult catch", "CatchOut/A good catch", "CatchOut/Caught", "CatchOut/Poor timing", "CatchOut/That was an easy"};
	//private string[] fourCommentary  = new string[] {"Four/A nice clean hit", "Four/A textbook shot", "Four/Brilliantly played", "Four/Good timing", "Four/GoodShot", "Four/He played like", "Four/O what a shot", "Four/That's a magnificent"};
	//private string[] CSKSpecialSoundArray = new string[] {"CSK/Option1", "CSK/Option2", "CSK/Option3", "CSK/Option4", "CSK/Option5", "CSK/Option6", "CSK/Option7", "CSK/Option8", "CSK/Option9", "CSK/Option10", "CSK/Option11", "CSK/Option12"};

	//private int specialSoundIndex = -1;

	public Text sixDistanceGO;

	private bool isTrailOn = false;
	public Mesh batsmanHighPolyMesh;
	public Mesh batsmanLowPolyMesh;
	private GameObject lensFlareHolder;
	private GameObject groundFlashGO;
	//private GameObject groundTreesGO ;
	//private GameObject groundTrees180GO;
	private GameObject swipeHighlight ;
	private Renderer swipeHighlightRenderer;

	private GameObject extrasGO ;
	private bool  canSwipeNow  = false;

	//CameraFlash Variables
	public GameObject CameraFlashPrefab;

	private GameObject flashReference1  ;
	private GameObject flashReference2 ;
	private GameObject flashReference3 ;
	private GameObject flashReference4; 
	private GameObject flashReference5 ;
	private GameObject flashReference6 ;
	private GameObject flashReference7 ;
	private GameObject flashReference8 ;

	private  GameObject shadowHolder;

	public  Animation wicketKeeperAnimationComponent,fielder10AnimationComponent;

	private bool CamShouldNotFollowBallY = false;
	private Vector3 _newVector3 = new Vector3();
	private Vector3 targetPostition;
	private float camMaxDist = 60f;
	private float maxFOV = 38f;
	private float dampCamSpeed = 25f;
	private float movingCamY = 4f;

	public  void  Awake ()
	{

		instance = this;
		playingInTouchDevice = true;
		//ball = this.gameObject;

		ball = GameObject.FindGameObjectWithTag("Ball");
		ballRot = ball.transform.GetChild(0).transform;
		ballSkin = ballRot.transform.GetChild(0).transform.gameObject;
		ballSkinTransform = ballSkin.transform;
		ballTransform = ball.transform;
        ballSkinRigidBody = ballSkin.GetComponent<Rigidbody>();
		ballSkinRenderer = ballSkin.GetComponent<Renderer>();
		ballSphereCollider = ballSkin.GetComponent("SphereCollider") as SphereCollider;

		ninjaSlice = GameObject.Find ("NinjaSlice");
		ballRayCastReferenceGO = GameObject.Find("BallRayCastReference");
		ballRayCastReferenceGOTransform = ballRayCastReferenceGO.transform;
		//glowEffect = GameObject.Find ("Glow");
		fielder10FocusGObjToCollectTheBall = new GameObject ("Fielder10FocusGObjToCollectTheBall");
		ballConnectToTheBatGO = new GameObject ("ballConnectToTheBatGO");
		ballConnectToTheBatGO.transform.position =new  Vector3 (0, 0, 0);
		fielder10FocusGObjToCollectTheBall.transform.position =new  Vector3 (0, 0, 0);
		ballOriginGO = GameObject.Find("BallOrigin");
		bowlingSpotGO = GameObject.Find("BowlingSpot");
		bowlingSpotScript = bowlingSpotGO.GetComponent<BowlingSpot>();
		ballTimingOrigin = GameObject.Find("BallTimingOrigin");

		ballTrail = ball.GetComponent<TrailRenderer>();
		dummyBall = GameObject.Find ("DummyBall");
		dummyBall.GetComponent<Renderer>().enabled = false;

		batCollider_Left = GameObject.Find("Batsman/metarig/hips/spine/chest/shoulder.L/upper_arm.L/forearm.L/hand.L/BatBone_LeftHand/BatColliderHolder/BatCollider");
		batColliderHolder_Left = GameObject.Find("Batsman/metarig/hips/spine/chest/shoulder.L/upper_arm.L/forearm.L/hand.L/BatBone_LeftHand/BatColliderHolder");

		batCollider_Right = GameObject.Find("Batsman/metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/BatBone/BatColliderHolder/BatCollider");
		batColliderHolder_Right = GameObject.Find("Batsman/metarig/hips/spine/chest/shoulder.R/upper_arm.R/forearm.R/hand.R/BatBone/BatColliderHolder");

		leftLowerLegObject = GameObject.Find("Batsman/metarig/hips/thigh.L/shin.L/LeftLowerLeg");
		rightLowerLegObject = GameObject.Find("Batsman/metarig/hips/thigh.R/shin.R/RightLowerLeg");
		leftUpperLegObject = GameObject.Find("Batsman/metarig/hips/thigh.L/LeftUpperLeg");
		rightUpperLegObject = GameObject.Find("Batsman/metarig/hips/thigh.R/RightUpperLeg");
		upperBodyObject = GameObject.Find("Batsman/metarig/hips/spine/chest/UpperBody");
		headObject = GameObject.Find("Batsman/metarig/hips/spine/chest/neck/head/Head");

		stump1Collider = GameObject.Find("Stump1Collider");
		boardCollider = GameObject.Find("Board");
		billBoardCollider = GameObject.Find("BillBoard");
		stadiumCollider = GameObject.Find ("Props").GetComponent<MeshCollider>();

		ballTimingFirstBounce = GameObject.Find("BallTimingFirstBounce");
		ballCatchingSpot = GameObject.Find("BallCatchingSpot");

		ballSpotAtCreaseLine = GameObject.Find("BallSpotAtCreaseLine");
		ballSpotAtStump = GameObject.Find("BallSpotAtStump");

		//		batsman = GameObject.Find("Batsman");

		//batsmanSkin = GameObject.Find("Batsman/Armature/Bone");
		batsmanUniform = GameObject.Find("Batsman/Body/Uniform");


		runner = GameObject.Find("Runner");//25march
		runnerSkin = GameObject.Find("Runner/Body/Uniform");//25march
		runnerInitPosition = runner.transform.position;//25march
		RHBatsmanMaxBowlLimitGO = GameObject.Find("RHBatsmanMaxBowlLimit");
		RHBatsmanMinBowlLimitGO = GameObject.Find("RHBatsmanMinBowlLimit");
		LHBatsmanMaxBowlLimitGO = GameObject.Find("LHBatsmanMaxBowlLimit");
		LHBatsmanMinBowlLimitGO = GameObject.Find("LHBatsmanMinBowlLimit");

		RHBatsmanInitPosition = batsman.transform.position;
		LHBatsmanInitSpot = GameObject.Find("LHBatsmanInitSpot");
		LHBatsmanInitPosition = LHBatsmanInitSpot.transform.position;
		shotActivationMinLimit = GameObject.Find("ShotActivationMinLimit");
		shotActivationMaxLimit = GameObject.Find("ShotActivationMaxLimit");

		RHBatsmanBackwardLimit = GameObject.Find("RHBatsmanBackwardLimit");
		RHBatsmanForwardLimit = GameObject.Find("RHBatsmanForwardLimit");
		LHBatsmanBackwardLimit = GameObject.Find("LHBatsmanBackwardLimit");
		LHBatsmanForwardLimit = GameObject.Find("LHBatsmanForwardLimit");

		RHBMaxWideLimit = GameObject.Find("RHBMaxWideLimit");
		RHBMinWideLimit = GameObject.Find("RHBMinWideLimit");
		LHBMaxWideLimit = GameObject.Find("LHBMaxWideLimit");
		LHBMinWideLimit = GameObject.Find("LHBMinWideLimit");

		mainUmpire = GameObject.Find("MainUmpire");

		fastBowler = GameObject.Find("/FastBowler");
		fastBowlerSkin = GameObject.Find("/FastBowler/Bowler");
		fastBowlerBall = GameObject.Find("/FastBowler/Sphere");

		spinBowler = GameObject.Find("/SpinBowler");
		spinBowlerSkin = GameObject.Find("/SpinBowler/Armature/Bowler");
		spinBowlerBall = GameObject.Find("/SpinBowler/Armature/Sphere");

		hideBowlingInterfaceSpot = GameObject.Find("HideBowlingInterface");
		userBowlingMinLimit = GameObject.Find("UserBowlingMinLimit");
		userBowlingMaxLimit = GameObject.Find("UserBowlingMaxLimit");

		fielder10 = GameObject.Find("/Fielders/Fielder10");
		fielder10Skin = GameObject.Find("/Fielders/Fielder10/Fielder");
		fielder10Ball = GameObject.Find("/Fielders/Fielder10/Sphere");

		fielder10FastInit = GameObject.Find("Fielder10FastInit");
		fielder10SpinInit = GameObject.Find("Fielder10SpinInit");

		//setting color code for IPL teams**************

		teamUniformColor [0] = new Color32 (219, 46, 32, 255); // punjab
		teamUniformColor [1] = new Color32 (32, 81, 219, 255); // mumbai
		teamUniformColor [2] = new Color32 (97, 32, 219, 255); // kolkata
		teamUniformColor [3] = new Color32 (219, 32, 32, 255); // Delhi
		teamUniformColor [4] = new Color32 (219, 32, 187, 255); // rajasthan
		teamUniformColor [5] = new Color32 (32, 146, 219, 255); // Deccan
		teamUniformColor [6] = new Color32 (32, 196, 219, 255); // Kochi
		teamUniformColor [7] = new Color32 (32, 182, 219, 255); // Pune
		teamUniformColor [8] = new Color32 (219, 84, 32, 255); // Hyderabad
		teamUniformColor [9] = new Color32 (219, 32, 32, 255); // Bangalore

		for(int i = 1; i <= noOfFielders; i++)
		{
			fielder[i] = GameObject.Find("/Fielders/Fielder"+i);

			fielderBall[i] = GameObject.Find("/Fielders/Fielder"+i+"/Sphere");
			GameObject fielderGO = fielder[i] as GameObject;
			fielderInitPosition[i] = fielderGO.transform.position;
			fieldRestriction1FielderPosition[i] = GameObject.Find("FieldRestriction1_Fielder"+i).transform.position;
			fieldRestriction2FielderPosition[i] = GameObject.Find("FieldRestriction2_Fielder"+i).transform.position;
			fieldRestriction3FielderPosition[i] = GameObject.Find("FieldRestriction3_Fielder"+i).transform.position;
			fieldRestriction4FielderPosition[i] = GameObject.Find("FieldRestriction4_Fielder"+i).transform.position;
			fieldRestriction5FielderPosition[i] = GameObject.Find("FieldRestriction5_Fielder"+i).transform.position;
			fieldRestriction6FielderPosition[i] = GameObject.Find("FieldRestriction6_Fielder"+i).transform.position;
			fieldRestriction7FielderPosition[i] = GameObject.Find("FieldRestriction7_Fielder"+i).transform.position;
			fieldRestriction8FielderPosition[i] = GameObject.Find("FieldRestriction8_Fielder"+i).transform.position;
			fieldRestriction9FielderPosition[i] = GameObject.Find("FieldRestriction9_Fielder"+i).transform.position;
			fieldRestriction10FielderPosition[i] = GameObject.Find("FieldRestriction10_Fielder"+i).transform.position;
			fielderChasePoint[i] = GameObject.Find("FielderChasePoint"+i);
			fielderSkin[i] = GameObject.Find("/Fielders/Fielder"+i+"/Fielder");
		}

		fielderSkin[10] = GameObject.Find("/Fielders/Fielder10/Fielder");
		slipFielderSpotForRHBSpin = GameObject.Find("SlipFielderSpotForRHBSpin");
		slipFielder2SpotForRHBSpin = GameObject.Find("SlipFielder2SpotForRHBSpin");

		wicketKeeper = GameObject.Find("WicketKeeper");
		wicketKeeperSkin = GameObject.Find("WicketKeeper/Armature/Wicket_keeper_");
		wicketKeeperBall = GameObject.Find("WicketKeeper/Armature/Sphere");
		wicketKeeperInitPosition4RHBFast = GameObject.Find("WicketKeeperInitPos4RHBFast").transform.position;
		wicketKeeperInitPosition4LHBFast = GameObject.Find("WicketKeeperInitPos4LHBFast").transform.position;
		wicketKeeperInitPosition4RHBSpin = GameObject.Find("WicketKeeperInitPos4RHBSpin").transform.position;
		wicketKeeperInitPosition4LHBSpin = GameObject.Find("WicketKeeperInitPos4LHBSpin").transform.position;

		wicketKeeperStraightBallStumping = GameObject.Find("WicketKeeperStraightBallStumpingPos");
		wicketKeeperLegSideBallStumping = GameObject.Find("WicketKeeperLegSideBallStumpingPos");
		wicketKeeperOffSideBallStumping = GameObject.Find("WicketKeeperOffSideBallStumpingPos");

		fielderStraightBallStumping = GameObject.Find("FielderStraightBallStumpingPos");
		fielderLegSideBallStumping = GameObject.Find("FielderLegSideBallStumpingPos");
		fielderOffSideBallStumping = GameObject.Find("FielderOffSideBallStumpingPos");

		RHBNonStickerRunningSpot = GameObject.Find("RHBNonStickerRunningSpot");
		RHBStickerRunningSpot = GameObject.Find("RHBStickerRunningSpot");
		runnerStickerRunningSpot = GameObject.Find("RunnerStickerRunningSpot");
		runnerNonStickerRunningSpot = GameObject.Find("RunnerNonStickerRunningSpot");
		nonStickerNearCreaseSpot = GameObject.Find("NonStickerNearCreaseSpot");
		stickerNearCreaseSpot = GameObject.Find("StickerNearCreaseSpot");

		nonStickerReachSpot = GameObject.Find ("NonStickerReachSpot");
		stickerReachSpot = GameObject.Find ("StickerReachSpot");

		groundCenterPoint = GameObject.Find("GroundCenterPoint");
		stump1 = GameObject.Find("Stump1");
		stump2 = GameObject.Find("Stump2");
		stump1Crease = GameObject.Find("Stump1Crease");
		stump2Crease = GameObject.Find("Stump2Crease");
		stump1Spot = GameObject.Find("Stump1Spot");
		stump2Spot = GameObject.Find("Stump2Spot");

		outOfPitch = GameObject.Find("OutOfPitch");

		crowdsGO = GameObject.Find("Crowds");
		exteriorGO = GameObject.Find("Exterior");
		fenceGO = GameObject.Find("Fence");
		stadiumGO = GameObject.Find("Stadium");
		//stadium180GO = GameObject.Find("Stadium180");
		cameraFlashReferences = GameObject.Find ("CameraFlashReferences");
		skyboxGO = GameObject.Find("SkyDome");

		introCameraPivot = GameObject.Find("IntroCameraPivot");


		//lensFlareHolder = GameObject.Find ("LensFlareHolder");
		//groundFlashGO = GameObject.Find ("Flash");
		//groundTreesGO = GameObject.Find ("Trees");
		//groundTrees180GO = GameObject.Find ("Trees180");
		swipeHighlight = GameObject.Find ("SwipeHighlight");
		swipeHighlightRenderer = swipeHighlight.GetComponent<Renderer>();
		SetSwipeHighlightRenderState ( false);

		extrasGO = GameObject.Find ("Extras");

		GameObject ICGO = GameObject.Find("GUIContainer");

		bowlingSpotScript.HideBowlingSpot ();

		ballInitPosition = ballTransform.position;
		mainCamera = Camera.main;

		shadowHolder = GameObject.Find("ShadowHolder");
		GameObject shadowGO ;
		GameObject shadowRef ;
		if(showShadows == true)
		{	
			for(int i = 1; i <= noOfFielders; i++)
			{
				shadowGO = GameObject.Find("FielderShadow"+i);
				ShadowsArray.Add(shadowGO);

				shadowRef = GameObject.Find ("Fielders/Fielder" + i + "/Armature/Bone/hip/spin/ShadowRef");
				ShadowRefArray.Add(shadowRef);
			}
			shadowGO = GameObject.Find("BowlerShadow");
			ShadowsArray.Add(shadowGO);
			shadowRef = GameObject.Find ("Fielders/Fielder10/Armature/Bone/hip/spin/ShadowRef");
			ShadowRefArray.Add(shadowRef);

			shadowGO = GameObject.Find("WicketKeeperShadow");
			ShadowsArray.Add(shadowGO);
			shadowRef = GameObject.Find ("WicketKeeper/Armature/Bone/hip/ShadowRef");
			ShadowRefArray.Add(shadowRef);

			shadowGO = GameObject.Find("BatsmanShadow");
			ShadowsArray.Add(shadowGO);
			//shadowRef = GameObject.Find ("Batsman/Armature/Bone/hip/spin/ShadowRef");
			shadowRef = GameObject.Find("Batsman/metarig/hips/spine/ShadowRef");

			ShadowRefArray.Add(shadowRef);

			shadowGO = GameObject.Find("BallShadow");
			ShadowsArray.Add(shadowGO);
			shadowRef = GameObject.Find ("Ball/ShadowRef");
			ShadowRefArray.Add(shadowRef);

			shadowGO = GameObject.Find("MainUmpireShadow");
			ShadowsArray.Add(shadowGO);
			shadowRef = GameObject.Find ("MainUmpire/Armature/Bone/hip/spin/ShadowRef");
			ShadowRefArray.Add(shadowRef);

			shadowGO = GameObject.Find("RunnerShadow");
			ShadowsArray.Add(shadowGO);
			shadowRef = GameObject.Find ("Batsman/metarig/hips/spine/ShadowRef");
			ShadowRefArray.Add(shadowRef);

			UpdateShadow ();

			shadowHolder.SetActive (true);
		}
		else
		{
			shadowHolder.SetActive (false);
		}
		// Confidence Level
		batsmanRefPoint = GameObject.Find("BatsmanRefPoint").transform;
		// Confidence Level

		//camera flash references
		flashReference1 = GameObject.Find("FlashReference1");
		flashReference2 = GameObject.Find("FlashReference2");
		flashReference3 = GameObject.Find("FlashReference3");
		flashReference4 = GameObject.Find("FlashReference4");
		flashReference5 = GameObject.Find("FlashReference5");
		flashReference6 = GameObject.Find("FlashReference6");
		flashReference7 = GameObject.Find("FlashReference7");
		flashReference8 = GameObject.Find("FlashReference8");

//		animationController = GetComponent<AnimationController>();
		ShotVariables.InitShotVariables ();
		InitCamera ();
	}

	public void Start ()
	{
		if (PlayerPrefs.HasKey("loft"))
		{
			if (PlayerPrefs.GetInt("loft") == 1)
			{
				loft = true;
			}
			else
			{
				loft = false;
			}
		}
		else
		{
			loft = true;
		}
        //By default loft is off as per Jaysree
        loft = false;

        SetLoftImage();
		if (CONTROLLER.gameMode != "superover")
		{
			if (CONTROLLER.BowlingEnd == "madrasclub")
			{
				stadiumGO.transform.eulerAngles = new Vector3(0,180f,0);
				extrasGO.transform.eulerAngles=new Vector3 (extrasGO.transform.eulerAngles.x,180f,extrasGO.transform.eulerAngles.z);
				cameraFlashReferences.transform.eulerAngles = new Vector3 (cameraFlashReferences.transform.eulerAngles.x, 0, cameraFlashReferences.transform.eulerAngles.z);
				stadiumRotationAngle = 0;
			}
			else
			{
				stadiumGO.transform.eulerAngles = new Vector3(0,0,0);
				extrasGO.transform.eulerAngles=new Vector3 (extrasGO.transform.eulerAngles.x ,0f,extrasGO.transform.eulerAngles.z ) ;
				cameraFlashReferences.transform.eulerAngles = new Vector3 (cameraFlashReferences.transform.eulerAngles.x, 180, cameraFlashReferences.transform.eulerAngles.z);
				stadiumRotationAngle = 180;
			}
		}
		else
		{
			stadiumGO.transform.eulerAngles = new Vector3(0,180f,0);
			extrasGO.transform.eulerAngles =new Vector3 (extrasGO.transform.eulerAngles.x,180f,extrasGO.transform.eulerAngles.z);
			cameraFlashReferences.transform.eulerAngles = new Vector3 (cameraFlashReferences.transform.eulerAngles.x, 0f, cameraFlashReferences.transform.eulerAngles.z);
			stadiumRotationAngle = 0;
		}



		Physics.IgnoreLayerCollision (11, 8 , true);
		rightSideCamera.enabled = false;
		leftSideCamera.enabled = false;
		straightCamera.enabled = false;

		ShowFielder10 (false, false);


		ResetAll();
		ResetFielders ();
		ActivateColliders (false);
		boardCollider.SetActive (false);
		billBoardCollider.SetActive (false);
		stadiumCollider.enabled = false;
		LoadingScreen.instance.Hide();

		updateBatsmanTiming();
    }

    public void  ShowFielder10 (bool fielder10Status, bool ball10Status)
	{ 
		fielder10Skin.GetComponent<Renderer>().enabled = fielder10Status;
		fielder10Ball.GetComponent<Renderer>().enabled = ball10Status;
		if(fielder10Status == true && canActivateBowler == false)
		{
			canActivateBowler = true;
			fielder10Action = "idle";
			if(ballStatus == "bowled")
			{
				/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent.Play("appeal");
				/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent["appeal"].speed=0.75f;
			}
			if(lbwAppeal == true)
			{ 
				stayStartTime = Time.time;
				fielder10Action = "lbwAppeal";
				fielder10AnimationComponent.Play("lbwAppeal");
				GameModelScript.PlayGameSound("lbwappealvoice");
				if(LBW == false)
				{
					StartCoroutine (_wait(1.0f));
				}
			}
		}
	}
    public int GetOppTeamNameIndex()            //getting oppname
    {
        int temp = 0;
        int tempIndex = 0;
        int tempIndex2 = 0;
        
        for (int i = 0; i < 10; i++)
        {
            tempIndex = CONTROLLER.SelectedCrusadeMatchIdx;
            tempIndex2 = CONTROLLER.SelectedCrusadeSeasonIdx;           
            if (CONTROLLER.SelectedCrusadeMatchIdx < 0)
            {
                tempIndex = 0;
            }
            if (CONTROLLER.SelectedCrusadeSeasonIdx < 0)
            {
                tempIndex2 = 0;
            }
            if (SuperCrusadesController.instance.oppTeamNameColor[i] == SuperCrusadesController.instance.SuperCrusadesData[tempIndex2].MatchDatas[tempIndex].OppTeamName)
            {
                temp = i;
                break;
            }
        }
        return temp;
    }

    public void SetFielderUniformColor()
	{
        
        int index = 0;
        if (GameModel.instance.ContinueMatch)
        {
            index = CONTROLLER.currentAIUniformColor;
        }
        else
        {
            if (CONTROLLER.gameMode == CONTROLLER.SUPER_Crusade_GameMode)
            {
                index = GetOppTeamNameIndex();
            }
            else
            {
				index = Enumerable.Range(0, JerseyHandler.instance.BatsmanJersey.Length).Where(i => i != CONTROLLER.JerseyIDX).ElementAt(Random.Range(0, JerseyHandler.instance.BatsmanJersey.Length - 1));
			}
            CONTROLLER.currentAIUniformColor = index;
        }
        if (index < 0)
        {
            index = 0;
        }
        for (int i=1; i<= noOfFielders; i++)
		{
            if (fielderSkin[i].GetComponent<Renderer>().materials[0] == null)
			{
			}
			else
			{
				//fielderSkin[i].GetComponent<Renderer>().materials[0].SetColor("_MainColor", teamUniformColor[index]);//.color = teamUniformColor[index];
				fielderSkin[i].GetComponent<Renderer>().materials[0].mainTexture = JerseyHandler.instance.FielderJersey[index];
			}
		}
		/*fielderSkin[10].GetComponent<Renderer>().materials[0].SetColor("_MainColor", teamUniformColor[index]);
		spinBowlerSkin.GetComponent<Renderer>().materials[0].SetColor("_MainColor", teamUniformColor[index]);//.color = teamUniformColor[index];
        fastBowlerSkin.GetComponent<Renderer>().materials[0].SetColor("_MainColor", teamUniformColor[index]);//.color = teamUniformColor[index];
        wicketKeeperSkin.GetComponent<Renderer>().materials[0].SetColor("_MainColor", teamUniformColor[index]);//.color = teamUniformColor[index];
		*/
		//CricMini-Gopi
		fielderSkin[10].GetComponent<Renderer>().materials[0].mainTexture = JerseyHandler.instance.FielderJersey[index];
		spinBowlerSkin.GetComponent<Renderer>().materials[0].mainTexture = JerseyHandler.instance.FielderJersey[index];
		fastBowlerSkin.GetComponent<Renderer>().materials[0].mainTexture = JerseyHandler.instance.FielderJersey[index];
		wicketKeeperSkin.GetComponent<Renderer>().materials[0].mainTexture = JerseyHandler.instance.WicketKeeperJersey[index];
	}

	public void ShowBowler (bool showStatus)
	{		
		bowlerSkin.GetComponent<Renderer>().enabled = showStatus;
	}
	public void  ResetYield ()
	{
		ResetAll ();
	}
    /*IEnumerable wait()
	{
		yield return new WaitForSeconds (0.1f);
	}*/
    public void ResetAll()
	{
		//ManojAdded
		radius = 5; //60
		speed = 1.25f;
		mainCameraOnTopDownView = false;
		mainCamera.fieldOfView = 60;//31;
		//ManojAdded
		savedStrikerIndex = CONTROLLER.StrikerIndex;
		GetBatsmanTappingStyleId();
		reflectionFromBlackBoard = false;
		fielderCollectedTheBall = false;
		ballBoundaryReflection = false;
		applyBallFriction = false;
		CONTROLLER.SixDistance = 0;
		CamShouldNotFollowBallY = false;
		sixDistanceCamera.enabled = false;
		rightSideCamera.enabled = false;
		leftSideCamera.enabled = false;
		straightCamera.enabled = false;
		sideCameraSelected = false;
		action = -2;
		SetSwipeHighlightRenderState(false);

		//StartCoroutine ("wait");
		//		yield return new WaitForSeconds (0.1f);
		//rotateAngle = 0;
		LateAttempt = false;
		fielderSpeed = 7;
//		GameObject fielderGO ;
		touchDeviceShotInput = false; // Android || iOS
		ballNoOfBounce = 0;
		ballProjectileAngle = 270;
		ballProjectileHeight = 2.3f; // meters
		horizontalSpeed = 22;//22; // meters per second
		SwitchToHighPoly ();
		swingProjectileAngle = 0; // if swing...
		swingValue = 0;
		swingingBall = false;
		playedUltraSlowMotion = false;
		slipShot = false;
		ballToFineLeg = false;
		prevSixDist = 0;
		BallHitTime = 0.0f;
		BallPickTime = 0.0f;
		prevMousePos = Vector2.zero;

		loftBtn.gameObject.SetActive (true);
		loftBtn2.gameObject.SetActive (true);
		/*loftText2.color = Color.black;
		loftText.color = Color.black;
		loftBtn.image.sprite = loftSprite [1];
		loftBtn2.image.sprite = loftSprite [1];
		loft = true;
		loftDesc.text = "Loft (On)";
		loftDesc2.text = "Loft (On)";*/
		isTimeToShowAd = true;
		canSwipeNow = false;
		ballProjectileAnglePerSecondFactor = 1.7f;


		if (GameModel.isGamePaused == false)
		{
			AdIntegrate.instance.SetTimeScale(1f);
		}
		if (GameModel.instance != null)
		{
            GameModel.instance.ShowShotTutorial (false);
			GameModel.instance.ShowBatsmanMoveTutorial (false);
		}
		ballPreCatchingDistance = 1.0f; // 1 meter before the bouncing the ball...
		bowlerRunningSpeed = 5; // for program controlled bowler runup animation...
		pauseTheBall = false;
		shotPlayed = ""; // new 04-May-2011, to avoid ball collision even if the batsman not playing the shot...
		ballResult = "";
		canTakeRun = false;
		applyBallFiction = false;
		currentBallNoOfRuns = 0;
		wideBall = false;
		freeHit = false;
		wideBallChecked = false;
		shortestBallPickupDistance = 1000; // init to max distance...
		ballReleased = false;
		ballOverTheFence = false;
		lbwAppeal = false;
		LBW = false;
		ballInline = false;
		throwingFirstBounceDistance = 0;
		canBe4or6 = 6;
		isBallTouchedTheRope = false;

		stump1.GetComponent<Animation>().Play("idle");
		//stump2.animation.Play("idle");

		boardCollider.SetActive (false);
		billBoardCollider.SetActive (false);
		stadiumCollider.enabled = false;
		//canActivateBowler = false;	 //====changes 
		fielder10Action = "";

		mainUmpire.GetComponent<Animation>().Play("foldidle");
		mainUmpire.transform.LookAt(new  Vector3(groundCenterPoint.transform.position.x, 0, groundCenterPoint.transform.position.z));
		/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent.Play ("idle");
		fielder10.transform.eulerAngles = new Vector3 (fielder10.transform.eulerAngles.x, 0, fielder10.transform.eulerAngles.z);

		/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("idle");
		wicketKeeperBall.GetComponent<Renderer>().enabled = false;
		wicketKeeper.transform.eulerAngles =new  Vector3(0f, 180f, 0f);
		wicketKeeperCatchingAnimationSelected = false;

		slipFielderWarmUpAction = false;
		slipFielder2WarmUpAction = false;

		if (CONTROLLER.currentMatchBalls != 0)
		{
			ResetFielders ();//shankar 08April
		}
		ballSpinningSpeedInX = Random.Range (-3600, -1800);
		ballSpinningSpeedInZ = Random.Range (-3600, -1800); // for spin bowler
		currentBowlerType = CONTROLLER.bowlerType;//ShankarEdit

		if(currentBowlerType == "fast")
		{	
			fielder10.transform.position = fielder10FastInit.transform.position; 
			spinBowlerSkin.GetComponent<Renderer>().enabled = false;
			spinBowlerBall.GetComponent<Renderer>().enabled = false;

			bowler = fastBowler;
			bowlerSkin = fastBowlerSkin;
			bowlerBall = fastBowlerBall;
			ballSpinningSpeedInZ = Random.Range (-500f, 500f);
			swingValue = Random.Range (-2.0f,2.0f);
			if(currentBatsmanHand == "right")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4RHBFast;
			}
			else if(currentBatsmanHand == "left")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4LHBFast;
			}

			bowlingBounceFactor = 1.0f * pitchFactor;

		}
		else if(currentBowlerType == "spin")
		{
			bowlingBounceFactor = 0.6f * pitchFactor;

			fielder10.transform.position = fielder10SpinInit.transform.position; 
			fastBowlerSkin.GetComponent<Renderer>().enabled = false;
			fastBowlerBall.GetComponent<Renderer>().enabled = false;
			bowler = spinBowler; 
			bowlerSkin = spinBowlerSkin;
			bowlerBall = spinBowlerBall;
			if(currentBatsmanHand == "right")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4RHBSpin;
			}
			else if(currentBatsmanHand == "left")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4LHBSpin;
			}
		}

		ballTransform.position = ballInitPosition;
		ballTransform.eulerAngles =new Vector3 (0f, 2f, 90f);
		ballSkin.transform.eulerAngles = Vector3.zero;

		ShowBall(false);

		strikerScript.resetSpeed(0.0f, 0.0f);
		runnerScript.resetSpeed(0.0f, 0.0f);


		/*if (currentBatsmanHand == "right")
		{
			batsman.transform.position = RHBatsmanInitPosition;
			batsman.transform.localScale = new Vector3(1, batsman.transform.localScale.y, batsman.transform.localScale.z);
			batsman.transform.eulerAngles = new Vector3(batsman.transform.eulerAngles.x, 270f, batsman.transform.eulerAngles.z);
		}
		else if (currentBatsmanHand == "left")
		{
			batsman.transform.localScale = new Vector3(-1f, batsman.transform.localScale.y, batsman.transform.localScale.z);
			batsman.transform.eulerAngles = new Vector3(batsman.transform.eulerAngles.x, 90f, batsman.transform.eulerAngles.z);
			batsmanInitXPos = batsman.transform.position.x;
		}*/

		if (currentBatsmanHand == "right")
		{
			batsman.transform.position = RHBatsmanInitPosition;
            batsmanInitXPos = batsman.transform.position.x;
			strikerScript.setMirror(false);
			batCollider = batCollider_Right;
			batColliderHolder = batColliderHolder_Right;
			batColliderComponent = batColliderComponent_Right;
			batColliderHolder_Left.transform.localScale = Vector3.zero;
			//batEdgeGO = rightBatsmanEdgeGO;
			batsman.transform.eulerAngles = new Vector3(batsman.transform.eulerAngles.x, 270, batsman.transform.eulerAngles.z);
			batColliderHolder.transform.localScale = GetBatColliderSize();
		}
		else if (currentBatsmanHand == "left")
		{
			batsman.transform.position = LHBatsmanInitPosition;
			batsmanInitXPos = batsman.transform.position.x;
			strikerScript.setMirror(true);
			batCollider = batCollider_Left;
			batColliderHolder = batColliderHolder_Left;
			batColliderComponent = batColliderComponent_Left;
			batColliderHolder_Right.transform.localScale = Vector3.zero;
			//batEdgeGO = leftBatsmanEdgeGO;
			batsman.transform.eulerAngles = new Vector3(batsman.transform.eulerAngles.x, 90, batsman.transform.eulerAngles.z);
			batColliderHolder.transform.localScale = GetBatColliderSize();
		}
		batsman.transform.localScale = new Vector3(1, batsman.transform.localScale.y, batsman.transform.localScale.z);
		SetStrikerIdleLoop();

		squareLegGlance = false;
		if (currentBowlerHand == "right")
		{
			bowler.transform.localScale = new Vector3(1, bowler.transform.localScale.y, bowler.transform.localScale.z);
			ballOriginGO.transform.position = new Vector3(-0.6f, ballOriginGO.transform.position.y, ballOriginGO.transform.position.z);
			ballTransform.position = new Vector3(-0.6f, ballTransform.position.y, ballTransform.position.z);
		}
		else if (currentBowlerHand == "left")
		{
			bowler.transform.localScale = new Vector3(-1, bowler.transform.localScale.y, bowler.transform.localScale.z);
			ballOriginGO.transform.position = new Vector3(-0.84f, ballOriginGO.transform.position.y, ballOriginGO.transform.position.z);
			ballTransform.position = new Vector3(-0.84f, ballTransform.position.y, ballTransform.position.z);
		}

			bowler.GetComponent<Animation>().Play("BowlerIdle");
			bowlerBall.GetComponent<Renderer>().enabled = true;
		

		canActivateBowler = false;			 //changes
		hideBowlingInterface = false;
		userBowlerCanMoveBowlingSpot = false;
		userBowlingSpotSelected = false;

		ballStatus = "";
		canMakeShot = false;
		batsmanTriggeredShot = false;
		batsmanMadeShot = false;
		batsmanCanMoveLeftRight = false;
		batsmanOnLeftRightMovement = false;
		/*******************Shankar Edit********/
		perfectShot = false;
		DisableTrail ();
		/***************************************/
		powerShot = false;
		powerKeyDown = false;
		ballOnboundaryLine = false;
		canApplyFriction = true;

		wicketKeeperIsActive = false;
		wicketKeeperStatus = "";

		stopTheFielders = false;


		runner.transform.position = runnerInitPosition;//25march
		runnerInitPosition = runner.transform.position;//25march

		//runner.GetComponent<Animation>().Play("idle");//25march
		//runner.GetComponent<Animation>()["idle"].speed = 0.3f;//25march
		//runner.transform.eulerAngles = new Vector3 (runner.transform.eulerAngles.x, 180, runner.transform.eulerAngles.z);//25march	180
		/*if (runnerStandingAnimationId == 1)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner1_Idle");
			}
			else
			{
				runnerScript._playAnimation("Runner1_Idle_Left");
			}

			runnerScript.setAnimationSpeed(1.0f);
		}
		else if (runnerStandingAnimationId == 2)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner2_Idle");
			}
			else
			{
				runnerScript._playAnimation("Runner2_Idle_Left");
			}
			runnerScript.setAnimationSpeed(1.0f);
		}*/

		//CanOptimize
		runner.transform.eulerAngles = new Vector3(runner.transform.eulerAngles.x, 90, runner.transform.eulerAngles.z); // 180


		stickerStatus = "idle";
		nonStickerStatus = "idle";
		strikerScript.resetLayerVal();
		runnerScript.resetLayerVal();
		takingRun = false;
		isRunOut = false;
		boundaryAction = "";
		waitForCommentary = false;


		rightSideCamera.transform.position=new Vector3 (rightSideCamera.transform.position.x ,rightSideCamera.transform.position.y ,10f);
		leftSideCamera.transform.position = mainCamera.transform.position;
		straightCamera.transform.position = new Vector3 (straightCamera.transform.position.x,straightCamera.transform.position.y,-33);

		//rightSideCamera.transform.position.z = 10;
		//leftSideCamera.transform.position.z = 10;
		//straightCamera.transform.position.z = 03;

		rightSideCamera.fieldOfView = 50;
		leftSideCamera.fieldOfView = 50;
		straightCamera.fieldOfView = 50;
		cameraToKeeper = false;


		upArrowKeyDown = false;
		downArrowKeyDown = false;
		leftArrowKeyDown = false;
		rightArrowKeyDown = false;

		ShowBowler (true);
		ShowFielder10 (false, false);

		updateBattingTimingMeterNeedle = false;

		if (playIntro == true)
		{
			action = -1;
			mainCamera.enabled = false;
			InitCamera ();
		}
		else
		{
			action = -2;// Wait to select bowling speed and angle...
			InitCamera ();
			FielderExtraActions ();
			/*if (CONTROLLER.canShowMainCamera == true)
		{
			mainCamera.enabled = true;//Shankar commented on May10
		}*/
			mainCamera.enabled = true;
			introCamera.enabled = false;
		}
		if (CONTROLLER.NewInnings == false)// && StartScreen.instance == null && BattingScoreCard.instance.gameObject.transform.localPosition.x != 0)
		{
			
			ZoomCameraToBowler ();
		}

		UpdateShadowsAndPreview ();
		ChangePlayerLeftRightTextures ();
		SetBowlerSide ();
		resetUltraMotionVariables ();
		AudioPlayer.instance.CallGarbageCollection ();
		ballSkinRigidBody.Sleep();
		//Postbatting
		cancelPostAnim();
        //if (highEndDevice == true)
        //{
        //	batCollider.transform.localScale = new Vector3 (0.85f, 1.0f, 0.85f);// for high-end device
        //}
        //else
        //{
        //	batCollider.transform.localScale =new  Vector3 (1.3f, 1.3f, 1.3f);// for low-end device
        //}
        //lensFlareHolder.SetActive (false);
        //groundFlashGO.SetActive (false);

        /*//By default loft is off as per Jaysree
        loft = false;
        SetLoftImage();*/
    }


	public void resetUltraMotionVariables ()
	{
		dummyBall.GetComponent<Renderer>().enabled = false;
		ultraMotionCamera.enabled = false;
		BowledReplayCamera.enabled = false;
		if (currentBatsmanHand == "left")
		{
			//		ultraMotionCamera.transform.position = new Vector3 (9, 2, 9);
			//		ultraMotionCamera.transform.eulerAngles = new Vector3 (7, -90, 0);

			ultraMotionCamera.transform.position = new Vector3 (5, 1, 9);
			ultraMotionCamera.transform.eulerAngles = new Vector3 (0, -90, 0);


			//		BowledReplayCamera.transform.position = new Vector3 (5, 1, 5);
			//		BowledReplayCamera.transform.position = new Vector3 (2.5, 1, 7.55);
			//		BowledReplayCamera.transform.position = new Vector3 (3.0, 0.6, 7.0);
			BowledReplayCamera.transform.position = new Vector3 (3.5f, 0.5f, 6.5f);

			//BowledReplayCamera.transform.eulerAngles = new Vector3 (0, -45, 0);
			//BowledReplayCamera.transform.eulerAngles = new Vector3 (-4, -45, 0);
			BowledReplayCamera.transform.eulerAngles = new Vector3 (-10, -45, 0);
		}
		else
		{
			//		ultraMotionCamera.transform.position = new Vector3 (-9, 2, 9);
			//		ultraMotionCamera.transform.eulerAngles = new Vector3 (7, 90, 0);

			ultraMotionCamera.transform.position = new Vector3 (-5, 1, 9);
			ultraMotionCamera.transform.eulerAngles = new Vector3 (0, 90, 0);


			//		BowledReplayCamera.transform.position = new Vector3 (-5, 1, 5);
			//BowledReplayCamera.transform.position = new Vector3 (-2.5, 1, 7.55);
			//BowledReplayCamera.transform.position = new Vector3 (-3.0, 0.6, 7.0);
			BowledReplayCamera.transform.position = new Vector3 (-3.5f, 0.5f, 6.5f);

			//		BowledReplayCamera.transform.eulerAngles = new Vector3 (-4, 45, 0);
			BowledReplayCamera.transform.eulerAngles = new Vector3 (-10, 45, 0);

		}
		playedUltraSlowMotion = false;
	}

	public void StartToBowl ()
	{
		ZoomCameraToBowler ();
		GameModel.instance.NewBall ();
	}

	private void  SetBowlerSide ()
	{
		if (CONTROLLER.BatsmanHand == "right")
		{
			bowler.transform.position = new Vector3 (-0.8f, bowler.transform.position.y, bowler.transform.position.z);
			//runner.transform.position = new Vector3(runnerInitPosition.x, runner.transform.position.y, runner.transform.position.z);//25march

			ballOriginGO.transform.position = new Vector3 (-0.6f, ballOriginGO.transform.position.y, ballOriginGO.transform.position.z);
			ballTransform.position = new Vector3 (-0.6f, ballTransform.position.y, ballTransform.position.z);

			if(currentBowlerType == "fast")
			{
				fielder10.transform.position = fielder10FastInit.transform.position; 
			}
			else if(currentBowlerType == "spin")
			{
				fielder10.transform.position = fielder10SpinInit.transform.position; 
			}
		}
		else if(CONTROLLER.BatsmanHand == "left")
		{
			bowler.transform.position = new Vector3 (0.8f, bowler.transform.position.y, bowler.transform.position.z);
			//runner.transform.position = new Vector3(runnerInitPosition.x * -1, runner.transform.position.y, runner.transform.position.z);//25march
																																		 //runner.transform.localScale = new Vector3 (Mathf.Abs (runner.transform.localScale.x), runner.transform.localScale.y, runner.transform.localScale.z);//25march
			ballOriginGO.transform.position = new Vector3 (0.9f, ballOriginGO.transform.position.y, ballOriginGO.transform.position.z);
			ballTransform.position = new Vector3 (0.9f, ballTransform.position.y, ballTransform.position.z);
			if (currentBowlerType == "fast" || currentBowlerType == "spin") 
			{
				fielder10.transform.position = new Vector3 (0.79f, fielder10.transform.position.y, fielder10.transform.position.z); 
			}
		}

		if (bowlerSide == "right")
		{
			runnerScript.setMirror(true);
			runner.transform.position = new Vector3(runnerInitPosition.x, runner.transform.position.y, runner.transform.position.z);
			runner.transform.localEulerAngles = new Vector3(runner.transform.localEulerAngles.x, 0, runner.transform.localEulerAngles.z); 
		}
		else
		{
			runnerScript.setMirror(false);
			runner.transform.position = new Vector3(runnerInitPosition.x * -1, runner.transform.position.y, runner.transform.position.z);
			runner.transform.localEulerAngles = new Vector3(runner.transform.localEulerAngles.x, 0, runner.transform.localEulerAngles.z); 
		}

		if (runnerStandingAnimationId == 1)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner1_Idle");
			}
			else
			{
				runnerScript._playAnimation("Runner1_Idle_Left");
			}

			runnerScript.setAnimationSpeed(0.3f);
		}
		else if (runnerStandingAnimationId == 2)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner2_Idle");
			}
			else
			{
				runnerScript._playAnimation("Runner2_Idle_Left");
			}
			runnerScript.setAnimationSpeed(1.0f);
		}
	}

	public void NewInnings ()
	{
		
		moveBannerTextureScript.Reset();

		InitCamera();
		ActivateColliders(false);

		// if user or computer is bowling...
		if (Random.Range (0.0f, 10.0f) <= 5.0) {
			bowlerSide = "left";
		}
		else {
			bowlerSide = "right";
		}
		SetStrikerIdleLoop();
		//runnerAnimation.Play("RunnerIdle");
		if (runnerScript.isMirror == false)
		{
			runnerScript._playAnimation("Runner1_Idle");
		}
		else
		{
			runnerScript._playAnimation("Runner1_Idle_Left");
		}
		int i = 0;
		GameObject fielderSkinGO ;
		for(i = 1; i <= 10; i++)
		{
			fielderSkinGO = fielderSkin[i] as GameObject;
			//fielderSkinGO.renderer.material.mainTexture = Resources.Load("Uniform/"+bowlingTeam+"/rightBowler");
		}
        SetFielderUniformColor();
		/*wicketKeeperSkin.renderer.material.mainTexture = Resources.Load("Uniform/"+bowlingTeam+"/wicketKeeper");
	fastBowlerSkin.renderer.material.mainTexture = Resources.Load("Uniform/"+bowlingTeam+"/rightBowler");
	spinBowlerSkin.renderer.material.mainTexture = Resources.Load("Uniform/"+bowlingTeam+"/rightBowler");
	batsmanSkin.renderer.material.mainTexture = Resources.Load("Uniform/"+battingTeam+"/rightBatsman");*/
	}

	public void  ChangePlayerLeftRightTextures ()
	{
		string battingHand = "L";
		if(CONTROLLER.StrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			battingHand = CONTROLLER.TeamList[0].PlayerList[(int)CONTROLLER.PlayingTeam[CONTROLLER.StrikerIndex]].BattingHand;
		}
		if (battingHand == "L")
		{
			currentBatsmanHand = "left";
		}
		else
		{
			currentBatsmanHand = "right";
		}



		/*string textureName ;
		if(CONTROLLER.StrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			//textureName = ""+CONTROLLER.TeamList[0].PlayerList[(int)CONTROLLER.PlayingTeam[CONTROLLER.StrikerIndex]].JerseyNumber;
			//batsmanSkin.GetComponent<Renderer>().material.mainTexture = Resources.Load("Uniform/CSK/"+textureName)as Texture ;

			if (CONTROLLER.TeamList[0].PlayerList[(int)CONTROLLER.PlayingTeam[CONTROLLER.StrikerIndex]].BattingHand == "R")
			{
				//batsmanSkin.GetComponent<Renderer>().material.mainTexture = Resources.Load("Uniform/Uniform/MyTeam_R") as Texture; 
				batsmanSkin.GetComponent<Renderer>().material.mainTexture = JerseyHandler.instance.Jersey_Right[CONTROLLER.JerseyIDX]; 
			}
			else
			{
				//batsmanSkin.GetComponent<Renderer>().material.mainTexture = Resources.Load("Uniform/Uniform/MyTeam_L") as Texture; 
				batsmanSkin.GetComponent<Renderer>().material.mainTexture = JerseyHandler.instance.Jersey_Left[CONTROLLER.JerseyIDX]; 
			}
		}
		if (CONTROLLER.NonStrikerIndex < CONTROLLER.PlayingTeam.Count)
		{
			//textureName = ""+CONTROLLER.TeamList[0].PlayerList[(int)CONTROLLER.PlayingTeam[CONTROLLER.NonStrikerIndex]].JerseyNumber;
			//runnerSkin.GetComponent<Renderer>().material.mainTexture = Resources.Load("Uniform/CSK/"+textureName)as Texture ;

			if (CONTROLLER.TeamList[0].PlayerList[(int)CONTROLLER.PlayingTeam[CONTROLLER.NonStrikerIndex]].BattingHand == "R")
			{
				//runnerSkin.GetComponent<Renderer>().material.mainTexture = Resources.Load("Uniform/Uniform/MyTeam_R") as Texture;
				runnerSkin.GetComponent<Renderer>().material.mainTexture = JerseyHandler.instance.Jersey_Right[CONTROLLER.JerseyIDX];
			}
			else
			{
				//runnerSkin.GetComponent<Renderer>().material.mainTexture = Resources.Load("Uniform/Uniform/MyTeam_L") as Texture;
				runnerSkin.GetComponent<Renderer>().material.mainTexture = JerseyHandler.instance.Jersey_Left[CONTROLLER.JerseyIDX];
			}
		}
		*/
		if (GameModel.instance.IsWicketBall == true)
		{
			GameModel.instance.IsWicketBall = false;
			BatsmanInfo.instance.TweenProfileCard ();
		}
		if (currentBatsmanHand == "left")
		{
			bowlerSide = "left";
			//mainCameraPitchPosition = new Vector3(0.4f, 2f, 15.5f);
			//mainCameraInitRotation = new Vector3(0f, 182f, 0f);
			//ManojAdded
			mainCameraPitchPosition = new Vector3(0.0f, 2f, 14.5f);
			mainCameraInitRotation = new Vector3(0f, 180f, 0f);
			mainCamera.fieldOfView = 60;
			//ManojAdded
		}
		else if (currentBatsmanHand == "right")
		{
			bowlerSide = "right";
			//mainCameraPitchPosition = new Vector3(-0.4f, 2f, 15.5f);
			//mainCameraInitRotation = new Vector3(0, 178, 0);
			//ManojAdded
			mainCameraPitchPosition = new Vector3(0.0f, 2f, 14.5f);
			mainCameraInitRotation = new Vector3(0f, 180f, 0f);
			mainCamera.fieldOfView = 60;
			//ManojAdded
		}	

		
		InitCamera ();
		//30march
		if(currentBatsmanHand == "right")
		{
			batsman.transform.position = RHBatsmanInitPosition;
            strikerScript.setMirror(false);
			//batsman.transform.localScale = new Vector3 (1, batsman.transform.localScale.y, batsman.transform.localScale.z);
			batsman.transform.eulerAngles = new Vector3 (batsman.transform.eulerAngles.x, 270, batsman.transform.eulerAngles.z);
			batsmanInitXPos = batsman.transform.position.x;
		}
		else if(currentBatsmanHand == "left")
		{
			batsman.transform.position = LHBatsmanInitPosition;
			strikerScript.setMirror(true);
			//batsman.transform.localScale = new Vector3 (-1, batsman.transform.localScale.y, batsman.transform.localScale.z);
			batsman.transform.eulerAngles = new Vector3 (batsman.transform.eulerAngles.x, 90, batsman.transform.eulerAngles.z);
			batsmanInitXPos = batsman.transform.position.x;
		}
		//30march
		CONTROLLER.BatsmanHand = currentBatsmanHand;
	}


	public void StartBowling ()
	{

        float bowlingAnglePercentage = Mathf.Abs (CONTROLLER.BowlingAngle) / 10; // Default 0 for fast bowler. range from 1 - 10; consider ...
		float  maxSpin = 4;
		bowlingSpeed = CONTROLLER.BowlingSpeed;	// range from 1 - 10;
		spinValue = maxSpin * bowlingAnglePercentage;
		if(currentBowlerSpinType == 1)
		{
			spinValue *= -1;
		}

		if(currentBowlerType == "fast")
		{
			swingValue = CONTROLLER.BowlingSwing - 2; // 0 to 4...
			swingValue = swingValue * 0.75f; // to avoid too much swing...
			if(swingValue != 0)
			{
				swingingBall = true;
				spinValue = -swingValue * 2; // 2*2 =>4 // 4 = max swing turn after pitching the ball... // -4 min..
			}
		}
		// reassigning the position and ball angle...
		ballTransform.position = ballInitPosition;
		/*
	if(currentBowlerHand == "right")
	{
		ball.transform.position.x = -0.6;
	}
	else if(currentBowlerHand == "left")
	{
		ball.transform.position.x = -0.84;
	}
	*/
		//SetBowlerSide ();
		ballTransform.eulerAngles =new  Vector3 (0f, 2f, 90f); // reset ball seam angle
		ballSkin.transform.eulerAngles = Vector3.zero;

		// reassigning the position and ball angle...

		// new 04April2013
		ballRayCastReferenceGOTransform.position = new Vector3 (ballTransform.position.x, ballTransform.position.y, ballTransform.position.z);
		// new 04April2013

		stopIsBowlerBatsmanIdle();
		if (runnerStandingAnimationId == 1)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner1_GetReady");
			}
			else
			{
				runnerScript._playAnimation("Runner1_GetReady_Left");
			}
		}
		else if (runnerStandingAnimationId == 2)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner2_GetReady");
			}
			else
			{
				runnerScript._playAnimation("Runner2_GetReady_Left");
			}
		}
		if (bowlingBy == "user")
		{
			//strikerAnimation.CrossFade("PreIdleV2");
			SetStrikerPreIdle();
		}
		currentBallStartTime = Time.time;
		action = 0;
	}



	public void ResetFielders ()
	{
		int i;
		GameObject fielderGO;
		CONTROLLER.computerFielderChangeIndex = Random.Range (1, 11);//ShankarEdit
		for(i = 1; i <= noOfFielders; i++)
		{
			fielderGO = fielder[i] as GameObject;
			if(CONTROLLER.myTeamIndex == CONTROLLER.BowlingTeamIndex)
			{
				if(fieldRestriction == true)
				{
					if(CONTROLLER.fielderChangeIndex == 1)
					{
						fielderGO.transform.position = fieldRestriction1FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 2)
					{
						fielderGO.transform.position = fieldRestriction2FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 3)
					{
						fielderGO.transform.position = fieldRestriction3FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 4)
					{
						fielderGO.transform.position = fieldRestriction4FielderPosition [i];
					}else if(CONTROLLER.fielderChangeIndex == 5)
					{
						fielderGO.transform.position = fieldRestriction5FielderPosition [i];
					}
					if(currentBatsmanHand == "left")
					{
						if(CONTROLLER.fielderChangeIndex == 1)
						{
							fielderGO.transform.position =new Vector3 (fieldRestriction1FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 2)
						{
							fielderGO.transform.position= new Vector3 (fieldRestriction2FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 3)
						{
							fielderGO.transform.position=new Vector3 (fieldRestriction3FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 4)
						{
							fielderGO.transform.position=new Vector3 (fieldRestriction4FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 5)
						{
							fielderGO.transform.position=new Vector3(fieldRestriction5FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
					}
				}
				else if(fieldRestriction == false)
				{
					if(CONTROLLER.fielderChangeIndex == 1)
					{
						fielderGO.transform.position = fieldRestriction1FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 2)
					{
						fielderGO.transform.position = fieldRestriction2FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 3)
					{
						fielderGO.transform.position = fieldRestriction3FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 4)
					{
						fielderGO.transform.position = fieldRestriction4FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 5)
					{
						fielderGO.transform.position = fieldRestriction5FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 6)
					{
						fielderGO.transform.position = fieldRestriction6FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 7)
					{
						fielderGO.transform.position = fieldRestriction7FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 8)
					{
						fielderGO.transform.position = fieldRestriction8FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 9)
					{
						fielderGO.transform.position = fieldRestriction9FielderPosition [i];
					}
					else if(CONTROLLER.fielderChangeIndex == 10)
					{
						fielderGO.transform.position = fieldRestriction10FielderPosition [i];
					}

					if(currentBatsmanHand == "left")
					{
						if(CONTROLLER.fielderChangeIndex == 1)
						{
							fielderGO.transform.position =new Vector3 (fieldRestriction1FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 2)
						{
							fielderGO.transform.position =new Vector3 (fieldRestriction2FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 3)
						{
							fielderGO.transform.position=new Vector3 (fieldRestriction3FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 4)
						{
							fielderGO.transform.position=new Vector3 (fieldRestriction4FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 5)
						{
							fielderGO.transform.position=new Vector3 (fieldRestriction5FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 6)
						{
							fielderGO.transform.position =new Vector3 ( fieldRestriction6FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 7)
						{
							fielderGO.transform.position =new Vector3 ( fieldRestriction7FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 8)
						{
							fielderGO.transform.position =new Vector3 (fieldRestriction8FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 9)
						{
							fielderGO.transform.position =new Vector3 (fieldRestriction9FielderPosition [i].x * -1f,fielderGO.transform.position .y,fielderGO.transform.position.z);
						}
						else if(CONTROLLER.fielderChangeIndex == 10)
						{
							fielderGO.transform.position =new Vector3 (fieldRestriction10FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
						}
					}
				}
			}
			else if(CONTROLLER.myTeamIndex == CONTROLLER.BattingTeamIndex)
			{
				if(CONTROLLER.computerFielderChangeIndex == 1 || CONTROLLER.computerFielderChangeIndex == 0)
				{
					fielderGO.transform.position = fieldRestriction1FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 2)
				{
					fielderGO.transform.position = fieldRestriction2FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 3)
				{
					fielderGO.transform.position = fieldRestriction3FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 4)
				{
					fielderGO.transform.position = fieldRestriction4FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 5)
				{
					fielderGO.transform.position = fieldRestriction5FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 6)
				{
					fielderGO.transform.position = fieldRestriction6FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 7)
				{
					fielderGO.transform.position = fieldRestriction7FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 8)
				{
					fielderGO.transform.position = fieldRestriction8FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 9)
				{
					fielderGO.transform.position = fieldRestriction9FielderPosition [i];
				}
				else if(CONTROLLER.computerFielderChangeIndex == 10)
				{
					fielderGO.transform.position = fieldRestriction10FielderPosition [i];
				}

				if(currentBatsmanHand == "left")
				{
					if(CONTROLLER.computerFielderChangeIndex == 1 || CONTROLLER.computerFielderChangeIndex == 0)
					{
						fielderGO.transform.position =  new Vector3 (fieldRestriction1FielderPosition [i].x * -1,fielderGO.transform.position.y,fielderGO.transform.position.z); 
					}
					else if(CONTROLLER.computerFielderChangeIndex == 2)
					{
						fielderGO.transform.position =new Vector3 (fieldRestriction2FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
					else if(CONTROLLER.computerFielderChangeIndex == 3)
					{
						fielderGO.transform.position =new Vector3  (fieldRestriction3FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
					else if(CONTROLLER.computerFielderChangeIndex == 4)
					{
						fielderGO.transform.position=new Vector3 (fieldRestriction4FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
					else if(CONTROLLER.computerFielderChangeIndex == 5)
					{
						fielderGO.transform.position =new Vector3 (fieldRestriction5FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
					else if(CONTROLLER.computerFielderChangeIndex == 6)
					{
						fielderGO.transform.position=new Vector3 (fieldRestriction6FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
					else if(CONTROLLER.computerFielderChangeIndex == 7)
					{
						fielderGO.transform.position =new Vector3 (fieldRestriction7FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z );
					}
					else if(CONTROLLER.computerFielderChangeIndex == 8)
					{
						fielderGO.transform.position =new Vector3 (fieldRestriction8FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
					else if(CONTROLLER.computerFielderChangeIndex == 9)
					{
						fielderGO.transform.position = new Vector3 (fieldRestriction9FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
					else if(CONTROLLER.computerFielderChangeIndex == 10)
					{
						fielderGO.transform.position =new Vector3 (fieldRestriction10FielderPosition [i].x * -1f,fielderGO.transform.position.y,fielderGO.transform.position.z);
					}
				}
			}
			fielderGO.GetComponent<Animation>().Play("idle");

			GameObject fielderBallGO = fielderBall [i] as GameObject;

			fielderBallGO.GetComponent<Renderer>().enabled = false;

			fielderGO.transform.LookAt(stump1Crease.transform);
		}
		//
		if(currentBowlerType == "spin")
		{
			if(currentBatsmanHand == "right")
			{
				if(fieldRestriction == true)
				{
					if(CONTROLLER.fielderChangeIndex == 1 || CONTROLLER.computerFielderChangeIndex == 1)
					{
						fielderGO = fielder[1] as GameObject;
						//fielderGO.transform.position = slipFielderSpotForRHBSpin.transform.position;
						fielderGO.transform.position = fieldRestriction1FielderPosition[1];
						fielderGO.transform.LookAt(groundCenterPoint.transform);

						fielderGO = fielder[2] as GameObject;
						fielderGO.transform.position = fieldRestriction1FielderPosition[2];
						//fielderGO.transform.position = slipFielder2SpotForRHBSpin.transform.position;
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex == 2 || CONTROLLER.computerFielderChangeIndex == 2)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = slipFielderSpotForRHBSpin.transform.position;
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex >= 3 || CONTROLLER.computerFielderChangeIndex >= 3)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = slipFielder2SpotForRHBSpin.transform.position;
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
				}
			}
			else if(currentBatsmanHand == "left")
			{
				if(fieldRestriction == true)
				{
					if(CONTROLLER.fielderChangeIndex == 1 || CONTROLLER.computerFielderChangeIndex == 1)
					{
						fielderGO = fielder[1] as GameObject;
						//fielderGO.transform.position = slipFielderSpotForRHBSpin.transform.position;
						//fielderGO.transform.position.x = slipFielderSpotForRHBSpin.transform.position.x * -1;
						fielderGO.transform.position = fieldRestriction1FielderPosition[1];
						fielderGO.transform.position =new Vector3 (fieldRestriction1FielderPosition[1].x * -1,fielderGO.transform.position.y ,fielderGO.transform.position.z)  ;
						fielderGO.transform.LookAt(groundCenterPoint.transform);

						fielderGO = fielder[2] as GameObject;
						//fielderGO.transform.position = slipFielder2SpotForRHBSpin.transform.position;
						//fielderGO.transform.position.x = slipFielder2SpotForRHBSpin.transform.position.x * -1;
						fielderGO.transform.position = fieldRestriction1FielderPosition[2];
						fielderGO.transform.position =new Vector3 (fieldRestriction1FielderPosition[2].x * -1,fielderGO.transform.position.y,fielderGO.transform.position.z);
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex == 2 || CONTROLLER.computerFielderChangeIndex == 2)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = slipFielderSpotForRHBSpin.transform.position;
						fielderGO.transform.position = new Vector3 (slipFielderSpotForRHBSpin.transform.position.x * -1,fielderGO.transform.position.y ,fielderGO.transform.position.z) ;
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex >= 3 || CONTROLLER.computerFielderChangeIndex >= 3)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = slipFielder2SpotForRHBSpin.transform.position;
						fielderGO.transform.position =new Vector3 (slipFielder2SpotForRHBSpin.transform.position.x * -1,fielderGO.transform.position.y,fielderGO.transform.position.z);
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
				}
			}
		}
		else if(currentBowlerType == "fast")
		{
			if(currentBatsmanHand == "right")
			{
				if(fieldRestriction == true)
				{
					if(CONTROLLER.fielderChangeIndex == 1 || CONTROLLER.computerFielderChangeIndex == 1)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = fieldRestriction1FielderPosition [1];
						fielderGO.transform.LookAt(groundCenterPoint.transform);

						fielderGO = fielder[2] as GameObject;
						fielderGO.transform.position = fieldRestriction1FielderPosition [2];
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex == 2 || CONTROLLER.computerFielderChangeIndex == 2)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = fieldRestriction1FielderPosition [1];
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex >= 3 || CONTROLLER.computerFielderChangeIndex >= 3)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = fieldRestriction2FielderPosition [1];
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
				}
			}
			else if(currentBatsmanHand == "left")
			{
				if(fieldRestriction == true)
				{
					if(CONTROLLER.fielderChangeIndex == 1 || CONTROLLER.computerFielderChangeIndex == 1)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = fieldRestriction1FielderPosition [1];
						fielderGO.transform.position =new Vector3 (fieldRestriction1FielderPosition [1].x * -1,fielderGO.transform.position.y,fielderGO.transform.position.z );
						fielderGO.transform.LookAt(groundCenterPoint.transform);

						fielderGO = fielder[2] as GameObject;
						fielderGO.transform.position = fieldRestriction1FielderPosition [2];
						fielderGO.transform.position =new Vector3 (fieldRestriction1FielderPosition [2].x * -1,fielderGO.transform.position.y,fielderGO.transform.position.z);
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex == 2 || CONTROLLER.computerFielderChangeIndex == 2)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = fieldRestriction1FielderPosition [1];
						fielderGO.transform.position =new Vector3 (fieldRestriction1FielderPosition [1].x * -1,fielderGO.transform.position.y ,fielderGO.transform.position.z );
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
					else if(CONTROLLER.fielderChangeIndex >= 3 || CONTROLLER.computerFielderChangeIndex >= 3)
					{
						fielderGO = fielder[1] as GameObject;
						fielderGO.transform.position = fieldRestriction2FielderPosition [1];
						fielderGO.transform.position=new Vector3 (fieldRestriction1FielderPosition [1].x * -1,fielderGO.transform.position.y,fielderGO.transform.position.z);
						fielderGO.transform.LookAt(groundCenterPoint.transform);
					}
				}
			}
		}
	}



	public void  FielderExtraActions ()
	{
		GameObject fielderGO;
		float chanceForExtrasAction = Random.Range (-5, 5);
		int warmUpIndex = 0;
		if(chanceForExtrasAction > 0)
		{
			warmUpIndex = Mathf.RoundToInt(chanceForExtrasAction);
			if(warmUpIndex >= 1 && warmUpIndex <= 5)
			{
				fielderGO = fielder[1] as GameObject;
				fielderGO.GetComponent<Animation>().CrossFade("warmUp"+warmUpIndex);
				fielderGO.GetComponent<Animation>().CrossFadeQueued("getReadyInSlip", 0.3f, QueueMode.CompleteOthers);
				fielderGO.GetComponent<Animation>()["getReadyInSlip"].speed = 2;//1;
				slipFielderWarmUpAction = true;
			}
		}

		chanceForExtrasAction = Random.Range (-10, 5);
		if(chanceForExtrasAction > 0)
		{
			int fielder1TempWarmUpIndex = warmUpIndex;
			warmUpIndex = Mathf.RoundToInt(chanceForExtrasAction);
			if(warmUpIndex >= 1 && warmUpIndex <= 5 && fielder1TempWarmUpIndex != warmUpIndex)
			{
				fielderGO = fielder[2] as GameObject;
				fielderGO.GetComponent<Animation>().CrossFade("warmUp"+warmUpIndex);
				fielderGO.GetComponent<Animation>().CrossFadeQueued("getReadyInSlip", 0.3f, QueueMode.CompleteOthers);
				fielderGO.GetComponent<Animation>()["getReadyInSlip"].speed = 1;
				slipFielder2WarmUpAction = true;
			}
		}
	}

	public void  GetFieldersAngle ()
	{
		float xDiff;
		float zDiff;
		float fAngle;
		int i;
		GameObject fielderGO;
		for(i = 1; i <= noOfFielders; i++)
		{
			fielderGO = fielder[i] as GameObject;
			xDiff = ballTimingOrigin.transform.position.x - fielderGO.transform.position.x;
			zDiff = ballTimingOrigin.transform.position.z - fielderGO.transform.position.z;
			fAngle = (Mathf.Atan2(xDiff, zDiff) * RAD2DEG + 360) % 360; // to convert all angles in 0 to 360 and to avoid minus angles...
			fAngle = ((270 - fAngle) + 360) % 360; // angle conversion related to the ball angle ...
			fielderAngle[i] = fAngle;
		}
	}

	public void GetFieldersDistance ()
	{
		int i;
		GameObject fielderGO;
		for(i = 1; i <= noOfFielders; i++)
		{
			fielderGO = fielder[i] as GameObject;
			fielderDistance[i] = DistanceBetweenTwoVector2 (ballTimingOrigin, fielderGO);
		}
	}


	public void SetActiveFielders ()
	{//fff
		float activationAngle = 30;//30; // scanning for +- 45 degrees...
		float diffInAngle;
		float nearestFielderReachTime = 100;// 100 seconds for max...
		int i;

		fielderSpeed = 6; // default value
		if(CONTROLLER.difficultyMode == "hard" && battingBy == "user")
		{
			activationAngle = 40;
			fielderSpeed = 9;
		}

		fielderToCatchTheBall = null;
		activeFielderNumber.Clear ();
		activeFielderAction.Clear ();
		float fielderAngleVal;
		float fielderDistanceVal;
		for(i = 1; i <= noOfFielders; i++)
		{
			fielderAngleVal = fielderAngle[i];
			fielderDistanceVal = fielderDistance[i];
			diffInAngle = Mathf.Abs(ballAngle - fielderAngleVal);
			if(diffInAngle > 180) // between opposite quadrants..
			{
				if(ballAngle > fielderAngleVal)
				{
					diffInAngle = 360 + fielderAngleVal - ballAngle;
				}
				else
				{
					diffInAngle = 360 - fielderAngleVal + ballAngle;
				}
			}
			fielderBallDiffInAngle[i] = diffInAngle;
			if(diffInAngle < activationAngle)
			{
				activeFielderNumber.Add(i);
				GameObject fielderGO = fielder [i] as GameObject;
				fielderGO.GetComponent<Animation>().Play("run");

				// to randomize the run cycle to start from differenct frame..
				float runAnimationLength = fielderGO.GetComponent<Animation>()["run"].length;
				fielderGO.GetComponent<Animation>()["run"].time = Random.Range (0, runAnimationLength);

				float catchDistance = Vector3.Distance(fielderGO.transform.position, ballCatchingSpot.transform.position);
				float fielderReachTime = catchDistance / fielderSpeed;
				float ballReachTime = (ballTimingFirstBounceDistance - ballPreCatchingDistance) / horizontalSpeed;
				if(fielderReachTime < ballReachTime)
				{
					activeFielderAction.Add ("goForCatch");	//activeFielderAction[activeFielderAction.length] = "goForCatch";
					if(fielderReachTime < nearestFielderReachTime)
					{
						nearestFielderReachTime = fielderReachTime;
						fielderToCatchTheBall = fielderGO;
						shortestBallPickupDistance = ballTimingFirstBounceDistance;
					}
				}
				else {
					activeFielderAction.Add ("goForChase");	//activeFielderAction[activeFielderAction.length] = "goForChase";
					//ballCatchingSpot

					float perpendicularChasePointDistance = Mathf.Sin(diffInAngle * DEG2RAD) * fielderDistanceVal;
					float fielderAdjacentDistanceForChasePoint = Mathf.Sqrt(Mathf.Pow(fielderDistance[i], 2) - Mathf.Pow(perpendicularChasePointDistance, 2));

					// ballOnAir initial here...

					float chasePointX = ballTimingOrigin.transform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
					float chasePointZ = ballTimingOrigin.transform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);

					// fielder's shortest point to reach the ball timing angle...
					GameObject fielderChasePointGO;
					fielderChasePointGO = fielderChasePoint[i] as GameObject;
					fielderChasePointGO.transform.position = new Vector3 (chasePointX, 0f, chasePointZ);

					bool ballOnAir = false;
					if(fielderAdjacentDistanceForChasePoint < ballTimingFirstBounceDistance)
					{
						fielderAdjacentDistanceForChasePoint = ballTimingFirstBounceDistance;
						chasePointX = ballTimingOrigin.transform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
						chasePointZ = ballTimingOrigin.transform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);
						fielderChasePointGO.transform.position = new Vector3 (chasePointX, fielderChasePointGO.transform.position.y, chasePointZ);

						// Solution for fielder running outside the boundary line...
						GameObject fielderMaxRunningPoint = fielderChasePoint [i];

						while(DistanceBetweenTwoVector2 (groundCenterPoint, fielderMaxRunningPoint) > groundRadius)
						{
							fielderAdjacentDistanceForChasePoint -= 2;
							chasePointX = ballTimingOrigin.transform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
							chasePointZ = ballTimingOrigin.transform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);
							fielderChasePointGO.transform.position = new Vector3 (chasePointX, fielderChasePointGO.transform.position.y, chasePointZ);
						}
						ballOnAir = true;
					}

					// finding the optimum chase point for the fielder...
					float fielderBallPickUpFrame  = 9;
					float ballPrePickupDistance  = (fielderBallPickUpFrame / animationFPS) * horizontalSpeed; // 10.8

					float fielderPickupDistance = Vector3.Distance(fielderGO.transform.position, fielderChasePointGO.transform.position);
					float fielderPickupReachTime  = fielderPickupDistance / fielderSpeed;
					float ballPrePickupPointReachTime  = (fielderAdjacentDistanceForChasePoint - ballPrePickupDistance) / horizontalSpeed;

					if(ballOnAir == false)
					{
						// pickup forward ... 
						if(fielderPickupReachTime < ballPrePickupPointReachTime)
						{
							while(fielderPickupReachTime < ballPrePickupPointReachTime)
							{
								fielderAdjacentDistanceForChasePoint -= 1;
								chasePointX = ballTimingOrigin.transform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
								chasePointZ = ballTimingOrigin.transform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);
								fielderChasePointGO.transform.position = new Vector3 (chasePointX, fielderChasePointGO.transform.position.y, chasePointZ);

								fielderPickupDistance = Vector3.Distance(fielderGO.transform.position, fielderChasePointGO.transform.position);
								fielderPickupReachTime = fielderPickupDistance / fielderSpeed;
								ballPrePickupPointReachTime = (fielderAdjacentDistanceForChasePoint - ballPrePickupDistance) / horizontalSpeed;
							}
							// spoting a step back..
							fielderAdjacentDistanceForChasePoint += 1;
							chasePointX = ballTimingOrigin.transform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
							chasePointZ = ballTimingOrigin.transform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);
							fielderChasePointGO.transform.position = new Vector3 (chasePointX, fielderChasePointGO.transform.position.y, chasePointZ);
						}
						else // chase behind...
						{
							//DistanceBetweenTwoVector2(groundCenterPoint, ballCatchingSpot) > (groundRadius - 2)
							GameObject fielderCPoint = fielderChasePoint [i];
							while(fielderPickupReachTime > ballPrePickupPointReachTime && 
								DistanceBetweenTwoVector2(groundCenterPoint, fielderCPoint) < (groundRadius - 3))
							{
								fielderAdjacentDistanceForChasePoint += 1;
								chasePointX = ballTimingOrigin.transform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
								chasePointZ = ballTimingOrigin.transform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);
								fielderChasePointGO.transform.position = new Vector3 (chasePointX, fielderChasePointGO.transform.position.y, chasePointZ);

								fielderPickupDistance = Vector3.Distance(fielderGO.transform.position, fielderChasePointGO.transform.position);
								fielderPickupReachTime = fielderPickupDistance / fielderSpeed;
								ballPrePickupPointReachTime = (fielderAdjacentDistanceForChasePoint - ballPrePickupDistance) / horizontalSpeed;
							}
						}
					}

					if(fielderAdjacentDistanceForChasePoint > ballTimingFirstBounceDistance)
					{
						if(fielderAdjacentDistanceForChasePoint < shortestBallPickupDistance)
						{
							shortestBallPickupDistance = fielderAdjacentDistanceForChasePoint;
						}
					}
				}
			}
		}
		if(shortestBallPickupDistance > 80) {
			shortestBallPickupDistance = 80;
		}
	}


	public void makeFieldersToCelebrate (GameObject fielderToAvoid)
	{
		int i;
		GameObject fielderGO;
		for(i = 1; i <= noOfFielders; i++)
		{
			fielderGO = fielder[i] as GameObject;
			if(fielderGO != fielderToAvoid)
			{
				fielderGO.GetComponent<Animation>().Play("appeal");
				fielderGO.GetComponent<Animation>()["appeal"].speed = 1.3f + Random.Range (0.0f, 1.0f);
			}
		}
	}

	public void ActivateBowler ()
	{
		if(action!=3 && action!=4)
		{
			return;
		}	
		//	if(bowler.animation.IsPlaying ("BowlerRunupEdit") == true && canActivateBowler == false)
		if((bowler.GetComponent<Animation>().IsPlaying ("BowlerRunupEdit") == true || bowler.GetComponent<Animation>()["BowlerRunupEdit"].time == 0) && canActivateBowler == false && action != 0)
		{
			if(bowler.GetComponent<Animation>()["BowlerRunupEdit"].time > (bowler.GetComponent<Animation>()["BowlerRunupEdit"].length - 0.2) || bowler.GetComponent<Animation>()["BowlerRunupEdit"].time == 0)
			{
				ShowFielder10 (true, false);
				ShowBowler (false);
			}
		}

			if (canActivateBowler == true && (((shotPlayed == "DownTheTrackDefensiveShot" || shotPlayed == "FrontFootDefense" || shotPlayed == "BackFootDefense") && ballStatus == "shotSuccess") || (ballStatus == "onPads" && lbwAppeal == false)))
		//if (canActivateBowler == true && ballStatus == "onPads" && lbwAppeal == false)
		{ 
			if (fielder10Action == "idle")
			{
				fielder10Action = "end";//"run";ShankarEdit
				stayStartTime = Time.time;//ShankarEdit
										  //fielder10.animation.CrossFade("run");ShankarEdit
			}
			if (fielder10Action == "run")
			{
				float fielder10Angle = AngleBetweenTwoGameObjects(fielder10, ball);
				fielder10.transform.position = new Vector3(fielder10.transform.position.x + (Mathf.Cos(fielder10Angle * DEG2RAD) * fielderSpeed * Time.deltaTime), fielder10.transform.position.y, fielder10.transform.position.z + (Mathf.Sin(fielder10Angle * DEG2RAD) * fielderSpeed * Time.deltaTime));
				fielder10.transform.LookAt(new Vector3(ballTransform.position.x, 0f, ballTransform.position.z));
				if (DistanceBetweenTwoVector2(fielder10, ball) < 1) // one meter...
				{
					fielder10Action = "pickupAttempt";
					/*fielder10.GetComponent<Animation>()*/
					fielder10AnimationComponent.CrossFade("lowCatch");
					/*fielder10.GetComponent<Animation>()*/
					fielder10AnimationComponent["lowCatch"].speed = 5;
					strikerScript._playAnimation("ReturnToStump");

				}
			}
			else if (fielder10Action == "pickupAttempt")
			{
				float pickupFrame = 16;
				float pickupTime = pickupFrame / animationFPS;
				if (pickupTime < /*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent["lowCatch"].time)
				{
					fielder10Action = "pickedup";
					fielder10Ball.GetComponent<Renderer>().enabled = true;
					/*fielder10.GetComponent<Animation>()*/
					fielder10AnimationComponent["lowCatch"].speed = 1;
					ShowBall(false);
					pauseTheBall = true;
					stopTheFielders = true;
				}
			}
			else if (fielder10Action == "pickedup")
			{
				if (/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent["lowCatch"].time == 0) // animation end ...
				{
					float angleOfRotation = AngleBetweenTwoGameObjects(fielder10, stump1) + 90;
					iTween.RotateTo(fielder10, iTween.Hash("y", angleOfRotation, "time", 0.2));
					/*fielder10.GetComponent<Animation>()*/
					fielder10AnimationComponent.Play("idle");
					stayStartTime = Time.time;
					fielder10Action = "end"; //"finish"
				}
			}
			else if (fielder10Action == "end")
			{
				if (stayStartTime - 0.5 + timeBetweenBalls + 2 < Time.time)//ShankarEdit
				{
					fielder10Action = "";
					ShowBall(false);//ShankarEdit
					if (GameModelScript != null)
					{
						// No ball
						if (noBall == true)
						{
							GameModelScript.UpdateCurrentBall(0, 1, 0, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if (freeHit == true)
						{
							GameModelScript.UpdateCurrentBall(1, 1, 0, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if (noBall == false && freeHit == false)
						{
							GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						// No ball
					}
				}
			}

		}
		else if (canActivateBowler == true && ballStatus == "shotSuccess" &&
			(shotPlayed != "BackFootDefense" && shotPlayed != "FrontFootDefense") && fielder10Action == "idle")
		{

			/*fielder10.animation.Play ("bowlerTurn");
		fielder10.animation["bowlerTurn"].speed = 2;
		fielder10.animation.PlayQueued("run", QueueMode.CompleteOthers);
		fielder10Action = "goToNonStickerStump";*/
		}
		else if (canActivateBowler == true && ballStatus == "shotSuccess" && /*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent.IsPlaying("run") == true &&
			(shotPlayed != "BackFootDefense" && shotPlayed != "FrontFootDefense") && fielder10Action == "goToNonStickerStump")
		{
			fielder10.transform.position = new Vector3(fielder10.transform.position.x, fielder10.transform.position.y, fielder10.transform.position.z - 1.6f);

			// for stump fielder positioning to collect the ball
			if (ballAngle >= 30 && ballAngle <= 150)
			{
				postBattingStumpFielderDirection = "straight";
				iTween.MoveTo(fielder10, iTween.Hash("position", fielderStraightBallStumping.transform.position, "time", 1.0, "easetype", "linear", "oncomplete", "EnableFielder10ToCollectBall", "oncompletetarget", ball));
				fielder10.transform.LookAt(fielderStraightBallStumping.transform);
			}
			else if (ballTransform.position.z < fielder10.transform.position.z)
			{
				postBattingStumpFielderDirection = "straightDown";
				iTween.MoveTo(fielder10, iTween.Hash("position", stump2Crease.transform.position, "time", 0.7f, "easetype", "linear", "oncomplete", "EnableFielder10ToCollectBall", "oncompletetarget", ball));
				fielder10.transform.LookAt(stump2Crease.transform);
			}
			else if (ballAngle > 150 && ballAngle < 270)
			{
				postBattingStumpFielderDirection = "offSide";
				iTween.MoveTo(fielder10, iTween.Hash("position", fielderOffSideBallStumping.transform.position, "time", 1.0, "easetype", "linear", "oncomplete", "EnableFielder10ToCollectBall", "oncompletetarget", ball));
				fielder10.transform.LookAt(fielderOffSideBallStumping.transform);
			}
			else
			{
				postBattingStumpFielderDirection = "legSide";
				iTween.MoveTo(fielder10, iTween.Hash("position", fielderLegSideBallStumping.transform.position, "time", 1.0, "easetype", "linear", "oncomplete", "EnableFielder10ToCollectBall", "oncompletetarget", ball));
				fielder10.transform.LookAt(fielderLegSideBallStumping.transform);
			}
			fielder10Action = "reachedNonStickerStump";
		}
		else if (fielder10Action == "waitForBall")
		{
			// four || six || wicket 
			if (ballOnboundaryLine == true)
			{
				/*fielder10.GetComponent<Animation>()*/
				fielder10AnimationComponent.CrossFade("idle");
				fielder10Action = "finish";
			}
			if (ballResult == "wicket")
			{
				/*fielder10.GetComponent<Animation>()*/
				fielder10AnimationComponent.Play("appeal");
				fielder10Action = "finish";
			}
		}
		else if (fielder10Action == "waitToCollect")
		{
			float fielder10CollectingFrame = 7;
			float fielder10BallCollectingTriggerDistance = (fielder10CollectingFrame / animationFPS) * horizontalSpeed;
			if (DistanceBetweenTwoVector2(ball, fielder10) < fielder10BallCollectingTriggerDistance)
			{
				canTakeRun = false;
				distanceBtwBallAndCollectingPlayerWhileThrowing = 10000; // maximum distance
				if (GameModelScript != null)
				{
					GameModelScript.EnableRun(false);
				}

				if (runOut == true)
				{
					/*fielder10.GetComponent<Animation>()*/
					fielder10AnimationComponent.Play("collectAndStump");
					/*fielder10.GetComponent<Animation>()*/
					fielder10AnimationComponent.PlayQueued("appeal");
					fielder10Action = "collectTheThrowAndStump";
				}
				else if (runOut == false)
				{
					/*fielder10.GetComponent<Animation>()*/
					fielder10AnimationComponent.Play("collectAndStand");
					fielder10Action = "collectTheThrow";
				}
			}

		}
		else if (fielder10Action == "collectTheThrow" || fielder10Action == "collectTheThrowAndStump")
		{
			float distanceBtwBallAndFielder10 = DistanceBetweenTwoVector2(ball, fielder10);
			if (distanceBtwBallAndCollectingPlayerWhileThrowing > distanceBtwBallAndFielder10)
			{
				distanceBtwBallAndCollectingPlayerWhileThrowing = distanceBtwBallAndFielder10;
			}
			else
			{
				distanceBtwBallAndCollectingPlayerWhileThrowing = -1; // when pass the wicketkeeper...
			}
			if (distanceBtwBallAndFielder10 < 1 || distanceBtwBallAndCollectingPlayerWhileThrowing == -1)
			//		if(DistanceBetweenTwoVector2(ball, fielder10) < 1)
			{
				ballStatus = "";
				pauseTheBall = true;
				fielder10Ball.GetComponent<Renderer>().enabled = true;
				ShowBall(false);
				if (fielder10Action == "collectTheThrow")
				{
					stayStartTime = Time.time;
					fielder10Action = "end";
				}
				else if (fielder10Action == "collectTheThrowAndStump")
				{
					float fielder10StumpingFrame = 10;
					fielder10.transform.LookAt(stump2Spot.transform);
					if (/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent["collectAndStump"].time > (fielder10StumpingFrame / animationFPS))
					{
						isRunOut = runOut;
						stayStartTime = Time.time;
						fielder10Action = "runOutAppeal";

						float appealingAngle = 270;
						iTween.RotateTo(fielder10, iTween.Hash("y", appealingAngle, "time", 0.5, "delay", 0.2));
						/*
					if(postBattingStumpFielderDirection == "straight")
					{
						stump2.animation.Play("legSideStumping");
					}
					else if(postBattingStumpFielderDirection == "straightDown")
					{
						stump2.animation.Play("offSideStumping");
					}
					else if(postBattingStumpFielderDirection == "offSide")
					{
						stump2.animation.Play("fielderRunoutAway");
					}
					else if(postBattingStumpFielderDirection == "legSide")
					{
						stump2.animation.Play("fielderRunoutIn");
					}
					*/
					}
				}
			}
		}
		else if (fielder10Action == "end")
		{
			if (stayStartTime + 1 + timeBetweenBalls < Time.time)
			{
				//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
				fielder10Action = "";
				if (GameModelScript != null)
				{
					//{STANLEY
					if (noBall == true)
					{
						GameModelScript.UpdateCurrentBall(0, 1, currentBallNoOfRuns, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
					}
					else if (freeHit == true)
					{
						GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
					}
					else if (noBall == false && freeHit == false)
					{
						GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
					}//STANLEY}
				}
				//			action = 10;
			}
		}
		else if (fielder10Action == "runOutAppeal")
		{
			if (stayStartTime + 1 + timeBetweenBalls < Time.time)
			{
				stayStartTime = Time.time;
				fielder10Action = "waitForResult";
				mainCamera.enabled = true;
				rightSideCamera.enabled = false;
				leftSideCamera.enabled = false;
				straightCamera.enabled = false;
				if (isRunOut == true)
				{
					if (GameModelScript != null)
					{
						//interfaceConnector.PlayGameSound ("Cheer"); 
					}
					waitForCommentary = true;
				}
				else
				{
					AudioPlayer.instance.PlayTheCrowdSound("BallMissingCrowd");
				}
			}
		}
		else if (fielder10Action == "waitForResult")
		{
			if (isRunOut == true && waitForCommentary == true && stayStartTime + 1 + timeBetweenBalls < Time.time)
			{
				waitForCommentary = false;
			}
			if (stayStartTime + 3 + timeBetweenBalls < Time.time)
			{
				if (isRunOut == true)
				{
					int batsmanOutIndex;
					if (currentBallNoOfRuns % 2 == 0)
					{
						batsmanOutIndex = CONTROLLER.StrikerIndex;
					}
					else
					{
						batsmanOutIndex = CONTROLLER.NonStrikerIndex;
					}
					//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
					fielder10Action = "";
					if (GameModelScript != null)
					{
						//{STANLEY
						if (noBall == true)
						{
							GameModelScript.UpdateCurrentBall(0, 1, currentBallNoOfRuns, 1, CONTROLLER.StrikerIndex, 1, 4, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if (freeHit == true)
						{
							GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 1, CONTROLLER.StrikerIndex, 1, 4, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if (noBall == false && freeHit == false)
						{
							GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 1, 4, CONTROLLER.CurrentBowlerIndex, 1, batsmanOutIndex, false);
						}
					}
				}
				else
				{
					fielder10Action = "";
					if (GameModelScript != null)
					{
						if (noBall == true)
						{
							GameModelScript.UpdateCurrentBall(0, 1, currentBallNoOfRuns, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if (freeHit == true)
						{
							GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if (noBall == false && freeHit == false)
						{
							GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}//STANLEY}
					}
				}

				//			action = 10;
			}
		}
		//// LBW appeal ////
		else if (fielder10Action == "lbwAppeal")
		{
			if (stayStartTime + 0.5 + timeBetweenBalls < Time.time)//2013
			{
				//ResetFielders ();
				if (GameModelScript != null)
				{
					GameModelScript.PlayGameSound("lbwpad");
				}
				stayStartTime = Time.time;
				fielder10Action = "waitForLbwResult";
				//mainCamera.enabled = true;
				rightSideCamera.enabled = false;
				leftSideCamera.enabled = false;
				straightCamera.enabled = false;
				if (LBW == true)
				{
					if (noBall == true || freeHit == true)
					{
						waitForCommentary = true;
					}
					if (noBall == false && freeHit == false)
					{
						mainUmpire.GetComponent<Animation>().Play("out2");//2013
						AudioPlayer.instance.PlayTheCrowdSound("lbwoutcrowd");
					}
				}
				else
				{
					mainUmpire.GetComponent<Animation>().CrossFade("notOut");//2013
					AudioPlayer.instance.PlayTheCrowdSound("lbwnotoutcrowd");
					StartCoroutine(_wait(1f));
					AnimationScreen.instance.StartAnimation(6);
					stayStartTime = Time.time;
				}
			}
		}
		else if (fielder10Action == "waitForLbwResult")
		{
			if (stayStartTime + 1.5 + timeBetweenBalls < Time.time)//2013
			{
				if (LBW == true)
				{
					fielder10Action = "";
					if (GameModelScript != null)
					{
						GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 1, 2, CONTROLLER.CurrentBowlerIndex, 0, CONTROLLER.StrikerIndex, false);
					}
				}
				else
				{
					fielder10Action = "";
					action = 10;
					if (GameModelScript != null)
					{
						GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);//Go for notout animation here
					}
				}
			}
		}
		//// LBW appeal ////
	}


	public void EnableFielder10ToCollectBall ()
	{
		/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent.Play ("wait2Collect");
		fielder10.transform.LookAt(new  Vector3(ballTransform.position.x, 0f, ballTransform.position.z));
		fielder10Action = "waitForBall";
	}


	public void ActivateFielders ()
	{
		int i;
		GameObject thisFielder;
		GameObject thisFielderBall;
		GameObject thisFielderChasePoint;
		float thisFielderAngle;

		for(i = 0; i < activeFielderNumber.Count ; i++)
		{
			thisFielder = fielder[activeFielderNumber[i]];
			thisFielderBall = fielderBall[activeFielderNumber[i]];
			thisFielderChasePoint = fielderChasePoint[activeFielderNumber[i]];

			// to avoid two or more fielders colliding eachother...
			if(activeFielderNumber.Count  > 1)
			{
				if(activeFielderAction[i] == "goForCatch" || 
					activeFielderAction[i] == "goForChase" || 
					activeFielderAction[i] == "waitToCatch")
				{
					for (int f1 = 0; f1 < activeFielderNumber.Count - 1; f1++)
					{
						GameObject mainFielder = fielder [activeFielderNumber [f1]];
						for (int f2 = (f1 + 1); f2 < activeFielderNumber.Count ; f2++)
						{
							GameObject otherFielder = fielder [activeFielderNumber [f2]];
							if(DistanceBetweenTwoVector2(mainFielder, otherFielder) < 3)
							{
								if(DistanceBetweenTwoGameObjects (mainFielder, ballCatchingSpot) < 
									DistanceBetweenTwoGameObjects (otherFielder, ballCatchingSpot))
								{
									if(activeFielderAction[f2] == "goForCatch" || activeFielderAction[f2] == "goForChase")  {
										activeFielderAction[f2] = "waitAndSeeTheCatch"; // catch/collect by other fielder...
										otherFielder.GetComponent<Animation>().Play ("idle");
									}
								}
								else 
								{
									if(activeFielderAction[f1] == "goForCatch" || activeFielderAction[f1] == "goForChase")  {
										activeFielderAction[f1] = "waitAndSeeTheCatch"; // catch/collect by other fielder...
										mainFielder.GetComponent<Animation>().Play ("idle");
									}
								}
							}

						}
					}
				}			
			}


			if(activeFielderAction[i] == "goForCatch")
			{
				if(DistanceBetweenTwoGameObjects (thisFielder, ballCatchingSpot) > 0.17) // half feet..
				{
					if(fielderToCatchTheBall == thisFielder ||
						(fielderToCatchTheBall != thisFielder && DistanceBetweenTwoGameObjects (thisFielder, ballCatchingSpot) > 3)) 
						// 3 meters away from the catching fielder ...
						// to avoid fielders collision while catching ...
					{
						thisFielderAngle = AngleBetweenTwoGameObjects (thisFielder, ballCatchingSpot);
						thisFielder.transform.position = new Vector3 (thisFielder.transform.position.x+(Mathf.Cos (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime), thisFielder.transform.position.y, thisFielder.transform.position.z+(Mathf.Sin (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime));
						thisFielder.transform.LookAt (ballCatchingSpot.transform);
					}
					else
					{
						activeFielderAction[i] = "waitAndSeeTheCatch"; // catch by other fielder...
						thisFielder.transform.LookAt (ballCatchingSpot.transform);
						thisFielder.GetComponent<Animation>().Play ("idle");
					}
				}
				else // near the catching spot ...
				{
					activeFielderAction[i] = "waitToCatch";
					thisFielder.transform.LookAt (ballTimingOrigin.transform);
					thisFielder.GetComponent<Animation>().Play ("idle");
				}
			}
			else if(activeFielderAction[i] == "waitToCatch")
			{
				float catchingFrame = 7.0f;
				float distanceToTriggerCatchAnimation = catchingFrame / animationFPS * horizontalSpeed;

				if(DistanceBetweenTwoVector2(thisFielder, ball) < distanceToTriggerCatchAnimation)
				{
					float preDistance = ballPreCatchingDistance / horizontalSpeed;
					float preProjectileAngle = preDistance * ballProjectileAnglePerSecond;
					float angleAtCatchingSpot = 360 - preProjectileAngle;
					float ballHeightAtCatchingSpot = Mathf.Abs (ballProjectileHeight * Mathf.Sin (angleAtCatchingSpot * DEG2RAD));

					if(ballHeightAtCatchingSpot < 0.33) // one feet..
					{
						thisFielder.GetComponent<Animation>().CrossFade ("lowCatch");
					}
					else if(ballHeightAtCatchingSpot < 1.33) // four feets..
					{
						thisFielder.GetComponent<Animation>().CrossFade ("hipCatch");
					}
					else if(ballHeightAtCatchingSpot < 2) // six feets..
					{
						thisFielder.GetComponent<Animation>().CrossFade ("sideCatch");
					}
					else if(ballHeightAtCatchingSpot < 2.5) // 7.5 feets..
					{
						thisFielder.GetComponent<Animation>().CrossFade ("highCatch");
					}
					activeFielderAction[i] = "catchAttempt";
				}
			}
			else if(activeFielderAction[i] == "catchAttempt")
			{
				if(DistanceBetweenTwoVector2(thisFielder, ball) < 0.5) // half meter
				{
					if(ballTransform.position.y < 2.5) // ball height should be less than 2.5 meters to make successful catch...
					{
						canTakeRun = false;

						if(GameModelScript != null)
						{
							GameModelScript.EnableRun (false);
						}

						ShowBall (false);
						thisFielderBall.GetComponent<Renderer>().enabled = true;

						//{STANLEY
						if(noBall == true || freeHit == true)
						{
							//thisFielder.animation.PlayQueued("idle", QueueMode.CompleteOthers);	
							thisFielder.GetComponent<Animation>().Play("throw");		
							activeFielderAction[i] = "waitToPick";				
						}
						else
						{
							if(GameModelScript != null)
							{
                                AudioPlayer.instance.StopBallTravelSound();
                                AudioPlayer.instance.PlayTheCrowdSound("catchcrowd");
							}
							int randomNumber = Random.Range (0, 3);
							if (randomNumber == 0){
								thisFielder.GetComponent<Animation>().PlayQueued("celebration", QueueMode.CompleteOthers);
							}
							else if (randomNumber == 1) {
								thisFielder.GetComponent<Animation>().PlayQueued("celebration2", QueueMode.CompleteOthers);
							}
							else {
								thisFielder.GetComponent<Animation>().PlayQueued("appeal", QueueMode.CompleteOthers);
							}
							//thisFielder.animation.PlayQueued("celebrationRun", QueueMode.CompleteOthers);			
							//activeFielderAction[i] = "catched";//Shankar Commented
							DisableTrail ();
							fielder10Action = "";
							/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent.Play("appeal");
							stayStartTime = Time.time;
							activeFielderAction[i] = "waitForResult";
							ballResult = "wicket";
							makeFieldersToCelebrate (thisFielder);
						}
						pauseTheBall = true;
						stopTheFielders = true;
						int  catchCommentaryIndex ;
						if(ballTransform.position.y >= 2.0)
						{
							catchCommentaryIndex = Random.Range(0, 2);
						}
						else
						{
							catchCommentaryIndex = Random.Range(2, 6);
						}
					}
					else // ball is too high... so just attempt a catch with 180 degree rotation to watch the ball...
					{
						if(ballTransform.position.y < 4)
						{
							thisFielder.GetComponent<Animation>().Play ("highCatch");
							thisFielder.GetComponent<Animation>()["highCatch"].speed = 2.0f;
						}
						iTween.RotateTo (thisFielder, iTween.Hash ("y", (thisFielder.transform.eulerAngles.y + 135), "time", 2));
						activeFielderAction[i] = "waitingTooHighBall";
					}
				}
			}
			else if(activeFielderAction[i] == "catched")
			{
				if (thisFielder.GetComponent<Animation>().IsPlaying("celebrationRun") == true)
				{
					stayStartTime = Time.time;
					activeFielderAction[i] = "celebrationRun";
				}
			}
			else if(activeFielderAction[i] == "celebrationRun")
			{
				thisFielderAngle = AngleBetweenTwoGameObjects (thisFielder, ballTimingOrigin);
				thisFielder.transform.position = new Vector3 (thisFielder.transform.position.x+(Mathf.Cos (thisFielderAngle * DEG2RAD) * fielderSpeed / 1.5f * Time.deltaTime), thisFielder.transform.position.y,thisFielder.transform.position.z+ (Mathf.Sin (thisFielderAngle * DEG2RAD) * fielderSpeed / 1.5f * Time.deltaTime));

				if(stayStartTime - 0.5 + timeBetweenBalls < Time.time)
				{
					ResetFielders ();
					stayStartTime = Time.time;
					activeFielderAction[i] = "waitForResult";
					rightSideCamera.enabled = false;
					leftSideCamera.enabled = false;
					straightCamera.enabled = false;
				}
			}
			else if(activeFielderAction[i] == "waitForResult")
			{
				if(stayStartTime + 1 + timeBetweenBalls < Time.time)//Shankar commented = 2 seconds
				{
					//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
					activeFielderAction[i] = "";
					ballOnboundaryLine = true;//ShankarEdit/*To Avoid Camera pan on halt*/
					if(GameModelScript != null)
					{
						if(noBall == true) // CATCH
						{
							GameModelScript.UpdateCurrentBall(0, 1, 0, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, activeFielderNumber[i], CONTROLLER.StrikerIndex, false);
						}
						else if(freeHit == true)
						{
							GameModelScript.UpdateCurrentBall(1, 1, 0, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, activeFielderNumber[i], CONTROLLER.StrikerIndex, false);
						}
						else if(noBall == false && freeHit == false)
						{
							GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 1, 3, CONTROLLER.CurrentBowlerIndex, activeFielderNumber[i], CONTROLLER.StrikerIndex, false);				
						}
					}
					// consider for catcherID, currently it is 5
				}		
			}

			else if(activeFielderAction[i] == "goForChase")
			{
				if(ballOnboundaryLine == true || stopTheFielders == true)
				{
					if(ballResult != "wicket")
					{
						if(thisFielder.GetComponent<Animation>().IsPlaying ("runComplete") == false)
						{
							thisFielder.GetComponent<Animation>().Play("runComplete");
						}
						fielderSpeed -= fielderSpeed * Time.deltaTime / 2;
						thisFielderAngle = AngleBetweenTwoGameObjects (thisFielder, ball);
						thisFielder.transform.position = new Vector3 (thisFielder.transform.position.x+(Mathf.Cos (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime), thisFielder.transform.position.y, thisFielder.transform.position.z+(Mathf.Sin (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime));
						if (ballOnboundaryLine != true)
						{
							thisFielder.transform.LookAt (new  Vector3(ballTransform.position.x, 0f, ballTransform.position.z));
						}
						float runCompleteTime = thisFielder.GetComponent<Animation> () ["runComplete"].time;		//runComplete
						float currentFrame = runCompleteTime * animationFPS;
						if(currentFrame > 50 || DistanceBetweenTwoVector2 (groundCenterPoint, thisFielder) > 68)//(70) 6th march 2013
						{
							thisFielder.GetComponent<Animation>().CrossFade("idle");
							activeFielderAction[i] = "stopChasing";
						}
					}
				}
				else if(DistanceBetweenTwoGameObjects (ballTimingOrigin, ball) > DistanceBetweenTwoGameObjects (ballTimingOrigin, thisFielderChasePoint))
				{
					thisFielderAngle = AngleBetweenTwoGameObjects (thisFielder, ball);
					thisFielder.transform.position = new Vector3 (thisFielder.transform.position.x+(Mathf.Cos (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime), thisFielder.transform.position.y,thisFielder.transform.position.z+ (Mathf.Sin (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime));
					thisFielder.transform.LookAt (new  Vector3(ballTransform.position.x, 0, ballTransform.position.z));
				}
				else if(DistanceBetweenTwoGameObjects (thisFielder, thisFielderChasePoint) > 0.17) // 0.17 half feet..
				{
					// fielder diving...
					float fielderChasePointDistance = DistanceBetweenTwoGameObjects (thisFielder, thisFielderChasePoint);
					float fielderTimeToReachChasePoint = fielderChasePointDistance / fielderSpeed;
					float ballTimeToReachChasePoint = DistanceBetweenTwoGameObjects (ball, thisFielderChasePoint) / horizontalSpeed;
					float fielderDistanceFromGroundCenter = DistanceBetweenTwoVector2 (groundCenterPoint, thisFielder);

					if(fielderToCatchTheBall != thisFielder && fielderChasePointDistance <= 4 && fielderChasePointDistance >= 2 &&
						DistanceBetweenTwoGameObjects (ballTimingOrigin, ball) < DistanceBetweenTwoGameObjects (ballTimingOrigin, thisFielderChasePoint) &&
						ballTimeToReachChasePoint < fielderTimeToReachChasePoint && ballTransform.position.y < 0.5 &&
						fielderDistanceFromGroundCenter > (groundRadius - 5))
					{
						activeFielderAction[i] = "diveToField";
						thisFielder.GetComponent<Animation>().Play ("diveStraight");
					}
					else if(fielderToCatchTheBall != thisFielder && DistanceBetweenTwoGameObjects (thisFielder, ballCatchingSpot) > 3)
					{
						thisFielderAngle = AngleBetweenTwoGameObjects (thisFielder, thisFielderChasePoint);
						thisFielder.transform.position = new Vector3 (thisFielder.transform.position.x+(Mathf.Cos (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime), thisFielder.transform.position.y,thisFielder.transform.position.z+ (Mathf.Sin (thisFielderAngle * DEG2RAD) * fielderSpeed * Time.deltaTime));
						thisFielder.transform.LookAt (thisFielderChasePoint.transform);
					}
					else
					{
						activeFielderAction[i] = "waitAndSeeTheCatch"; // catch by other fielder...
						thisFielder.GetComponent<Animation>().Play ("idle");
					}
				}
				else // wait to pick the ball...
				{
					thisFielder.GetComponent<Animation>().Play("idle");
					thisFielder.transform.LookAt (ballTimingOrigin.transform);
					activeFielderAction[i] = "waitToPick";
				}
			}
			else if(activeFielderAction[i] == "diveToField")
			{
				float maxDivingFrames = 15;
				if(thisFielder.GetComponent<Animation>().IsPlaying("diveStraight") == true && thisFielder.GetComponent<Animation>()["diveStraight"].time < (maxDivingFrames/animationFPS))
				{
					thisFielderAngle = AngleBetweenTwoGameObjects (thisFielder, thisFielderChasePoint);
					thisFielder.transform.position += new Vector3 ((Mathf.Cos (thisFielderAngle * DEG2RAD) * fielderSpeed / 1.5f * Time.deltaTime), 0, (Mathf.Sin (thisFielderAngle * DEG2RAD) * fielderSpeed / 1.5f * Time.deltaTime));
				}
				else
				{
					activeFielderAction[i] = "diveEnd";
				}
			}
			else if(activeFielderAction[i] == "waitToPick")
			{
				float pickingupFrame = 6;//4.5;//9.0;
				float distanceToTriggerPickupAnimation = pickingupFrame / animationFPS * horizontalSpeed;

				if(DistanceBetweenTwoVector2(thisFielder, ball) < distanceToTriggerPickupAnimation)
				{
					if(ballTransform.position.y < 0.33) // one feet...
					{
						pickingupAnimationToPlay = "lowCatch";
					}
					else if(ballTransform.position.y < 1.33) // four/six feets..
					{
						pickingupAnimationToPlay = "hipCatch";
					}
					else if(ballTransform.position.y < 2) // six feets..
					{
						pickingupAnimationToPlay = "sideCatch";
					}
					else// if(ball.transform.position.y < 2.5) // 7.5 feets..
					{
						pickingupAnimationToPlay = "highCatch";
					}

					thisFielder.GetComponent<Animation>().Play(pickingupAnimationToPlay);
					thisFielder.GetComponent<Animation>()[pickingupAnimationToPlay].speed = 2;
					thisFielder.transform.LookAt (ballTimingOrigin.transform);
					activeFielderAction[i] = "pickupAttempt";
				}
			}
			else if(activeFielderAction[i] == "pickupAttempt")
			{//lll
				if(DistanceBetweenTwoVector2(thisFielder, ball) < .85) // 2 feets
				{
					if(ballTransform.position.y < 2.5) // ball height should be less than 2.5 meters to make successful catch...
					{
						fielderCollectedTheBall = true;
						ShowBall (false);
						thisFielderBall.GetComponent<Renderer>().enabled = true;
						if(DistanceBetweenTwoVector2(groundCenterPoint, thisFielder) > 20 || takingRun == true) // OR running between the wicket is happening...
						{
							//thisFielder.animation.CrossFade("throw", 0.3);
							//activeFielderAction[i] = "pickedup";// Shankar Commented

							if (pickingupAnimationToPlay != "lowCatch")
							{
								pickingupAnimationToPlay = "";
								BallPickTime = Time.time;
								//thisFielder.animation.CrossFade("throw", 0.3);//Shankar Commented
								//thisFielder.animation.CrossFade("idle", 0.3);
								activeFielderAction[i] = "end";
								stayStartTime = Time.time;
								//thisFielder.animation.Play("idle");
							}
							else
							{
								float currAnimTime = thisFielder.GetComponent<Animation> () [pickingupAnimationToPlay].time;
								float currFrame = currAnimTime * animationFPS;
								if(currFrame > 10)
								{
									pickingupAnimationToPlay = "";
									BallPickTime = Time.time;
									//thisFielder.animation.CrossFade("throw", 0.3);//Shankar Commented
									//thisFielder.animation.CrossFade("idle", 0.3);
									activeFielderAction[i] = "end";
									stayStartTime = Time.time;
									//thisFielder.animation.Play("idle");
								}
							}
						}
						else
						{
							// go to next ball...
							canTakeRun = false;

							if(GameModelScript != null)
							{
								GameModelScript.EnableRun (false);
							}

							pickingupAnimationToPlay = "";
							BallPickTime = Time.time;

							stayStartTime = Time.time;
							activeFielderAction[i] = "end";
						}

						fielderThrowElapsedTime = 0;
						//throw...
						pauseTheBall = true;
						stopTheFielders = true;
					}
				}
				else if(DistanceBetweenTwoVector2 (thisFielder, ball) > 10)
				{
					foreach (AnimationState state in thisFielder.GetComponent<Animation>())
					{
						state.speed = 1;
					}
					activeFielderAction[i] = "goForChase";
					thisFielder.GetComponent<Animation>().Play("run");
				}
			}
			else if(activeFielderAction[i] == "pickedup")
			{
				float fielderThrowReleaseFrame = 15;
				float fielderThrowReleaseTime = fielderThrowReleaseFrame / animationFPS;
				if (thisFielder.GetComponent<Animation>().IsPlaying("throw") == true)
				{
					if(DistanceBetweenTwoVector2 (thisFielder, fielder10) < DistanceBetweenTwoVector2 (thisFielder, wicketKeeper))
					{
						thisFielder.transform.LookAt(fielder10.transform);
					}

					fielderThrowElapsedTime += Time.deltaTime;
					if(fielderThrowElapsedTime >= fielderThrowReleaseTime)
					{
						thisFielderBall.GetComponent<Renderer>().enabled = false;
						//ShowBall(true);//2013
						float throwStartingHeight = 2.0f;
						string fielderName = thisFielder.name;//ShankarEdit
						GameObject FielderBallReleasePointGO;
						FielderBallReleasePointGO = GameObject.Find (fielderName+"/Armature/Bone/hand_r/fms_r/ballRef");
						FielderBallReleasePointGO.transform.position= new Vector3 (FielderBallReleasePointGO.transform.position.x, throwStartingHeight, FielderBallReleasePointGO.transform.position.z);
						ballTransform.position = FielderBallReleasePointGO.transform.position;

						GameObject throwTo = null;
						float throwLength = 0;
						// Fielder10 is shortest throwing distance
						if(DistanceBetweenTwoVector2 (thisFielder, fielder10) < DistanceBetweenTwoVector2 (thisFielder, wicketKeeper))
							//					if(2 < 1)
						{
							if(postBattingStumpFielderDirection == "straight")
							{
								throwTo = fielderStraightBallStumping;
							}
							else if(postBattingStumpFielderDirection == "straightDown")
							{
								throwTo = stump2Crease;
							}
							else if(postBattingStumpFielderDirection == "offSide")
							{
								throwTo = fielderOffSideBallStumping;
							}
							else if(postBattingStumpFielderDirection == "legSide")
							{
								throwTo = fielderLegSideBallStumping;
							}
							fielder10Action = "waitToCollect";
							fielder10.transform.LookAt(thisFielder.transform);
							/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.CrossFade ("idle");
							// +1 meter more length to match the catching height..
							throwLength = DistanceBetweenTwoGameObjects (thisFielder, throwTo) + 1;
							// for one bounce throw...
							if(throwLength > 15 && Random.Range(1, 10) <= 5)
							{
								throwingFirstBounceDistance = Random.Range (5, throwLength/2);
								throwLength -= throwingFirstBounceDistance;
							}
							// for one bounce throw...
						}
						else
						{
							if(postBattingWicketKeeperDirection == "straight")
							{
								throwTo = wicketKeeperStraightBallStumping;
							}
							else if(postBattingWicketKeeperDirection == "offSide")
							{
								throwTo = wicketKeeperOffSideBallStumping;
							}
							else if(postBattingWicketKeeperDirection == "legSide")
							{
								throwTo = wicketKeeperLegSideBallStumping;
							}
							wicketKeeperStatus = "waitToCollect";
							wicketKeeper.transform.LookAt(thisFielder.transform);
							/*fielder10.GetComponent<Animation>()*/fielder10AnimationComponent.CrossFade ("idle");
							// +1 meter more length to match the catching height..
							throwLength = DistanceBetweenTwoGameObjects (thisFielder, throwTo);
							// for one bounce throw...
							if(throwLength > 15 && Random.Range(1, 10) <= 5)
							{
								throwingFirstBounceDistance = Random.Range (5, throwLength/2);
								throwLength -= throwingFirstBounceDistance;
							}
							// for one bounce throw...
						}

						ballAngle = AngleBetweenTwoGameObjects (FielderBallReleasePointGO, throwTo);
						horizontalSpeed = 24;
						ballProjectileHeight = throwLength / 6; // 8

						float ballThrowingStartingAngle = Mathf.Asin (throwStartingHeight / ballProjectileHeight) * RAD2DEG;
						// to avoid NaN...
						if(throwStartingHeight >= ballProjectileHeight)
						{
							ballThrowingStartingAngle = 90;
							ballProjectileHeight = throwStartingHeight;
						}

						ballProjectileAngle = 180 + ballThrowingStartingAngle;
						ballProjectileAnglePerSecond = ((180 - ballThrowingStartingAngle)/throwLength) * horizontalSpeed;
						pauseTheBall = false;
						ballStatus = "throw";
						activeFielderAction[i] = "throw";
					}
				}
			}
			else if(activeFielderAction[i] == "throw")
			{

			}
			else if(activeFielderAction[i] == "end")
			{
				//if (CONTROLLER .gameMode =="multiplayer"&& (shotPlayed == "DownTheTrackDefensiveShot" || shotPlayed == "FrontFootDefense" || shotPlayed == "BackFootDefense") )
				//		return;
				if (stayStartTime + 0.2 + timeBetweenBalls < Time.time)
				{
					fielderCollectedTheBall = true;

					//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
					activeFielderAction[i] = "";
					ballOnboundaryLine = true;//ShankarEdit/*To Avoid Camera pan on halt*/
					if(GameModelScript != null)
					{
						/*var ballInGround : int = DistanceBetweenTwoVector2 (groundCenterPoint, ball);
					if (ballInGround > 25 && ballInGround < 50)
					{
						currentBallNoOfRuns = 1;
					}
					else if (ballInGround > 50)
					{
						currentBallNoOfRuns = 2;
					}*/					
						float decidedRun = BallPickTime - BallHitTime;

						if (decidedRun > 2 && decidedRun < 3)
						{
							currentBallNoOfRuns = 1;
						}
						else if (decidedRun > 3)
						{
							currentBallNoOfRuns = 2;
						}

						//0036652
						if (shotPlayed == "DownTheTrackDefensiveShot" || shotPlayed == "FrontFootDefense" || shotPlayed == "BackFootDefense")
							currentBallNoOfRuns = 0;

						GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
					}
				}
			}

		}
	}


	public float DistanceBetweenTwoVector2 (GameObject go1, GameObject go2)
	{
		float xDiff = go1.transform.position.x - go2.transform.position.x;
		float zDiff = go1.transform.position.z - go2.transform.position.z;

		float distance = Mathf.Sqrt (xDiff * xDiff + zDiff * zDiff);
		return distance;
	}

	public float DistanceBetweenTwoGameObjects (GameObject go1, GameObject go2)
	{
		float distance = Vector3.Distance (go1.transform.position, go2.transform.position);
		return distance;
	}

	public float AngleBetweenTwoGameObjects (GameObject go1, GameObject go2)
	{
		float xDiff = go1.transform.position.x - go2.transform.position.x;
		float zDiff = go1.transform.position.z - go2.transform.position.z;
		float angle = Mathf.Atan2 (xDiff, zDiff) * RAD2DEG;
		angle = ((270 - angle) + 360) % 360;
		return angle;
	}

	public float AngleBetweenTwoVector3 (Vector3 v1, Vector3 v2)
	{
		float xDiff = v1.x - v2.x;
		float zDiff = v1.z - v2.z;
		float angle = Mathf.Atan2 (xDiff, zDiff) * RAD2DEG;
		angle = ((270 - angle) + 360) % 360;
		return angle;
	}


	public void  FindNewBowlingSpot ()
	{
		bowlingSpotScript.ShowBowlingSpot ();
		//bowlingSpotScript.HideBowlingSpot ();
		if(bowlingBy == "computer")
		{
			if(currentBatsmanHand == "right")
			{
				if (CONTROLLER.gameMode == "multiplayer")
				{
					bowlingSpotGO.transform.position = Multiplayer.oversData [CONTROLLER.currentMatchBalls / 6].bowlingSpotR [CONTROLLER.currentMatchBalls % 6];
				}
				else
					bowlingSpotGO.transform.position = new Vector3 (Random.Range (RHBatsmanMaxBowlLimitGO.transform.position.x, RHBatsmanMinBowlLimitGO.transform.position.x), bowlingSpotGO.transform.position.y, Random.Range (RHBatsmanMaxBowlLimitGO.transform.position.z, RHBatsmanMinBowlLimitGO.transform.position.z));
				if(bowlerSide == "right") 
				{
					//bowlingSpotGO.transform.position.x += 0.5;
				}
			}
			else if(currentBatsmanHand == "left")
			{
				if (CONTROLLER.gameMode == "multiplayer") 
				{
					bowlingSpotGO.transform.position = Multiplayer.oversData [CONTROLLER.currentMatchBalls / 6].bowlingSpotL [CONTROLLER.currentMatchBalls % 6];
				} 
				else 
					bowlingSpotGO.transform.position = new Vector3 (Random.Range (LHBatsmanMaxBowlLimitGO.transform.position.x, LHBatsmanMinBowlLimitGO.transform.position.x), bowlingSpotGO.transform.position.y, Random.Range (LHBatsmanMaxBowlLimitGO.transform.position.z, LHBatsmanMinBowlLimitGO.transform.position.z));
																																							
			}
		}	
		else if(bowlingBy == "user")
		{
			bowlingSpotGO.transform.position = new Vector3 (Random.Range (userBowlingMinLimit.transform.position.x, userBowlingMaxLimit.transform.position.x), bowlingSpotGO.transform.position.y, Random.Range (userBowlingMaxLimit.transform.position.z, userBowlingMinLimit.transform.position.z));
			suggestedBowlingSpot = bowlingSpotGO.transform.position;
		}
		if (bowlingSpotGO.transform.position.z > 8.5)
		{
			bowlingSpotGO.transform.position = new Vector3 (bowlingSpotGO.transform.position.x, bowlingSpotGO.transform.position.y, 8.5f);
		}
		/*if (Application.isEditor == true)
	{
		bowlingSpotGO.transform.position.x = -0.1;
		bowlingSpotGO.transform.position.z = 8.5;
	}*/

		//testing 
//		bowlingSpotGO.transform.position=new Vector3 (0f,0f,9f);
	}

	public void UserChangingBowlingSpot ()
	{
		if(bowlingBy == "user" && userBowlerCanMoveBowlingSpot == true)
		{
			if(Input.GetKeyDown(KeyCode.UpArrow) == true){
				upArrowKeyDown = true;
			}
			if(Input.GetKeyDown(KeyCode.DownArrow) == true){
				downArrowKeyDown = true;
			}
			if(Input.GetKeyDown(KeyCode.LeftArrow) == true){
				leftArrowKeyDown = true;
			}
			if(Input.GetKeyDown(KeyCode.RightArrow) == true){
				rightArrowKeyDown = true;

			}

			if(Input.GetKeyUp(KeyCode.UpArrow) == true){
				upArrowKeyDown = false;
			}
			if(Input.GetKeyUp(KeyCode.DownArrow) == true){
				downArrowKeyDown = false;
			}
			if(Input.GetKeyUp(KeyCode.LeftArrow) == true){
				leftArrowKeyDown = false;
			}
			if(Input.GetKeyUp(KeyCode.RightArrow) == true){
				rightArrowKeyDown = false;
			}	

			if(Input.GetKeyDown(KeyCode.S) == true && userBowlingSpotSelected == false)
			{
				//userBowlingSpotSelected = true;
				//userBowlerCanMoveBowlingSpot = false;
				//bowlingSpotScript.FreezeBowlingSpot ();
			}
			else if(userBowlingSpotSelected == false)
			{
				float xSpeed = 2;
				float zSpeed = 5;
				/*Ultrabook*/
				if (Input.GetMouseButton(0))
				{
					if(prevMousePos.x == 0 && prevMousePos.y == 0)
					{
						prevMousePos = Input.mousePosition;
					}
					float maxDeltaMousePosition = 20;
					Vector2 mouseDeltaPosition = new Vector2 ((Input.mousePosition.x - prevMousePos.x), (Input.mousePosition.y - prevMousePos.y));
					prevMousePos = Input.mousePosition;

					if(mouseDeltaPosition.x < 0) {
						leftArrowKeyDown = true;
						xSpeed = 12 * Mathf.Abs(mouseDeltaPosition.x) /maxDeltaMousePosition;
					}
					else {
						leftArrowKeyDown = false;
						xSpeed = 12;
					}
					if(mouseDeltaPosition.x > 0) {
						rightArrowKeyDown = true;
						xSpeed = 12 * Mathf.Abs(mouseDeltaPosition.x) /maxDeltaMousePosition;
					}
					else {
						rightArrowKeyDown = false;
						xSpeed = 12;
					}
					if(mouseDeltaPosition.y > 0) {
						upArrowKeyDown = true;
						zSpeed = 20 * Mathf.Abs(mouseDeltaPosition.y) /maxDeltaMousePosition;
					}
					else {
						upArrowKeyDown = false;
						zSpeed = 20;
					}
					if(mouseDeltaPosition.y < 0) {
						downArrowKeyDown = true;
						zSpeed = 20 * Mathf.Abs(mouseDeltaPosition.y) /maxDeltaMousePosition;
					}
					else {
						downArrowKeyDown = false;
						zSpeed = 20;
					}
				}
				if(Input.GetMouseButtonUp(0))
				{
					leftArrowKeyDown = false;
					rightArrowKeyDown = false;
					upArrowKeyDown = false;
					downArrowKeyDown = false;
					prevMousePos = Vector2.zero;
				}
				/*Ultrabook*/
				if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
				{
					float maxDeltaPosition = 25;
					Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;

					if(touchDeltaPosition.x < 0) {
						leftArrowKeyDown = true;
						xSpeed = 2 * Mathf.Abs(touchDeltaPosition.x) /maxDeltaPosition;
					}
					else {
						leftArrowKeyDown = false;
						xSpeed = 2;
					}
					if(touchDeltaPosition.x > 0) {
						rightArrowKeyDown = true;
						xSpeed = 2 * Mathf.Abs(touchDeltaPosition.x) /maxDeltaPosition;
					}
					else {
						rightArrowKeyDown = false;
						xSpeed = 2;
					}
					if(touchDeltaPosition.y > 0) {
						upArrowKeyDown = true;
						zSpeed = 5 * Mathf.Abs(touchDeltaPosition.y) /maxDeltaPosition;
					}
					else {
						upArrowKeyDown = false;
						zSpeed = 5;
					}
					if(touchDeltaPosition.y < 0) {
						downArrowKeyDown = true;
						zSpeed = 5 * Mathf.Abs(touchDeltaPosition.y) /maxDeltaPosition;
					}
					else {
						downArrowKeyDown = false;
						zSpeed = 5;
					}

				}

				if(Application.isEditor == false)
				{
					#if UNITY_ANDROID || UNITY_IPHONE
					if(Input.touchCount == 0)
					{
					leftArrowKeyDown = false;
					rightArrowKeyDown = false;
					upArrowKeyDown = false;
					downArrowKeyDown = false;
					}
					#endif
				}

				if (leftArrowKeyDown == true) {
					bowlingSpotGO.transform.position = new Vector3 (bowlingSpotGO.transform.position.x - xSpeed * Time.deltaTime, bowlingSpotGO.transform.position.y, bowlingSpotGO.transform.position.z);
					if (bowlingSpotGO.transform.position.x < userBowlingMinLimit.transform.position.x) {
						bowlingSpotGO.transform.position = new Vector3 (userBowlingMinLimit.transform.position.x, bowlingSpotGO.transform.position.y, bowlingSpotGO.transform.position.z);
					}
				}
				if (rightArrowKeyDown == true) {
					bowlingSpotGO.transform.position = new Vector3 (bowlingSpotGO.transform.position.x + xSpeed * Time.deltaTime, bowlingSpotGO.transform.position.y, bowlingSpotGO.transform.position.z);

					if (bowlingSpotGO.transform.position.x > userBowlingMaxLimit.transform.position.x) {
						bowlingSpotGO.transform.position = new Vector3 (userBowlingMaxLimit.transform.position.x, bowlingSpotGO.transform.position.y, bowlingSpotGO.transform.position.z);					
					}
				}
				if (upArrowKeyDown == true) {
					bowlingSpotGO.transform.position = new Vector3 (bowlingSpotGO.transform.position.x, bowlingSpotGO.transform.position.y, bowlingSpotGO.transform.position.z + zSpeed * Time.deltaTime);
					if (bowlingSpotGO.transform.position.z > userBowlingMinLimit.transform.position.z) {
						bowlingSpotGO.transform.position = new Vector3 (bowlingSpotGO.transform.position.x, bowlingSpotGO.transform.position.y, userBowlingMinLimit.transform.position.z);
					}
				}
				if (downArrowKeyDown == true) {
					bowlingSpotGO.transform.position = new Vector3 (bowlingSpotGO.transform.position.x, bowlingSpotGO.transform.position.y, bowlingSpotGO.transform.position.z - zSpeed * Time.deltaTime);

					if (bowlingSpotGO.transform.position.z < userBowlingMaxLimit.transform.position.z) {
						bowlingSpotGO.transform.position = new Vector3 (bowlingSpotGO.transform.position.x, bowlingSpotGO.transform.position.y, userBowlingMaxLimit.transform.position.z);
					}
				}

				/*
			if(leftArrowKeyDown == false && rightArrowKeyDown == false && upArrowKeyDown == false && downArrowKeyDown == false)
			{
				bowlingSpotGO.transform.position.x -= (bowlingSpotGO.transform.position.x - suggestedBowlingSpot.x) * (xSpeed * Time.deltaTime);
				bowlingSpotGO.transform.position.z -= (bowlingSpotGO.transform.position.z - suggestedBowlingSpot.z) * (zSpeed * Time.deltaTime);
			}
			*/
			}

		}
	}




	public void FindBatsmanCanMakeShot ()
	{
		if (ballTransform.position.z > shotActivationMinLimit.transform.position.z &&
			ballTransform.position.z < shotActivationMaxLimit.transform.position.z &&
			batsmanTriggeredShot == false) {
			canMakeShot = true;
		}
	}

	public void BallMovement()
	{
		float xPos = Mathf.Cos(ballAngle * DEG2RAD) * horizontalSpeed * Time.deltaTime;
		float zPos = Mathf.Sin(ballAngle * DEG2RAD) * horizontalSpeed * Time.deltaTime;

		ballProjectileAngle += (ballProjectileAnglePerSecond * Time.deltaTime);
		if (ballProjectileAngle >= 360)
		{
			float prevAngle = ballProjectileAngle - (ballProjectileAnglePerSecond * Time.deltaTime);
			float diffAngle = ballProjectileAngle - prevAngle;
			float requiredAngle = 360 - prevAngle;
			float anglePercentage = requiredAngle / diffAngle;

			if (anglePercentage < 1 && ballStatus != "onPads" && horizontalSpeed > 10.0f) // chances of missing the exceeding the 360 degree at high speed...
			{
				float reducedHorizontalSpeed = horizontalSpeed * anglePercentage;
				xPos = Mathf.Cos(ballAngle * DEG2RAD) * reducedHorizontalSpeed * Time.deltaTime;
				zPos = Mathf.Sin(ballAngle * DEG2RAD) * reducedHorizontalSpeed * Time.deltaTime;
			}
			ballProjectileAngle = 360;
		}

		float projectileY = Mathf.Sin(ballProjectileAngle * DEG2RAD) * ballProjectileHeight - ballRadius;

		if (float.IsNaN(projectileY))
		{
			projectileY = 0;
		}

		ballTransform.position = new Vector3(ballTransform.position.x, -projectileY, ballTransform.position.z);
		ballTransform.position += new Vector3(xPos, 0, zPos);
		ballSkinTransform.localPosition = Vector3.zero;

		//karthik
		leftSwipe.transform.position = ballTransform.position;
		leftSwipe.transform.position = new Vector3(leftSwipe.transform.position.x, leftSwipe.transform.position.y - 0.25f, leftSwipe.transform.position.z);


		ballRayCastReferenceGOTransform.position = new Vector3(ballTransform.position.x, ballTransform.position.y, ballTransform.position.z);

		ballRayCastReferenceGOTransform.eulerAngles = new Vector3(ballRayCastReferenceGOTransform.eulerAngles.x, ((90 - ballAngle) + 360) % 360, ballRayCastReferenceGOTransform.eulerAngles.z);

		ballRot.Rotate(Vector3.right * Time.deltaTime * horizontalSpeed * 80, Space.Self);
	}
	/*public void BallMovement ()
	{
		float xPos = Mathf.Cos (ballAngle * DEG2RAD) * horizontalSpeed * Time.deltaTime;
		float zPos = Mathf.Sin (ballAngle * DEG2RAD) * horizontalSpeed * Time.deltaTime;
		float projectileY = Mathf.Sin (ballProjectileAngle * DEG2RAD) * ballProjectileHeight - ballRadius;

		ballTransform.position = new Vector3 ((ballTransform.position.x + xPos), -projectileY, (ballTransform.position.z+zPos));

		//karthik
		leftSwipe.transform.position = ballTransform.position;
		leftSwipe.transform.position = new Vector3 (leftSwipe.transform.position.x,leftSwipe.transform.position.y- 0.25f, leftSwipe.transform.position.z);

		ballProjectileAngle += (ballProjectileAnglePerSecond * Time.deltaTime);

		ballRayCastReferenceGOTransform.position = new Vector3 (ballTransform.position.x, ballTransform.position.y, ballTransform.position.z);
		//	ballRayCastReferenceGOTransform.position.y = ballTransform.position.y;
		//	ballRayCastReferenceGOTransform.position.z = ballTransform.position.z;

		ballRayCastReferenceGOTransform.eulerAngles = new Vector3 (ballRayCastReferenceGOTransform.eulerAngles.x, (((90 - ballAngle) + 360) % 360), ballRayCastReferenceGOTransform.eulerAngles.z);

		ballTransform.Rotate (Vector3.right * Time.deltaTime * ballSpinningSpeedInX, Space.World);	
		ballTransform.Rotate (Vector3.forward * Time.deltaTime * ballSpinningSpeedInZ, Space.World);
	}
	*/
	public void ActivateWicketKeeper ()
	{
		wicketKeeperIsActive = true;
		float xDiff = wicketKeeper.transform.position.x - ballTransform.position.x;
		float zDiff = wicketKeeper.transform.position.z - ballTransform.position.z;

		wicketKeeperAdjacentLength = Mathf.Sqrt (xDiff * xDiff + zDiff * zDiff);
		wicketKeeperThetaBtwAdjAndHyp = Mathf.Atan2 (xDiff, zDiff) * RAD2DEG - (90 - ballAngle);

		wicketKeeperHypotenuse = wicketKeeperAdjacentLength / Mathf.Cos (wicketKeeperThetaBtwAdjAndHyp * DEG2RAD);
		wicketKeeperOppositeLength = Mathf.Sqrt (wicketKeeperHypotenuse * wicketKeeperHypotenuse - wicketKeeperAdjacentLength * wicketKeeperAdjacentLength);
	}

	public void  WicketKeeperPreBattingActions ()
	{
		if(wicketKeeperIsActive == true)
		{
			float xDiff = wicketKeeper.transform.position.x - ballTransform.position.x;
			float zDiff = wicketKeeper.transform.position.z - ballTransform.position.z;
			float distanceBtwBallAndKeeper = Mathf.Sqrt (xDiff * xDiff + zDiff * zDiff);

			if(wicketKeeperCatchingAnimationSelected == false)
			{
				wicketKeeperCatchingAnimationSelected = true;
				float chestCatchFrame = 4;
				float shortCatchFrame = 6;
				float sideCatchFrame = 10;
				float spinHipCatchFrame = 4;
				float extremeSideCatchFrame = 8;

				if(wicketKeeperOppositeLength < 0.3) {
					wicketKeeperCatchingFrame = chestCatchFrame;
					wicketKeeperAnimationClip = "chestCatch";
					if(currentBowlerType == "spin")
					{
						if(DistanceBetweenTwoVector2(bowlingSpotGO, wicketKeeper) < 6) // 5 meters...
						{
							wicketKeeperCatchingFrame = spinHipCatchFrame;
							wicketKeeperAnimationClip = "spinHipCatch";
						}
					}
				}
				else {
					if(wicketKeeperThetaBtwAdjAndHyp > 0) // left
					{
						if(wicketKeeperOppositeLength < 0.8) // 0.6
						{
							wicketKeeperAnimationClip = "rightShortCatch";
							wicketKeeperCatchingFrame = shortCatchFrame;
						}
						else if(wicketKeeperOppositeLength < 1.2)
						{
							wicketKeeperAnimationClip = "rightSideCatch";
							wicketKeeperCatchingFrame = sideCatchFrame;
						}
						else
						{
							wicketKeeperAnimationClip = "extremeRightJump2";
							wicketKeeperCatchingFrame = extremeSideCatchFrame;
						}
					}
					else // right
					{
						if(wicketKeeperOppositeLength < 0.8) // 0.6
						{
							wicketKeeperAnimationClip = "leftShortCatch";
							wicketKeeperCatchingFrame = shortCatchFrame;
						}
						else if(wicketKeeperOppositeLength < 1.2)
						{
							wicketKeeperAnimationClip = "leftSideCatch";
							wicketKeeperCatchingFrame = sideCatchFrame;
						}
						else
						{
							wicketKeeperAnimationClip = "extremeLeftJump";
							wicketKeeperCatchingFrame = extremeSideCatchFrame;
						}
					}
				}
			}

			if((wicketKeeperStatus == "" || wicketKeeperStatus == "catchAttempt") && ballStatus == "bowled")
			{
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("appealFast");
				stayStartTime = Time.time;
				wicketKeeperStatus = "bowledEnd";
				waitForCommentary = true;
			}
			else if(distanceBtwBallAndKeeper < (wicketKeeperCatchingFrame/animationFPS * horizontalSpeed) && wicketKeeperStatus == "" ||
				(ballTransform.position.z - wicketKeeper.transform.position.z) > 5.0f && wicketKeeperStatus == "") // additional check to avoid freeze issue, if keeper misses
			{ 
				//runnerScript._playAnimation("Idle2");
				wicketKeeperAnimationComponent.CrossFade(wicketKeeperAnimationClip);
				wicketKeeperStatus = "catchAttempt";
				if (wicketKeeperAnimationClip == "chestCatch" || wicketKeeperAnimationClip == "spinHipCatch") {
					if (wicketKeeperThetaBtwAdjAndHyp > 0) {
						iTween.MoveTo (wicketKeeper, iTween.Hash ("x", (wicketKeeper.transform.position.x - wicketKeeperOppositeLength), "time", 0.2));
					} else {
						iTween.MoveTo (wicketKeeper, iTween.Hash ("x", (wicketKeeper.transform.position.x + wicketKeeperOppositeLength), "time", 0.2));
					}
				}
			}
			else if((Mathf.Abs(wicketKeeper.transform.position.z - ballTransform.position.z) < 0.5 || 
				wicketKeeper.transform.position.z < ballTransform.position.z) && 
				wicketKeeperStatus == "catchAttempt" && wicketKeeperOppositeLength <= wicketKeeprMaxCatchingDistance)
			{
				stayStartTime = Time.time;
				wicketKeeperStatus = "catchEnd";
				wicketKeeperBall.GetComponent<Renderer>().enabled = true;
				ShowBall (false);
				pauseTheBall = true;
				if(wideBall == false)
				{
					if(swingingBall == true)
					{
						if(Random.Range (0.0f,10.0f) < 7.0f) {
							GameModelScript.PlayCommentarySound ("Swing/A swinging delivery");
						}
						else {
							GameModelScript.PlayCommentarySound ("Swing/Leaves the batsman");
						}
					}
					else if(ballSpotLength <= 13)
					{
						if(GameModelScript != null)
						{
							GameModelScript.PlayCommentarySound ("WicketKeeper/Shortish delivery");
						}
					}
					else if(ballSpotLength > 13 && ballSpotLength < 15)
					{
						if(Random.Range(0, 1)  == 0)
						{
							if(GameModelScript != null)
							{
								GameModelScript.PlayCommentarySound ("WicketKeeper/Good length");
							}
						}
						else
						{
							if(GameModelScript != null)
							{
								GameModelScript.PlayCommentarySound ("WicketKeeper/Perfect length");
							}
						}
					}
					else if(ballSpotLength >= 15 && Mathf.Abs (ballSpotAtStump.transform.position.x) <= 0.3)
					{
						if(GameModelScript != null)
						{
							GameModelScript.PlayCommentarySound ("WicketKeeper/O that was a close");
						}
					}
				}
			}
			else if((Mathf.Abs(wicketKeeper.transform.position.z - ballTransform.position.z) < 0.5 || 
				wicketKeeper.transform.position.z < ballTransform.position.z) && 
				wicketKeeperStatus == "catchAttempt" && wicketKeeperOppositeLength > wicketKeeprMaxCatchingDistance)
			{
				wicketKeeperStatus = "catchMissed";//2013
				stayStartTime = Time.time;//2013
			}
			else if(wicketKeeperStatus == "bowledEnd") // wicket...
			{
				if(stayStartTime + 1 + timeBetweenBalls < Time.time)
				{
					wicketKeeperStatus = "loopEnd";
					if(GameModelScript != null)
					{
						//{STANLEY
						if(noBall == true)
						{
							GameModelScript.UpdateCurrentBall(0, 1, 0, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, CONTROLLER.StrikerIndex, false);
						}
						else if(freeHit == true)
						{
							GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, CONTROLLER.StrikerIndex, false);
						}
						else if(noBall == false && freeHit == false)
						{ 
							GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 1, 1, CONTROLLER.CurrentBowlerIndex, 0, CONTROLLER.StrikerIndex, false);
						}//STANLEY}
					}
					//				action = 10;
				}
			}
			else if(wicketKeeperStatus == "catchMissed") // wide...2013
			{
				if(stayStartTime + 1 + timeBetweenBalls < Time.time)
				{
					canBe4or6 = 4;
					//wicketKeeperStatus = "";
					wicketKeeperStatus = "EndOfTheBall"; //

					leftSideCamera.fieldOfView = 35;
					GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
				}
			}//2013
			else if(wicketKeeperStatus == "catchEnd") // ball collection...
			{
				if(stayStartTime + 1 + timeBetweenBalls < Time.time)
				{
					wicketKeeperStatus = "loopEnd";
					if(GameModelScript != null)
					{
						//{STANLEY
						if(noBall == true)
						{
							GameModelScript.UpdateCurrentBall(0, 1, 0, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if(freeHit == true)
						{
							GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
						else if(noBall == false && freeHit == false)
						{
							GameModelScript.UpdateCurrentBall(1, 1, 0, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
					}

					//					action = 10;
				}
			}
			else if(wicketKeeperStatus == "waitForWideSignal")
			{
				if(stayStartTime + 2 + timeBetweenBalls < Time.time)
				{
					//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
					wicketKeeperStatus = "loopEnd";
					if(GameModelScript != null)
					{
						GameModelScript.UpdateCurrentBall(0, 0, 0, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
					}
					//				action = 10;
				}
			}
		}
	}

	public void WicketKeeperPostBattingActions ()
	{
		if(wicketKeeperStatus == "waitForBall")
		{
			// four || six || wicket
			if(ballOnboundaryLine == true)
			{
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.CrossFade ("idle");
				wicketKeeperStatus = "finish";
			}
			if(ballResult == "wicket")
			{
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play ("idle");
				wicketKeeperStatus = "finish";
			}
		}
		else if(wicketKeeperStatus == "waitToCollect")
		{
			float keeperBallCollectingFrame = 6;
			float keeperBallCollectingTriggerDistance = (keeperBallCollectingFrame / animationFPS) * horizontalSpeed;

			if(DistanceBetweenTwoVector2(ball, wicketKeeper) < keeperBallCollectingTriggerDistance)
			{
				canTakeRun = false;
				distanceBtwBallAndCollectingPlayerWhileThrowing = 10000; // maximum distance
				if(GameModelScript != null)
				{
					GameModelScript.EnableRun (false);
				}

				if(runOut == true)
				{
					if(GameModelScript != null)
					{
						GameModelScript.PlayGameSound ("bowledsound"); 
					}
					/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("collectStumpAppeal");
					wicketKeeperStatus = "collectTheThrowAndStump";
				}
				else
				{
					wicketKeeperStatus = "collectTheThrow";
					/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("collectAndStand");
				}
			}
		}
		else if(wicketKeeperStatus == "collectTheThrow" || wicketKeeperStatus == "collectTheThrowAndStump")
		{
			float distanceBtwBallAndKeeper = DistanceBetweenTwoVector2 (ball, wicketKeeper);
			if(distanceBtwBallAndCollectingPlayerWhileThrowing > distanceBtwBallAndKeeper)
			{
				distanceBtwBallAndCollectingPlayerWhileThrowing = distanceBtwBallAndKeeper;
			}
			else
			{
				distanceBtwBallAndCollectingPlayerWhileThrowing = -1; // when pass the wicketkeeper...
			}
			if(distanceBtwBallAndKeeper < 1 || distanceBtwBallAndCollectingPlayerWhileThrowing == -1)
			{
				ballStatus = "";
				pauseTheBall = true;
				wicketKeeperBall.GetComponent<Renderer>().enabled = true;
				ShowBall (false);
				if(wicketKeeperStatus == "collectTheThrow")
				{
					stayStartTime = Time.time;
					wicketKeeperStatus = "end";
				}
				else if(wicketKeeperStatus == "collectTheThrowAndStump")
				{
					float wicketKeeperStumpingFrame = 10;
					wicketKeeper.transform.LookAt(stump1Spot.transform);
					if(/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent["collectStumpAppeal"].time > (wicketKeeperStumpingFrame/animationFPS))
					{
						isRunOut = runOut;
						stayStartTime = Time.time;
						wicketKeeperStatus = "runOutAppeal";

						// keeper has to rotate and appeal to side umpire...
						float appealingAngle = 90;
						iTween.RotateTo (wicketKeeper, iTween.Hash ("y", appealingAngle, "time", 0.5, "delay", 0.2)); 
						// keeper has to rotate and appeal to side umpire...

						if(postBattingWicketKeeperDirection == "straight")
						{
							stump1.GetComponent<Animation>().Play("legSideStumping");
						}
						else if(postBattingWicketKeeperDirection == "offSide")
						{
							stump1.GetComponent<Animation>().Play("fielderRunoutIn");
						}
						else if(postBattingWicketKeeperDirection == "legSide")
						{
							stump1.GetComponent<Animation>().Play("fielderRunoutAway");
						}
					}
				}

			}
		}

		else if(wicketKeeperStatus == "end")
		{
			if(stayStartTime + 1 + timeBetweenBalls < Time.time)
			{
				//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
				wicketKeeperStatus = "loopEnd";
				if(GameModelScript != null)
				{
					//{STANLEY
					if(noBall == false && freeHit == false)
					{
						GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
					}//STANLEY}
				}
				//			action = 10;
			}
		}
		else if(wicketKeeperStatus == "runOutAppeal")
		{
			if(stayStartTime + 1 + timeBetweenBalls < Time.time)
			{
				stayStartTime = Time.time;
				wicketKeeperStatus = "waitForResult";

				mainCamera.enabled = true;
				rightSideCamera.enabled = false;
				leftSideCamera.enabled = false;
				straightCamera.enabled = false;

				if(isRunOut == true)
				{
					if(GameModelScript != null)
					{
						//interfaceConnector.PlayGameSound ("Cheer"); 
					}
					waitForCommentary = true;
				}
				else
				{
					if(GameModelScript != null)
					{
						AudioPlayer.instance.PlayTheCrowdSound("BallMissingCrowd");
					}
				}
			}
		}
		else if(wicketKeeperStatus == "waitForResult")
		{
			if(stayStartTime + 3 + timeBetweenBalls < Time.time) // 3 seconds for umpire decision animation...
			{
				wicketKeeperStatus = "loopEnd";
				if(isRunOut == true)
				{
					int batsmanOutIndex;
					if(currentBallNoOfRuns % 2 == 0) {
						batsmanOutIndex = CONTROLLER.NonStrikerIndex;
					}
					else {
						batsmanOutIndex = CONTROLLER.StrikerIndex;
					}
					//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
					if(GameModelScript != null)
					{
						if(noBall == false && freeHit == false)
						{
							GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 1, 4, CONTROLLER.CurrentBowlerIndex, 0, batsmanOutIndex, false);
						}
					}
				}
				else
				{
					if(GameModelScript != null)
					{
						if(freeHit == false && noBall == false)
						{
							GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
						}
					}
				}
			}		
		}

	}

	public void MoveWicketKeeperToStumps ()
	{
		float reachTime = 0.0f;
		/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.CrossFade ("keeperRun");
		if(currentBowlerType == "fast"){
			reachTime = 1.0f;//1.5;
		}
		if(postBattingWicketKeeperDirection == "straight")
		{
			if(currentBowlerType == "spin"){
				reachTime = 0.3f;
			}
			iTween.MoveTo (wicketKeeper, iTween.Hash ("position", wicketKeeperStraightBallStumping.transform.position, "time", reachTime, "easetype", "linear", "oncomplete", "EnableWicketKeeperToCollectBall", "oncompletetarget", ball));
			wicketKeeper.transform.LookAt(wicketKeeperStraightBallStumping.transform);
		}
		else if(postBattingWicketKeeperDirection == "offSide")
		{
			if(currentBowlerType == "spin"){
				reachTime = 0.7f;
			}
			iTween.MoveTo (wicketKeeper, iTween.Hash ("position", wicketKeeperOffSideBallStumping.transform.position, "time", reachTime, "easetype", "linear", "oncomplete", "EnableWicketKeeperToCollectBall", "oncompletetarget", ball));
			wicketKeeper.transform.LookAt(wicketKeeperOffSideBallStumping.transform);
		}
		else if(postBattingWicketKeeperDirection == "legSide")
		{
			if(currentBowlerType == "spin"){
				reachTime = 0.4f;
			}
			iTween.MoveTo (wicketKeeper, iTween.Hash ("position", wicketKeeperLegSideBallStumping.transform.position, "time", reachTime, "easetype", "linear", "oncomplete", "EnableWicketKeeperToCollectBall", "oncompletetarget", ball));
			wicketKeeper.transform.LookAt(wicketKeeperLegSideBallStumping.transform);
		}
	}

	public void EnableWicketKeeperToCollectBall ()
	{
		/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play ("waitForBall");
		wicketKeeper.transform.LookAt(new  Vector3(ballTimingFirstBounce.transform.position.x, 0f, ballTimingFirstBounce.transform.position.z));
		wicketKeeperStatus = "waitForBall";
	}

	public void ShowBall (bool status)
	{
		ballSkinRenderer.enabled = status;
		ballSphereCollider.enabled = status;
		/* //zig-zag// */ballTrail.enabled = status;
	}
	public IEnumerator EnableTrail()
	{
		int boundaryRand = Random.Range(0, 3);
		yield return new WaitForSeconds(0.3f);
		isTrailOn = true;
		/* //zig-zag// */ballTrail.enabled = true; 

	}

	public void DisableTrail()
	{
		/* //zig-zag// */ballTrail.enabled = false; 
		isTrailOn = false;
		SixDistanceDisplayer.instance.show(false);
	}
	public void ThrowingBallMovement ()
	{
		if (pauseTheBall == false && ballStatus == "throw") 
		{
			float disFromBallToGroundCentre = DistanceBetweenTwoVector2(ball, groundCenterPoint);
			BallMovement();
			if (ballProjectileAngle >= 360) 
			{
				//==============//
				if (applyBallFriction == false)
				{
					horizontalSpeed *= 0.95f; // on pitch/bounce, reduce the speed by 5% // later on this can also impact w.r.t weather condition // new 
					if (ballNoOfBounce == 1)
					{
						ballProjectileAnglePerSecond *= 1.5f; // 1.2f (normal ball), 2.0f (top edge ball);
					}
					else
					{
						ballProjectileAnglePerSecond *= 1.1f;
					}
				}
				if (ballBoundaryReflection == true)
				{
					ballProjectileHeight *= 0.5f;
				}
				if (applyBallFriction == true && edgeCatch == false && horizontalSpeed > 0.0f) // other friction due to boundary...
				{
					horizontalSpeed *= 0.4f; //0.2f
				}
				//===============//
				ballProjectileAngle = 180; // to start new parabola...
				horizontalSpeed *= 0.8f; // reduce horizontal speed after each bounce
				ballProjectileAnglePerSecond = (90 / throwingFirstBounceDistance) * horizontalSpeed; // half of semi circle / 5 meters (one pitch)
				ballProjectileHeight = 0.75f; // stumps height 

			}

			if (ballNoOfBounce > 4)                     // after the 4th bounce, ball should roll on the ground from hereafter...
			{
				ballProjectileAnglePerSecond = 0;       // this is to make the ball to roll on the ground...
				if (canApplyFriction == true)
				{
					horizontalSpeed -= outFieldFriction * Time.deltaTime;   // ball friction...
				}
				// to avoid ball stopping near the boundary to outcome rope low-poly issue...
				if (horizontalSpeed < 2.0f && disFromBallToGroundCentre > (groundRadius - 4) && ballOnboundaryLine == false)
				{
					horizontalSpeed = 2.0f;
				}
			}
		}
	}

	public void CustomRayCastForBowlingBallMovement ()
	{
		Vector3 fwd = ballRayCastReferenceGOTransform.TransformDirection (Vector3.forward);
		RaycastHit hitInfo;
		float rayCastDistance = 1;//0.66; // 0.3

		if (Physics.Raycast (ballRayCastReferenceGOTransform.position, fwd, out hitInfo, rayCastDistance)) {
			//var distanceToGround : float = hitInfo.distance;
			OnCustomTriggerEnter (hitInfo.collider);

			/*if (distanceToGround < 0.5)
			{
				OnCustomTriggerEnter (hitInfo.collider);
			}*/
		}
	}

	protected bool reflectionFromBlackBoard = false;

	public void CustomRayCastForBattingBallMovement ()
	{
		Vector3 fwd = ballRayCastReferenceGOTransform.TransformDirection (Vector3.forward);
		RaycastHit hitInfo;
		float rayCastDistance = 1;

		if (Physics.Raycast (ballRayCastReferenceGOTransform.position, fwd, out hitInfo, rayCastDistance))
		{
			CheckIfTheBallHitTheBoard (hitInfo.collider);
			if (hitInfo.collider.gameObject.name == "Board" && reflectionFromBlackBoard == false)
			{
				BallRebouncesFromBoundary();
				reflectionFromBlackBoard = true;
			}
		}
	}


	public void BowlingBallMovement ()
	{
		if(pauseTheBall == false)
		{
			BallMovement ();
			BallSwingMovement ();
			UpdateBattingTimingMeter();

			CustomRayCastForBowlingBallMovement();

			if(hideBowlingInterface == false)
			{
				if(hideBowlingInterfaceSpot.transform.position.z < ballTransform.position.z)
				{
					if(GameModelScript != null)
					{
						GameModelScript.ShowBowlingInterface (false);
					}
					hideBowlingInterface = true;
				}
			}

			if(wideBallChecked == false)
			{
				isWideBall ();
				/*if(stump1Crease.transform.position.z < ball.transform.position.z)
			{
				wideBallChecked = true;
				var maxWideLimit : GameObject;
				var minWideLimit : GameObject;
				if(currentBatsmanHand == "right") {
					maxWideLimit = RHBMaxWideLimit;
					minWideLimit = RHBMinWideLimit;
				}
				else if(currentBatsmanHand == "left") {
					maxWideLimit = LHBMaxWideLimit;
					minWideLimit = LHBMinWideLimit;
				}
			}*/
			}

			if(ballProjectileAngle >= 360) 
			{
				ballNoOfBounce++;
				ballProjectileAngle = 180; // to start new parabola...
				
				if (currentBowlerType != "spin" && ballNoOfBounce <= 1)
				{
					if (CONTROLLER.BowlingSpeed > 5 && ballPitchingDistanceFactor > 0.8f)
					{
						bowlingBounceFactor = 1.1f * pitchFactor;// 0.2 for flipper anf 1.2 for Top Spin
					}
				}
				
				if (ballNoOfBounce == 1) // after first pitch
				{
					ballProjectileAnglePerSecond *= ballProjectileAnglePerSecondBowlingFactor;//0.7f;
					ballProjectileHeight *= (bowlingBounceFactor * ballPitchHeightFactor);  // reduce projectile height after each bounce
				}
				else // after second pitch...
				{
					ballProjectileAnglePerSecond *= 1.3f;
					ballProjectileHeight *= bowlingBounceFactor;  // reduce projectile height after each bounce
				}
				bowlingBounceFactor = 0.6f; // for second bounce onwards....
				nextPitchDistance = ballProjectileHeight;

				if (ballStatus == "onPads" || ballStatus == "bowled")
				{
					ballProjectileAnglePerSecond *= 2.0f;
					ballProjectileHeight *= 0.3f;
					horizontalSpeed *= 0.98f;
				}

				if (ballNoOfBounce == 1)
				{
					ballProjectileHeight *= 1.33f; // for raising the ball...
					if(currentBowlerType == "spin") // + | 90 | -
					{
						ballAngle += spinValue; // +4 || - 4
					} 
					else if(currentBowlerType == "fast" && swingValue != 0) // FAST bowler and swing ball...
					{
						ballAngle += spinValue; // 2*2 =>4 // 4 = max swing turn after pitching the ball... // -4 min..
						swingValue = 0;
					}
					ActivateWicketKeeper ();
					//if(Mathf.Abs (ball.transform.position.x) <= 0.1) // ball pitches within the line...
					//{
					//	ballInline = true;
					//}
					if (currentBatsmanHand == "right" && ballTransform.position.x > -0.7f && ballTransform.position.x < 0.2f ||
						currentBatsmanHand == "left" && ballTransform.position.x > -0.2f && ballTransform.position.x < 0.7f)
					{
						ballInline = true;
					}
				}
			}
		}
	}

	public void isWideBall ()
	{
		if (stump1Crease.transform.position.z < ballTransform.position.z) 
		{
			wideBallChecked = true;
			Vector3 maxWideLimit = Vector3.zero;
			Vector3 minWideLimit = Vector3.zero;
			if (currentBatsmanHand == "right") 
			{
				if (batsman.transform.position.x >= 0) {				
					minWideLimit = RHBMinWideLimit.transform.position;
					maxWideLimit = batsman.transform.position;
				} else if (batsman.transform.position.x < 0) {				
					minWideLimit = RHBMinWideLimit.transform.position;
					maxWideLimit = stump1Crease.transform.position;
					maxWideLimit = new Vector3 (maxWideLimit.x+0.18f,maxWideLimit.y, maxWideLimit.z); // batsman x offset diff...
				}			
			} 
			else if (currentBatsmanHand == "left") 
			{
				if (batsman.transform.position.x >= 0) {
					minWideLimit = stump1Crease.transform.position;
					maxWideLimit = LHBMaxWideLimit.transform.position;
					minWideLimit = new Vector3 (minWideLimit.x-0.18f, minWideLimit.y, minWideLimit.z); // batsman x offset diff...
				} else if (batsman.transform.position.x < 0) {
					minWideLimit = batsman.transform.position;
					maxWideLimit = LHBMaxWideLimit.transform.position;				
				}
			}
			if (ballTransform.position.x < maxWideLimit.x && ballTransform.position.x > minWideLimit.x)
			{
				if (ballStatus != "shotSuccess" && ballStatus != "bowled" && ballStatus != "onPads" && shotPlayed != "WellLeftNormalHeight" && shotPlayed != "LeaveTheBallBouncer" && shotPlayed != string.Empty)
				{
					GameModelScript.PlayGameSound("BallMissing");
				}
				
            }
			else 
			{
				wideBall = true;
			}

            if (ballStatus != "shotSuccess" && ballStatus != "bowled" && ballStatus != "onPads" && (shotPlayed == "WellLeftNormalHeight" || shotPlayed == "LeaveTheBallBouncer" || shotPlayed == string.Empty))
            {
                AudioPlayer.instance.PlayTheCrowdSound("BallMissingCrowd");
            }
			else if (ballStatus == "bowling" && ballStatus != "bowled" && ballStatus != "onPads" && shotPlayed != string.Empty)
            {
                AudioPlayer.instance.PlayTheCrowdSound("BallMissingCrowd");
            }
        }		
	}

	public void BallSwingMovement ()
	{
		if (swingValue != 0) 
		{
			float swingX = Mathf.Cos (swingProjectileAngle * DEG2RAD) * swingValue * Time.deltaTime; // -2 to 2 meters swing... 
			swingProjectileAngle += (swingProjectileAnglePerSecond * Time.deltaTime);
			ballTransform.position = new Vector3 (ballTransform.position.x-swingX, ballTransform.position.y, ballTransform.position.z);
		}
	}



	public void FindBowlingParameters ()
	{
		if (CONTROLLER.gameMode != "multiplayer")
		{
			Vector3 relative = ballOriginGO.transform.InverseTransformPoint(bowlingSpotGO.transform.position);
			ballAngle = 90 - Mathf.Atan2(relative.x, relative.z) * RAD2DEG;
			// find ball spotting length..
			ballSpotLength = (ballOriginGO.transform.position - bowlingSpotGO.transform.position).magnitude;

			/***************/
			float bowlingAnglePercentage = Random.Range(1.0f, 10.0f) / 10.0f; //// Default 0 for fast bowler. range from 1 - 10; consider ...
			float maxSpin = 4;
			/***************/
			/*if(currentBowlerType == "fast")
			{
				horizontalSpeed = 20 + (bowlingSpeed/10) * 2;//22;	
			}	
			else if(currentBowlerType == "spin")
			{
				horizontalSpeed = 18 + (bowlingSpeed/10) * 2;//20;
			}*/

			//horizontalSpeed = 18;
			if (currentBowlerType == "fast")
			{
				bowlingSpeed = Random.Range(25, 30);
			}
			else if (currentBowlerType == "spin")
			{
				spinValue = maxSpin * bowlingAnglePercentage;
				currentBowlerSpinType = Random.Range(1, 3);
				if (currentBowlerSpinType == 2)
				{
					spinValue *= -1;
				}
				bowlingSpeed = Random.Range(0, 11);//(-10, 5)//aaaaaaa
			}

			horizontalSpeed = 13;

			ballProjectileAnglePerSecond = (90 / ballSpotLength) * horizontalSpeed;
			swingProjectileAnglePerSecond = (180 / ballSpotLength) * horizontalSpeed;

			// ball spot crossing the crease line... (used for computer batting intelligence)
			ballSpotAtCreaseLine.transform.position = new Vector3(bowlingSpotGO.transform.position.x, ballSpotAtCreaseLine.transform.position.y, ballSpotAtCreaseLine.transform.position.z);
			float xDiff = ballSpotAtCreaseLine.transform.position.x - bowlingSpotGO.transform.position.x;
			float zDiff = ballSpotAtCreaseLine.transform.position.z - bowlingSpotGO.transform.position.z;

			float creaseLineAdjacentLength = Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff);
			float creaseLineThetaBtwAdjAndHyp = Mathf.Atan2(xDiff, zDiff) * RAD2DEG - (90 - ballAngle - spinValue);

			float creaseLineHypotenuse = creaseLineAdjacentLength / Mathf.Cos(creaseLineThetaBtwAdjAndHyp * DEG2RAD);
			float creaseLineOppositeLength = Mathf.Sqrt(creaseLineHypotenuse * creaseLineHypotenuse - creaseLineAdjacentLength * creaseLineAdjacentLength);

			if (creaseLineThetaBtwAdjAndHyp < 0)
			{
				ballSpotAtCreaseLine.transform.position = new Vector3(ballSpotAtCreaseLine.transform.position.x + creaseLineOppositeLength, ballSpotAtCreaseLine.transform.position.y, ballSpotAtCreaseLine.transform.position.z);
			}
			else
			{
				ballSpotAtCreaseLine.transform.position = new Vector3(ballSpotAtCreaseLine.transform.position.x - creaseLineOppositeLength, ballSpotAtCreaseLine.transform.position.y, ballSpotAtCreaseLine.transform.position.z);
			}

			// ball spot crossing the stump ... (used for LBW)
			ballSpotAtStump.transform.position = new Vector3(bowlingSpotGO.transform.position.x, ballSpotAtStump.transform.position.y, ballSpotAtStump.transform.position.z);
			float xDiff2 = ballSpotAtStump.transform.position.x - bowlingSpotGO.transform.position.x;
			float zDiff2 = ballSpotAtStump.transform.position.z - bowlingSpotGO.transform.position.z;

			float stumpAdjacentLength = Mathf.Sqrt(xDiff2 * xDiff2 + zDiff2 * zDiff2);
			float stumpThetaBtwAdjAndHyp = Mathf.Atan2(xDiff2, zDiff2) * RAD2DEG - (90 - ballAngle - spinValue);
			float stumpHypotenuse = stumpAdjacentLength / Mathf.Cos(stumpThetaBtwAdjAndHyp * DEG2RAD);
			float stumpOppositeLength = Mathf.Sqrt(stumpHypotenuse * stumpHypotenuse - stumpAdjacentLength * stumpAdjacentLength);

			if (stumpThetaBtwAdjAndHyp < 0)
			{
				ballSpotAtStump.transform.position = new Vector3(ballSpotAtStump.transform.position.x + stumpOppositeLength, ballSpotAtStump.transform.position.y, ballSpotAtStump.transform.position.z);
			}
			else
			{
				ballSpotAtStump.transform.position = new Vector3(ballSpotAtStump.transform.position.x - stumpOppositeLength, ballSpotAtStump.transform.position.y, ballSpotAtStump.transform.position.z);
			}

			///////
			ballPitchingDistanceFactor = (halfPitchDistance - bowlingSpotGO.transform.position.z) / halfPitchDistance;
			ballPitchHeightFactor = 0.6f + ballPitchingDistanceFactor * 0.4f; // 0.6f to 1.0f
			ballProjectileAnglePerSecondBowlingFactor = (1.2f - ballPitchingDistanceFactor * 0.5f); // 0.7f to 1.2f

			if (ballSpotLength > 17.4f)
			{
				// if ball pitch is near or over the crease line...
				ballSpotHeight = 0.2f;
				ballHeightAtStump = ballSpotHeight;
			}
			else
			{
				// ball will pitch before the crease line
				float tempBallProjectileAnglePerSecond = ballProjectileAnglePerSecond * ballProjectileAnglePerSecondBowlingFactor; //0.7f; //1.1f;
				float tempBallProjectileHeight = ballProjectileHeight * bowlingBounceFactor * ballPitchHeightFactor;
				float tempNextPitchDistance = (180 / tempBallProjectileAnglePerSecond) * horizontalSpeed;

				float tempProjectileAngleAtCrease = creaseLineHypotenuse / tempNextPitchDistance * 180;
				ballSpotHeight = Mathf.Sin(tempProjectileAngleAtCrease * DEG2RAD) * tempBallProjectileHeight;

				tempProjectileAngleAtCrease = (creaseLineHypotenuse + 1.2f) / tempNextPitchDistance * 180;
				ballHeightAtStump = Mathf.Sin(tempProjectileAngleAtCrease * DEG2RAD) * tempBallProjectileHeight;
			}
		}
		else
		{
			// MULTIPLAYER

			Vector3 relative = ballOriginGO.transform.InverseTransformPoint(bowlingSpotGO.transform.position);
			ballAngle = 90 - Mathf.Atan2(relative.x, relative.z) * RAD2DEG;
			// find ball spotting length..
			ballSpotLength = (ballOriginGO.transform.position - bowlingSpotGO.transform.position).magnitude;

			//float bowlingAnglePercentage = Random.Range (1.0f, 10.0f) / 10.0f; //// Default 0 for fast bowler. range from 1 - 10; consider ...
			float bowlingAnglePercentage = Multiplayer.oversData[CONTROLLER.currentMatchBalls / 6].bowlingAngle[CONTROLLER.currentMatchBalls % 6]; //// Default 0 for fast bowler. range from 1 - 10; consider ...
			float maxSpin = 4;
			//horizontalSpeed = 18;
			if (currentBowlerType == "fast")
			{
				//bowlingSpeed = Random.Range (25, 30);

			}
			else if (currentBowlerType == "spin")
			{
				spinValue = maxSpin * bowlingAnglePercentage;
				if (Multiplayer.oversData[CONTROLLER.currentMatchBalls / 6].bowlerType == "offspin")
				{
					currentBowlerSpinType = 1;
				}
				else
				{
					currentBowlerSpinType = 2;
				}
				if (currentBowlerSpinType == 2)
				{
					spinValue *= -1;
				}

			}

			horizontalSpeed = 13;
			ballProjectileAnglePerSecond = (90 / ballSpotLength) * horizontalSpeed;
			swingProjectileAnglePerSecond = (180 / ballSpotLength) * horizontalSpeed;

			// ball spot crossing the crease line... (used for computer batting intelligence)
			ballSpotAtCreaseLine.transform.position = new Vector3(bowlingSpotGO.transform.position.x, ballSpotAtCreaseLine.transform.position.y, ballSpotAtCreaseLine.transform.position.z);
			float xDiff = ballSpotAtCreaseLine.transform.position.x - bowlingSpotGO.transform.position.x;
			float zDiff = ballSpotAtCreaseLine.transform.position.z - bowlingSpotGO.transform.position.z;

			float creaseLineAdjacentLength = Mathf.Sqrt(xDiff * xDiff + zDiff * zDiff);
			float creaseLineThetaBtwAdjAndHyp = Mathf.Atan2(xDiff, zDiff) * RAD2DEG - (90 - ballAngle - spinValue);

			float creaseLineHypotenuse = creaseLineAdjacentLength / Mathf.Cos(creaseLineThetaBtwAdjAndHyp * DEG2RAD);
			float creaseLineOppositeLength = Mathf.Sqrt(creaseLineHypotenuse * creaseLineHypotenuse - creaseLineAdjacentLength * creaseLineAdjacentLength);

			if (creaseLineThetaBtwAdjAndHyp < 0)
			{
				ballSpotAtCreaseLine.transform.position += new Vector3(creaseLineOppositeLength, 0, 0);
			}
			else
			{
				ballSpotAtCreaseLine.transform.position -= new Vector3(creaseLineOppositeLength, 0, 0);
			}

			// ball spot crossing the stump ... (used for LBW)
			ballSpotAtStump.transform.position = new Vector3(bowlingSpotGO.transform.position.x, ballSpotAtStump.transform.position.y, ballSpotAtStump.transform.position.z);
			float xDiff2 = ballSpotAtStump.transform.position.x - bowlingSpotGO.transform.position.x;
			float zDiff2 = ballSpotAtStump.transform.position.z - bowlingSpotGO.transform.position.z;

			float stumpAdjacentLength = Mathf.Sqrt(xDiff2 * xDiff2 + zDiff2 * zDiff2);
			float stumpThetaBtwAdjAndHyp = Mathf.Atan2(xDiff2, zDiff2) * RAD2DEG - (90 - ballAngle - spinValue);
			float stumpHypotenuse = stumpAdjacentLength / Mathf.Cos(stumpThetaBtwAdjAndHyp * DEG2RAD);
			float stumpOppositeLength = Mathf.Sqrt(stumpHypotenuse * stumpHypotenuse - stumpAdjacentLength * stumpAdjacentLength);

			if (stumpThetaBtwAdjAndHyp < 0)
			{
				ballSpotAtStump.transform.position += new Vector3(stumpOppositeLength, 0, 0);
			}
			else
			{
				ballSpotAtStump.transform.position -= new Vector3(stumpOppositeLength, 0, 0);
			}

			///////
			ballPitchingDistanceFactor = (halfPitchDistance - bowlingSpotGO.transform.position.z) / halfPitchDistance;
			ballPitchHeightFactor = 0.6f + ballPitchingDistanceFactor * 0.4f; // 0.6f to 1.0f
			ballProjectileAnglePerSecondBowlingFactor = (1.2f - ballPitchingDistanceFactor * 0.5f); // 0.7f to 1.2f

			if (ballSpotLength > 17.4f)
			{
				// if ball pitch is near or over the crease line...
				ballSpotHeight = 0.2f;
				ballHeightAtStump = ballSpotHeight;
			}
			else
			{
				// ball will pitch before the crease line
				float tempBallProjectileAnglePerSecond = ballProjectileAnglePerSecond * ballProjectileAnglePerSecondBowlingFactor; //0.7f; //1.1f;
				float tempBallProjectileHeight = ballProjectileHeight * bowlingBounceFactor * ballPitchHeightFactor;
				float tempNextPitchDistance = (180 / tempBallProjectileAnglePerSecond) * horizontalSpeed;

				float tempProjectileAngleAtCrease = creaseLineHypotenuse / tempNextPitchDistance * 180;
				ballSpotHeight = Mathf.Sin(tempProjectileAngleAtCrease * DEG2RAD) * tempBallProjectileHeight;

				tempProjectileAngleAtCrease = (creaseLineHypotenuse + 1.2f) / tempNextPitchDistance * 180;
				ballHeightAtStump = Mathf.Sin(tempProjectileAngleAtCrease * DEG2RAD) * tempBallProjectileHeight;
			}
		}
	}




	private int prevSixDist = 0;
	public void BattingBallMovement ()
	{
		if(pauseTheBall == false && ballStatus != "throw")
		{
			BallMovement ();
			fielder10.transform.LookAt(new  Vector3(ballTransform.position.x, 0f, ballTransform.position.z));//ShankarEdit
			mainUmpire.transform.LookAt(new  Vector3(ballTransform.position.x, 0f, ballTransform.position.z));//ShankarEdit
			if(ballProjectileAngle >= 360)
			{
				CamShouldNotFollowBallY = true;
				ballNoOfBounce++;
				ballProjectileAngle = 180; // to start new parabola...
				if(ballNoOfBounce == 1) 
				{
					if(DistanceBetweenTwoVector2 (ball, groundCenterPoint) < groundRadius) {
						canBe4or6 = 4;
					}
				}
				if (applyBallFiction == false)
				{
					ballProjectileAnglePerSecond *= 1.1f;

					horizontalSpeed *= 0.95f; // on pitch/bounce, reduce the speed by 5% // later on this can also impact w.r.t weather condition // new 
					ballProjectileAnglePerSecond *= ballProjectileAnglePerSecondFactor; // 1.2f (normal ball), 2.0f (top edge ball);

					nextPitchDistance += (180 / ballProjectileAnglePerSecond * horizontalSpeed);

					ReassignFielderChasePoint(0.5f); // how many meters to reduce
				}
				else if(applyBallFiction == true)
				{
					ballProjectileAnglePerSecond *= 2f;
				}
				//		horizontalSpeed *= 0.9; // reduce horizontal speed after each bounce
				ballProjectileHeight *= 0.5f;  // reduce projectile height after each bounce
				if(ballProjectileHeight > 2.5) // fielder max pickingup height..
				{
					ballProjectileHeight = 2.2f;
				}
				if (ballBoundaryReflection == true)
				{
					ballProjectileHeight *= 0.5f;
				}

				if (ballProjectileHeight > ballBounceMaxHeight) // fielder max pickingup height...
				{
					ballProjectileHeight = ballBounceMaxHeight;
					//	ballProjectileAnglePerSecond *= ballProjectileAnglePerSecondFactor; 
				}
			}

			if(applyBallFiction == true && horizontalSpeed > 0.0f)
			{
				horizontalSpeed *= 0.4f; //0.2f
				//horizontalSpeed *= (100-90*Time.deltaTime)/100;
			}

			if(ballNoOfBounce > 3)
			{ // ball should roll on the ground from hereafter...
				ballProjectileAnglePerSecond = 0; // this is to make the ball to roll on the ground 
				//		horizontalSpeed *= (100-20*Time.deltaTime)/100;// ground resistance/friction... // reducing 20% per second...
				if(horizontalSpeed < 0.4)
				{
					horizontalSpeed = 0;
				}
			}
			if (ballNoOfBounce >= 4)                     // after the 4th bounce, ball should roll on the ground from hereafter...
			{
				ballProjectileAnglePerSecond = 0;       // this is to make the ball to roll on the ground...
				
				ApplyBallFriction();   // ball friction...

				// to avoid ball stopping near the boundary to outcome rope low-poly issue...
				if (horizontalSpeed < 2.0f && DistanceBetweenTwoVector2(ball, groundCenterPoint) > (groundRadius - 4) && ballOnboundaryLine == false)
				{
					horizontalSpeed = 2.0f;
				}

				ReassignFielderChasePoint(0.5f); // how many meters to reduce
			}
			/////////////////////////////////////////
			int SixDistance = Mathf.FloorToInt(DistanceBetweenTwoVector2(ballConnectToTheBatGO, ball));
			int diff = Mathf.FloorToInt(DistanceBetweenTwoVector2(fielder10FocusGObjToCollectTheBall, groundCenterPoint));
			//sixdisplayer
			if (diff > groundRadius && ballNoOfBounce == 0 && isTrailOn == true && prevSixDist < SixDistance)
			{
				SixDistanceDisplayer.instance.show (true); 
				sixDistanceCamera.enabled = true;
				sixDistanceGO.text  = "" + SixDistance + " Meters";
				prevSixDist = SixDistance;
				SixDistanceDisplayer.instance.PositionSixDistanceProfile (ball, currentBatsmanHand);
			}

			/////////////////////////////////////////
		}
	}
	protected void ApplyBallFriction()
	{
		if (!fielderCollectedTheBall)
		{
			float maxFric;
			if (BattingTimingMeterDisplayText == ShotStatus.PERFECT)
			{
					maxFric = 3f;
			}
			else if (BattingTimingMeterDisplayText == ShotStatus.EARLY_NICETRY)
				maxFric = 2.2f;
			else
				maxFric = 2f;

			//outFieldFriction += .02f;
			outFieldFriction += 1.5f * Time.deltaTime;


			outFieldFriction = Mathf.Clamp(outFieldFriction, 0f, maxFric);

			horizontalSpeed -= outFieldFriction * Time.deltaTime;
			horizontalSpeed = Mathf.Clamp(horizontalSpeed, 0f, 50f);
		}
	}

	private void ReassignFielderChasePoint(float metersToReduce)
	{
		GameObject thisFielderChasePoint;
		int thisFielderIndex;

		float chasePointX;
		float chasePointZ;
		metersToReduce = 1.0f * Time.deltaTime;

		for (int i = 0; i < activeFielderNumber.Count; i++)
		{
			thisFielderChasePoint = fielderChasePoint[activeFielderNumber[i]];
			thisFielderIndex = activeFielderNumber[i];

			if (activeFielderAction[i] == "goForChase" && horizontalSpeed <= 0.0f)
			{
				thisFielderChasePoint.transform.position = new Vector3(ballTransform.position.x, thisFielderChasePoint.transform.position.y, ballTransform.position.z);
			}
			else if (activeFielderAction[i] == "goForChase")
			{
				// finding the optimum chase point for the fielder...
				float fielderBallPickUpFrame = 11.0f;//10; // buffer animation frame number where fielder catches the ball...
				float ballPrePickupDistance = (fielderBallPickUpFrame * animationFPSDivide) * horizontalSpeed;

				float fielderPickupDistance = Vector3.Distance(fielder[thisFielderIndex].transform.position, thisFielderChasePoint.transform.position);
				float fielderPickupReachTime = fielderPickupDistance / fielderSpeed;
				float fielderAdjacentDistanceForChasePoint = Vector3.Distance(ballTransform.position, thisFielderChasePoint.transform.position);
				float ballPrePickupPointReachTime = (fielderAdjacentDistanceForChasePoint - ballPrePickupDistance) / horizontalSpeed;

				// pickup forward....
				if (fielderPickupReachTime < ballPrePickupPointReachTime)
				{
					while (fielderPickupReachTime < ballPrePickupPointReachTime)
					{
						fielderAdjacentDistanceForChasePoint -= metersToReduce;
						chasePointX = ballTransform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
						chasePointZ = ballTransform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);
						thisFielderChasePoint.transform.position = new Vector3(chasePointX, thisFielderChasePoint.transform.position.y, chasePointZ);

						fielderPickupDistance = Vector3.Distance(fielder[thisFielderIndex].transform.position, thisFielderChasePoint.transform.position);
						fielderPickupReachTime = fielderPickupDistance / fielderSpeed;
						ballPrePickupPointReachTime = (fielderAdjacentDistanceForChasePoint - ballPrePickupDistance) / horizontalSpeed;
					}
					// spoting a step back..
					fielderAdjacentDistanceForChasePoint += metersToReduce;
					chasePointX = ballTransform.position.x + fielderAdjacentDistanceForChasePoint * Mathf.Cos(ballAngle * DEG2RAD);
					chasePointZ = ballTransform.position.z + fielderAdjacentDistanceForChasePoint * Mathf.Sin(ballAngle * DEG2RAD);
					thisFielderChasePoint.transform.position = new Vector3(chasePointX, thisFielderChasePoint.transform.position.y, chasePointZ);
				}
			}
		}
	}

	public void UpdateBallShadow ()
	{
		if (ballSkinRenderer.enabled == true)
		{
			ShadowsArray[12].SetActive(true);
			ShadowsArray[12].transform.position = new Vector3(ShadowsArray[12].transform.position.x, 0, ShadowsArray[12].transform.position.z);
		}
		else
		{
			ShadowsArray[12].SetActive(false);
		}

		/*GameObject ballShadowGO = ShadowsArray [12] as GameObject;
		if (ball.GetComponent<Renderer> ().enabled == true) {
			ballShadowGO.SetActive (true);
			GameObject shadowRef = ShadowRefArray [12];
			ballShadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, 0, shadowRef.transform.position.z);
		} else {
			if (showShadows == true) {
				ballShadowGO.SetActive (false);
			}
		}*/
	}

	public void GetUserBattingKeyboardInput ()
	{
		// down states
		if (Input.GetKeyDown (KeyCode.UpArrow) == true) {
			upArrowKeyDown = true;
		}
		if (Input.GetKeyDown (KeyCode.DownArrow) == true) {
			downArrowKeyDown = true;
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow) == true) {
			if (currentBatsmanHand == "right") {
				leftArrowKeyDown = true;
			} else {
				rightArrowKeyDown = true;
			}
		}
		if (Input.GetKeyDown (KeyCode.RightArrow) == true) {
			if (currentBatsmanHand == "right") {
				rightArrowKeyDown = true;
			} else {
				leftArrowKeyDown = true;
			}
		}

		// up states
		if (Input.GetKeyUp (KeyCode.UpArrow) == true) {
			upArrowKeyDown = false;
		}
		if (Input.GetKeyUp (KeyCode.DownArrow) == true) {
			downArrowKeyDown = false;
		}
		if (Input.GetKeyUp (KeyCode.LeftArrow) == true) {
			if (currentBatsmanHand == "right") {
				leftArrowKeyDown = false;
			} else {
				rightArrowKeyDown = false;
			}
		}
		if (Input.GetKeyUp (KeyCode.RightArrow) == true) {
			if (currentBatsmanHand == "right") {
				rightArrowKeyDown = false;
			} else {
				leftArrowKeyDown = false;			
			}
		}
		/*Ultrabook*/
		if (Input.GetMouseButtonDown (0)) {
			batsmanStep = 1.0f;
		}
		if (Input.GetMouseButtonUp (0)) {
			batsmanStep = 0.3f;
		}
		/*Ultrabook*/
	}

	public void SetLoft()
	{
		AudioPlayer.instance.PlayButtonSnd();
		if (loft == false) {
			loftText.color = Color.black;
			loftBtn.image.sprite = loftSprite [1];
			loftText2.color = Color.black;
			loftBtn2.image.sprite = loftSprite [1];
			loft = true;
            PlayerPrefs.SetInt("loft", 1);
			loftDesc.text = "Loft (On)";
			loftDesc2.text = "Loft (On)";
		} else {
			loftText.color = Color.white;
			loftBtn.image.sprite = loftSprite [0];
			loftText2.color = Color.white;
			loftBtn2.image.sprite = loftSprite [0];
			loft = false;
            PlayerPrefs.SetInt("loft", 0);
            loftDesc.text = "Loft (Off)";
			loftDesc2.text = "Loft (Off)";
		}
	}

    public void SetLoftImage()
    {
        if (loft == true)
        {
            loftText.color = Color.black;
            loftBtn.image.sprite = loftSprite[1];
            loftText2.color = Color.black;
            loftBtn2.image.sprite = loftSprite[1];
            loft = true;
            PlayerPrefs.SetInt("loft", 1);
            loftDesc.text = "Loft (On)";
            loftDesc2.text = "Loft (On)";
        }
        else
        {
            loftText.color = Color.white;
            loftBtn.image.sprite = loftSprite[0];
            loftText2.color = Color.white;
            loftBtn2.image.sprite = loftSprite[0];
            loft = false;
            PlayerPrefs.SetInt("loft", 0);
			loftDesc.text = "Loft (Off)";
			loftDesc2.text = "Loft (Off)";
		}
	}
	
	

	public void GetBattingInput ()
	{
		if(battingBy == "user")
		{
			GetUserBattingInput ();
			if(playingInTouchDevice == true && action == 3 && ball.transform.position.z > 2.5 && touchDeviceShotInput == false)
			{
				GameModelScript.GetShotSelected ();
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			mouseDownD = true;
		}

		if(Input.GetMouseButtonUp(0))
		{
			mouseDownD = false;
		}
		//shankar 08April
		/*if(mouseDownD == true && batsmanCanMoveLeftRight == false && GameModel.instance.isGamePaused == false && BattingScoreCard.instance.gameObject.transform.position.x != 0)
	{
		ninjaSlice.transform.position = mainCamera.ScreenToWorldPoint(Vector3 (Input.mousePosition.x,Input.mousePosition.y, 2));
	}*/
		// if batsman does not made any shot till the ball reaches the batsman crease line... then play the LEAVE shot automatically...
		if(ballTransform.position.z > shotActivationMaxLimit.transform.position.z && action == 3) // bowling and ball towards the batsman...
		{
			if(batsmanTriggeredShot == false)
			{
				batsmanTriggeredShot = true;
				canSwipeNow = false;
				if (ballSpotHeight < 1.4f)
				{
					batsmanAnimation = "WellLeftNormalHeight"; // LeaveTheBall
					shotPlayed = "WellLeftNormalHeight";
				}
				else
				{
					//if ball height is over the shoulder height then play the following....
					batsmanAnimation = "LeaveTheBallBouncer"; // LeaveTheBall
					shotPlayed = "LeaveTheBallBouncer";
					//CONTROLLER.sndController.EnqueueCommentary(48, 0, 0);
				}
				AudioPlayer.instance.PlayTheCrowdSound("wellleftcrowd");

				if (GameModel.isGamePaused == false)
				{
					AdIntegrate.instance.SetTimeScale(1f);
				}
				/*var PitchEffect : Transform = Instantiate (BallPitchEffect, bowlingSpotGO.transform.position, Quaternion.identity);
			PitchEffect.transform.position.z += 2;*/
			}
			bowlingSpotScript.HideBowlingSpot (); // hide the bowling spot even when the batsman triggers the shot and ball missing the bat...
		}
		if (mouseDownD == true && canSwipeNow == true)
		{
			ninjaSlice.transform.position = mainCamera.ScreenToWorldPoint(new  Vector3 (Input.mousePosition.x,Input.mousePosition.y, 2));
		}
		// make the corresponding shot...
		//if((leftArrowKeyDown == true || rightArrowKeyDown == true|| upArrowKeyDown == true || downArrowKeyDown == true || touchDeviceShotInput == true) && canMakeShot == true && batsmanTriggeredShot == false)
		//{
		if((Input.GetKeyDown(KeyCode.S) == true || touchDeviceShotInput == true) && canMakeShot == true && batsmanTriggeredShot == false && action == 3)
		{
			// to avoid re-attempting other shot for the same ball...
			batsmanTriggeredShot = true;
			if(ballTransform.position.z >= shotActivationMaxLimit.transform.position.z - 3)
			{
				LateAttempt = true;
			}
			ballToBatsmanDistance = DistanceBetweenTwoVector2 (stump1Crease, ball);
			if (ballToBatsmanDistance < 11f && ballToBatsmanDistance > 8f && LateAttempt == false)
			{
				perfectShot = true;
			}
			powerShot = loft;

			ShotType(shotSelectedSide);

			if (shotPlayed != "WellLeftNormalHeight" && shotPlayed != "LeaveTheBallBouncer" && shotPlayed != string.Empty)
			{
				float currentOptimalShotLength = ShotVariables.optimalShotTable[shotPlayed + "OptimalShotLength"];
				float currentOptimalShotFrame = ShotVariables.optimalShotTable[shotPlayed + "OptimalShotFrame"];
				optimalShotTime = currentOptimalShotLength / horizontalSpeed;
				batReachingTimeForOptimalShotLength = currentOptimalShotFrame * animationFPSDivide;
				optimalShotActivationTime = optimalShotTime - batReachingTimeForOptimalShotLength - 0.25f; //-0.1 is include to make smooth frame rate...
				/*if (optimalShotActivationTime >= 0.75f)
				{
					powerShot = true;
				}*/
			}			
		}
	}




	public void  ExecuteTheShot ()
	{
		GameObject fielderGO;
		if(batsmanTriggeredShot == true)
		{
			//if(Time.time > (ballReleasedTime + optimalShotActivationTime) && batsmanMadeShot == false)ShankarEdit
            if (Time.time > (ballReleasedTime + optimalShotActivationTime) && batsmanMadeShot == false)
				//if (batsmanMadeShot == false)
			{
				if (Time.timeScale == 0.5f)
				{
					AdIntegrate.instance.SetTimeScale(1f);
				}
				batsmanMadeShot = true;
				//SwitchToLowPoly ();
				
				strikerScript._crossFade(batsmanAnimation, 0.02f);

				strikerScript.action = 7;
				strikerScript.shotPlayed = shotPlayed;
				if (shotPlayed == "WellLeftNormalHeight" || shotPlayed == "LeaveTheBallBouncer" || shotPlayed == "GlennMaxwell_SwitchHit") //BigBash
				{
					strikerScript.setAnimationSpeed(2.0f);
				}
				//if (shotPlayed != "leave")
				if (shotPlayed != "WellLeftNormalHeight" && shotPlayed != "LeaveTheBallBouncer" && shotPlayed != string.Empty)
				{
					// controlling speed of shot's animation
					float optimalShotTime = (float)ShotVariables.optimalShotTable[shotPlayed + "OptimalShotLength"];
					float optimalShotFrame = (float)ShotVariables.optimalShotTable[shotPlayed + "OptimalShotFrame"];
					float ballRemainingDistanceToReachOptimalSpot = optimalShotTime - DistanceBetweenTwoVector2(ballOriginGO, ball);
					float ballTimeToReachOptimalSpot = ballRemainingDistanceToReachOptimalSpot / horizontalSpeed;
					float animationTimeToReachOptimalFrame = optimalShotFrame / animationFPS;
					float wantedAnimationSpeed = animationTimeToReachOptimalFrame / ballTimeToReachOptimalSpot;
					if (wantedAnimationSpeed > 2)
					{
						wantedAnimationSpeed = 2;
					}
					if ((shotPlayed == "YorkerBall1" || shotPlayed == "YorkerBall2") && wantedAnimationSpeed > 1.4f)
					{
						wantedAnimationSpeed = 1.4f;
					}
					if ((shotPlayed == "DownTheTrackMidOn" || shotPlayed == "DownTheTrackMidOff" || shotPlayed == "ExtraCoverDrive") && wantedAnimationSpeed > 1.1f)
					{
						wantedAnimationSpeed = 1.1f;
					}
					strikerScript.setAnimationSpeed(wantedAnimationSpeed);

					/*batsman.GetComponent<Animation>()*///batsmanAnimationComponent[batsmanAnimation].speed = wantedAnimationSpeed;
														 // controlling speed of shot's animation
				}

				fielderGO = fielder[1] as GameObject;
				fielderGO.GetComponent<Animation>().Play("backToIdle");
				fielderGO = fielder[2] as GameObject;
				fielderGO.GetComponent<Animation>().Play("backToIdle");
				shotExecutionTime = Time.time;
				computerBatsmanNewRunAttempt = true;
				playShotCommentary = true;
				if(GameModelScript != null)
				{
					GameModelScript.EnableShot (false);
				}
			}
		}
	}

	public void  CommentaryForShotMade ()
	{
		/*if(GameModelScript != null && action == 4)
		{
			if(Time.time > (shotExecutionTime + 0.5) && playShotCommentary == true)
			{
				playShotCommentary = false;
				if((shotPlayed == "offDrive" || shotPlayed == "coverDrive" || shotPlayed == "extraCoverDrive") && powerShot == false && shortestBallPickupDistance > 45)
				{
					if(slipShot == false)
					{
						int coverDriveIndex  = Random.Range(0, coverDriveCommentary.Count () );
						GameModelScript.PlayCommentarySound (coverDriveCommentary[coverDriveIndex]);
					}
				}
				else if((shotPlayed == "onDrive" || shotPlayed == "backFootOnDrive" || shotPlayed == "legGlance") && powerShot == false && shortestBallPickupDistance > 45)
				{
					if(slipShot == false)
					{
						GameModelScript.PlayCommentarySound ("OnSideShot/Played beautifully");
					}
				}
				else if((shotPlayed == "StraightDrive" || shotPlayed == "backFootStraightDrive") && powerShot == false && shortestBallPickupDistance > 50)
				{
					if(slipShot == false)
					{
						GameModelScript.PlayCommentarySound ("CoverDrive/That was a superb drive");
					}
				}
				else if(shotPlayed == "squareCut" && shortestBallPickupDistance > 45)
				{
					int commentaryIndex = Random.Range (0, 2);
					GameModelScript.PlayCommentarySound (squareCommentary[commentaryIndex]);
				}
				else if(shotPlayed == "squareDrive" && shortestBallPickupDistance > 45)
				{
					GameModelScript.PlayCommentarySound (squareCommentary[1]);
				}
			}
		}*/
	}

	public void BallAngle(float angle) {
		shotAngle = angle;
	}


	//	public void BallTiming ()//ShankarEdit
	//	{
	//		bool maySlip = false;
	//		float ballTimingStartingAngle;
	//		ballAngle = shotAngle - 180f + Random.Range(-7, 7);


	//		if (shotPlayed == "defense" || shotPlayed == "backFootDefenseHighBall" || shotPlayed == "frontFootOffSideDefense") 
	//		{
	//			ballAngle = 270 + Random.Range (-10, 10);
	//			maySlip = true;
	//			if (currentBatsmanHand == "left") {
	//				ballAngle = (180 - ballAngle) + 360;
	//				ballAngle = ballAngle % 360;
	//			}
	//		} 
	//		else if (shotPlayed == "lateCut" || shotPlayed == "reverseSweep" || shotPlayed == "lateCutLowHeight") 
	//		{
	//			ballAngle = 110 + Random.Range (-15, 15);
	//			if (currentBatsmanHand == "left") {
	//				ballAngle = (180 - ballAngle) + 360;
	//				ballAngle = ballAngle % 360;
	//			}
	//		}
	//		else if (shotPlayed == "legGlance" || shotPlayed == "legGlanceYorkerLength"  || shotPlayed == "paddleSweep" || shotPlayed == "powerfulSweepShot") 
	//		{
	//			if ((shotPlayed == "legGlance" || shotPlayed == "legGlanceYorkerLength") && squareLegGlance == true || shotPlayed == "powerfulSweepShot") {
	//				if (shotPlayed == "legGlance") {
	//					ballAngle = Random.Range (-10, 45);
	//				} else
	//					ballAngle = 45 + Random.Range (-25, 25);
	//				if (currentBatsmanHand == "left") {
	//					ballAngle = (180 - ballAngle) + 360;
	//					ballAngle = ballAngle % 360;
	//				}
	//			} else {
	//				ballAngle = 55 + Random.Range (-25, 25);
	//				if (currentBatsmanHand == "left") {
	//					ballAngle = (180 - ballAngle) + 360;
	//					ballAngle = ballAngle % 360;
	//				}
	//			}
	//		}
	//		if (shotPlayed == "defense" || shotPlayed == "backFootDefenseHighBall" || shotPlayed == "frontFootOffSideDefense") 
	//		{
	//			horizontalSpeed	= 5 + Random.Range(0, 5);
	//			ballProjectileAngle = 270;
	//			ballProjectileHeight = ballBatMeetingHeight;
	//			ballTimingFirstBounceDistance = ballBatMeetingHeight * 3;
	//			ballProjectileAnglePerSecond = (90/ballTimingFirstBounceDistance) * horizontalSpeed;
	//		}
	//		else if ((shotPlayed == "loftOffSide" ||	shotPlayed == "loftStraight" || shotPlayed == "straightDrivePowerShot" ||
	//			shotPlayed == "loftLegSide")
	//			&& (slipShot == false || playedUltraSlowMotion == true))
	//		{
	//			//ballTimingFirstBounceDistance = 70 + Random.Range (-40, 40);//100 + Random.Range (-30, 20); // shankarEdit
	//			ballTimingFirstBounceDistance = 70 + Random.Range (-20, 30); // MOHAN_AS_TODO
	//			if (powerShot) {
	//				ballTimingFirstBounceDistance = 80 + Random.Range (-15, 15);
	//			}
	//			else
	//				ballTimingFirstBounceDistance = 70 + Random.Range (-20, 30);
	//			if (playedUltraSlowMotion == true) {
	//				slipShot = false;//29march
	//				ballTimingFirstBounceDistance = Random.Range (80, 110);
	//			}
	//			horizontalSpeed = 25 + Random.Range (0, 5);	
	//			ballProjectileHeight = ballTimingFirstBounceDistance / (6 + Random.Range(-1, 2));
	//			ballTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight/ballProjectileHeight) * RAD2DEG;

	//			ballProjectileAngle = 180 + ballTimingStartingAngle;
	//			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle)/ballTimingFirstBounceDistance) * horizontalSpeed;
	//			Transform HitEffect = Instantiate (BallHitEffect, batCollider.transform.position, batCollider.transform.rotation) as Transform;
	//			HitEffect.transform.position = ball.transform.position;
	//		} 
	//		else if ((shotPlayed == "hookShot" || shotPlayed == "pullShot" || shotPlayed == "powerfulSweepShot") && (slipShot == false || playedUltraSlowMotion == true)) 
	//		{
	//			if (loft) {
	//				if (powerShot) {
	//					ballTimingFirstBounceDistance = 70 + Random.Range (-15, 15);
	//				}
	//				else
	//					ballTimingFirstBounceDistance = 50 + Random.Range (-20, 20);

	//			}
	//			else
	//				ballTimingFirstBounceDistance = 19; 
	//			if (playedUltraSlowMotion == true) {
	//				slipShot = false;//29march
	//				ballTimingFirstBounceDistance = Random.Range (70, 95);
	//			}
	//			horizontalSpeed = 25 + Random.Range (0, 5);	
	//			ballProjectileHeight = ballTimingFirstBounceDistance / (6 + Random.Range (-1, 2));
	//			ballTimingStartingAngle = Mathf.Asin (ballBatMeetingHeight / ballProjectileHeight) * RAD2DEG;
	//			ballProjectileAngle = 180 + ballTimingStartingAngle;
	//			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle) / ballTimingFirstBounceDistance) * horizontalSpeed;
	//			Transform HitEffect = Instantiate (BallHitEffect, batCollider.transform.position, batCollider.transform.rotation) as Transform;
	//			HitEffect.transform.position = ball.transform.position;
	//		}
	//		else if (shotPlayed == "squareCut"  && (slipShot == false || playedUltraSlowMotion == true))
	//		{
	//			if (loft) {
	//				if (currentBowlerType == "fast") {
	//					if (powerShot) {
	//						ballTimingFirstBounceDistance = 80 + Random.Range (-15, 15);
	//					}
	//					else
	//						ballTimingFirstBounceDistance = 70 + Random.Range (-20, 30);

	//				} else if (currentBowlerType == "spin") {
	//					if (powerShot) {
	//						ballTimingFirstBounceDistance = 60 + Random.Range (-10, 35);
	//					}
	//					else
	//						ballTimingFirstBounceDistance = 55 + Random.Range (-40, 15);
	//				}
	//			} else {
	//				ballTimingFirstBounceDistance = 19;
	//			}

	//			if (playedUltraSlowMotion == true)
	//			{
	//				slipShot = false;//29march
	//				ballTimingFirstBounceDistance = Random.Range (80, 110);
	//			}
	//			horizontalSpeed = 25 + Random.Range (0, 5);	
	//			ballProjectileHeight = ballTimingFirstBounceDistance / (6 + Random.Range(-1, 2));
	//			ballTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight/ballProjectileHeight) * RAD2DEG;
	//			ballProjectileAngle = 180 + ballTimingStartingAngle;
	//			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle)/ballTimingFirstBounceDistance) * horizontalSpeed;
	//		}
	//		else if(shotPlayed == "lateCut" || shotPlayed == "lateCutLowHeight" || shotPlayed == "reverseSweep")
	//		{
	//			if(currentBowlerType == "fast")
	//			{
	//				ballTimingFirstBounceDistance = 50 + Random.Range (-25, 15);
	//			}
	//			else if (currentBowlerType == "spin")
	//			{
	//				ballTimingFirstBounceDistance = 20 + Random.Range (-5, 10);
	//			}
	////			if (shotPlayed != "lateCut") {
	////				ballTimingFirstBounceDistance /= 4;
	////			}
	//			ballTimingFirstBounceDistance /= 4;
	//			horizontalSpeed = 25 + Random.Range (0, 5);	
	//			ballProjectileHeight = ballTimingFirstBounceDistance / (6 + Random.Range(-1, 2));
	//			if(ballBatMeetingHeight > ballProjectileHeight) {
	//				ballProjectileHeight = ballBatMeetingHeight + 0.2f;
	//			}


	//			ballTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight/ballProjectileHeight) * RAD2DEG;
	//			ballProjectileAngle = 180 + ballTimingStartingAngle;
	//			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle)/ballTimingFirstBounceDistance) * horizontalSpeed;
	//		}
	//		else if ((shotPlayed == "legGlanceYorkerLength" || shotPlayed == "paddleSweep" || shotPlayed == "legGlance") && loft) //April1
	//		{
	//			StartCoroutine (EnableTrail ());	
	//			if(currentBowlerType == "fast")
	//			{
	//				ballTimingFirstBounceDistance = 70 + Random.Range (-40, 0);
	//			}
	//			else if (currentBowlerType == "spin")
	//			{
	//				ballTimingFirstBounceDistance = 55 + Random.Range (-30, 0);
	//			}
	//			horizontalSpeed = 25 + Random.Range (0, 5);	
	//			ballProjectileHeight = ballTimingFirstBounceDistance / (6 + Random.Range(-1, 2));
	//			ballTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight/ballProjectileHeight) * RAD2DEG;
	//			ballProjectileAngle = 180 + ballTimingStartingAngle;
	//			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle)/ballTimingFirstBounceDistance) * horizontalSpeed;
	//		}
	//		else if (slipShot == true && playedUltraSlowMotion == false)
	//		{
	//			ballTimingFirstBounceDistance = 19;
	//			horizontalSpeed = 20 + Random.Range (0.0f, 4.0f); // maximum 24...
	//			ballProjectileHeight = Random.Range (2,4); // 2 or 3 meters height...
	//			float  slipBallTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight/ballProjectileHeight) * RAD2DEG;
	//			ballProjectileAngle = 180 + slipBallTimingStartingAngle;
	//			ballProjectileAnglePerSecond = ((180 - slipBallTimingStartingAngle)/ballTimingFirstBounceDistance) * horizontalSpeed;
	//		}
	//		else
	//		{
	//			horizontalSpeed = 15 + Random.Range (5, 10);
	//			ballProjectileAngle = 270;
	//			ballProjectileHeight = ballBatMeetingHeight;
	//			ballTimingFirstBounceDistance = ballBatMeetingHeight * 10;
	//			ballProjectileAnglePerSecond = (90/ballTimingFirstBounceDistance) * horizontalSpeed;
	//			//April3
	//			/*if ((shotPlayed == "legGlance" || shotPlayed == "legGlanceYorkerLength") && (Random.Range (0,5) > 2) && currentBowlerType == "fast")//April1
	//		{
	//			EnableTrail ();
	//			ballTimingFirstBounceDistance = 45 + Random.Range (-10, 20);
	//			horizontalSpeed = 25 + Random.Range (0, 5);	
	//			ballProjectileHeight = ballTimingFirstBounceDistance / (6 + Random.Range(-1, 2));
	//			ballTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight/ballProjectileHeight) * RAD2DEG;
	//			ballProjectileAngle = 180 + ballTimingStartingAngle;
	//			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle)/ballTimingFirstBounceDistance) * horizontalSpeed;
	//		}*/
	//		}

	//		ballTimingFirstBounce.transform.position = new Vector3 (ballTimingOrigin.transform.position.x + ballTimingFirstBounceDistance * Mathf.Cos (ballAngle * DEG2RAD), ballTimingFirstBounce.transform.position.y, ballTimingOrigin.transform.position.z + ballTimingFirstBounceDistance * Mathf.Sin (ballAngle * DEG2RAD));

	//		fielder10FocusGObjToCollectTheBall.transform.position = new Vector3 (ballTimingFirstBounce.transform.position.x, fielder10FocusGObjToCollectTheBall.transform.position.y, ballTimingFirstBounce.transform.position.z);

	//		float minSixDistance = DistanceBetweenTwoVector2(fielder10FocusGObjToCollectTheBall, groundCenterPoint);

	//		if(slipShot == false && loft && (shotPlayed == "hookShot" || shotPlayed == "pullShot" || shotPlayed == "powerfulSweepShot" || shotPlayed == "loftOffSide" || shotPlayed == "loftStraight" || shotPlayed == "loftLegSide" ||
	//			shotPlayed == "straightDrivePowerShot"))
	//		{
	//			StartCoroutine(EnableTrail ());
	//		}
	//		else if (shotPlayed == "squareCut" && slipShot == false && currentBowlerType == "fast")
	//		{
	//			StartCoroutine(EnableTrail ());
	//		}
	//		FixBallCatchingSpot ();

	//		// for keeper positioning to collect the ball
	//		if(ballAngle >= 90 && ballAngle <= 210)
	//		{
	//			postBattingWicketKeeperDirection = "offSide";
	//		}
	//		else if(ballAngle > 210 && ballAngle < 330)
	//		{
	//			postBattingWicketKeeperDirection = "straight";
	//		}
	//		else
	//		{
	//			postBattingWicketKeeperDirection = "legSide";
	//		}

	//	}

	public void BallTiming()//ShankarEdit
	{
		Shotangle = (Shotangle % 360);
		ballAngle = Shotangle;

		bool maySlip = false;
		bool UltraCatch = false;

		if (shotPlayed == "CoverDrive2" || shotPlayed == "CoverSlog" || shotPlayed == "LoftedCoverDrive" || shotPlayed == "OffDrive" || shotPlayed == "OffDrive3" || shotPlayed == "BackFootDrive" || shotPlayed == "LoftedOffDrive" || shotPlayed == "StraightDrive" || shotPlayed == "LoftedStraightDrive" || shotPlayed == "StraightSlog" || shotPlayed == "BackFootStraightDrive" || shotPlayed == "BackfootPush")//nija Batsman
		{
			maySlip = true;
		}
		if (shotPlayed == "CoverDrive2" || shotPlayed == "OffDrive" || shotPlayed == "OffDrive3" || shotPlayed == "BackFootDrive" || shotPlayed == "StraightDrive" || shotPlayed == "LoftedStraightDrive" || shotPlayed == "BackFootPush")//nija Batsman BackFootPush //|| shotPlayed == "BackFootStraightDrive"
		{
			maySlip = true;
			UltraCatch = true;
		}
		// slip catch / keeper catch percentage... It will happen more in 5 overs match & less in 50 overs matchs; 5% chance in 50 Overs match, 30% chance in 2 Overs match
		float slipCatchChances = 30 - (CONTROLLER.Overs[CONTROLLER.oversSelectedIndex] / 2);
		if (slipCatchChances < 5.0f)
		{
			slipCatchChances = 5.0f;
		}
		float randomVal = Random.Range(0.0f, 100.0f);
		if (battingBy == "user")
		{
			randomVal = Random.Range(0.0f, 70.0f);
		}
		// slip catch, when power shot and ball has swing...
		//	if(maySlip == true && Random.Range (0.0f,100.0f) < 30.0f && UltraSlow == false && playedUltraSlowMotion == false)
		if ((maySlip == true && randomVal < slipCatchChances && playedUltraSlowMotion == false) && DistanceBetweenTwoGameObjects(batsman, bowlingSpotGO) < 3.0f && UltraCatch == false)
		{
			slipShot = true;
			if (currentBatsmanHand == "right")
			{
				ballAngle = 105;
			}
			else
			{
				ballAngle = 75;
			}
		}

		float customBallSpeed = 1.0f;

		if ((Mathf.Abs(battingTimingMeter) <= btmPerfectValue || ballBatMeetingHeight < 0.3f) && UltraCatch == false) // if perfect timing then it will not slip or edge... && to avoid yorker length ball should not slip/edge
		{
			ballAngle = Shotangle;
			slipShot = false;
		}
		bool handleTheBall = false;
		bool playSafeShot = false;
		float fielderDodgeDistance = 13f;

		bool isGoodOrPerfectShot = false;
		if (slipShot == true)
		{
			float val = 5;
			if (UltraCatch)
			{
				val = 16f;
			}
			if (Random.Range(0.0f, 20.0f) < val) // && DistanceBetweenTwoGameObjects(batsman, bowlingSpotGO) > 2.0f
			{
				slipShot = false;
				if (currentBatsmanHand == "right")
				{
					if (UltraCatch)
					{
						ballAngle = 91 + Random.Range(0.3f, 0.7f);
					}
					else
					{
						ballAngle = 96 + Random.Range(0.5f, 1.5f);//Manoj Keeper Catch
					}
				}
				else
				{
					if (UltraCatch)
					{
						ballAngle = 89 - Random.Range(0.3f, 0.7f);
					}
					else
					{
						ballAngle = 85 - Random.Range(0.5f, 1.5f);//Manoj Keeper Catch
					}
				}

				wicketKeeperStatus = string.Empty;

				if (activeFielderNumber.Count > 0)
				{
					activeFielderNumber.Clear();
				}
			}
			// Manoj set random keeperCatch
		}


		if (shotPlayed == "DownTheTrackDefensiveShot" || shotPlayed == "FrontFootDefense" || shotPlayed == "BackFootDefense")
		{
			//defensePlayed = true;
			ballAngle = 270 + Random.Range(-15, 15);

			horizontalSpeed = 8 + Random.Range(-1.0f, 1.0f);//5
			ballProjectileAngle = 270;
			ballProjectileHeight = ballBatMeetingHeight;
			ballTimingFirstBounceDistance = ballBatMeetingHeight * 3;
			ballProjectileAnglePerSecond = (90 / ballTimingFirstBounceDistance) * horizontalSpeed;      //150 degrees
		}
		else if (handleTheBall)
		{
			ballAngle = 60f;
			horizontalSpeed = (horizontalSpeed / 2.15f);// + Random.Range (0.0f, 5f); 
			ballProjectileHeight = ballBatMeetingHeight;
			ballTimingFirstBounceDistance = 10f;
			ballProjectileAnglePerSecond = (90 / ballTimingFirstBounceDistance) * horizontalSpeed;
		}
		else if (slipShot == true)
		{
			if (currentBowlerType == "spin")
			{
				ballTimingFirstBounceDistance = 10;
				horizontalSpeed = 20 + Random.Range(0.0f, 4.0f); // maximum 24...
																 //ballProjectileHeight = Random.Range (2.0f, 3.0f); // 2 or 3 meters height...
				ballProjectileHeight = Random.Range(1.0f, 1.2f); // 2 or 3 meters height...

				float slipBallTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight / ballProjectileHeight) * RAD2DEG;
				ballProjectileAngle = 180 + slipBallTimingStartingAngle;
				ballProjectileAnglePerSecond = ((180 - slipBallTimingStartingAngle) / ballTimingFirstBounceDistance) * horizontalSpeed;
			}
			else
			{
				ballTimingFirstBounceDistance = 19;
				horizontalSpeed = 20 + Random.Range(0.0f, 4.0f); // maximum 24...
																 //ballProjectileHeight = Random.Range (2.0f, 3.0f); // 2 or 3 meters height...
				ballProjectileHeight = Random.Range(1.5f, 2.4f); // 2 or 3 meters height...
				if (UltraCatch)
				{
					ballProjectileHeight = Random.Range(1.5f, 2.0f);
				}
				float slipBallTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight / ballProjectileHeight) * RAD2DEG;
				ballProjectileAngle = 180 + slipBallTimingStartingAngle;
				ballProjectileAnglePerSecond = ((180 - slipBallTimingStartingAngle) / ballTimingFirstBounceDistance) * horizontalSpeed;
			}
		}
		//AI Improve
		else if (playSafeShot == true)
		{
			horizontalSpeed = Random.Range(25, 30) - ((Mathf.Abs(battingTimingMeter) / 100) * 1.2f);
			//ballProjectileHeight = 1.2f;
			ballBatMeetingHeight = ballTransform.position.y;
			ballProjectileHeight = ballBatMeetingHeight + 0.1f;
			ballTimingFirstBounceDistance = ballProjectileHeight * (5.0f + 5.0f * (1 - (Mathf.Abs(battingTimingMeter) / 100)));
			if (ballTimingFirstBounceDistance > fielderDodgeDistance)
			{
				ballTimingFirstBounceDistance = Random.Range(10.0f, 13.0f);
			}
			float ballTimingStartingAngle1 = Mathf.Asin(ballBatMeetingHeight / ballProjectileHeight) * RAD2DEG;
			ballProjectileAngle = 180 + ballTimingStartingAngle1;
			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle1) / ballTimingFirstBounceDistance) * horizontalSpeed;
		}
		//AI Improve
		else if (powerShot == true)
		{
			//If late cut with power shot... ball should not go to six or long distance...
			if (shotPlayed == "LateCut" || shotPlayed == "LateCut2" || shotPlayed == "LateCut3" || shotPlayed == "WideHitLow" || shotPlayed == "SquareCut")
			{
				ballTimingFirstBounceDistance = Random.Range(15.0f, 25f);
				ballTimingFirstBounceDistance *= customBallSpeed;
				horizontalSpeed = 18 + Random.Range(0, 5);
				horizontalSpeed *= customBallSpeed;
				ballProjectileHeight = ballTimingFirstBounceDistance / (6 + Random.Range(-1, 2));
			}
			//Rj misstimming special(special catches)
			else if (battingTimingMeter > 25f && battingTimingMeter < 55f && ballBatMeetingHeight < .4f && ballBatMeetingHeight > .1f && ((ballAngle > 355f && ballAngle <= 360f) ||
				ballAngle < 5f && ballAngle >= 0))
			{
				horizontalSpeed = 19.54256f;
				ballProjectileAngle = 180.7369f;
				ballProjectileHeight = 14.11436f;
				ballTimingFirstBounceDistance = 46.21344f;
				ballProjectileAnglePerSecond = 75.80608f;
				slipShot = false;
				ballToFineLeg = false;
				edgeCatch = false;
			}
			//Top edge & ball angle change...
			else if (battingTimingMeter > 30 && (shotPlayed == "LoftedPullShot" || shotPlayed == "PullShot" || shotPlayed == "HookShot"))
			{
				//topEdgeCatch = true;
				//CONTROLLER.sndController.EnqueueCommentary(6, 12, 0.1f);
				ballProjectileAnglePerSecondFactor = 2.0f;
				ballTimingFirstBounceDistance = 40;
				if (currentBowlerType == "fast" || currentBowlerType == "medium")
				{
					ballProjectileHeight = Random.Range(11.0f, 13.0f); //12;
				}
				else
				{
					ballProjectileHeight = Random.Range(7.0f, 9.0f);
				}
				horizontalSpeed = 15;
				ballAngle = ballAngle + 180 + Random.Range(35.0f, -35.0f);
			}
			//Early power shot...
			//			-7 -15
			else if (battingTimingMeter <= -btmPerfectValue)//AI Improve
			{
				horizontalSpeed = 28 - ((Mathf.Abs(battingTimingMeter) / 100) * 16);
				//horizontalSpeed *= customBallSpeed;
				if (ismissTimed == false)
				{
					ballTimingFirstBounceDistance = 55 - ((Mathf.Abs(battingTimingMeter) / 100) * 35); // 15 to 50
																									   //ballTimingFirstBounceDistance *= customBallSpeed;
					ballProjectileHeight = 8 - ((Mathf.Abs(battingTimingMeter) / 100) * 4) + Random.Range(-0.5f, 0.5f);
				}
				else
				{
					ballTimingFirstBounceDistance = 50 - ((Mathf.Abs(battingTimingMeter) / 100) * 35); // 15 to 50
																									   // ballTimingFirstBounceDistance *= customBallSpeed;
					ballProjectileHeight = 5f + ((missTimedBattingMeter / 100) * 4);
				}
			}
			//Late power shot...
			else if (battingTimingMeter >= gudTimingValue)//AI Improve
			{
				isGoodOrPerfectShot = true;
				if (ismissTimed == false)
				{
					horizontalSpeed = 28 - ((Mathf.Abs(battingTimingMeter) / 100) * 16); // 18 to 25
					ballTimingFirstBounceDistance = 70 - ((Mathf.Abs(battingTimingMeter) / 100) * 45);
					ballProjectileHeight = 12 + ((battingTimingMeter / 100) * 4);
				}
				else
				{
					horizontalSpeed = 28 - ((Mathf.Abs(missTimedBattingMeter) / 100) * 14); // 18 to 25
																							//horizontalSpeed *= customBallSpeed;

					if (ballAngle > 30 && ballAngle <= 145)
					{
						ballTimingFirstBounceDistance = 65 - ((Mathf.Abs(battingTimingMeter) / 100) * 45);
						//   ballTimingFirstBounceDistance *= customBallSpeed;
						ballProjectileHeight = 7 + ((battingTimingMeter / 100) * 4);
					}
					else
					{
						ballTimingFirstBounceDistance = 68 - ((Mathf.Abs(battingTimingMeter) / 100) * 45);
						// ballTimingFirstBounceDistance *= customBallSpeed;
						ballProjectileHeight = 8 + ((battingTimingMeter / 100) * 4);
					}
				}
			}
			//Normal power shot...
			else
			{
				isGoodOrPerfectShot = true;
				horizontalSpeed = 28 - ((Mathf.Abs(battingTimingMeter) / 100) * 16); // 18 to 25
				bool isCleanPerfect = false;
				float cleanPerfectVal = (btmPerfectValue / 100) * 35; // 30 % of perfectval
				if (Mathf.Abs(battingTimingMeter) <= (cleanPerfectVal + 0.5f)) //AI Improve
				{
					isCleanPerfect = true;
				}
				if (Random.Range(0, 10) < 6)
				{
					isCleanPerfect = true;
				}
				if (batsmanSkill/100f < 0.65f)
				{
					isCleanPerfect = false;
				}

				bool isPerfect = false;

				if (Mathf.Abs(battingTimingMeter) <= btmPerfectValue) //AI Improve
				{
					isPerfect = true;
				}
				if (isPerfect)
				{
					if (ballAngle > 30 && ballAngle <= 145)
					{
						ballTimingFirstBounceDistance = 90 - ((Mathf.Abs(battingTimingMeter) / 100) * 80) + (10 - (Mathf.Abs(battingTimingMeter) / 10) * 20);
					}
					else if (isCleanPerfect)
					{
						ballTimingFirstBounceDistance = 125 - ((Mathf.Abs(battingTimingMeter) / 100) * 80) + (20 - (Mathf.Abs(battingTimingMeter) / 10) * 20);
						if (ballTimingFirstBounceDistance >= 90)
							ballTimingFirstBounceDistance = 90;
					}
					else
					{
						ballTimingFirstBounceDistance = 100 - ((Mathf.Abs(battingTimingMeter) / 100) * 80) + (20 - (Mathf.Abs(battingTimingMeter) / 10) * 20);
					}
				}
				else
				{
					if (ballAngle > 30 && ballAngle <= 145)
					{
						ballTimingFirstBounceDistance = 73 - ((Mathf.Abs(battingTimingMeter) / 100) * 80) + (14 - (Mathf.Abs(battingTimingMeter) / 10) * 20);
					}
					else
					{
						ballTimingFirstBounceDistance = 75 - ((Mathf.Abs(battingTimingMeter) / 100) * 80) + (15 - (Mathf.Abs(battingTimingMeter) / 10) * 20);
					}
				}

				ballProjectileHeight = Random.Range(8, 12) + ((battingTimingMeter / 100) * 4) + Random.Range(-2.0f, 4.0f);

				if (isPerfect)
				{
					if (isCleanPerfect == false)
					{
						ballProjectileHeight = Random.Range(8, 12) + ((battingTimingMeter / 100) * 4) + Random.Range(-2.0f, 4.0f);
					}
					else
					{
						ballProjectileHeight = Random.Range(10, 14) + ((battingTimingMeter / 100) * 4) + Random.Range(-2.0f, 4.0f);
						ballProjectileHeight += 25;

					}
				}
				else
				{
					ballProjectileHeight = Random.Range(8, 11) + ((battingTimingMeter / 100) * 4) + Random.Range(-2.0f, 3.0f);
				}
			}
			if(ballProjectileHeight > 10f)
            {
				ballProjectileHeight = 10f;
			}
			float ballTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight / ballProjectileHeight) * RAD2DEG;
			ballProjectileAngle = 180 + ballTimingStartingAngle;
			ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle) / ballTimingFirstBounceDistance) * horizontalSpeed;
		}
		else if (powerShot == false && edgeCatch == false)
		{
			horizontalSpeed = 24 - ((Mathf.Abs(battingTimingMeter) / 100) * 12); //22
																				 //aimoveandplay
			if (CONTROLLER.BattingTeamIndex == CONTROLLER.opponentTeamIndex)
			{
				horizontalSpeed = Random.Range(24, 29) - ((Mathf.Abs(battingTimingMeter) / 100) * 12);
			}
			//aimoveandplay
			horizontalSpeed *= customBallSpeed;
			if (horizontalSpeed < 18)//hardmode
			{
				horizontalSpeed = 18.0f;
			}
			if (ballBatMeetingHeight < 0.6f)
			{
				horizontalSpeed += horizontalSpeed * 0.2f;
			}
			if (ballAngle > 190 && ballAngle < 350) // straight shot, so make the ball roll on the ground as much as possible...
			{
				ballProjectileAngle = 270;
				ballProjectileHeight = ballBatMeetingHeight;
				if (currentBowlerType == "fast")
				{
					ballTimingFirstBounceDistance = ballBatMeetingHeight * 7;
				}
				else
				{
					ballTimingFirstBounceDistance = ballBatMeetingHeight * 10;
				}

				ballProjectileAnglePerSecond = (90 / ballTimingFirstBounceDistance) * horizontalSpeed;
			}
			else
			{
				if (shotPlayed == "DilsonScoop" || shotPlayed == "McCullum_UnorthodoxShot")
				{
					ballProjectileHeight = Random.Range(1.5f, 2.2f);//nija batsman
				}
				else
				{
					// rest of the shots...
					ballProjectileHeight = ballBatMeetingHeight * (2 + (1 - (Mathf.Abs(battingTimingMeter) / 100)));//2
					if (ballProjectileHeight > 1.5f && ballAngle > 210 && ballAngle < 330)
					{
						ballProjectileHeight = 1.5f;
						ballBatMeetingHeight = 1.5f;
					}
				}
				if (shotPlayed == "SquareCut11" || shotPlayed == "SquareCut12")
				{
					ballTimingFirstBounceDistance *= 2.5f;
					horizontalSpeed += horizontalSpeed * 0.2f;
				}
				ballTimingFirstBounceDistance = ballProjectileHeight * (5.0f + 5.0f * (1 - (Mathf.Abs(battingTimingMeter) / 100))); // 10.0f

				float ballTimingStartingAngle = Mathf.Asin(ballBatMeetingHeight / ballProjectileHeight) * RAD2DEG;
				ballProjectileAngle = 180 + ballTimingStartingAngle;
				ballProjectileAnglePerSecond = ((180 - ballTimingStartingAngle) / ballTimingFirstBounceDistance) * horizontalSpeed;
			}
			//RjEdit need more wickets 
			if (!UltraCatch && !edgeCatch && !slipShot)
			{
				if ((battingTimingMeter < -30 && battingTimingMeter > -50) || (battingTimingMeter > 30 && battingTimingMeter < 50))
				{
					if (ballAngle < 185 && ballAngle > 170f && ballBatMeetingHeight <= .5f && ballBatMeetingHeight > .12f)
					{
						int i = Random.Range(0, 5);
						if (i != 0 && i != 5)
						{
							float[] bANG = new float[] { 178f, 179f, 181f, 182f, 183f };
							ballAngle = bANG[Random.Range(0, bANG.Length)];
							horizontalSpeed = 20f;
							ballProjectileAngle = 187.496f;
							float[] pHeight = new float[] { 1.8f, 1.5f };
							ballProjectileHeight = pHeight[Random.Range(0, pHeight.Length)];
							ballTimingFirstBounceDistance = 35.21587f;
							ballProjectileAnglePerSecond = 92.85461f;
						}
					}
				}

				else if (((battingTimingMeter < -50 && battingTimingMeter > -80) || (battingTimingMeter > 50 && battingTimingMeter < 80))
					&& ballBatMeetingHeight <= .5f && ballBatMeetingHeight > .2f)
				{

					int i = Random.Range(0, 10);
					if (i == 0 || i == 5 || i == 9 || i == 3)
					{
						int j = Random.Range(0, 3);
						if (j == 0)
						{
							horizontalSpeed = 22f;
							ballProjectileAngle = 187.496f;
							ballProjectileHeight = 2.1f;
							ballTimingFirstBounceDistance = 35.21587f;
							ballProjectileAnglePerSecond = 92.85461f;
						}
						else if (j == 1)
						{
							horizontalSpeed = 20f;
							ballProjectileAngle = 187.496f;
							ballProjectileHeight =/* 1.8f;*/1.5f;
							ballTimingFirstBounceDistance = 35.21587f;
							ballProjectileAnglePerSecond = 92.85461f;							
						}
						else if (j == 2)
						{
							horizontalSpeed = 20f;
							ballProjectileAngle = 187.496f;
							ballProjectileHeight = 1.8f;
							ballTimingFirstBounceDistance = 35.21587f;
							ballProjectileAnglePerSecond = 92.85461f;
						}
					}

				}

			}
		}

		/*horizontalSpeed = 24.97018f;
		//ballAngle = 303.7872f;
		ballTimingFirstBounceDistance = 100f;
		ballProjectileAngle = 181.9553f;
		ballProjectileAnglePerSecond = 82.7023f;
		ballProjectileHeight = 7.180584f;

		Debug.LogError("horizontalSpeed:::"+ horizontalSpeed+"::"+ ballProjectileAngle+"::"+ ballProjectileHeight);
		Debug.LogError("ballTimingFirstBounceDistance:::" + ballTimingFirstBounceDistance + "::"+ ballProjectileAnglePerSecond);*/

		nextPitchDistance = ballTimingFirstBounceDistance;

		ballTimingFirstBounce.transform.position = new Vector3(ballTimingOrigin.transform.position.x + ballTimingFirstBounceDistance * Mathf.Cos(ballAngle * DEG2RAD), ballTimingFirstBounce.transform.position.y, ballTimingOrigin.transform.position.z + ballTimingFirstBounceDistance * Mathf.Sin(ballAngle * DEG2RAD));

		fielder10FocusGObjToCollectTheBall.transform.position = new Vector3(ballTimingFirstBounce.transform.position.x, fielder10FocusGObjToCollectTheBall.transform.position.y, ballTimingFirstBounce.transform.position.z);

		float minSixDistance = DistanceBetweenTwoVector2(fielder10FocusGObjToCollectTheBall, groundCenterPoint);

		//if (slipShot == false && loft && (shotPlayed == "hookShot" || shotPlayed == "pullShot" || shotPlayed == "powerfulSweepShot" || shotPlayed == "loftOffSide" || shotPlayed == "loftStraight" || shotPlayed == "loftLegSide" ||
		//	shotPlayed == "straightDrivePowerShot"))
		//{
		//	StartCoroutine(EnableTrail());
		//}
		//else if (shotPlayed == "squareCut" && slipShot == false && currentBowlerType == "fast")
		//{
		//}
		StartCoroutine(EnableTrail());
		FixBallCatchingSpot();
		//ManojAdded
		if (ballAngle > 255 && ballAngle < 285)
		{
			speed = 5.2f;
		}
		//ManojAdded
		// for keeper positioning to collect the ball
		if (ballAngle >= 90 && ballAngle <= 210)
		{
			postBattingWicketKeeperDirection = "offSide";
		}
		else if (ballAngle > 210 && ballAngle < 330)
		{
			postBattingWicketKeeperDirection = "straight";
		}
		else
		{
			postBattingWicketKeeperDirection = "legSide";
		}
	}
	
	public void FixBallCatchingSpot ()
	{
		ballCatchingSpot.transform.position = new Vector3 (ballTimingOrigin.transform.position.x + (ballTimingFirstBounceDistance - ballPreCatchingDistance) * Mathf.Cos (ballAngle * DEG2RAD), ballCatchingSpot.transform.position.y, ballTimingOrigin.transform.position.z + (ballTimingFirstBounceDistance - ballPreCatchingDistance) * Mathf.Sin (ballAngle * DEG2RAD));

		// find if ball catching spot is over the boundary line...
		while (DistanceBetweenTwoVector2 (groundCenterPoint, ballCatchingSpot) > (groundRadius - 2)) {
			ballPreCatchingDistance++;
			ballCatchingSpot.transform.position = new Vector3 (ballTimingOrigin.transform.position.x + (ballTimingFirstBounceDistance - ballPreCatchingDistance) * Mathf.Cos (ballAngle * DEG2RAD), ballCatchingSpot.transform.position.y, ballTimingOrigin.transform.position.z + (ballTimingFirstBounceDistance - ballPreCatchingDistance) * Mathf.Sin (ballAngle * DEG2RAD));
		}
	}

	public void FreezeTheBowlingSpot ()
	{
        if (runnerStandingAnimationId == 1)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner1_GettingReady");
			}
			else
			{
				runnerScript._playAnimation("Runner1_GettingReady_Left");
			}
			runnerScript.setAnimationSpeed(0.9f);
		}
		else if (runnerStandingAnimationId == 2)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner2_GettingReady");
			}
			else
			{
				runnerScript._playAnimation("Runner2_GettingReady_Left");
			}
			runnerScript.setAnimationSpeed(0.9f);
		}

		//nonStrikerStatus = "getReady";
		runnerScript.status = "getReady";
		nonStickerStatus = "getReady";//25march
		batsmanCanMoveLeftRight = false;
		SetStrikerFocusTapping();

		if (userBowlingSpotSelected == false) // if user bowling and spot selection is not made...
		{
			userBowlerCanMoveBowlingSpot = false;
			bowlingSpotScript.FreezeBowlingSpot ();
		}

		if(GameModelScript != null)
		{
			GameModelScript.EnableMovement (false);
		}	
	}

	public void ReleaseTheBall ()
	{
		applyBallFriction = false;
		ballReleased = true;
		canSwipeNow = true;
		ballStatus = "bowling";
		FindBowlingParameters ();
		ballReleasedTime = Time.time;
		ShowBall (true);
		updateBattingTimingMeterNeedle = true;

        if (GameModelScript.CanShowTutorial() && CONTROLLER.currentMatchBalls == 0)
		{
			AdIntegrate.instance.SetTimeScale(0.5f);
		}
		else if (GameModel.isGamePaused == false)
		{
			AdIntegrate.instance.SetTimeScale(1f);
		}

		if (GameModelScript != null)
		{
			GameModelScript.ShowBatsmanMoveTutorial (false);//shankar 09April
		}
		ballSkinRigidBody.WakeUp();
		ActivateColliders (true);
		bowlerBall.GetComponent<Renderer>().enabled = false;
		ZoomCameraToBatsman ();


		if (battingBy == "computer")
		{
			//GetComputerBattingKeyInput ();
		}
		/*if(CONTROLLER.BattingTeamIndex == CONTROLLER.myTeamIndex)
	{
		action = 3;
	}
	else
	{
		action = 3;
	}*/
		action = 3; 
		if(noBall == true)
		{
			if(currentBowlerType == "fast")
			{
				bowler.transform.position = new Vector3 (bowler.transform.position.x, bowler.transform.position.y, -6.25f);
			}
			if(currentBowlerType == "spin")
			{
				bowler.transform.position = new Vector3 (bowler.transform.position.x, bowler.transform.position.y, 0.9f);
			}
		}

		if(GameModelScript != null)
		{
			GameModelScript.EnableShotSelection (true);
		}
		if(CONTROLLER.gameMode!="multiplayer")
			Scoreboard.instance.HideButtonsWhenShotMade();
	}


	public void  ZoomCameraToPitch ()
	{
		//strikerScript._playAnimation("Idle4");
		//runnerScript._playAnimation("Idle4");

		GameObject fielderGO;
		if(slipFielderWarmUpAction == false)
		{
			fielderGO = fielder[1] as GameObject;
			fielderGO.GetComponent<Animation>().Play("getReadyInSlip");
			fielderGO.GetComponent<Animation>()["getReadyInSlip"].speed = 1 + Random.Range(0.0f, 0.5f);
		}
		if(slipFielder2WarmUpAction == false)
		{
			fielderGO = fielder[2] as GameObject;
			fielderGO.GetComponent<Animation>().Play("getReadyInSlip");
			fielderGO.GetComponent<Animation>()["getReadyInSlip"].speed = 2 + Random.Range(0, 1);
		}
	}

	public void ZoomCameraToBowler ()
	{
		if (GameModelScript != null && GameModelScript.CanShowTutorial())
		{
			GameModelScript.ShowBatsmanMoveTutorial(true);
		}
		CONTROLLER.CurrentPage = "ingame";
		ZoomCameraToBowlerOnComplete ();
	}

	public void ZoomCameraToBowlerOnComplete ()
	{
        AudioPlayer.instance.StopIntroSound();

        ActivateColliders(false);
		batsmanTriggeredShot = false;
		canMakeShot = false;
		bowlerIsWaiting = true;
		currentBallStartTime = Time.time;
		action = 0;  // Debug.Log ("ZoomCameraToBowlerOnComplete"); 
	} 

	public void ZoomCameraToBatsman ()
	{
		//var mainCameraZoomToBatsmanPosition : Vector3 = mainCameraZoomInPosition;
		//mainCameraZoomToBatsmanPosition.z += 2;
		//iTween.MoveTo(mainCamera.gameObject, {"position":mainCameraZoomToBatsmanPosition, "time":0.3, "easetype":"easeInOutSine"});
	}

	public void ZoomCameraToWicketKeeper ()
	{
		/*closeUpCamera.transform.position = wicketKeeper.transform.position;
	closeUpCamera.transform.position.y = 2;
	closeUpCamera.transform.position.z -= 8;
	closeUpCamera.transform.eulerAngles.y = 0;
	closeUpCamera.enabled = true;*/
	}

	public void InitCamera ()
	{
		mainCamera.transform.position = mainCameraPitchPosition;
		mainCamera.transform.eulerAngles = mainCameraInitRotation;
	}



	public void BatsmanWaiting ()
	{
		if (Time.time > currentBallStartTime + batsmanWaitSeconds)
		{
			SetStrikerPreIdle();
			SetStrikerTappingIdlePlayQueued();

			if (currentBowlerType == "fast")
			{
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("getReady");
			}
			else
			{
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("getReadyForSpin");
			}
			action = 1; 	
		}
	}


	public void BlockBowlerSideChange ()
	{
		bowlerIsWaiting = false;
	}

	public  IEnumerator DisableLoft() {
		if(CONTROLLER .gameMode !="multiplayer")
			yield return new WaitForSeconds (3f);
		else
			yield return new WaitForSeconds (4f);
		loftBtn.gameObject.SetActive (false);
		loftBtn2.gameObject.SetActive (false);
	}

	public void BowlerWaiting ()
	{			
		if(Time.time > currentBallStartTime + bowlerWaitSeconds && bowler != null)
		{			
			StartCoroutine (DisableLoft ());
			SetSwipeHighlightRenderState(false);
			bowler.GetComponent<Animation>().CrossFade ("BowlerRunupEdit", 3);
			bowler.GetComponent<Animation>()["BowlerRunupEdit"].speed = 1;

			FindNewBowlingSpot ();
			batsmanCanMoveLeftRight = true;
			userBowlerCanMoveBowlingSpot = true;
			//		bowlerIsWaiting = false;
			Scoreboard.instance.ShowChallengeTitle ();
			Scoreboard.instance.TargetToWin ();
			action = 2;	

			CameraFlashStart (15, 0.4f, stadiumRotationAngle); // 30, 0.2

			if(GameModelScript != null)
			{
				GameModelScript.EnableMovement (true);
			}
		}
	}

	IEnumerator _wait(float time)
	{
		yield return new WaitForSeconds (time );//yyy
	}
	
	private Vector3 NewVector3(float x, float y, float z)
	{
		_newVector3.x = x;
		_newVector3.y = y;
		_newVector3.z = z;
		return _newVector3;
	}
	//ManojAdded
	float radius = 5; //60
	float speed = 1.25f; //60
	Vector3 midpoint = Vector3.zero;
	//ManojAdded
	public void LookForMainCameraTopDownView ()
	{
		if(ballTransform.position.z > outOfPitch.transform.position.z && ballStatus == "bowling" && cameraToKeeper == false)
		{
			cameraToKeeper = true;
			ActivateColliders (false);
			/*batsman.GetComponent<Animation>()*///batsmanAnimationComponent[batsmanAnimation].speed = 1;
			// if WK is diving, show cut screen with 50% random times...
			if(wicketKeeperOppositeLength > 1 && Random.Range(0, 10) > 5)
			{
				//ZoomCameraToWicketKeeper ();
			}
		}
		else if (ballStatus == "shotSuccess" && mainCameraOnTopDownView == false && ballStatus != "bowled"
			&& shotPlayed != "defense" && shotPlayed != "backFootDefenseHighBall" && shotPlayed != "frontFootOffSideDefense"
			&& shotPlayed != "DownTheTrackDefensiveShot" && shotPlayed != "FrontFootDefense" && shotPlayed != "BackFootDefense")
		{
			//StartCoroutine (_wait(0.5f));
			mainCameraOnTopDownView = true;
			topDownViewStartTime = Time.time;
		}
		/*if(mainCameraOnTopDownView == true && ballStatus == "shotSuccess")
		{
			if (currentBallNoOfRuns != 6 && currentBallNoOfRuns != 4)
			{
				mainCamera.transform.LookAt(ball.transform); //kavin
			}
			if(Time.time < topDownViewStartTime + topDownViewZoomingSecs)
			{
				// targetted FOV = 60, POS (0,10,-10), Rotation = (25, 0, 0)
				//mainCamera.transform.position.x = ball.transform.position.x; // new...
				//mainCamera.transform.position.y += 3 * Time.deltaTime;
				//mainCamera.transform.position.z += 28 * Time.deltaTime; // new...
				//mainCamera.transform.eulerAngles.x += 17 * Time.deltaTime;
				//mainCamera.fieldOfView += (52 * Time.deltaTime); // FOV = 60, initial view = 8;
				//yield WaitForSeconds (0.5);
				if (shotPlayed != "lateCut"  && shotPlayed != "reverseSweep" && shotPlayed != "lateCutLowHeight")
				{
					//followMainCamera ();
				}
			}
			else
			{
				if(ballStatus == "shotSuccess" && sideCameraSelected == false) // not "throw" || "bowled" || ""
				{
					enableSideCamera ();
				}
				float rightCameraBallDistance = (ball.transform.position - rightSideCamera.transform.position).magnitude;
				if(rightCameraBallDistance > 25 && rightCameraBallDistance < 90 && ballOnboundaryLine == false)//camcam
				{
					rightSideCamera.fieldOfView = 50 - rightCameraBallDistance/2 + 15;
					//rightSideCamera.fieldOfView = 50 - rightCameraBallDistance/4 + 15;
				}
				float leftCameraBallDistance  = (ball.transform.position - leftSideCamera.transform.position).magnitude;
				if(leftCameraBallDistance > 25 && leftCameraBallDistance < 90 && ballOnboundaryLine == false)
				{
					leftSideCamera.fieldOfView = 50f - leftCameraBallDistance/2f + 15f ;
					//leftSideCamera.fieldOfView = 50 - leftCameraBallDistance/4 + 15;
				}
				float straightCameraBallDistance = (ball.transform.position - straightCamera.transform.position).magnitude;
				if(straightCameraBallDistance > 25 && straightCameraBallDistance < 90 && ballOnboundaryLine == false)
				{
					straightCamera.fieldOfView = 50 - straightCameraBallDistance/2 + 10;
					//straightCamera.fieldOfView = 50 - straightCameraBallDistance/4 + 10;
				}
				if (leftSideCamera.enabled == true)
			{
				leftSideCamera.transform.LookAt(ballTrail.transform);
			}
			else if (rightSideCamera.enabled == true)
			{
				rightSideCamera.transform.LookAt(ballTrail.transform);
			}
			else if (straightCamera.enabled == true)
			{
				straightCamera.transform.LookAt(ballTrail.transform);
				}
			}
			}*/
		/*if (slipShot == true && replayMode == false && ballPickedByFielder == false)
		{
			mainCamera.transform.position += new Vector3(0, 0, 3 * Time.deltaTime);
			mainCamera.transform.LookAt(ballRayCastReferenceGOTransform);
		}
		else if (slipShot == true && replayMode == false && ballPickedByFielder == true)
		{
			mainCamera.transform.position -= new Vector3(0, 0, 3 * Time.deltaTime);
			mainCamera.transform.LookAt(ballRayCastReferenceGOTransform);
		}*/
		if (mainCameraOnTopDownView == true && Time.time > topDownViewStartTime + topDownViewZoomingSecs)
		{
			/*if (Time.time < topDownViewStartTime + topDownViewZoomingSecs)
			{
				mainCamera.transform.position = new Vector3(ballTransform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
				mainCamera.transform.position += new Vector3(0, 3 * Time.deltaTime, 28 * Time.deltaTime);
				mainCamera.transform.eulerAngles += new Vector3(17 * Time.deltaTime, 0, 0);
				mainCamera.fieldOfView += (52 * Time.deltaTime);
				mainCamera.transform.LookAt(ballTransform);
			}
            else
            {*/			
			/*if(mainCameraOnTopDownView == true && Time.time > topDownViewStartTime + topDownViewZoomingSecs+lookAtBallTime)
            {
				targetPostition = new Vector3(ballTransform.position.x, ballTransform.position.y, ballTransform.position.z);
				mainCamera.transform.LookAt(targetPostition);
				return;
            }*/
				if(DistanceBetweenTwoGameObjects(mainCamera.gameObject, groundCenterPoint) < camMaxDist && ballOnboundaryLine == false)
                {
				/*if (ballAngle >= 90 && ballAngle <= 250)
				{
					mainCamera.transform.position = NewVector3(ballTransform.position.x, 3, ballTransform.position.z) + NewVector3(15f, 0, 10f);
				}
				else if(ballAngle > 250 && ballAngle <= 290)
				{
					mainCamera.transform.position = NewVector3(ballTransform.position.x, 3.5f, ballTransform.position.z) + NewVector3(5, 0, 10f);
				}
				else
				{
					mainCamera.transform.position = NewVector3(ballTransform.position.x, 3, ballTransform.position.z) - NewVector3(-2f, 0, -20f);
				}*/

				//ManojAdded
				#region Logic02
				
				
				if (ballAngle > 255 && ballAngle < 285)
				{
					//speed = 2.2f;
				}
				else
				{
					
				}
				radius = 25.0f;
				//float angle = (ballAngle - float.Parse(AngTxt.text)); //30.0f
				float x = Mathf.Cos(ballAngle * DEG2RAD) * radius;
				float z = Mathf.Sin(ballAngle * DEG2RAD) * radius;
				Vector3 pos = groundCenterPoint.transform.position + NewVector3(x, movingCamY, z);
				var step = speed * Time.deltaTime; // calculate distance to move
				mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, pos, step);
				//mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, pos, 50);
				if (ballTransform.position.y < 6f)
				{
					targetPostition = ballTransform.position;
				}
				else
				{
					targetPostition = new Vector3(ballTransform.position.x, 6f, ballTransform.position.z);
				}
				//targetPostition = new Vector3(ballTransform.position.x, mainCamera.transform.position.y, ballTransform.position.z);

				mainCamera.transform.LookAt(targetPostition);
				//radius *= 2.25f; //1.1
				speed *= 1.025f;//1.05f;
				if(speed > 14) //14
                {
					speed = 14;
				}
				//ManojAdded
				#endregion
				#region Logic01
				/*float radius = float.Parse(DistTxt.text); //60
				float angle; //30.0f
				if (ballAngle > 90 && ballAngle < 270)
                {
					angle = (ballAngle + float.Parse(AngTxt.text)); //30.0f
				}
                else
                {
					angle = (ballAngle - float.Parse(AngTxt.text)); //30.0f
				}

				//float angle = (ballAngle - float.Parse(AngTxt.text)); //30.0f
				float x = Mathf.Cos(angle * DEG2RAD) * radius;
				float z = Mathf.Sin(angle * DEG2RAD) * radius;
				mainCamera.transform.position = ballTimingOrigin.transform.position + NewVector3(x, movingCamY, z);

				mainCamera.fieldOfView -= (dampCamSpeed * Time.deltaTime);
				if (mainCamera.fieldOfView <= maxFOV)
				{
					mainCamera.fieldOfView = maxFOV;
				}*/
				#endregion
				/*if (ballAngle > 0 && ballAngle < 90)//Fine-leg
					{
						//mainCamera.transform.position = NewVector3(1f, movingCamY, 0f);
						mainCamera.fieldOfView -= (dampCamSpeed * Time.deltaTime);
						if (mainCamera.fieldOfView <= maxFOV)
						{
							mainCamera.fieldOfView = maxFOV;
						}
					}
					else if (ballAngle >= 290 && ballAngle < 360)//Mid-Wicket
					{
						//mainCamera.transform.position = NewVector3(1f, movingCamY, -11f);
						mainCamera.fieldOfView -= (dampCamSpeed * Time.deltaTime);
						if (mainCamera.fieldOfView <= maxFOV)
						{
							mainCamera.fieldOfView = maxFOV;
						}
					}
					else if (ballAngle >= 90 && ballAngle < 180)//Thirdman && Gully point
					{
						//mainCamera.transform.position = NewVector3(-1f, 3f, -20f);
						//mainCamera.transform.position = NewVector3(-1f, movingCamY, 0f);
						mainCamera.fieldOfView -= (dampCamSpeed * Time.deltaTime);
						if (mainCamera.fieldOfView <= maxFOV)
						{
							mainCamera.fieldOfView = maxFOV;
						}
					}
					else if (ballAngle >= 180 && ballAngle < 250)//Covers && Extra Covers
					{
					//mainCamera.transform.position = NewVector3(-1f, movingCamY, -11f);
					mainCamera.fieldOfView -= (dampCamSpeed * Time.deltaTime);
						if (mainCamera.fieldOfView <= maxFOV)
						{
							mainCamera.fieldOfView = maxFOV;
						}
					}
					else if (ballAngle >= 250 && ballAngle < 270)//Straight(LongOff)
					{
						//mainCamera.transform.position = NewVector3(-1f, 3f, -20f);
						//mainCamera.transform.position = NewVector3(-25f, movingCamY, -25f);
						mainCamera.fieldOfView -= (dampCamSpeed * Time.deltaTime);
						if (mainCamera.fieldOfView <= maxFOV)
						{
							mainCamera.fieldOfView = maxFOV;
						}
					}
					else//Straight(LongOn)
					{
						///mainCamera.transform.position = NewVector3(25f, movingCamY, -25f);
						mainCamera.fieldOfView -= (dampCamSpeed * Time.deltaTime);
						if (mainCamera.fieldOfView <= maxFOV)
						{
							mainCamera.fieldOfView = maxFOV;
						}
					}*/
			}
				//ManojCommented
            /*if (CamShouldNotFollowBallY)
				{
					targetPostition = new Vector3(ballTransform.position.x, 0.05f, ballTransform.position.z);
					mainCamera.transform.LookAt(targetPostition);
				}
				else
				{
				if (ballTransform.position.y >= 0.05f)
				{
					targetPostition = new Vector3(ballTransform.position.x, ballTransform.position.y, ballTransform.position.z);
				}
                else
                {
					targetPostition = new Vector3(ballTransform.position.x, 0.05f, ballTransform.position.z);
				}
					mainCamera.transform.LookAt(targetPostition);
					//mainCamera.transform.LookAt(ballTransform);
				}

			*/
				//mainCamera.transform.LookAt(ballTransform);
			//}
			/*else
			{
				if (ballStatus == "shotSuccess" && sideCameraSelected == false)
				{
					if (ballAngle >= 90 && ballAngle <= 270)
					{
						rightSideCamera.enabled = true;
					}
					else
					{
						leftSideCamera.enabled = true;
					}
					sideCameraSelected = true;
					mainCamera.enabled = false;
				}
				float rightCameraBallDistance = (ballTransform.position - rightSideCamera.transform.position).magnitude;
				if (rightCameraBallDistance > 25 && rightCameraBallDistance < 90)
				{
					rightSideCamera.fieldOfView = 50 - rightCameraBallDistance / 2 + 10;
				}
				float leftCameraBallDistance = (ballTransform.position - leftSideCamera.transform.position).magnitude;
				if (leftCameraBallDistance > 25 && leftCameraBallDistance < 90)
				{
					leftSideCamera.fieldOfView = 50 - leftCameraBallDistance / 2 + 10;
				}
			}*/
		}
		else
        {
			if (ballStatus == "shotSuccess" && ballStatus != "bowled")
			{
				/*if ((ballAngle < 360 && ballAngle > 290) || (ballAngle > 0 && ballAngle < 90))//LegSide
				{
					mainCamera.transform.RotateAround(batsman.transform.position, Vector3.down, 50 * Time.deltaTime);
				}
				else if (ballAngle > 90 && ballAngle < 270)//OffSide
				{
					mainCamera.transform.RotateAround(batsman.transform.position, Vector3.up, 50 * Time.deltaTime);
				}*/
				if (ballTransform.position.y >= 0.05f)
				{
					targetPostition = new Vector3(ballTransform.position.x, mainCamera.transform.position.y, ballTransform.position.z);
				}
				else
				{
					targetPostition = new Vector3(ballTransform.position.x, 0.05f, ballTransform.position.z);
				}
				//mainCamera.transform.LookAt(targetPostition);
			}
		}
	}

	void enableSideCamera() {
		float ballDistance = DistanceBetweenTwoVector2 (stump1Crease, ball);

		if ((loft && ballDistance > 20f) || (!loft && ballDistance > 15f)) {
			if (ballAngle >= 200 && ballAngle <= 320) {
				straightCamera.enabled = false;
				leftSideCamera.enabled = true;
				rightSideCamera.enabled = false;
				sideCameraSelected = true;
				mainCamera.enabled = false;
			} else if (ballAngle >= 0 && ballAngle <= 130) {
				straightCamera.enabled = true;
				leftSideCamera.enabled = false;
				rightSideCamera.enabled = false;
				sideCameraSelected = true;
				mainCamera.enabled = false;
			} else {
				rightSideCamera.enabled = true;
				leftSideCamera.enabled = false;
				straightCamera.enabled = false;
				sideCameraSelected = true;
				mainCamera.enabled = false;
			}
		}
		else {
			mainCamera.enabled = true;
			sideCameraSelected = false;
		}

	}

	public void followMainCamera ()
	{
		mainCamera.transform.position = new Vector3 (ballTransform.position.x, mainCamera.transform.position.y + (1 * Time.deltaTime), ballTransform.position.z + 10);


		Quaternion rotation = Quaternion.LookRotation (ballTransform.position - mainCamera.transform.position);
		mainCamera.transform.rotation = Quaternion.Slerp (mainCamera.transform.rotation, rotation, Time.deltaTime * 16);
	}
	
	public void ScanForBoundaryOrSix ()
	{
		float xDiff = ballTransform.position.x - groundCenterPoint.transform.position.x;
		float zDiff = ballTransform.position.z - groundCenterPoint.transform.position.z;
		float ball2GroundMidDistance = Mathf.Sqrt (xDiff * xDiff + zDiff * zDiff);

		if(ball2GroundMidDistance > groundRadius && ballOnboundaryLine == false && ballStatus != "bowled") // ground radius is 68.4...
		{
			FinalPoint = new Vector3(ballTransform.position.x, 0.1f, ballTransform.position.z);//Mantis id - 0026686

			ballOnboundaryLine = true;
			canTakeRun = false;
			CheckForRopeLineBallPhysics();
			boardCollider.SetActive (true);
			billBoardCollider.SetActive (true);
			stadiumCollider.enabled = true;
			//ball.rigidbody.WakeUp();
			CustomRayCastForBowlingBallMovement ();

			if (GameModelScript != null)
			{
				GameModelScript.EnableRun(false);

			}			

			stayStartTime = Time.time;
			ballBoundaryReflection = false;		
			boundaryAction = "boundary";
			
			if (canBe4or6 == 6)
			{
				currentBallNoOfRuns = 6;
                AudioPlayer.instance.StopBallTravelSound();
				AudioPlayer.instance.playFourSixCommentary(6);
                AudioPlayer.instance.PlayTheCrowdSound("sixcrowd");
				GameModelScript.InitAnimation(4);
			}
			else
			{
                AudioPlayer.instance.StopBallTravelSound();
				AudioPlayer.instance.playFourSixCommentary(4);
                AudioPlayer.instance.PlayTheCrowdSound("FourCrowd");
				currentBallNoOfRuns = 4;
				GameModelScript.InitAnimation(3);
			}

			if (wicketKeeperStatus == "catchMissed") // wide ball and four ...
			{
				AudioPlayer.instance.PlayTheCrowdSound("BallMissingCrowd");
				boundaryAction = "wideAndBoundary";
				currentBallNoOfRuns = 5;
			}
		}
		if(boundaryAction == "boundary")
		{
			if(stayStartTime + 1.55 + timeBetweenBalls < Time.time)
			{
				boundaryAction = "";
				if(GameModelScript != null)
				{
					//DisableTrail ();

					if (noBall == true)
					{
						GameModelScript.UpdateCurrentBall(0, 1, currentBallNoOfRuns, 1, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, true);
					}
					else
					{
						GameModelScript.UpdateCurrentBall(1, 1, currentBallNoOfRuns, 0, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, true);						
					}
				}
			}
		}
		else if(boundaryAction == "wideAndBoundary")
		{
			if(stayStartTime + 0.2 + timeBetweenBalls < Time.time)
			{
				stayStartTime = Time.time;
				boundaryAction = "waitForWideSignal";
				mainCamera.enabled = true;
				rightSideCamera.enabled = false;
				leftSideCamera.enabled = false;

				if(bowlingBy == "computer") {
					bowlerSide = "left";
				}

				AudioPlayer.instance.PlayTheCrowdSound("BallMissingCrowd");
			}
		}
		else if(boundaryAction == "waitForWideSignal")
		{
			if(stayStartTime + 2 + timeBetweenBalls < Time.time)
			{
				//			UpdateCurrentBall (validBall : int, canCountBall : int, runsScored : int, extraRun : int, batsmanID : int, isWicket : int, wicketType : int, bowlerID : int, catcherID : int, batsmanOut : int, isBoundary : boolean)
				boundaryAction = "";
				if(GameModelScript != null)
				{
					GameModelScript.UpdateCurrentBall(0, 0, 0, 5, CONTROLLER.StrikerIndex, 0, 0, CONTROLLER.CurrentBowlerIndex, 0, 0, false);
				}
				//			action = 10;
			}
		}

		if (ball2GroundMidDistance > groundBannerRadius && ballBoundaryReflection == false && ballOverTheFence == false)
		{
			//ManojAdded
			if (ballTransform.position.y > boundaryFenceHeight)
			{
				ballOverTheFence = true;
			}
			else
			{
				BallRebouncesFromBoundary();
			}
		}
		if (ball2GroundMidDistance > groundRadius)
		{
			CustomRayCastForBattingBallMovement();
		}
		/*
		if (ball2GroundMidDistance > (groundRadius + 4) && ballBoundaryReflection == false && ballOverTheFence == false) // ground radius is 68.5
		{
			if(ball.transform.position.y > boundaryFenceHeight)
			{
				ballOverTheFence = true;
			}
			else if (ball.transform.position.y < boundaryFenceHeight && (ballAngle > 305 && ballAngle < 315))
			{

			}
			else
			{
				//BallRebouncesFromBoundary ();
			}
		}
		if(ball2GroundMidDistance > (groundRadius + 40) && ballBoundaryReflection == false && ballOverTheFence == true) // over the Fence && in crowds...
		{
			horizontalSpeed *= 0.2f;//0.3;
			applyBallFiction = true;	
			ballBoundaryReflection = true;
		}*/
	}


	private void CheckForRopeLineBallPhysics()
	{
		float ropeLineBounceFactor = 0.0f; // 0.0 to 1
		float maxHorizontalSpeed = 16.0f;
		float expectedBounceDistance = 1.0f;
		if (ballNoOfBounce >= 4 && ballTransform.position.y < 0.1f)
		{
			ballNoOfBounce = 2; // to enable the bouncing after the rope-line...
			ropeLineBounceFactor = Mathf.Clamp01(horizontalSpeed / maxHorizontalSpeed);
			expectedBounceDistance = 0.4f + (1.6f * ropeLineBounceFactor); // expected distance is 0.5 to 2.0 meters
			horizontalSpeed *= 0.4f;
			ballProjectileHeight = 0.2f + (0.4f * ropeLineBounceFactor); // expected height is between 0.2 to 0.6 meter
			ballProjectileAnglePerSecond = (180.0f / expectedBounceDistance) * horizontalSpeed;
			isBallTouchedTheRope = true;
		}
	}

	public void RandomizeArray (string[] arr)
	{
		for (int i = arr.Length - 1; i > 0; i--) {
			int r = (int)Random.Range (0, i);
			string tmp = arr [i];
			arr [i] = arr [r];
			arr [r] = tmp;
		}
	}

	protected bool canApplyFriction;
	protected bool applyBallFriction = false;
	protected float ballBounceMaxHeight = 1.2f;
	protected float outFieldFriction = 4.0f; // sunny -> 4, overcast -> 5, cloudy -> 6
	public Vector3 FinalPoint;
	protected bool fielderCollectedTheBall = false;

	public void BallRebouncesFromBoundary ()
	{
		/*
		ballAngle = ballAngle + 180 + Random.Range(-20, 20);
		ballAngle = ballAngle % 360;
		horizontalSpeed *= 0.2f;//0.3;
		applyBallFiction = true;
		ballBoundaryReflection = true;*/

		FinalPoint = new Vector3(ballTransform.position.x, 0.1f, ballTransform.position.z);
		ballAngle = ballAngle + 180 + Random.Range(-20, 20);
		ballAngle = ballAngle % 360;

		horizontalSpeed *= 0.075f;
		ballProjectileAnglePerSecond *= 1.5f;

		if (action == 3)
		{
			// wicketkeeper misses the ball, it went to boundary line... // reduce the speed by additional 50%...
			horizontalSpeed = 0.0f;
		}
		canApplyFriction = true;
		applyBallFriction = true;
		ballBoundaryReflection = true;
		SetBallRotationAngle();
	}

	void SetBallRotationAngle()
	{
		ballRot.localEulerAngles = Vector3.zero;
		float tempBallRotationAngle = ((90 - ballAngle) + 360) % 360;

		ballSkinTransform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 90f));
		ballRot.transform.localEulerAngles = new Vector3(0f, tempBallRotationAngle, 0f);
	}
	public void RunnerActions ()
	{
		if(nonStickerStatus == "getReady")
		{
			if(ballTransform.position.z > stump1Crease.transform.position.z || ballStatus == "shotSuccess")
			{
				stickerStatus = "backToCrease";
				nonStickerStatus = "backToCrease";
				//runner.animation.Play ("backToCrease");
			}
		}
		if(canTakeRun == true && takeRun == true && (nonStickerStatus == "backToCrease" || nonStickerStatus == "comeToHalt")) // proper shot (not defense)
		{
			takeRun = false;
			takingRun = true;
			runOut = true;
			nonStickerSpeed = 8;//6
			stickerSpeed = 8;//6
			stickerStatus = "run";
			nonStickerStatus = "run";

			if(currentBallNoOfRuns % 2 == 0) // first, third run...
			{
				sticker = batsman;
				//nonSticker = runner;
				sticker.transform.eulerAngles = new Vector3 (sticker.transform.eulerAngles.x, 180, sticker.transform.eulerAngles.z);
				nonSticker.transform.eulerAngles = new Vector3 (nonSticker.transform.eulerAngles.x, 180, nonSticker.transform.eulerAngles.z);
				stickerRunningAngle = AngleBetweenTwoGameObjects (sticker, RHBStickerRunningSpot);
				//			nonStickerRunningAngle = AngleBetweenTwoGameObjects (nonSticker, runnerNonStickerRunningSpot); // old
				if (bowlerSide == "left") {
					//nonStickerRunningAngle = AngleBetweenTwoVector3  (nonSticker.transform.position, runnerNonStickerRunningSpot.transform.position);
				}
				else if(bowlerSide == "right") 
				{
					Vector3 tempSpot1 = runnerNonStickerRunningSpot.transform.position;
					//tempSpot1.x *= -1;
					tempSpot1 = new Vector3 (tempSpot1.x * -1, tempSpot1.y, tempSpot1.z);
					nonStickerRunningAngle = AngleBetweenTwoVector3 (nonSticker.transform.position, tempSpot1);
				}
			}
			else // second, fourth run...
			{
				//sticker = runner;
				nonSticker = batsman;
				sticker.transform.eulerAngles = new Vector3 (sticker.transform.eulerAngles.x, 0, sticker.transform.eulerAngles.z);
				nonSticker.transform.eulerAngles = new Vector3 (nonSticker.transform.eulerAngles.x, 0, nonSticker.transform.eulerAngles.z);

				if(bowlerSide == "left") {
					stickerRunningAngle = AngleBetweenTwoVector3 (sticker.transform.position, runnerStickerRunningSpot.transform.position);
				}
				else if(bowlerSide == "right") {
					Vector3 tempSpot2 = runnerStickerRunningSpot.transform.position;
					//tempSpot2.x *= -1;
					tempSpot2 = new Vector3 (tempSpot2.x * -1, tempSpot2.y, tempSpot2.z);
					stickerRunningAngle = AngleBetweenTwoVector3 (sticker.transform.position, tempSpot2);
				}

				//			stickerRunningAngle = AngleBetweenTwoGameObjects (sticker, runnerStickerRunningSpot);
				nonStickerRunningAngle = AngleBetweenTwoGameObjects (nonSticker, RHBNonStickerRunningSpot);
			}
			sticker.GetComponent<Animation>().Play ("run");
			nonSticker.GetComponent<Animation>().CrossFade ("run");

			// for the first run only...
			/*if(currentBallNoOfRuns == 0)
		{
			previewCamera.enabled = true;
		}*/
		}
		// Non-Sticker...
		else if(nonStickerStatus == "run" || nonStickerStatus == "reachTheCrease")
		{
			nonSticker.transform.position = new Vector3 (nonSticker.transform.position .x+(Mathf.Cos (nonStickerRunningAngle * DEG2RAD) * nonStickerSpeed * Time.deltaTime), nonSticker.transform.position .y,nonSticker.transform.position .z+ (Mathf.Sin (nonStickerRunningAngle * DEG2RAD) * nonStickerSpeed * Time.deltaTime));
			if(nonSticker.transform.position.z > nonStickerNearCreaseSpot.transform.position.z && nonStickerStatus == "run")
			{
				//interfaceConnector.ReachedCrease ();//Updated in 2013
				nonStickerStatus = "reachTheCrease";
				nonSticker.GetComponent<Animation>().CrossFade ("reachTheCrease");
			}

			if(nonSticker.transform.position.z > nonStickerReachSpot.transform.position.z)
			{
				runOut = false;
				if(isRunOut == false)
				{
					if(ballOnboundaryLine == false)
					{
						currentBallNoOfRuns++;
					}
				}
				nonStickerStatus = "comeToHalt";

				// AI second or more runs...
				if(battingBy == "computer" && canTakeRun == true && DistanceBetweenTwoVector2 (groundCenterPoint, ball) > 45)
				{
					takeRun = true;
				}
			}
		}
		else if(nonStickerStatus == "comeToHalt")
		{
			nonSticker.transform.position  =new Vector3 (nonSticker.transform.position .x+(Mathf.Cos (nonStickerRunningAngle * DEG2RAD) * nonStickerSpeed * Time.deltaTime), nonSticker.transform.position .y,nonSticker.transform.position .z+ (Mathf.Sin (nonStickerRunningAngle * DEG2RAD) * nonStickerSpeed * Time.deltaTime));
			nonStickerSpeed *= (100 - 70 * Time.deltaTime)/100;
			if(nonStickerSpeed < 3 && nonStickerSpeed != 0)
			{
				nonStickerSpeed = 0;
				nonSticker.GetComponent<Animation>().CrossFade ("postRunIdle");
			}
		}

		// batsman running... // sticker...
		if(stickerStatus == "run" || stickerStatus == "reachTheCrease")
		{
			sticker.transform.position = new Vector3 (sticker.transform.position.x+(Mathf.Cos (stickerRunningAngle * DEG2RAD) * stickerSpeed * Time.deltaTime), sticker.transform.position.y,sticker.transform.position.z+ (Mathf.Sin (stickerRunningAngle * DEG2RAD) * stickerSpeed * Time.deltaTime));
			if(sticker.transform.position.z < stickerNearCreaseSpot.transform.position.z && stickerStatus == "run")
			{
				stickerStatus = "reachTheCrease";
				sticker.GetComponent<Animation>().CrossFade ("reachTheCrease");
			}
			if(sticker.transform.position.z < stickerReachSpot.transform.position.z)
			{
				stickerStatus = "comeToHalt";
			}
		}
		else if(stickerStatus == "comeToHalt")
		{
			sticker.transform.position = new Vector3 (sticker.transform.position .x+(Mathf.Cos (stickerRunningAngle * DEG2RAD) * stickerSpeed * Time.deltaTime), sticker.transform.position .y,sticker.transform.position .z+ (Mathf.Sin (stickerRunningAngle * DEG2RAD) * stickerSpeed * Time.deltaTime));
			stickerSpeed *= (100 - 70 * Time.deltaTime)/100;
			if(stickerSpeed < 3 && stickerSpeed != 0)
			{
				stickerSpeed = 0;
				sticker.GetComponent<Animation>().CrossFade ("postRunIdle");
			}
		}
	}

	public void SkipIntro ()
	{
		playIntro = false;
	}

	public void ShowIntro ()
	{
		introCameraPivot.transform.Rotate(Vector3.up * Time.deltaTime * introRotationSpeed, Space.World);
	}

	public void GetInputs ()
	{
		GetBattingInput ();
		BowlerSideChange ("key");
	}

	public void  ActivateBowlerSideChangeViaUI ()
	{
		if(bowlerSide == "left") {
			BowlerSideChange ("right"); 
		}
		else if(bowlerSide == "right") {
			BowlerSideChange ("left");
		}
	}



	public void  BowlerSideChange (string from) // "key" = for KEY input, "left/right" for UI touch input...
	{
		if(bowlingBy == "user")
		{
			if(bowlerIsWaiting == true && FadeView.instance.gameObject.transform.localPosition == CONTROLLER.HIDEPOS)
			{
				float  fadeInOutTime = 0.2f;
				// to do, can change the bowling side, over the wicket or round the wicket...
				if(Input.GetKeyDown(KeyCode.LeftArrow) == true || from == "left")
				{
					if(bowlerSide == "right")
					{
						FadeView.instance.Hide (false);
						FadeView.instance.FadeIn (fadeInOutTime);
						StartCoroutine (_wait (fadeInOutTime)); 		//yield return new  WaitForSeconds (fadeInOutTime);
						bowlerSide = "left";
						//SetBowlerSide ();
						if(currentBowlerHand == "right") {
							Scoreboard.instance.UpdateStripText("Bowling over the wicket");
						}
						else if(currentBowlerHand == "left") {
							Scoreboard.instance.UpdateStripText("Bowling round the wicket");
						}
						FadeView.instance.FadeOut (fadeInOutTime);
						StartCoroutine (_wait (fadeInOutTime)); 	//yield return new  WaitForSeconds (fadeInOutTime);
						FadeView.instance.Hide (true);
					}
				}
				else if(Input.GetKeyDown(KeyCode.RightArrow) == true || from == "right")
				{
					if(bowlerSide == "left")
					{
						FadeView.instance.Hide (false);
						FadeView.instance.FadeIn (fadeInOutTime);
						StartCoroutine (_wait (fadeInOutTime)); 	//yield return new WaitForSeconds (fadeInOutTime);
						bowlerSide = "right";
						//SetBowlerSide ();
						if(currentBowlerHand == "right") {
							Scoreboard.instance.UpdateStripText("Bowling round the wicket");
						}
						else if(currentBowlerHand == "left") {
							Scoreboard.instance.UpdateStripText("Bowling over the wicket");
						}
						FadeView.instance.FadeOut (fadeInOutTime);
						StartCoroutine (_wait (fadeInOutTime)); 	//yield  return new WaitForSeconds (fadeInOutTime);
						FadeView.instance.Hide (true);
					}
				}
			}
		}
	}



	public void UpdateShadowsAndPreview()
	{
		UpdateBallShadow();
		UpdatePreview();

		if (showShadows == true)
		{
			UpdateShadow();
		}
	}

	public void Update()
	{
		if (!CONTROLLER.GameIsOnFocus)
		{
			return;
		}

		GetInputs ();

		
		switch (action)
		{ 
		case -2:
			// wait for bowling interface data...
			UpdateShadowsAndPreview ();
			break;

		case -1:
			ShowIntro ();
			break;

		case 0:
			BatsmanWaiting ();
			UpdateShadowsAndPreview ();
			break;

		case 1:
			BowlerWaiting ();
			UpdateShadowsAndPreview ();
			break;

		case 2: // Bowler runup to bowl the ball
			UserChangingBowlingSpot ();
			UpdateShadowsAndPreview ();
			break;

		case 3: // Bowler post bowling actions and ball movement towards the striker batsman...
			WicketKeeperPreBattingActions ();
			BowlingBallMovement ();
			FindBatsmanCanMakeShot ();
			ExecuteTheShot ();
			CheckForOptimalShot ();
			//RunnerActions ();
			ActivateBowler ();
			LookForMainCameraTopDownView (); // when batsman is unsuccessful with the shot and ball towards the keeper...
			//ScanForBoundaryOrSix ();	//ShankarEdit
			UpdateShadowsAndPreview ();
			break;

		case 4: // ball movement when the batsman strikes the ball...
			BattingBallMovement ();
			CustomRayCastForBattingBallMovement ();
			ActivateFielders ();
			ActivateBowler ();
			//CommentaryForShotMade ();
			//ThrowingBallMovement ();ShankarEdit
			//WicketKeeperPostBattingActions ();ShankarEdit
			CheckForOptimalShot ();
			//RunnerActions ();
			LookForMainCameraTopDownView ();
			ScanForBoundaryOrSix ();
			UpdateShadowsAndPreview ();
			break;

		case 10:
			//			ResetAll ();
			break;

		case 20:
			// idle wait state
			break;

		case 21:
			RotateUltraMotionCamera ();
			break;

		case 22:
			RotateBowledReplayCamera ();
			UpdateShadowsAndPreview ();
			break;
		}
	}


	private void  CheckForOptimalShot ()
	{
		if (batsmanTriggeredShot == true)
		{
			float currAnimTime = 0;// /*batsman.GetComponent<Animation>()*/batsmanAnimationComponent[batsmanAnimation].time;
			batInCurrFrame = currAnimTime * animationFPS;
			//jjj
		}
	}

	public void isOptimumShot ()
	{
		float  defaultOptimalShotFrame = (float )ShotVariables.optimalShotTable[shotPlayed + "OptimalShotFrame"];
		float  currentOptimalShotLength = (float )ShotVariables.optimalShotTable[shotPlayed + "OptimalShotLength"];
		float  ballDistance = DistanceBetweenTwoVector2 (stump1Crease, ball);
		//if (defaultOptimalShotLength - 4 <= Mathf.Floor(batInCurrFrame) || defaultOptimalShotLength + 4 >= Mathf.Floor(batInCurrFrame))
		//if (defaultOptimalShotLength - Mathf.Floor(batInCurrFrame) <= 4)
		if (perfectShot == true && ballDistance > batToBallConnectLength && (defaultOptimalShotFrame - Mathf.Floor(batInCurrFrame) < bufferFrame || Mathf.Floor(batInCurrFrame) - defaultOptimalShotFrame > bufferFrame))
		{
			perfectShot = false;
			//powerShot = true;
		}
	}

	public void CheckIfTheBallHitTheBoard (Collider other)
	{
		//ManojAdded
		if((other.gameObject.name == "Board" || other.gameObject.name == "BillBoard" || other.gameObject.name == "Props") && ballStatus == "shotSuccess" && ballOverTheFence == false)
		{
			BallRebouncesFromBoundary ();
		}
	}

		
	public void RotateUltraMotionCamera ()
	{
		Scoreboard.instance.HidePause (true);
		ultraMotionCamera.transform.LookAt (dummyBall.transform.position);
		if (CONTROLLER.BatsmanHand == "right") 
		{
			ultraMotionCamera.transform.RotateAround (dummyBall.transform.position, -Vector3.up, 50 * Time.deltaTime);
		}
		else 
		{
			ultraMotionCamera.transform.RotateAround (dummyBall.transform.position, Vector3.up, 50 * Time.deltaTime);
		}
	}



	public IEnumerator continuePlayingGame ()
	{
		yield return new WaitForSeconds (4);
		//Scoreboard.instance.HidePause (false);
		CONTROLLER.CurrentPage = "ingame";//shankar 08April
		GameObject blastGO = GameObject.Find ("Blast");
		if (blastGO != null)
		{
			Destroy (blastGO);
		}
		SwitchToLowPoly ();

		dummyBall.GetComponent<Renderer>().enabled = false;
		mainCamera.enabled = true;
		ultraMotionCamera.enabled = false;
		ballSkinRenderer.enabled = true;
		pauseTheBall = false;
		//rotateAngle = 0;
		bowler.GetComponent<Animation>()["BowlerRunupEdit"].speed = 3;
		/*batsman.GetComponent<Animation>()*///batsmanAnimationComponent[batsmanAnimation].speed = 1;
		//////////////////////////////////
		mainUmpire.GetComponent<Animation>().Play("idle");
		ballSkinRigidBody.Sleep();
		ActivateColliders (false);
		bowler.GetComponent<Animation>()["BowlerRunupEdit"].speed = 3;
		ballTimingOrigin.transform.position = new Vector3 (ballTransform.position.x, ballTimingOrigin.transform.position.y, ballTransform.position.z);
		ballBatMeetingHeight = ballTransform.position.y;
		ballNoOfBounce = 0;
		ballStatus = "shotSuccess";
		BallHitTime = Time.time;

		if(shotPlayed != "defense" && shotPlayed != "backFootDefenseHighBall" && shotPlayed != "frontFootOffSideDefense")
		{
			canTakeRun = true;
			if(GameModelScript != null)
			{
				GameModelScript.EnableRun (true);
			}
		}
		/*batsman.GetComponent<Animation>()*///batsmanAnimationComponent[batsmanAnimation].speed = 1;
        strikerScript.setAnimationSpeed(1.0f);
		BallTiming();
		GetFieldersAngle ();
		GetFieldersDistance ();
		SetActiveFielders ();
		action = 4;		
	}

	public void RotateBowledReplayCamera ()
	{
		if(Scoreboard.instance.muteBtn.transform.parent.gameObject.activeSelf)
		{
			Scoreboard.instance.HidePause (true);
		}

		if (CONTROLLER .gameMode !="multiplayer")
		{
			BowledReplayCamera.transform.RotateAround (batsman.transform.position, -Vector3.up, 25 * Time.deltaTime);
			BowledReplayCamera.transform.LookAt(midpoint);
		}
		if (GameModel.isGamePaused == false) 
		{
			//ball.transform.position.z += 0.01;
			ballTransform.eulerAngles = new Vector3 (ballTransform.eulerAngles.x, ballTransform.eulerAngles.y+ 0.5f, ballTransform.eulerAngles.z);
			float xPos = Mathf.Cos (ballAngle * DEG2RAD) * horizontalSpeed * Time.deltaTime;
			float zPos = Mathf.Sin (ballAngle * DEG2RAD) * horizontalSpeed * Time.deltaTime;
			float projectileY = Mathf.Sin (ballProjectileAngle * DEG2RAD) * ballProjectileHeight - ballRadius;
			ballTransform.position = new Vector3 (ballTransform.position.x + (xPos / 35),  -projectileY, ballTransform.position.z+(zPos / 35));
		}
	}



	public void OnCustomTriggerEnter (Collider other)
	{
		if(other.gameObject.name == "BatCollider" && ballStatus == "bowling" && ballStatus != "onPads")
		{
			canSwipeNow = false;
			bowlingSpotScript.HideBowlingSpot ();
			if((shotPlayed != "WellLeftNormalHeight" && shotPlayed != "LeaveTheBallBouncer" && shotPlayed != "" && LateAttempt == false)) // if it is "leave", leave the ball to the wicketkeeper...
			{
				isOptimumShot ();
				AudioPlayer.instance.PlayBatSnd();
				strikerScript.setAnimationSpeed(1.0f);
				mainUmpire.GetComponent<Animation>().Play("idle");
				
				ballSkinRigidBody.Sleep();

				ActivateColliders (false);
				bowler.GetComponent<Animation>()["BowlerRunupEdit"].speed = 3;
				ballTimingOrigin.transform.position = new Vector3 (ballTransform.position.x, ballTimingOrigin.transform.position.y, ballTransform.position.z);
				ballBatMeetingHeight = ballTransform.position.y;
				ballNoOfBounce = 0;
				ballStatus = "shotSuccess";
				BallHitTime = Time.time;

				//if(shotPlayed != "defense" && shotPlayed != "backFootDefenseHighBall" && shotPlayed != "frontFootOffSideDefense")
				if (shotPlayed != "WellLeftNormalHeight" && shotPlayed != "LeaveTheBallBouncer")
				{
					canTakeRun = true;
					if (GameModelScript != null)
					{
						GameModelScript.EnableRun(true);
					}
				}
				if(AdIntegrate.instance != null && CONTROLLER .gameMode !="multiplayer")
				{
					isTimeToShowAd = true;
					AdIntegrate.instance.ShowBannerAd ();
				}
				/*batsman.GetComponent<Animation>()*///batsmanAnimationComponent[batsmanAnimation].speed = 1;
				BallTiming ();
				//MoveWicketKeeperToStumps ();//Shankar Commented
				GetFieldersAngle ();
				GetFieldersDistance ();
				SetActiveFielders ();
				action = 4;  

                if (shotPlayed != "DownTheTrackDefensiveShot" && shotPlayed != "FrontFootDefense" && shotPlayed != "BackFootDefense")
                {
                    AudioPlayer.instance.PlayBallTravelSound();
				}

            }
        }
		else if(other.gameObject.name == "Stump1Collider" && ballStatus == "bowling")
		{
			canSwipeNow = false;
			ActivateColliders(false);
			ballStatus = "bowled";
			strikerScript.setAnimationSpeed(1.0f);
			strikerScript.action = -1;
			strikerScript.setAnimationSpeed(1.1f);
			strikerScript.crossFadeAnimation("BowledDisappointed");
			strikerScript.setAnimationSpeed(0.6f);
			if (runnerScript.isMirror == false)
			{
				runnerScript._playAnimation("Runner1_BacktoIdle");
			}
			else
			{
				runnerScript._playAnimation("Runner1_BacktoIdle_Left");
			}
				StumpAnimation(stump1, ballTransform.position.x);
			/*batsman.GetComponent<Animation>()*///batsmanAnimationComponent[batsmanAnimation].speed = 1;
			bowler.GetComponent<Animation>()["BowlerRunupEdit"].speed = 3;
			makeFieldersToCelebrate (null);
			//if(highEndDevice == true)
			//{
			//	lensFlareHolder.SetActive (true);
			//}
			//groundFlashGO.SetActive (true);

			StartCoroutine (PlayUltraSlowMotion ());		//remove this for multiplayer alone
			if(AdIntegrate.instance != null && CONTROLLER .gameMode !="multiplayer")
			{
				isTimeToShowAd = true;
				AdIntegrate.instance.ShowBannerAd ();
			}
			//Before adding ultra slow motion for bowled
			/*if(interfaceConnector != null)
		{
			interfaceConnector.PlayGameSound ("Bowled"); 
			yield WaitForSeconds (0.5);
			interfaceConnector.PlayGameSound ("Cheer"); 
		}*/
		}
		else if(ballStatus == "shotSuccess")
		{
			//	StumpAnimation (stump2, ball.transform.position.x);
			//if(GameModelScript != null)
			//{
			//	GameModelScript.PlayGameSound ("Bowled"); 
			//}
		}
        else if (ballStatus == "bowling" && ((other.gameObject.name == "LeftLowerLeg" || other.gameObject.name == "RightLowerLeg" || other.gameObject.name == "LeftUpperLeg" || other.gameObject.name == "RightUpperLeg" || other.gameObject.name == "UpperBody" || other.gameObject.name == "Head") ))
		{
			if ((other.gameObject.name == "UpperBody" || other.gameObject.name == "Head") && ballTransform.position.y > 1.5f)
			{
				strikerScript.crossFadeAnimation("HeadInjury1");
                strikerScript.setAnimationSpeed(0.3f);
			}
			else if (other.gameObject.name == "Head" && ballTransform.position.y > 1.0f && ballTransform.position.y <= 1.5f)
			{
				strikerScript._playAnimation("HeadInjury2");
            }
            else if (other.gameObject.name == "UpperBody" && ballTransform.position.y > 1.0f && ballTransform.position.y <= 1.5f)
			{
				strikerScript._playAnimation("StomachInjury");
            }
             
			GameModelScript.PlayGameSound("ballhittingbody");
            AudioPlayer.instance.PlayTheCrowdSound("BallMissingCrowd");
            ballStatus = "onPads";
			if (runnerStandingAnimationId == 1)
			{
				// runnerAnimation.CrossFade ("RunnerBackToCrease");
				if (runnerScript.isMirror == false)
				{
					runnerScript._playAnimation("Runner1_BacktoIdle");
				}
				else
				{
					runnerScript._playAnimation("Runner1_BacktoIdle_Left");
				}
			}
			else if (runnerStandingAnimationId == 2)
			{
				// runnerAnimation.CrossFade ("RunnerBackToCrease2");
				if (runnerScript.isMirror == false)
				{
					runnerScript._playAnimation("Runner2_BacktoIdle");
				}
				else
				{
					runnerScript._playAnimation("Runner2_BacktoIdle_Left");
				}
			}
			ActivateColliders (false);
			if (batsmanAnimation != string.Empty)
			{
				strikerScript.setAnimationSpeed(1.0f);
			}
			/*batsman.GetComponent<Animation>()*///batsmanAnimationComponent[batsmanAnimation].speed = 1;
			bowlingSpotScript.HideBowlingSpot ();
			ballAngle = ballAngle + 180 + Random.Range(-20, 20);
			ballAngle = ballAngle % 360;
			horizontalSpeed *= 0.05f;//0.3;
			ballProjectileAnglePerSecond *= 1.5f;//1.1;
			ballProjectileHeight *= 0.5f;
			applyBallFiction = true;
			canSwipeNow = false;
			if(AdIntegrate.instance != null && CONTROLLER .gameMode !="multiplayer")
			{
				isTimeToShowAd = true;
				AdIntegrate.instance.ShowBannerAd ();
			}
			// check for ball height which is sufficient to hit the stumps and check the line of the ball for LBW
			// LBW appeal cases :
			// height should be 0.7 meter
			// ball distance to stumps should be less then 0.2 meter
			// LBW out cases:
			// when ball distances is 0.1 meter && height is < 0.6 meter
			if(ballTransform.position.y < 0.7 && Mathf.Abs (ballSpotAtStump.transform.position.x) < 0.15)  //0.2   //gtesting
			{
				lbwAppeal = true;   
				bowler.GetComponent<Animation>()["BowlerRunupEdit"].speed = 3;
				if(currentBowlerType == "fast")
				{
					/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("appealFast");
				}
				else if(currentBowlerType == "spin")
				{
					/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("appealSpin");
				}
				makeFieldersToCelebrate (null);
				
				if(ballTransform.position.y < 0.6 && Mathf.Abs (ballSpotAtStump.transform.position.x) < 0.1 && ballInline == true)
				{
					LBW = true; 
				}
			}
			else
			{
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent.Play("waitForBall");
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent["waitForBall"].time = /*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent["waitForBall"].length;
				/*wicketKeeper.GetComponent<Animation>()*/wicketKeeperAnimationComponent["waitForBall"].speed = -1; 
			}
		}
	}


	private string stumpAnim;
	public IEnumerator PlayUltraSlowMotion ()
	{
		CONTROLLER.CurrentPage = "";//shankar 08April
		if(GameModelScript != null)
		{
			AudioPlayer.instance.PlayBowledSnd();
			AudioPlayer.instance.PlayTheCrowdSound("bowledcrowd");

			float delayTime = 0.05f;
			if (currentBowlerType == "spin")
			{
				delayTime = 0.08f;
			}
			yield return new WaitForSeconds (delayTime);
			pauseTheBall = true;
			BowledReplayCamera.enabled = true;
			//ManojAdded
			midpoint = (stump1Spot.transform.position+ batsman.transform.position) * 0.5f;
			
			if (currentBatsmanHand == "left")
			{
				BowledReplayCamera.transform.position = new Vector3(3.5f, 0.5f, midpoint.z);
				BowledReplayCamera.transform.eulerAngles = new Vector3(-10, -45, 0);
			}
            else
            {
				BowledReplayCamera.transform.position = new Vector3(-3.5f, 0.5f, midpoint.z);
				BowledReplayCamera.transform.eulerAngles = new Vector3(-10, 45, 0);
			}
			BowledReplayCamera.transform.LookAt(midpoint);
			//ManojAdded
			fielder10AnimationComponent["appeal"].speed = 0;
			wicketKeeperAnimationComponent["appealFast"].speed = 0;
			//batsmanAnimationComponent[batsmanAnimation].speed = 0;

			stump1.GetComponent<Animation>()[stumpAnim].speed = 0.05f;
			if (currentBowlerType == "fast")
			{
				stump1.GetComponent<Animation>()[stumpAnim].time = 0.16f;//0.15
			}
			else
			{
				stump1.GetComponent<Animation>()[stumpAnim].time = 0.22f;
			}

			mainCamera.enabled = false;
			action = 22;
			if(CONTROLLER .gameMode !="multiplayer")
				yield return new WaitForSeconds (3);
			else
				yield return new WaitForSeconds (1.5f);
			
			CONTROLLER.CurrentPage = "ingame";//shankar 08April
			pauseTheBall = false;
			fielder10AnimationComponent["appeal"].speed = 1;
			//batsmanAnimationComponent[batsmanAnimation].speed = 1;
			wicketKeeperAnimationComponent["appealFast"].speed = 1;
			stump1.GetComponent<Animation>()[stumpAnim].speed = 1;
			ShowBall (true);
			action = 3;  
		}
	}

	public void StumpAnimation (GameObject stump, float contactPoint)
	{
		if(contactPoint < -0.06)
		{
			stumpAnim = currentBowlerType+"OffStump";
			stump.GetComponent<Animation>().Play(stumpAnim);
		}
		else if(contactPoint > 0.06)
		{
			stumpAnim = currentBowlerType+"LegStump";
			stump.GetComponent<Animation>().Play(stumpAnim);
		}
		else if(Mathf.Abs(contactPoint) < 0.03)
		{
			stumpAnim = currentBowlerType+"MidStump";
			stump.GetComponent<Animation>().Play(stumpAnim);
		}
		else
		{
			stumpAnim = currentBowlerType+"MidStump";
			stump.GetComponent<Animation>().Play(stumpAnim);
		}
	}

	public void ActivateColliders (bool boolean)
	{
		if(batCollider!=null)
			batCollider.SetActive(boolean);

		leftLowerLegObject.SetActive(boolean);
		rightLowerLegObject.SetActive(boolean);
		leftUpperLegObject.SetActive(boolean);
		rightUpperLegObject.SetActive(boolean);
		upperBodyObject.SetActive(boolean);
		headObject.SetActive(boolean);
		stump1Collider.SetActive (boolean);
	}

	public void GameIsPaused (bool pauseStatus,bool isComingFromApplicationPause)
	{
		if(pauseStatus == true)
		{
			iTween.Pause (mainCamera.gameObject);
			ninjaSlice.SetActive (false);
			ActivateColliders(false);

			if (isComingFromApplicationPause && action == 4)
			{
                CONTROLLER.GameIsOnFocus = false;
				SetFieldersAnimSpeed(true);
			}
			else
			{
               AdIntegrate.instance.SetTimeScale(0f);
            }
        }
		else
		{
			AdIntegrate.instance.SetTimeScale(1f);
			iTween.Resume (mainCamera.gameObject);
			ActivateColliders (true);
			ninjaSlice.SetActive (true);//shankar 08April			
		}
    }


	public void SetFieldersAnimSpeed(bool isStop)
	{
        for (var i = 1; i <= noOfFielders; i++)
        {
            fielder[i] = GameObject.Find("/Fielders/Fielder" + i);
            GameObject fielderGO = fielder[i] as GameObject;
            Animation anim = fielderGO.GetComponent<Animation>();
			if (isStop)
			{
                foreach (AnimationState state in anim)
                {
                    state.speed = 0f;
                }
            }
			else
			{
                foreach (AnimationState state in anim)
                {
                    state.speed = 1f;
                }
            }
        }
    }

	public void BowlNextBall (string batting, string bowling)
	{
		/*
For Current Strike Batsman Index			-	CONTROLLER.StrikerIndex
For Current Strike Batsman Style			-	CONTROLLER.StrikerHand

For Current Non Strike Batsman Index		-	CONTROLLER.NonStrikerIndex
For Current Non Strike Batsman Style		-	CONTROLLER.NonStrikerHand

For Current Bowler Index					-	CONTROLLER.CurrentBowlerIndex
For Current Bowler Type						-	CONTROLLER.BowlerType               (0-Fast,   1-Off Spin,  2-Leg Spin)
For Current Bowler Style					-	CONTROLLER.BowlerHand

For Wicket Keeper Index						-	CONTROLLER.wickerKeeperIndex
*/
		battingBy = "user";//batting; 					// user || computer
		bowlingBy = "computer";//bowling; 					// user || computer

		currentBatsmanHand = CONTROLLER.BatsmanHand;
		//currentBowlerHand = CONTROLLER.BowlerHand;	

		fieldRestriction = CONTROLLER.PowerPlay;
		hattrickBall = CONTROLLER.HattrickBall;


		//spinValue = 0;
		bowlerIsWaiting = false;

		if (bowlingBy == "computer") { // user is batting
			//LookToChangeBowlerSideInBetweenTheOver ();
		}

		//batCollider.transform.localScale =new  Vector3 (0.5f, 1f, 0.5f);//2013
		batColliderHolder.transform.localScale = GetBatColliderSize();

		if (CONTROLLER.gameMode == "multiplayer") 
		{
				currentBowlerHand = Multiplayer.oversData [CONTROLLER.currentMatchBalls / 6].bowlerHand;
				if (Multiplayer.oversData [CONTROLLER.currentMatchBalls / 6].bowlerType == "fast") {
					CONTROLLER.BowlerType = 0;
				} else if (Multiplayer.oversData [CONTROLLER.currentMatchBalls / 6].bowlerType == "offspin") {
					CONTROLLER.BowlerType = 1;
				} else {
					CONTROLLER.BowlerType = 2;
				}

		}

		if(CONTROLLER.BowlerType == 0)
		{
			currentBowlerType = "fast";
		}
		else if(CONTROLLER.BowlerType == 1 || CONTROLLER.BowlerType == 2)
		{
			currentBowlerType = "spin";
			currentBowlerSpinType = CONTROLLER.BowlerType;
			if(currentBowlerHand == "left")
			{
				if(currentBowlerSpinType == 1) {
					currentBowlerSpinType = 2;
				}
				else if(currentBowlerSpinType == 2) {
					currentBowlerSpinType = 1;
				}
			}
		}


		StartBowling ();
		/*if(bowlingBy == "computer" && ballToFineLeg == true && (currentBallNoOfRuns == 6 || currentBallNoOfRuns == 4))
	{
		if(fieldRestriction == true) {
			CONTROLLER.computerFielderChangeIndex = 1;
		}
		else if(fieldRestriction == false) {
			CONTROLLER.computerFielderChangeIndex = 8;			
		}
	}*/
		ResetAll ();
	}



	public void NewOver ()
	{	
		if (CONTROLLER.gameMode != "superover")
		{
			if (CONTROLLER.BowlingEnd == "madrasclub")
			{
				//stadiumGO.transform.eulerAngles.y = 180;
//				stadium180GO.SetActiveRecursively(true);
//				stadiumGO.SetActiveRecursively(false);
				stadiumGO.transform.eulerAngles = new Vector3(0,180f,0);
				extrasGO.transform.eulerAngles = new Vector3 (extrasGO.transform.eulerAngles.x, 180, extrasGO.transform.eulerAngles.z);
				cameraFlashReferences.transform.eulerAngles = new Vector3 (cameraFlashReferences.transform.eulerAngles.x, 0, cameraFlashReferences.transform.eulerAngles.z);
				stadiumRotationAngle = 0;
			}
			else
			{
				//		stadiumGO.transform.eulerAngles.y = 0;
//				stadium180GO.SetActiveRecursively(false);
//				stadiumGO.SetActiveRecursively(true);
				stadiumGO.transform.eulerAngles = new Vector3(0,0,0);
				extrasGO.transform.eulerAngles = new Vector3 (extrasGO.transform.eulerAngles.x, 0, extrasGO.transform.eulerAngles.z);
				cameraFlashReferences.transform.eulerAngles = new Vector3 (cameraFlashReferences.transform.eulerAngles.x, 180, cameraFlashReferences.transform.eulerAngles.z);
				stadiumRotationAngle = 180;
			}
		}
		else
		{
//			stadium180GO.SetActiveRecursively(true);
//			stadiumGO.SetActiveRecursively(false);	
			stadiumGO.transform.eulerAngles = new Vector3(0,180f,0);
			extrasGO.transform.eulerAngles = new Vector3 (extrasGO.transform.eulerAngles.x, 180, extrasGO.transform.eulerAngles.z);
			cameraFlashReferences.transform.eulerAngles = new Vector3 (cameraFlashReferences.transform.eulerAngles.x, 0, cameraFlashReferences.transform.eulerAngles.z);

			stadiumRotationAngle = 0;
		}

		if(bowlingBy == "computer")
		{
			if(Random.Range(0.0f, 10.0f) > 5.0)
			{
				bowlerSide = "right";
			}
			else
			{
				bowlerSide = "left";
			}
		}
	}


	public void UpdatePreview ()
	{
		GameObject refGO;
		Dictionary<string,object> hash = new Dictionary<string,object> ();
		hash.Add ("Striker", batsman.transform.position);
		//hash.Add ("NonStriker", runner.transform.position);
		refGO = fielder[1] as GameObject;
		hash.Add ("field_01", refGO.transform.position);
		refGO = fielder[2] as GameObject;
		hash.Add ("field_02", refGO.transform.position);
		refGO = fielder[3] as GameObject;
		hash.Add ("field_03", refGO.transform.position);
		refGO = fielder[4] as GameObject;
		hash.Add ("field_04", refGO.transform.position);
		refGO = fielder[5] as GameObject;
		hash.Add ("field_05", refGO.transform.position);
		refGO = fielder[6] as GameObject;
		hash.Add ("field_06", refGO.transform.position);
		refGO = fielder[7] as GameObject;
		hash.Add ("field_07", refGO.transform.position);
		refGO = fielder[8] as GameObject;
		hash.Add ("field_08", refGO.transform.position);
		refGO = fielder[9] as GameObject;
		hash.Add ("field_09", refGO.transform.position);
		if(showShadows == true)
		{
			refGO = ShadowsArray[9];
			hash.Add ("field_10", refGO.transform.position); // fast bowler || spin bowler || fielder10...
		}
		if(ballReleased == true) {
			hash.Add ("Ball", ballTransform.position);		
		}
		else {
			if(showShadows == true)
			{
				refGO = ShadowsArray[9];
				hash.Add ("Ball", refGO.transform.position);
			}
		}
		if(showShadows == true)
		{
			refGO = ShadowsArray[10];
			hash.Add ("field_11", refGO.transform.position); // wicketKeeper...
		}

		if(GameModelScript != null)
		{
			GameModelScript.UpdatePreview(hash);
		}
	}

	public void ZoomToBatsmanEntry ()
	{	
		EnableFielders (false);

		StrikerOnFocus = true;
		mainCamera.enabled = false;
		introCamera.enabled = true;
	}

	public void ZoomToBatsmanExit ()
	{
		EnableFielders (false);
		mainCamera.enabled = false;
		rightSideCamera.enabled = false;
		leftSideCamera.enabled = false;

		GameObject batsmanExitGO = Resources.Load("BatsmanExit") as GameObject ;
		batsmanExit = Instantiate (batsmanExitGO) as GameObject;
		batsmanExit.name = "BatsmanExit";
		batsmanExit.transform.position = new Vector3 (-50, 0, -8);
		batsmanExit.transform.eulerAngles = new Vector3 (batsmanExit.transform.eulerAngles.x, 270, batsmanExit.transform.eulerAngles.z);

//		GameObject exitBatsmanSkin = GameObject.Find("BatsmanExit/Armature/Bone");

		introCamera.transform.position = new Vector3 (-50, 2, -12);
		introCamera.transform.eulerAngles = new Vector3 (0, 0, introCamera.transform.eulerAngles.z);
		introCamera.fieldOfView = 40;
		introCamera.enabled = true;
	}

	public void StartGameAfterIntro ()
	{
		EnableFielders (true);
	}

	public void IntroCompleted ()
	{
		SkipIntro ();
		introCamera.enabled = false;
		mainCamera.enabled = true;
		EnableFielders (true);
	}

	private void EnableFielders (bool boolean)
	{
		for (int i = 0; i < fielder.Length; i++) {
			if (fielder [i] != null) {
				GameObject fielderGO = fielder [i] as GameObject;
				fielderGO.SetActive (boolean);
			}
		}
		batsmanUniform.GetComponent<Renderer> ().enabled = true;
	}


	public void UpdateShadow ()
	{
		GameObject shadowRef;
		GameObject shadowGO;

		shadowRef = ShadowRefArray [0];
		shadowGO = ShadowsArray [0];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [1];
		shadowGO = ShadowsArray [1];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [2];
		shadowGO = ShadowsArray [2];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [3];
		shadowGO = ShadowsArray [3];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [4];
		shadowGO = ShadowsArray [4];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [5];
		shadowGO = ShadowsArray [5];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [6];
		shadowGO = ShadowsArray [6];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [7];
		shadowGO = ShadowsArray [7];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		shadowRef = ShadowRefArray [8];
		shadowGO = ShadowsArray [8];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);


		if(canActivateBowler == false)
		{
			if(currentBowlerType == "fast")
			{
				shadowRef = GameObject.Find ("FastBowler/Armature/Bone/hip/spin/ShadowRef");
				shadowGO = ShadowsArray[9];
				shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);
			}
			else if(currentBowlerType == "spin")
			{
				shadowRef = GameObject.Find ("SpinBowler/Armature/Bone/hip/spin/ShadowRef");
				shadowGO = ShadowsArray[9];
				shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

			}
		}
		else if(canActivateBowler == true && fielder10Skin.GetComponent<Renderer>().enabled == true)
		{
			shadowRef = ShadowRefArray[9];
			shadowGO = ShadowsArray[9];
			shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);
		}

		shadowRef = ShadowRefArray [10];
		shadowGO = ShadowsArray [10];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		/*Batsman Shadow*/
		shadowRef = ShadowRefArray [11];//Updated in 2013
		shadowRef.transform.localPosition = new Vector3 ((leftLowerLegObject.transform.localPosition.x + rightLowerLegObject.transform.localPosition.x) / 2, shadowRef.transform.localPosition.y, (leftLowerLegObject.transform.localPosition.z + rightLowerLegObject.transform.localPosition.z) / 2);

		shadowGO = ShadowsArray [11];
		shadowRef = ShadowRefArray [11];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x - 0.2f, -0.15f, shadowRef.transform.position.z);
		/****************/

		shadowRef = ShadowRefArray [12];
		shadowGO = ShadowsArray [12];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);

		/*Batsman Shadow*/
		shadowRef = ShadowRefArray [13];
		shadowGO = ShadowsArray [13];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x, shadowGO.transform.position.y, shadowRef.transform.position.z);
		/****************/

		/*Runner Shadow*/
		shadowRef = ShadowRefArray [14];
		shadowGO = ShadowsArray [14];
		shadowGO.transform.position = new Vector3 (shadowRef.transform.position.x - 0.2f, -0.15f, shadowRef.transform.position.z);
		/****************/
	}

	public void MoveLeftSide (bool boolean)
	{
		if (boolean == true) {
			if (currentBatsmanHand == "right") {
				leftArrowKeyDown = true;
			} else {
				rightArrowKeyDown = true;
			}
		} else {
			if (currentBatsmanHand == "right") {
				leftArrowKeyDown = false;
			} else {
				rightArrowKeyDown = false;
			}
		}
	}

	public void MoveRightSide (bool boolean)
	{
		if (boolean == true) {
			if (currentBatsmanHand == "right") {
				rightArrowKeyDown = true;
			} else {
				leftArrowKeyDown = true;
			}
		} else {
			if (currentBatsmanHand == "right") {
				rightArrowKeyDown = false;
			} else {
				leftArrowKeyDown = false;			
			}
		}
	}
	public float Shotangle = 0.0f;
	public void ShotSelected (int SelectedAngle, float swipeAngle)
	{
		UpdateBattingTimingMeter();
		updateBattingTimingMeterNeedle = false;

		ballToBatsmanDistance = DistanceBetweenTwoVector2 (stump1Crease, ball);
		if (ballToBatsmanDistance < 11f && ballToBatsmanDistance > 8f)
		{
			perfectShot = true;
		}
		Shotangle = swipeAngle;
		powerKeyDown = loft;
		powerShot = loft;
		touchDeviceShotInput = true;
		


		leftArrowKeyDown = false;
		downArrowKeyDown = false;
		upArrowKeyDown = false;
		rightArrowKeyDown = false;
		shotSelectedSide = 0;

        if (SelectedAngle == 1)
        {
            shotSelectedSide = 1;
        }
        else if (SelectedAngle == 2)
        {
            shotSelectedSide = 2;
        }
        else if (SelectedAngle == 3)
        {
            shotSelectedSide = 3;
        }
        else if (SelectedAngle == 4)
        {
            shotSelectedSide = 4;
        }
        else if (SelectedAngle == 5)
        {
            shotSelectedSide = 5;
        }
        else if (SelectedAngle == 6)
        {
            shotSelectedSide = -1;//defensemode
        }
        else if (SelectedAngle == 7)
        {
            shotSelectedSide = -5;
        }
        else if (SelectedAngle == 8)
        {
            shotSelectedSide = -4;
        }
        else if (SelectedAngle == 9)
        {
            shotSelectedSide = -3;
        }
        else if (SelectedAngle == 10)
        {
            shotSelectedSide = -2;
        }
        // for left-handed batsman...
        if (currentBatsmanHand == "left" && SelectedAngle != 1 && SelectedAngle != 6)
        {
            shotSelectedSide = -(shotSelectedSide);
        }

	}

	public void InitRun (bool boolean)
	{
		//#if UNITY_ANDROID || UNITY_IPHONE
		takeRun = boolean;
		//#endif
	}


	public Vector3 UpdateCameraPosition ()
	{
		return mainCamera.WorldToScreenPoint (batsmanRefPoint.position);
	}

	public void stopAllAnimations ()
	{
		for(var i = 1; i <= noOfFielders; i++)
		{
			fielder[i] = GameObject.Find("/Fielders/Fielder"+i);
			GameObject fielderGO = fielder [i] as GameObject;
			fielderGO.GetComponent<Animation>().Stop ();
			strikerScript.reset();
			runnerScript.reset();
			bowler.GetComponent<Animation>().Stop ();
		}
		action = 20;
	}

	public void SwitchToHighPoly ()
	{
		/*if(highEndDevice == true)
		{
			Mesh highpolyMesh = Instantiate (batsmanHighPolyMesh) as Mesh;
			highpolyMesh.name = "batsman";
			batsmanSkin.GetComponent<SkinnedMeshRenderer>().sharedMesh = highpolyMesh;
		}*/
	}

	public void SwitchToLowPoly ()
	{
		/*if(highEndDevice == true)
		{
			Mesh lowpolyMesh = Instantiate (batsmanLowPolyMesh) as Mesh;
			lowpolyMesh.name = "batsman";
			batsmanSkin.GetComponent<SkinnedMeshRenderer> ().sharedMesh = lowpolyMesh;
		}*/
	}



	public IEnumerator CameraFlashStart (int count, float delay, int ground) // ground = 180 || 0
	{
		yield return null;
		float x;
		float y;
		float z;	
		GameObject TempGO;

		for( int i= 0; i < count; i++)
		{	
			//flash in frontside of ground
			if(ground == 0)
			{
				x = Random.Range(flashReference1.transform.position.x, flashReference2.transform.position.x);
				z = -65; // was -74 (behind black board)
				y =  Random.Range(3, 16); // Height where the crowd sits... 3 to 16 in Yaxis
				if(y > 6 && y < 10)
					y = y+4;
				if(x > -15 && x < 15)
					y = 13;
				TempGO = Instantiate(CameraFlashPrefab,new  Vector3(x,y,z) , Quaternion.identity);	
				TempGO.gameObject.transform.eulerAngles = new Vector3(70,0,0);
				float  randomScale = Random.Range (1.0f, 1.4f);
				TempGO.gameObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
				yield return new WaitForSeconds(delay);
			}
			//flash in backside of ground
			else
			{
				int  stadiumselect= Random.Range(1,4);
				if(stadiumselect == 1)
					x = Random.Range(flashReference3.transform.position.x, flashReference4.transform.position.x);
				else if (stadiumselect == 2)
					x = Random.Range(flashReference5.transform.position.x, flashReference6.transform.position.x);
				else
					x = Random.Range(flashReference7.transform.position.x, flashReference8.transform.position.x);

				z = -65; 

				if(stadiumselect == 2)
					y =  Random.Range(11, 22);
				else
					y =  Random.Range(3, 22); // Height where the crowd sits... 3 to 16 in Yaxis

				TempGO = Instantiate(CameraFlashPrefab,new  Vector3(x,y,z) , Quaternion.identity);	
				TempGO.gameObject.transform.eulerAngles = new Vector3(70f, 0f, 0f);
				float  randomScale2 = Random.Range (1.0f, 1.4f);
				TempGO.gameObject.transform.localScale = new Vector3(randomScale2, randomScale2, randomScale2);
				yield return new WaitForSeconds (delay);
			}
		}	
	}


    public void MoveBatsmanHardcode()
    {
        batsman.transform.position = new Vector3(2.5f, 0f, 8.8f);
    }
    public void MoveBowlingSpotHardcode()
    {
        bowlingSpotGO.transform.position = new Vector3(0f, 0f,8.8f);
    }


	#region WCC2 ANIMATIONS
	
	public BatsmanScript strikerScript;
	public BatsmanScript runnerScript;

	private bool IsPostBatAnim = true;

	private float ballSpotHeight = 0.0f; // ball height at crease line...
	private float ballHeightAtStump = 0.0f; // ball height at stump...
	private int shotSelectedSide = 0;
	public int shotDirection;
	private float bowlingBounceFactor;
	private float pitchFactor = 1.0f;
	private float halfPitchDistance = 8.8f;
	private float ballPitchingDistanceFactor = 0.0f;
	private float ballPitchHeightFactor = 0.0f;
	private float ballProjectileAnglePerSecondBowlingFactor = 0.0f;
	private float nextPitchDistance = 0.0f; // to determine fielder catching animation to play...
	private float animationFPSDivide = 0.0333f;//In which, for multiple of 1/animationFPS // 30 FPS
	private bool isBouncerBallBowled = false;
	private float batRadius = 1.1f;
	private float ballProjectileAnglePerSecondFactor = 0.0f;

	public void ShotType(int type)
	{
		shotDirection = type;

		float distanceBtwBatsmanAndBall = 0.0f;

		if (currentBatsmanHand == "right")
		{
			distanceBtwBatsmanAndBall = (batsman.transform.position.x - batsmanInitXPos) - ballSpotAtCreaseLine.transform.position.x;
		}
		else if (currentBatsmanHand == "left")
		{
			distanceBtwBatsmanAndBall = (batsmanInitXPos - batsman.transform.position.x) + ballSpotAtCreaseLine.transform.position.x;
		}

		int randomId;
		string ballLength;

		if (ballSpotHeight < 0.3f)
		{
			ballLength = "Yorker";
		}
		else if (ballSpotHeight < 0.7f)
		{
			ballLength = "FullPitch";
		}
		else if (ballSpotHeight < 1.0f)
		{
			ballLength = "GoodLength";
		}
		else if (ballSpotHeight < 1.6f)
		{
			ballLength = "ShortPitch";
		}
		else
		{
			ballLength = "Bouncer";
		}
		if (currentBowlerType == "spin" && ballSpotHeight > 1.0f)
		{
			ballLength = "GoodLength";
		}
		//if (GameModelScript.BattingControlsScript.isWellLeftSelected)
		//{
		//	type = 11;
		//}
		switch (type)
		{
			case 1://Straight
				if (ballLength == "Yorker" || ballLength == "FullPitch" || ballLength == "GoodLength" || ballLength == "ShortPitch" || ballLength == "Bouncer")
				{
					if (powerShot == true)
					{
						//aimoveandplay
						if (distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.2f)
						{

							randomId = Random.Range(0, 5);
							if (randomId >= 3)
							{
								batsmanAnimation = "DownTheTrackStraightSlog";
								shotPlayed = "DownTheTrackStraightSlog";
							}
							else
							{

								batsmanAnimation = "MoisesHenriques_DownTheTrack"; //BigBash
								shotPlayed = "MoisesHenriques_DownTheTrack";
							}
						}
						else if (distanceBtwBatsmanAndBall < 0.5f)
						{
							batsmanAnimation = "StraightSlog";
							shotPlayed = "StraightSlog";
						}
						else
						{

							randomId = Random.Range(0, 5);
							if (randomId >= 3)
							{
								batsmanAnimation = "LoftedStraightDrive";
								shotPlayed = "LoftedStraightDrive";
							}
							else
							{
								batsmanAnimation = "MichaelKlinger_LongOffSlog";//BigBash
								shotPlayed = "MichaelKlinger_LongOffSlog";
							}


						}
					}
					else
					{
						if (ballLength == "Yorker" || ballLength == "FullPitch")
						{
							if (currentBowlerType == "spin")
							{
								randomId = Random.Range(0, 5);

								if (randomId >= 2)
								{
									batsmanAnimation = "DownTheTrackSpin_StraightDrive";//nija batsman
									shotPlayed = "DownTheTrackSpin_StraightDrive";
								}
								else
								{
									batsmanAnimation = "StraightDrive";
									shotPlayed = "StraightDrive";
								}
							}
							else
							{
								batsmanAnimation = "StraightDrive";
								shotPlayed = "StraightDrive";
							}
						}
						else
						{
							batsmanAnimation = "BackFootStraightDrive";
							shotPlayed = "BackFootStraightDrive";
							batRadius = 0.5f; // less radius because of wrong shot selection, ie., playing straight drive in bouncer 
						}
					}
					//aimoveandplay
				}
				break;
			case 2://Long off
				if (powerShot == true)
				{
					batsmanAnimation = "LoftedOffDrive";
					shotPlayed = "LoftedOffDrive";
					if (ballLength == "GoodLength" && distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.2f)//wicket-downthetrack
					{
						batsmanAnimation = "DownTheTrackOffSideLoft";
						shotPlayed = "DownTheTrackOffSideLoft";
					}
				}
				else
				{
					if (ballLength == "Yorker" || ballLength == "FullPitch")
					{
						batsmanAnimation = "OffDrive3";
						shotPlayed = "OffDrive3";
						randomId = Random.Range(0, 50);
						if (ballLength == "FullPitch" && randomId > 25 && distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.2f)//wicket-downthetrack
						{
							batsmanAnimation = "DownTheTrackMidOff";
							shotPlayed = "DownTheTrackMidOff";
						}
						else if (distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.2f)
						{
							batsmanAnimation = "MidOffDrive";
							shotPlayed = "MidOffDrive";
						}
					}
					else
					{
						randomId = Random.Range(0, 3);
						if (randomId == 0)
						{
							randomId = Random.Range(0, 3);
							if (randomId == 1)
							{
								batsmanAnimation = "BackFootDrive";
								shotPlayed = "BackFootDrive";
							}
							else
							{
								batsmanAnimation = "StraightDrive_Pace";
								shotPlayed = "StraightDrive_Pace";
							}
						}
						else
						{
							//if (Random.Range (0, 10) > 5) 
							if (distanceBtwBatsmanAndBall > 0.7f)
							{
								batsmanAnimation = "BackfootPunch";
								shotPlayed = "BackfootPunch";
							}
							else
							{
								batsmanAnimation = "Spin_CutShot";
								shotPlayed = "Spin_CutShot";
							}
						}
					}
				}
				break;
			case 3://Cover || Extra Cover
				if (ballLength == "Yorker" || ballLength == "FullPitch" || ballLength == "GoodLength")
				{
					if (powerShot == true)
					{
						randomId = Random.Range(0, 20);
						//					if(currentBowlerType == "spin")
						//					{
						//						batsmanAnimation = "Spin_DeepExtraCover";
						//						shotPlayed = "Spin_DeepExtraCover";
						//					}
						if (distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall < 0.5f)
						{
							if (randomId < 10)
							{
								if (currentBowlerType == "spin" && ballLength == "Yorker" && randomId < 6)
								{
									batsmanAnimation = "Spin_DeepExtraCover";
									shotPlayed = "Spin_DeepExtraCover";
								}
								else if (currentBowlerType != "spin" && (ballLength == "Yorker" || ballLength == "FullPitch") && randomId < 6)
								{
									batsmanAnimation = "Spin_DeepExtraCover";
									shotPlayed = "Spin_DeepExtraCover";
								}
								else
								{
									batsmanAnimation = "CoverSlog";
									shotPlayed = "CoverSlog";
								}
							}
							else
							{
								if (currentBowlerType == "spin" && ballLength == "Yorker" && randomId < 4)
								{
									batsmanAnimation = "Spin_DeepExtraCover";
									shotPlayed = "Spin_DeepExtraCover";
								}
								else if (currentBowlerType != "spin" && (ballLength == "Yorker" || ballLength == "FullPitch") && randomId < 4)
								{
									batsmanAnimation = "Spin_DeepExtraCover";
									shotPlayed = "Spin_DeepExtraCover";
								}
								else
								{
									batsmanAnimation = "StepOutOffSlog";
									shotPlayed = "StepOutOffSlog";
								}
							}
						}
						else
						{
							if (ballLength == "GoodLength" && distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.2f)//wicket-downthetrack
							{
								batsmanAnimation = "DownTheTrackOffSideLoft";
								shotPlayed = "DownTheTrackOffSideLoft";
							}
							else
							{
								randomId = Random.Range(0, 5);
								if (randomId > 3)
								{
									batsmanAnimation = "LoftedCoverDrive";
									shotPlayed = "LoftedCoverDrive";
								}
								else
								{
									batsmanAnimation = "DownTheTrackOffSideLoft";
									shotPlayed = "DownTheTrackOffSideLoft";
								}
							}
						}
					}
					else
					{
						randomId = Random.Range(0, 100);
						if (ballLength == "GoodLength" && randomId < 25)
						{
							if (distanceBtwBatsmanAndBall > 0.7f)
							{
								batsmanAnimation = "BackfootPunch";
								shotPlayed = "BackfootPunch";
							}
							else  //if (randomId < 10)
							{
								batsmanAnimation = "BackFootDrive";
								shotPlayed = "BackFootDrive";
							}
						}
						else if (randomId < 40)
						{
							batsmanAnimation = "CoverDrive2";
							shotPlayed = "CoverDrive2";
						}
						else if (randomId < 60)
						{
							batsmanAnimation = "ExtraCoverDrive";
							shotPlayed = "ExtraCoverDrive";
						}
						else if (randomId < 80)
						{
							batsmanAnimation = "SquarePush";
							shotPlayed = "SquarePush";
						}
						else
						{
							batsmanAnimation = "OffDrive3";
							shotPlayed = "OffDrive3";
						}
					}
				}
				else if (ballLength == "ShortPitch" || ballLength == "Bouncer")
				{
					randomId = Random.Range(0, 30);
					if (powerShot == true)
					{
						if (currentBowlerType == "spin")
						{
							batsmanAnimation = "CoverSlog";
							shotPlayed = "CoverSlog";
						}
						else
						{
							if (distanceBtwBatsmanAndBall >= 0.2f && distanceBtwBatsmanAndBall <= 1.2f)
							{
								if (currentBowlerType == "fast")
								{
									if (randomId < 10 && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)//Mantis ID - 0013550
									{
										batsmanAnimation = "PointSlog";
										shotPlayed = "PointSlog";
									}
									else if (randomId < 20)
									{
										batsmanAnimation = "UpperCut2";
										shotPlayed = "UpperCut2";
									}
									else
									{
										batsmanAnimation = "LoftedDeepCoverShot";
										shotPlayed = "LoftedDeepCoverShot";
									}
								}
								else if (randomId < 15)
								{
									batsmanAnimation = "PointSlogHip";
									shotPlayed = "PointSlogHip";
								}
								else
								{
									batsmanAnimation = "LoftedDeepCoverShot";
									shotPlayed = "LoftedDeepCoverShot";
								}
							}
							else
							{
								batsmanAnimation = "LoftedDeepCoverShot";
								shotPlayed = "LoftedDeepCoverShot";
							}
						}
					}
					else
					{
						if (distanceBtwBatsmanAndBall > 0.3f && distanceBtwBatsmanAndBall < 0.9f)
						{
							if (currentBowlerType == "fast" && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)
							{
								batsmanAnimation = "UpperCut2";
								shotPlayed = "UpperCut2";
								if (randomId < 15 && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)//Mantis ID - 0013550
								{
									batsmanAnimation = "PointSlog";
									shotPlayed = "PointSlog";
								}
							}
							else
							{
								batsmanAnimation = "PointSlogHip";
								shotPlayed = "PointSlogHip";
							}
						}
						else
						{
							if (currentBowlerType == "fast" && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)//Mantis ID - 0013550
							{
								batsmanAnimation = "PointSlog";
								shotPlayed = "PointSlog";
							}
							else
							{
								batsmanAnimation = "PointSlogHip";
								shotPlayed = "PointSlogHip";
							}
						}
					}
				}
				break;
			case 4://Point || Square
				if (ballLength == "Yorker" || ballLength == "FullPitch")
				{
					if (powerShot == true)
					{
						if (distanceBtwBatsmanAndBall > 0.7f)
						{
							batsmanAnimation = "Spin_BackwardPoint";
							shotPlayed = "Spin_BackwardPoint";
						}
						else
						{
							batsmanAnimation = "LoftedSquareCut2";
							shotPlayed = "LoftedSquareCut2";
						}
						//////       //CONTROLLER.GameLog("LSC2 2");
					}
					else
					{
						randomId = Random.Range(0, 3);
						if (distanceBtwBatsmanAndBall > 1.2f)
						{
							if (Random.Range(0, 7) > 2)
							{
								batsmanAnimation = "SquareCut_New";
								shotPlayed = "SquareCut_New";
							}
							else
							{
								batsmanAnimation = "WideSquareCut";
								shotPlayed = "WideSquareCut";
							}
						}
						else if (distanceBtwBatsmanAndBall > 0.65f && ballLength == "Yorker")
						{
							if (randomId == 0)
							{
								batsmanAnimation = "SquareCut11";
								shotPlayed = "SquareCut11";
							}
							else
							{
								batsmanAnimation = "Spin_BackwardPoint";
								shotPlayed = "Spin_BackwardPoint";
							}
						}
						else if (distanceBtwBatsmanAndBall > 0.65f && ballLength == "FullPitch")
						{
							batsmanAnimation = "SquareCut12";
							shotPlayed = "SquareCut12";
						}
						else // close to body...
						{
							//todo
							batsmanAnimation = "OffDrive3";
							shotPlayed = "OffDrive3";
						}
					}
				}
				else if (ballLength == "GoodLength" || ballLength == "ShortPitch")
				{
					randomId = Random.Range(0, 10);
					if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.4f && randomId >= 5)
					{
						batsmanAnimation = "DavidWarner_Switch_Hit";
						shotPlayed = "DavidWarner_Switch_Hit";
					}
					if (ballLength == "GoodLength" && distanceBtwBatsmanAndBall > 0.75f)
					{
						randomId = Random.Range(0, 20);
						if (randomId < 10)
						{
							batsmanAnimation = "WideHitLow";
							shotPlayed = "WideHitLow";
						}
						else
						{
							batsmanAnimation = "SquareCut";
							shotPlayed = "SquareCut";
						}
						if (powerShot == true)
						{
							batsmanAnimation = "SquareSlog";
							shotPlayed = "SquareSlog";
						}
					}
					else if (ballLength == "GoodLength")
					{
						if (powerShot == true)
						{
							batsmanAnimation = "SquareSlog";
							shotPlayed = "SquareSlog";
						}
						else if (distanceBtwBatsmanAndBall > 0.4f)
						{
							batsmanAnimation = "SquareCut2";
							shotPlayed = "SquareCut2";
						}
						else
						{
							batsmanAnimation = "LoftedDeepCoverShot";
							shotPlayed = "LoftedDeepCoverShot";
						}
					}
					else // ShortPitch
					{
						//aimoveandplay
						randomId = Random.Range(0, 100);
						if (randomId < 25)
						{
							batsmanAnimation = "LoftedSquareCut";
							shotPlayed = "LoftedSquareCut";
						}
						else if (randomId < 50)
						{
							if (currentBowlerType == "fast" && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)//Mantis ID - 0013550
							{
								batsmanAnimation = "PointSlog";
								shotPlayed = "PointSlog";
							}
							else
							{
								batsmanAnimation = "PointSlogHip";
								shotPlayed = "PointSlogHip";
							}
						}
						else if (randomId < 75)
						{
							//////       //CONTROLLER.GameLog("Fill the blank 1");
							batsmanAnimation = "LoftedDeepCoverShot";
							shotPlayed = "LoftedDeepCoverShot";
						}
						else
						{
							batsmanAnimation = "Lofted3rdManGlance";
							shotPlayed = "Lofted3rdManGlance";
						}
						//aimoveandplay
					}
				}
				else // Bouncer
				{
					//aimoveandplay
					randomId = Random.Range(0, 100);
					if (randomId < 33)
					{
						batsmanAnimation = "LoftedDeepCoverShot";
						shotPlayed = "LoftedDeepCoverShot";
					}
					else if (randomId < 66 && currentBowlerType == "fast" && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)//Mantis ID - 0013550
					{
						batsmanAnimation = "PointSlog";
						shotPlayed = "PointSlog";
					}
					else
					{
						batsmanAnimation = "Lofted3rdManGlance";
						shotPlayed = "Lofted3rdManGlance";
					}
					//aimoveandplay
				}
				break;
			case 5://3rd-Man
				if (ballLength == "Yorker")
				{
					if (powerShot == true)
					{
						if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.5f)
						{
							randomId = Random.Range(0, 50);
							//if(distanceBtwBatsmanAndBall >= )
							if (randomId < 10)
							{
								batsmanAnimation = "LoftedReverseSweep";
								shotPlayed = "LoftedReverseSweep";
							}
							else if (randomId < 40)
							{
								batsmanAnimation = "GlennMaxwell_SwitchHit2"; //BigBash
								shotPlayed = "GlennMaxwell_SwitchHit2";

							}
							else
							{
								batsmanAnimation = "LoftedReverseSweep2";
								shotPlayed = "LoftedReverseSweep2";
							}
						}
						else if (distanceBtwBatsmanAndBall >= 0.4f)
						{
							batsmanAnimation = "GlennMaxwell_SwitchHit"; // BigBash
							shotPlayed = "GlennMaxwell_SwitchHit";
						}
						else
						{
							//////       //CONTROLLER.GameLog("Fill the blank 2");
							randomId = Random.Range(0, 7);
							if (randomId >= 5)
							{
								batsmanAnimation = "StepOutOffSlog";
								shotPlayed = "StepOutOffSlog";
							}
							else
							{
								batsmanAnimation = "Spin_CoverPoint";
								shotPlayed = "Spin_CoverPoint";
							}
						}
					}
					else
					{
						if (distanceBtwBatsmanAndBall > 0.3f && distanceBtwBatsmanAndBall < 0.65f)
						{
							batsmanAnimation = "SquarePush";
							shotPlayed = "SquarePush";
						}
						else if (distanceBtwBatsmanAndBall >= 0.65f && distanceBtwBatsmanAndBall <= 0.9f)
						{
							randomId = Random.Range(0, 3);
							if (randomId == 0)
							{
								batsmanAnimation = "SquareCut11";
								shotPlayed = "SquareCut11";
							}
							else
							{
								batsmanAnimation = "Spin_CoverPoint";
								shotPlayed = "Spin_CoverPoint";
							}
						}
						else if (distanceBtwBatsmanAndBall > 0.9f)
						{
							if (Random.Range(0, 7) > 2)
							{
								batsmanAnimation = "SquareCut_New";
								shotPlayed = "SquareCut_New";
							}
							else
							{
								batsmanAnimation = "WideSquareCut";
								shotPlayed = "WideSquareCut";
							}
						}
						//aimoveandplay
						else if (distanceBtwBatsmanAndBall < -0.2f)
						{
							batsmanAnimation = "ReverseSweep2";
							shotPlayed = "ReverseSweep2";
						}
						else
						{
							//todo
							batsmanAnimation = "ReverseSweep2";
							shotPlayed = "ReverseSweep2";
						}
						//aimoveandplay
					}
				}
				else if (ballLength == "FullPitch")
				{
					if (powerShot == false)
					{
						if (distanceBtwBatsmanAndBall > 1.2f)
						{
							batsmanAnimation = "SquareCut";
							shotPlayed = "SquareCut";
						}
						else if (distanceBtwBatsmanAndBall > 0.9f)
						{
							batsmanAnimation = "LateCut";
							shotPlayed = "LateCut";
						}
						else if (distanceBtwBatsmanAndBall > 0.65f)
						{
							batsmanAnimation = "SquareCut12";
							shotPlayed = "SquareCut12";
						}
						else if (distanceBtwBatsmanAndBall > 0.5f)
						{
							batsmanAnimation = "3rdManGlance";
							shotPlayed = "3rdManGlance";
						}
						else
						{
							//aimoveandplay
							randomId = Random.Range(0, 10);//22Dec
							if (currentBowlerType == "spin" && distanceBtwBatsmanAndBall < -0.1f)
							{
								batsmanAnimation = "ReverseSweep2";
								shotPlayed = "ReverseSweep2";
							}
							else if (currentBowlerType == "spin" && distanceBtwBatsmanAndBall > -0.4f && randomId > 5)//22Dec
							{
								batsmanAnimation = "ReverseSweepSpin";
								shotPlayed = "ReverseSweepSpin";
							}
							else
							{
								//todo
								batsmanAnimation = "LateCut2";
								shotPlayed = "LateCut2";
							}
							//aimoveandplay
						}
					}
					else
					{
						if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.4f)
						{
							randomId = Random.Range(0, 20);
							if (randomId < 10)
							{
								batsmanAnimation = "LoftedReverseSweep";
								shotPlayed = "LoftedReverseSweep";
							}
							else
							{
								batsmanAnimation = "LoftedReverseSweep2";
								shotPlayed = "LoftedReverseSweep2";
							}
						}
						else
						{
							//////       //CONTROLLER.GameLog("Fill the blank 3");
							batsmanAnimation = "LoftedReverseSweep2";
							shotPlayed = "LoftedReverseSweep2";
						}
					}
				}
				else if (ballLength == "GoodLength" || ballLength == "ShortPitch")
				{
					if (powerShot == false)
					{
						if (distanceBtwBatsmanAndBall >= 0.1f) // away from batsman
						{
							randomId = Random.Range(0, 40);//24Feb
							if (randomId > 15)
							{
								if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.4f && randomId > 25)
								{
									batsmanAnimation = "DavidWarner_Switch_Hit";
									shotPlayed = "DavidWarner_Switch_Hit";
								}
								else
								{
									batsmanAnimation = "LateCut";
									shotPlayed = "LateCut";
								}
							}
							else if (distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall < 0.8f)
							{
								batsmanAnimation = "LateCut3";
								shotPlayed = "LateCut3";
							}
							else if (distanceBtwBatsmanAndBall >= 0.1f && distanceBtwBatsmanAndBall < 0.3f)
							{
								batsmanAnimation = "LateCut";//LateCut2 removed for looking bad
								shotPlayed = "LateCut";
							}
							else if (distanceBtwBatsmanAndBall > 0.4f)
							{
								//24feb
								if (ballLength == "ShortPitch" && ballSpotHeight > 1.2f)
								{
									batsmanAnimation = "ThirdManFlick";
									shotPlayed = "ThirdManFlick";
								}
								else
								{
									batsmanAnimation = "SquareCut2";
									shotPlayed = "SquareCut2";
								}
							}
							else
							{
								batsmanAnimation = "LoftedDeepCoverShot";
								shotPlayed = "LoftedDeepCoverShot";
							}
						}
						else
						{
							randomId = Random.Range(0, 10);//22Dec
							if (distanceBtwBatsmanAndBall > -0.1f && distanceBtwBatsmanAndBall < 0.1f)
							{
								batsmanAnimation = "LateCut4";
								shotPlayed = "LateCut4";
							}
							else if (currentBowlerType == "spin" && distanceBtwBatsmanAndBall > -0.4f && randomId > 5)//22Dec
							{
								batsmanAnimation = "ReverseSweepSpin";
								shotPlayed = "ReverseSweepSpin";
							}
							else if (distanceBtwBatsmanAndBall >= 0.1f && distanceBtwBatsmanAndBall < 0.3f)
							{
								batsmanAnimation = "LateCut";//LateCut2 removed for looking bad
								shotPlayed = "LateCut";
							}
							else
							{
								batsmanAnimation = "LateCut3";
								shotPlayed = "LateCut3";
							}
						}
					}
					else
					{
						//////       //CONTROLLER.GameLog("Fill the blank 4");
						if (ballLength == "ShortPitch")
						{
							batsmanAnimation = "UpperCut2";
							shotPlayed = "UpperCut2";
						}
						else
						{
							//wicket-downthetrack
							randomId = Random.Range(0, 12);
							if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.4f && randomId >= 5)
							{
								batsmanAnimation = "DavidWarner_Switch_Hit";
								shotPlayed = "DavidWarner_Switch_Hit";
							}
							else if (distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.3f)
							{
								batsmanAnimation = "DownTheTrackOffSideLoft";
								shotPlayed = "DownTheTrackOffSideLoft";
							}
							else
							{
								batsmanAnimation = "LateCut";
								shotPlayed = "LateCut";
							}
						}
					}
				}
				else//Bouncer
				{
					randomId = Random.Range(0, 25);
					if (powerShot == false)
					{
						if (currentBowlerType == "fast" && randomId < 15 && distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall <= 0.85f)
						{
							int _randId = Random.Range(0, 50);

							if (_randId < 13)
							{
								batsmanAnimation = "ThirdManFlick";
								shotPlayed = "ThirdManFlick";
							}
							else if (_randId < 38 && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)//Mantis ID - 0013550
							{
								batsmanAnimation = "PointSlog";
								shotPlayed = "PointSlog";
							}
							else
							{
								batsmanAnimation = "UpperCut2";
								shotPlayed = "UpperCut2";
							}
						}
						else
						{
							batsmanAnimation = "PointSlogHip";
							shotPlayed = "PointSlogHip";
						}
						//CONTROLLER.sndController.EnqueueCommentary(33, 0, 0);
					}
					else
					{
						if (currentBowlerType == "fast" && randomId < 15 && distanceBtwBatsmanAndBall >= 0.2f && distanceBtwBatsmanAndBall <= 0.7f)
						{
							if (randomId < 8 && ballSpotHeight >= 1.35f && ballSpotHeight < 1.6f)//Mantis ID - 0013550
							{
								batsmanAnimation = "PointSlog";
								shotPlayed = "PointSlog";
							}
							else if (randomId < 17)
							{
								randomId = Random.Range(0, 5);
								if (randomId < 2)
								{
									batsmanAnimation = "UpperCut";
									shotPlayed = "UpperCut";
								}
								else
								{
									batsmanAnimation = "Thirdman_UpperCut";//BigBash
									shotPlayed = "Thirdman_UpperCut";
								}
							}
							else
							{
								batsmanAnimation = "UpperCut2";
								shotPlayed = "UpperCut2";
							}
						}
						else if (randomId > 20 && distanceBtwBatsmanAndBall > 0.2f)
						{
							batsmanAnimation = "Lofted3rdManGlance";
							shotPlayed = "Lofted3rdManGlance";
						}
						else
						{
							randomId = Random.Range(0, 5);
							if (randomId < 2)
							{
								batsmanAnimation = "UpperCut";
								shotPlayed = "UpperCut";
							}
							else
							{
								batsmanAnimation = "Thirdman_UpperCut";//BigBash
								shotPlayed = "Thirdman_UpperCut";
							}


						}
						//CONTROLLER.sndController.EnqueueCommentary(33, 0, 0);
					}
				}
				break;
			case -1://defense
				if (ballLength == "Yorker" || ballLength == "FullPitch")
				{
					randomId = Random.Range(0, 20);
					if (distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.2f)
					{
						if (randomId > 17 && ballLength == "Yorker" && distanceBtwBatsmanAndBall >= -0.1f && distanceBtwBatsmanAndBall <= 0.1f)
						{
							batsmanAnimation = "DownTheTrackDefensiveShot";
							shotPlayed = "DownTheTrackDefensiveShot";
						}
						else
						{
							batsmanAnimation = "FrontFootDefense";
							shotPlayed = "FrontFootDefense";
						}
					}
					else
					{
						batsmanAnimation = "FrontFootDefense";
						shotPlayed = "FrontFootDefense";
					}
				}
				else
				{
					batsmanAnimation = "BackFootDefense";
					shotPlayed = "BackFootDefense";
				}
				AudioPlayer.instance.PlayTheCrowdSound("defence");
				break;
			case -2://For Left Hand Bat - Long-on || Mid-Wicket
				if (ballLength == "Yorker")
				{
					randomId = Random.Range(0, 3);
					if (powerShot == true)
					{
						if (distanceBtwBatsmanAndBall < 0.6f && currentBowlerType != "spin")
						{
							randomId = Random.Range(0, 12);
							if (randomId < 5)
							{
								batsmanAnimation = "HelicoptorShot";
								shotPlayed = "HelicoptorShot";

							}
							else
							{
								batsmanAnimation = "DhoniHeliCopter_New";//BigBash
								shotPlayed = "DhoniHeliCopter_New";
							}
						}
						else if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.3f)
						{
							if (randomId == 0)
							{
								batsmanAnimation = "DownTheTrackStraightSlog";
								shotPlayed = "DownTheTrackStraightSlog";
							}
							else
							{
								batsmanAnimation = "MidOnSlog";
								shotPlayed = "MidOnSlog";
							}
						}
						else
						{
							if (randomId == 0)
							{
								batsmanAnimation = "LoftedStraightDrive";
								shotPlayed = "LoftedStraightDrive";
							}
							else
							{
								batsmanAnimation = "MidOnSlog";
								shotPlayed = "MidOnSlog";
							}
						}
					}
					else
					{
						if (distanceBtwBatsmanAndBall > 0.1f)
						{
							batsmanAnimation = "OnDrive";
							shotPlayed = "OnDrive";
						}
						else
						{
							batsmanAnimation = "OnDrive";
							shotPlayed = "OnDrive";
						}

					}
				}
				else if (ballLength == "FullPitch" || ballLength == "GoodLength")
				{
					if (powerShot == true)
					{
						if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.3f)
						{
							batsmanAnimation = "DownTheTrackStraightSlog";
							shotPlayed = "DownTheTrackStraightSlog";
						}
						else if (distanceBtwBatsmanAndBall >= -0.15f && distanceBtwBatsmanAndBall <= 0.2f)//wicket-downthetrack
						{
							randomId = Random.Range(0, 10);
							if (randomId < 4)
							{
								batsmanAnimation = "DownTheTrackHittingLegSide";
								shotPlayed = "DownTheTrackHittingLegSide";
							}
							else
							{
								batsmanAnimation = "DownTheTrackSpin_LongOn";//BigBash
								shotPlayed = "DownTheTrackSpin_LongOn";
							}


						}
						else
						{
							randomId = Random.Range(0, 10);
							if (randomId < 4)
							{
								batsmanAnimation = "LoftedStraightDrive";
								shotPlayed = "LoftedStraightDrive";

							}
							else if (ballLength == "GoodLength")
							{
								batsmanAnimation = "ShortPitch_ LongOnSlog";//BigBash
								shotPlayed = "ShortPitch_ LongOnSlog";
							}
							else
							{
								batsmanAnimation = "LoftedStraightDrive";
								shotPlayed = "LoftedStraightDrive";
							}


						}
					}
					else
					{
						randomId = Random.Range(0, 60);
						if (ballLength == "GoodLength" && distanceBtwBatsmanAndBall <= -0.1f && distanceBtwBatsmanAndBall > -0.4f)
						{
							if (randomId < 30)
							{
								batsmanAnimation = "DownTheTrackMidOn";
								shotPlayed = "DownTheTrackMidOn";
							}
							else
							{
								batsmanAnimation = "BackFootPush";
								shotPlayed = "BackFootPush";
							}
						}
						else
						{
							if (distanceBtwBatsmanAndBall > 0.1f)
							{
								batsmanAnimation = "OnDrive";
								shotPlayed = "OnDrive";
							}
							else
							{
								batsmanAnimation = "OnDrive2";
								shotPlayed = "OnDrive2";
							}
						}
					}
				}
				else
				{
					if (powerShot == true)
					{
						batsmanAnimation = "LoftedStraightDrive";
						shotPlayed = "LoftedStraightDrive";
					}
					else
					{
						batsmanAnimation = "BackFootPush";
						shotPlayed = "BackFootPush";
					}
				}
				break;
			case -3://Left Hand Bat - MidWicket
				if (ballLength == "Yorker" || ballLength == "FullPitch")
				{
					if (powerShot == false)
					{
						//aimoveandplay
						randomId = Random.Range(0, 60);
						if (distanceBtwBatsmanAndBall > 0.25f)
						{
							batsmanAnimation = "FrontFootPush";
							shotPlayed = "FrontFootPush";
						}
						else if (distanceBtwBatsmanAndBall >= 0.4f)
						{
							if (randomId < 30)
							{
								batsmanAnimation = "UnOrthodoxShot2";
								shotPlayed = "UnOrthodoxShot2";
							}
							else
							{
								batsmanAnimation = "UnOrthodoxShot1";
								shotPlayed = "UnOrthodoxShot1";
							}
						}
						else
						{
							if (distanceBtwBatsmanAndBall > 0.1f)
							{
								batsmanAnimation = "OnDrive";
								shotPlayed = "OnDrive";
							}
							else
							{
								batsmanAnimation = "OnDrive2";
								shotPlayed = "OnDrive2";
							}
						}
						//aimoveandplay
					}
					else // powerShot == true
					{
						//aimoveandplay
						randomId = Random.Range(0, 60);

						if (ballLength == "Yorker" && distanceBtwBatsmanAndBall < 0.6f && currentBowlerType != "spin")
						{

							if (distanceBtwBatsmanAndBall < 0.3f)
							{
								batsmanAnimation = "HelicoptorShot";
								shotPlayed = "HelicoptorShot";
							}
							else
							{
								batsmanAnimation = "DhoniHeliCopter_New";//BigBash
								shotPlayed = "DhoniHeliCopter_New";
							}

						}
						else if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.3f)
						{
							if (randomId < 25)
							{
								batsmanAnimation = "DownTheTrackStraightSlog";
								shotPlayed = "DownTheTrackStraightSlog";
							}
							else
							{
								batsmanAnimation = "MidOnSlog";
								shotPlayed = "MidOnSlog";
							}
						}
						else if (distanceBtwBatsmanAndBall >= 0.4f)
						{
							batsmanAnimation = "LoftedMidWicket";
							shotPlayed = "LoftedMidWicket";
						}
						else // fullPitch ... etc.,
						{
							if (currentBowlerType == "fast")
							{
								if (randomId < 25)
								{
									batsmanAnimation = "LoftedOnDrive";
									shotPlayed = "LoftedOnDrive";
								}
								else
								{
									batsmanAnimation = "MidOnSlog";
									shotPlayed = "MidOnSlog";
								}
							}
							else
							{
								if (distanceBtwBatsmanAndBall > 0.1f)
								{
									randomId = Random.Range(0, 20);
									if (randomId < 10)
									{
										batsmanAnimation = "LoftedSweepShot";
										shotPlayed = "LoftedSweepShot";
									}
									else
									{
										batsmanAnimation = "OnSideSlog";
										shotPlayed = "OnSideSlog";
									}
								}
								else
								{
									if (randomId < 25)
									{
										batsmanAnimation = "LoftedOnDrive";
										shotPlayed = "LoftedOnDrive";
									}
									else
									{
										batsmanAnimation = "MidOnSlog";
										shotPlayed = "MidOnSlog";
									}
								}
							}
						}
						//aimoveandplay
					}
				}
				else if (ballLength == "GoodLength")
				{
					randomId = Random.Range(0, 150);
					if (powerShot == false)
					{
						if (randomId < 30)
						{
							if (distanceBtwBatsmanAndBall > 0.1f)
							{
								batsmanAnimation = "OnDrive";
								shotPlayed = "OnDrive";
							}
							else
							{
								batsmanAnimation = "OnDrive2";
								shotPlayed = "OnDrive2";
							}
						}
						else if (randomId < 75)
						{
							batsmanAnimation = "MidWicketPush";//aimoveandplay
							shotPlayed = "MidWicketPush";//aimoveandplay
						}
						else if (randomId < 100)
						{
							batsmanAnimation = "MidWicketPush";
							shotPlayed = "MidWicketPush";
						}
						//mantisupdate3
						else if (distanceBtwBatsmanAndBall >= 0.4f && randomId < 125)
						{
							if (randomId < 112)
							{
								batsmanAnimation = "UnOrthodoxShot1";
								shotPlayed = "UnOrthodoxShot1";
							}
							else
							{
								batsmanAnimation = "UnOrthodoxShot2";
								shotPlayed = "UnOrthodoxShot2";
							}
						}
						//mantisupdate3
						else
						{
							batsmanAnimation = "PullShotHip01";
							shotPlayed = "PullShotHip01";
						}
					}
					else
					{
						randomId = Random.Range(0, 50);
						if (distanceBtwBatsmanAndBall > 0.3f)
						{

							if (randomId > 25)
							{
								batsmanAnimation = "ChrisLynn_OnSideHeave"; //BigBash
								shotPlayed = "ChrisLynn_OnSideHeave";
							}
							else
							{
								batsmanAnimation = "OnSideSlog";
								shotPlayed = "OnSideSlog";
							}
						}
						else if (distanceBtwBatsmanAndBall > 0.4f && distanceBtwBatsmanAndBall < 0.6f && randomId < 10)
						{
							batsmanAnimation = "UnOrthodoxShot2";
							shotPlayed = "UnOrthodoxShot2";
						}
						else if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.2f && randomId < 20)
						{
							batsmanAnimation = "LoftedMidWicket";
							shotPlayed = "LoftedMidWicket";
						}
						else if (distanceBtwBatsmanAndBall > -0.2f && randomId < 30)
						{
							batsmanAnimation = "LoftedOnDrive";
							shotPlayed = "LoftedOnDrive";
						}
						else if (distanceBtwBatsmanAndBall <= 0.0f && distanceBtwBatsmanAndBall >= -0.3f && randomId < 40)
						{
							batsmanAnimation = "DownTheTrackStraightSlog";
							shotPlayed = "DownTheTrackStraightSlog";
						}
						else if (randomId < 50 && distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall < 0.85f)
						{
							//wicket-downthetrack
							if (randomId < 45 && distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall <= 0.4f)
							{
								batsmanAnimation = "DownTheTrackHittingLegSide";
								shotPlayed = "DownTheTrackHittingLegSide";
							}
							else
							{
								if (Random.Range(0, 7) > 2)
								{
									batsmanAnimation = "Spin_DeepMidWicket";
									shotPlayed = "Spin_DeepMidWicket";
								}
								else
								{
									batsmanAnimation = "PullShotHip01";
									shotPlayed = "PullShotHip01";
								}
							}
						}
						else
						{
							batsmanAnimation = "LoftedPullShot";
							shotPlayed = "LoftedPullShot";
						}
					}
				}
				else // Bouncer || ShortPitch
				{
					randomId = Random.Range(0, 40);//20feb
					if (distanceBtwBatsmanAndBall > 0.0f && distanceBtwBatsmanAndBall < 0.3f)
					{
						if (randomId > 30)
						{
							batsmanAnimation = "GeorgeBailey_DeepMidWicketSlog";//BigBash
							shotPlayed = "GeorgeBailey_DeepMidWicketSlog";



						}
						else
						{

							batsmanAnimation = "HookShot";
							shotPlayed = "HookShot";
						}
					}
					else if (currentBowlerType != "spin" && distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall < 0.85f)
					{
						randomId = Random.Range(0, 70);//20feb
						if (randomId < 10)
						{
							batsmanAnimation = "DeepMidWicketShot";
							shotPlayed = "DeepMidWicketShot";
						}
						else if (randomId < 20)
						{
							batsmanAnimation = "PullShotChest01";
							shotPlayed = "PullShotChest01";
						}
						else if (randomId < 30 && distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall <= 0.45f)//wicket-downthetrack
						{
							batsmanAnimation = "DownTheTrackHittingLegSide";
							shotPlayed = "DownTheTrackHittingLegSide";
						}
						else if (randomId < 40)
						{
							batsmanAnimation = "PullShotChest";
							shotPlayed = "PullShotChest";
						}
						else
						{
							if (!powerShot)
							{
								batsmanAnimation = "TravisHead_SlapShot";//BigBash
								shotPlayed = "TravisHead_SlapShot";
							}
							else
							{
								batsmanAnimation = "PullShotChest";
								shotPlayed = "PullShotChest";
							}

						}
					}
					else
					{
						//aimoveandplay
						batsmanAnimation = "HookShot";
						shotPlayed = "HookShot";
						//aimoveandplay
					}
				}
				break;
			case -4: // 8 (-4) // Square Leg...		
				if (ballLength == "Yorker" || ballLength == "FullPitch")
				{
					if (powerShot == false)
					{
						randomId = Random.Range(0, 30);
						////       //CONTROLLER.GameLog("distanceBtwBatsmanAndBall ::" + distanceBtwBatsmanAndBall);
						if (distanceBtwBatsmanAndBall > 0.0f && distanceBtwBatsmanAndBall < 0.1f && ballLength == "FullPitch")
						{
							if (randomId < 20)
							{
								if (randomId < 12)
								{
									batsmanAnimation = "LegGlance_Frontfoot";
									shotPlayed = "LegGlance_Frontfoot";
								}
								else
								{
									int randno = Random.Range(0, 10);
									if ((randno >= 7) || powerShot)
									{
										batsmanAnimation = "LegGlance";
										shotPlayed = "LegGlance";
									}
									else if (randno > 4)
									{
										batsmanAnimation = "Spin_LegGlance_02";
										shotPlayed = "Spin_LegGlance_02";
									}
									else
									{
										batsmanAnimation = "Spin_LegGlance_01";
										shotPlayed = "Spin_LegGlance_01";
									}
								}
							}
							else
							{
								batsmanAnimation = "OnSidePush";
								shotPlayed = "OnSidePush";
							}
						}
						else if (distanceBtwBatsmanAndBall >= 0.4f && distanceBtwBatsmanAndBall < 0.8f)
						{
							batsmanAnimation = "UnOrthodoxShot1";
							shotPlayed = "UnOrthodoxShot1";
						}
						else if (distanceBtwBatsmanAndBall >= 0.8f && distanceBtwBatsmanAndBall < 1.2f)
						{
							batsmanAnimation = "UnOrthodoxShot2";
							shotPlayed = "UnOrthodoxShot2";
						}
						else if (currentBowlerType == "spin" && randomId > 15) //
						{
							batsmanAnimation = "SweepShotSpin";
							shotPlayed = "SweepShotSpin";
						}
						else
						{
							batsmanAnimation = "OnSidePush";
							shotPlayed = "OnSidePush";
						}
					}
					else
					{
						randomId = Random.Range(0, 20);
						if (randomId < 10 && distanceBtwBatsmanAndBall > 0.2f && distanceBtwBatsmanAndBall < 0.4f)
						{
							if (randomId < 5)
							{
								batsmanAnimation = "LegGlance_Frontfoot";
								shotPlayed = "LegGlance_Frontfoot";
							}
							else
							{
								batsmanAnimation = "MidWicketSlog";
								shotPlayed = "MidWicketSlog";
							}
						}
						else if (ballLength == "FullPitch" && distanceBtwBatsmanAndBall >= 0.4f && distanceBtwBatsmanAndBall <= 0.8f)
						{
							if (randomId > 10)
							{
								batsmanAnimation = "UnOrthodoxShot1";
								shotPlayed = "UnOrthodoxShot1";
							}
							else
							{
								batsmanAnimation = "UnOrthodoxShot2";
								shotPlayed = "UnOrthodoxShot2";
							}
						}
						else
						{
							if (distanceBtwBatsmanAndBall < 0.2f)
							{
								if (randomId < 10)
								{
									batsmanAnimation = "LegGlance_Frontfoot";
									shotPlayed = "LegGlance_Frontfoot";
								}
								else
								{
									batsmanAnimation = "SweepShotSpin";
									shotPlayed = "SweepShotSpin";
								}
							}
							else
							{
								batsmanAnimation = "LoftedSweepShot";
								shotPlayed = "LoftedSweepShot";
							}
						}
					}
				}
				else if (ballLength == "GoodLength")
				{
					randomId = Random.Range(0, 40);
					if (powerShot == true)
					{
						if (randomId < 10 && distanceBtwBatsmanAndBall > 0.3f && distanceBtwBatsmanAndBall < 0.5f)
						{
							batsmanAnimation = "UnOrthodoxShot1";
							shotPlayed = "UnOrthodoxShot1";
						}
						else if (randomId < 20 && distanceBtwBatsmanAndBall >= 0.5f && distanceBtwBatsmanAndBall < 0.7f)
						{
							batsmanAnimation = "UnOrthodoxShot2";
							shotPlayed = "UnOrthodoxShot2";
						}
						else if (randomId < 30 && distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall < 0.85f)
						{
							if (Random.Range(0, 7) > 2)
							{
								batsmanAnimation = "Spin_DeepMidWicket";
								shotPlayed = "Spin_DeepMidWicket";
							}
							else
							{
								batsmanAnimation = "PullShotHip01";
								shotPlayed = "PullShotHip01";
							}
						}
						else
						{

							if (Random.Range(0, 7) > 2)
							{
								batsmanAnimation = "GeorgeBailey_PullSlog";//BigBash
								shotPlayed = "GeorgeBailey_PullSlog";
							}
							else
							{
								batsmanAnimation = "LoftedPullShot";
								shotPlayed = "LoftedPullShot";
							}

						}
					}
					else
					{
						randomId = Random.Range(0, 70);
						if (randomId < 30 && distanceBtwBatsmanAndBall > 0f && distanceBtwBatsmanAndBall < 0.5f && (currentBowlerType == "fast" || currentBowlerType == "medium"))
						{

							randomId = Random.Range(0, 10);
							if (randomId > 4)
							{
								batsmanAnimation = "AaronFinch_BouncerFlick";//BigBash
								shotPlayed = "AaronFinch_BouncerFlick";
							}
							else
							{
								batsmanAnimation = "AaronFinch_FineLegFlick";//BigBash
								shotPlayed = "AaronFinch_FineLegFlick";
							}

						}
						else if (randomId < 10 && distanceBtwBatsmanAndBall > 0.2f)
						{
							batsmanAnimation = "MidWicketPush";
							shotPlayed = "MidWicketPush";
						}
						else if (randomId < 20 && distanceBtwBatsmanAndBall > 0.3f && distanceBtwBatsmanAndBall < 0.5f)
						{
							randomId = Random.Range(0, 15);
							if (randomId < 3)
							{
								batsmanAnimation = "UnOrthodoxShot1";
								shotPlayed = "UnOrthodoxShot1";
							}
							else
							{
								batsmanAnimation = "DeepSquareLeg_Flick";//BigBash
								shotPlayed = "DeepSquareLeg_Flick";
							}
						}
						else if (randomId < 30 && distanceBtwBatsmanAndBall >= 0.5f && distanceBtwBatsmanAndBall < 0.7f)
						{
							batsmanAnimation = "UnOrthodoxShot2";
							shotPlayed = "UnOrthodoxShot2";
						}
						else
						{
							randomId = Random.Range(0, 10);
							if (distanceBtwBatsmanAndBall < 0 && distanceBtwBatsmanAndBall > -0.2f)
							{
								batsmanAnimation = "MidOnStrike";
								shotPlayed = "MidOnStrike";
							}
							else if (randomId > 4)
							{
								batsmanAnimation = "ShaneWatson_PullShot";//BigBash
								shotPlayed = "ShaneWatson_PullShot";
							}
							else
							{
								if (Random.Range(0, 7) > 2)
								{
									batsmanAnimation = "Spin_DeepMidWicket";
									shotPlayed = "Spin_DeepMidWicket";
								}
								else
								{
									batsmanAnimation = "PullShotHip01";
									shotPlayed = "PullShotHip01";
								}
							}
						}
					}

				}
				else // ShortPitch || Bouncer
				{
					randomId = Random.Range(0, 30);
					if (distanceBtwBatsmanAndBall > 0.0f && distanceBtwBatsmanAndBall < 0.3f)
					{
						if (randomId > 10)
						{
							batsmanAnimation = "DArcyShort_LongOnSlog";//BigBash
							shotPlayed = "DArcyShort_LongOnSlog";
						}
						else
						{

							batsmanAnimation = "HookShot";
							shotPlayed = "HookShot";
						}
					}
					else if (distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall < 0.85f)
					{
						randomId = Random.Range(0, 80);
						if (randomId < 10)
						{
							batsmanAnimation = "DeepMidWicketShot";
							shotPlayed = "DeepMidWicketShot";
						}
						else if (randomId < 20)
						{
							batsmanAnimation = "PullShotChest01";
							shotPlayed = "PullShotChest01";
						}
						else if (randomId < 30)
						{
							batsmanAnimation = "PullShotChest";
							shotPlayed = "PullShotChest";
						}
						else
						{
							batsmanAnimation = "ShortPitch_LongOnPullShot";//BigBash
							shotPlayed = "ShortPitch_LongOnPullShot";
						}
					}
					else
					{
						//aimoveandplay
						if (currentBowlerType == "fast")
						{
							batsmanAnimation = "HookShot";
							shotPlayed = "HookShot";
						}
						else
						{
							randomId = Random.Range(0, 10);
							if (randomId > 4)
							{
								batsmanAnimation = "ShaneWatson_PullShot";//BigBash
								shotPlayed = "ShaneWatson_PullShot";
							}
							else
							{
								batsmanAnimation = "PullShotHip";
								shotPlayed = "PullShotHip";
							}
						}
						//aimoveandplay
					}
				}
				break;
			case -5:// 7 (-5) // FineLeg area
				if (ballLength == "Yorker" || ballLength == "FullPitch")
				{
					if (currentBowlerType == "fast" || currentBowlerType == "medium")
					{

						if (distanceBtwBatsmanAndBall > 0.0f)
						{
							randomId = Random.Range(0, 50);
							////       //CONTROLLER.GameLog("paddleSweep ::" + distanceBtwBatsmanAndBall + "::" + randomId);
							if (distanceBtwBatsmanAndBall < 0.1f && randomId < 25)
							{
								if (ballLength == "FullPitch")
								{
									if (randomId < 10)
									{
										batsmanAnimation = "MidWicketPush";
										shotPlayed = "MidWicketPush";
									}
									else
									{
										batsmanAnimation = "Spin_MidwicketPush";
										shotPlayed = "Spin_MidwicketPush";
									}
								}
								else
								{
									if (randomId < 10)
									{
										int randno = Random.Range(0, 10);
										if (randno >= 7 || powerShot)
										{
											batsmanAnimation = "LegGlance";
											shotPlayed = "LegGlance";
										}
										else if (randno > 4)
										{
											batsmanAnimation = "Spin_LegGlance_02";
											shotPlayed = "Spin_LegGlance_02";
										}
										else
										{
											batsmanAnimation = "Spin_LegGlance_01";
											shotPlayed = "Spin_LegGlance_01";
										}
									}
									else
									{
										batsmanAnimation = "Spin_MidwicketPush";
										shotPlayed = "Spin_MidwicketPush";
									}
								}
							}
							else
							{
								batsmanAnimation = "PaddleSweep";
								shotPlayed = "PaddleSweep";
							}
						}
						else
						{
							/*
                                batsmanAnimation = "LegGlance";
                                shotPlayed = "LegGlance";
                            */

							if (powerShot == false)
							{
								batsmanAnimation = "Spin_LegGlance_01";
								shotPlayed = "Spin_LegGlance_01";
							}
							else
							{
								batsmanAnimation = "Spin_MidwicketPush";
								shotPlayed = "Spin_MidwicketPush";
							}

							randomId = Random.Range(0, 5);

							if (ballLength == "FullPitch")
							{
								if (randomId < 3)
								{
									batsmanAnimation = "MidWicketPush";
									shotPlayed = "MidWicketPush";
								}
								else
								{
									batsmanAnimation = "Spin_MidwicketPush";
									shotPlayed = "Spin_MidwicketPush";
								}
								//								if (distanceBtwBatsmanAndBall < 0.3f && currentBowlerType != "spin")
								//								{
								//									batsmanAnimation = "McCullum_UnorthodoxShot";
								//									shotPlayed = "McCullum_UnorthodoxShot";
								//								}
							}
						}
					}
					else
					{
						randomId = Random.Range(0, 80);

						if (distanceBtwBatsmanAndBall > 0.0f)
						{
							if (randomId < 20)
							{

								batsmanAnimation = "MidWicketPush";
								shotPlayed = "MidWicketPush";

							}
							else if (distanceBtwBatsmanAndBall > 0.1f && distanceBtwBatsmanAndBall < 0.2f && randomId < 40)
							{
								batsmanAnimation = "PaddleSweep";
								shotPlayed = "PaddleSweep";
							}
							else if (distanceBtwBatsmanAndBall > 0.1f && distanceBtwBatsmanAndBall < 0.2f && randomId < 60)
							{
								batsmanAnimation = "SweepShotSpin";
								shotPlayed = "SweepShotSpin";
							}
							else
							{
								if (randomId < 70)
								{
									batsmanAnimation = "SweepShot";
									shotPlayed = "SweepShot";
								}
								else
								{
									batsmanAnimation = "Spin_MidwicketPush";
									shotPlayed = "Spin_MidwicketPush";
								}
							}
						}
						else
						{

							if (randomId > 60)
							{
								int rand = Random.Range(0, 10);
								if (rand >= 7 || powerShot)
								{
									batsmanAnimation = "LegGlance";
									shotPlayed = "LegGlance";
								}
								else if (rand > 4)
								{
									batsmanAnimation = "Spin_LegGlance_02";
									shotPlayed = "Spin_LegGlance_02";
								}
								else
								{
									batsmanAnimation = "Spin_LegGlance_01";
									shotPlayed = "Spin_LegGlance_01";
								}
							}
							else
							{
								if (distanceBtwBatsmanAndBall > 0.8f)
								{
									batsmanAnimation = "SwitchHit_To_Sweep";
									shotPlayed = "SwitchHit_To_Sweep";
								}
								else
								{

									batsmanAnimation = "SweepShot2";
									shotPlayed = "SweepShot2";
									//									if (distanceBtwBatsmanAndBall < 0.3f && currentBowlerType != "spin" && Random.Range(0,5) >= 3)
									//									{
									//										batsmanAnimation = "McCullum_UnorthodoxShot";
									//										shotPlayed = "McCullum_UnorthodoxShot";
									//									}
								}
							}
						}
					}
				}
				else if (ballLength == "GoodLength")
				{
					randomId = Random.Range(0, 5);
					if (randomId < 3 && distanceBtwBatsmanAndBall > 0f && distanceBtwBatsmanAndBall < 0.5f && (currentBowlerType == "fast" || currentBowlerType == "medium"))
					{

						randomId = Random.Range(0, 10);
						if (randomId > 4)
						{
							batsmanAnimation = "AaronFinch_BouncerFlick";//BigBash
							shotPlayed = "AaronFinch_BouncerFlick";
						}
						else
						{
							batsmanAnimation = "AaronFinch_FineLegFlick";//BigBash
							shotPlayed = "AaronFinch_FineLegFlick";
						}

					}
					else if (distanceBtwBatsmanAndBall > 0.5f)
					{
						if (randomId == 0)
						{
							batsmanAnimation = "DilsonScoop";
							shotPlayed = "DilsonScoop";
						}
						else
						{
							batsmanAnimation = "ScoopShot";//BigBash
							shotPlayed = "ScoopShot";
						}
					}
					else
					{
						if (distanceBtwBatsmanAndBall > 0.0f)
						{
							batsmanAnimation = "MidWicketPush";
							shotPlayed = "MidWicketPush";
							if (distanceBtwBatsmanAndBall > 0.3f && distanceBtwBatsmanAndBall < 0.5f)
							{
								batsmanAnimation = "UnOrthodoxShot1";
								shotPlayed = "UnOrthodoxShot1";
							}
							else if (distanceBtwBatsmanAndBall >= 0.5f)
							{
								batsmanAnimation = "UnOrthodoxShot2";
								shotPlayed = "UnOrthodoxShot2";
							}
						}
						else
						{
							if (powerShot == true)
							{
								batsmanAnimation = "MidWicketPush";
								shotPlayed = "MidWicketPush";
							}
							else
							{
								batsmanAnimation = "BackPush";
								shotPlayed = "BackPush";
							}
						}
					}

				}
				else // ShortPitch || Bouncer
				{
					if (distanceBtwBatsmanAndBall > 0.0f && distanceBtwBatsmanAndBall < 0.3f)
					{
						batsmanAnimation = "HookShot";
						shotPlayed = "HookShot";
					}
					else if (currentBowlerType != "spin" && distanceBtwBatsmanAndBall >= 0.3f && distanceBtwBatsmanAndBall < 0.85f)
					{
						batsmanAnimation = "PullShotChest";
						shotPlayed = "PullShotChest";
					}
					else if (ballSpotHeight < 1.3)
					{
						batsmanAnimation = "PullShotHip";
						shotPlayed = "PullShotHip";
					}
					else
					{
						batsmanAnimation = "PullShotChest";
						shotPlayed = "PullShotChest";
					}
				}
				break;
			case 11:
				batColliderHolder.transform.localScale = new Vector3(0, 0, 0);
				batRadius = 0;
				if (currentBowlerType == "spin")
				{
					if (ballSpotHeight < 0.4f)
					{
						batsmanAnimation = "Well_Left_Spin01"; // LeaveTheBall
						shotPlayed = "Well_Left_Spin01";
					}
					else if (ballSpotHeight < 0.8f)
					{
						if (distanceBtwBatsmanAndBall > 0.35f && distanceBtwBatsmanAndBall < 0.45f)
						{
							if (Random.Range(0, 5) > 2)
							{
								batsmanAnimation = "Well_Left_Spin01"; // LeaveTheBall
								shotPlayed = "Well_Left_Spin01";
							}
							else
							{
								batsmanAnimation = "Well_Left_Spin04"; // LeaveTheBall
								shotPlayed = "Well_Left_Spin04";
							}
						}
						else if (distanceBtwBatsmanAndBall >= 0.45f)
						{
							batsmanAnimation = "Well_Left_Spin04"; // LeaveTheBall
							shotPlayed = "Well_Left_Spin04";
						}
						else
						{
							if (Random.Range(0, 5) > 1)
							{
								batsmanAnimation = "Well_Left_Spin03"; // LeaveTheBall
								shotPlayed = "Well_Left_Spin03";
							}
							else
							{
								batsmanAnimation = "Well_Left_Spin02"; // LeaveTheBall
								shotPlayed = "Well_Left_Spin02";
							}
						}
					}
					else
					{
						if (distanceBtwBatsmanAndBall > 0.4f)
						{
							batsmanAnimation = "Well_Left_Pace06"; // LeaveTheBall
							shotPlayed = "Well_Left_Pace06";
						}
						else
						{
							batsmanAnimation = "Well_Left_Pace05"; // LeaveTheBall
							shotPlayed = "Well_Left_Pace05";
						}
					}
				}
				else
				{
					if (ballLength == "Yorker" || ballLength == "FullPitch")
					{
						if (distanceBtwBatsmanAndBall > 0.6f)
						{
							batsmanAnimation = "Well_Left_Pace04";
							shotPlayed = "Well_Left_Pace04";
						}
						else
						{
							if (Random.Range(0, 2) == 1)
							{
								batsmanAnimation = "Well_Left_Spin01"; // LeaveTheBall
								shotPlayed = "Well_Left_Spin01";
							}
							else
							{
								batsmanAnimation = "Well_Left_Pace03";
								shotPlayed = "Well_Left_Pace03";
							}
						}
					}
					else if (ballLength == "GoodLength")
					{
						randomId = Random.Range(0, 4);
						if (randomId == 0)
						{
							batsmanAnimation = "Well_Left_Pace01";
							shotPlayed = "Well_Left_Pace01";
						}
						else if (randomId == 1)
						{
							batsmanAnimation = "Well_Left_Pace02";
							shotPlayed = "Well_Left_Pace02";
						}
						else if (randomId == 2)
						{
							batsmanAnimation = "Well_Left_Spin01";
							shotPlayed = "Well_Left_Spin01";
						}
						else if (randomId == 3)
						{
							batsmanAnimation = "Well_Left_Pace03";
							shotPlayed = "Well_Left_Pace03";
						}

					}
					else if (ballLength == "ShortPitch" || ballLength == "Bouncer")
					{
						randomId = Random.Range(0, 2);

						if (randomId == 0)
						{
							batsmanAnimation = "Well_Left_Pace05";
							shotPlayed = "Well_Left_Pace05";
						}
						else if (randomId == 1)
						{
							batsmanAnimation = "Well_Left_Pace06";
							shotPlayed = "Well_Left_Pace06";
						}
					}
				}
				AudioPlayer.instance.PlayTheCrowdSound("wellleftcrowd");
				break;
		}


		/*if (ballLength == "Yorker" && distanceBtwBatsmanAndBall > -0.15f && distanceBtwBatsmanAndBall < 0.4f  && currentBowlerType == "fast" && Random.Range(0,10)< 1)
		{
			if (Mathf.Abs(ballSpotAtCreaseLine.transform.position.x) < 0.1f)
			{
				batsmanAnimation = "YorkerBall1";
				shotPlayed = "YorkerBall1";
			}
			else
			{
				batsmanAnimation = "YorkerBall2";
				shotPlayed = "YorkerBall2";
			}
			batColliderHolder.transform.localScale = new Vector3(0, 0, 0);
		}*/

		if (CONTROLLER.BowlingSpeed > 5 && ballPitchingDistanceFactor > 0.8f)
		{
			isBouncerBallBowled = true;
		}

		if (isBouncerBallBowled && battingBy != "user" && currentBowlerType != "spin")
		{
				randomId = Random.Range(0, 30);
			if (randomId > 15) //15
			{
				batsmanAnimation = "BouncerNew_02";
				shotPlayed = "BouncerNew_02";
			}
			else if (randomId >= 18)
			{
				batsmanAnimation = "BouncerNew_01";
				shotPlayed = "BouncerNew_01";
			}
			isBouncerBallBowled = false;
		}

		float timer = 0.0f;

		if (batsmanAnimation == "OffDrive" || batsmanAnimation == "YorkerBall2")
		{
			timer = 5.3f;
		}
		else
		{
			timer = 4f;
		}

		Invoke("PlayPostAnim", timer);
		IsPostBatAnim = true;
	}

	public void PlayPostAnim()
	{
		if (IsPostBatAnim == true)
		{
			strikerScript.crossFadeAnimation("BatsmanIdle_PostShot03");
		}
		else
		{
			//			//       //CONTROLLER.GameLog("IS POST BAT ANIM IS FALSE");
		}
	}

	private int batsmanTappingStyle = 1;
	private int savedStrikerIndex = 0;
	private int runnerStandingAnimationId = 2; // 1,2,3...

	public void GetUserBattingInput()
	{
		GetUserBattingKeyboardInput();
		// move the batsman left / right, when the bowler is in the bowling runup...
		if (batsmanCanMoveLeftRight == true)
		{
			if (rightArrowKeyDown == true)
			{
				if (currentBatsmanHand == "right" && RHBatsmanForwardLimit.transform.position.x < batsman.transform.position.x)
				{
					batsman.transform.position -= new Vector3(batsmanStep * Time.deltaTime, 0, 0);
				}
				else if (currentBatsmanHand == "left" && LHBatsmanForwardLimit.transform.position.x > batsman.transform.position.x)
				{
					batsman.transform.position += new Vector3(batsmanStep * Time.deltaTime, 0, 0);
				}

				if (batsmanOnLeftRightMovement == false)
				{
					strikerScript.crossFadeAnimation("Forward");
					strikerScript.setAnimationSpeed(1.5f);
					batsmanOnLeftRightMovement = true;
				}
			}
			else if (leftArrowKeyDown == true)
			{
				if (currentBatsmanHand == "right" && RHBatsmanBackwardLimit.transform.position.x > batsman.transform.position.x)
				{
					batsman.transform.position += new Vector3(batsmanStep * Time.deltaTime, 0, 0);
				}
				else if (currentBatsmanHand == "left" && LHBatsmanBackwardLimit.transform.position.x < batsman.transform.position.x)
				{
					batsman.transform.position -= new Vector3(batsmanStep * Time.deltaTime, 0, 0);
				}

				if (batsmanOnLeftRightMovement == false)
				{
					strikerScript.crossFadeAnimation("Backward");
					strikerScript.setAnimationSpeed(1.5f);
					batsmanOnLeftRightMovement = true;
				}
			}
			else
			{
				if (batsmanOnLeftRightMovement == true)//&& GameModelScript.CutScenesScript.batsmanOnLeftRightMovement == false)
				{
					batsmanOnLeftRightMovement = false;
					SetStrikerTappingIdle();
				}
			}
		}
		else if (batsmanCanMoveLeftRight == false && batsmanOnLeftRightMovement == true)// && GameModelScript.CutScenesScript.batsmanCanMoveLeftRight == false)
		{
			batsmanOnLeftRightMovement = false;
			//if (ShowTutorial() == 0)
			SetStrikerTappingIdle();
		}
	}
	
	private void GetBatsmanTappingStyleId()
	{
		strikerScript.setAnimationSpeed(1.0f);

		if (savedStrikerIndex == 2 || savedStrikerIndex == 6)
		{
			batsmanTappingStyle = 3; //3
		}
		else if (savedStrikerIndex == 1 || savedStrikerIndex == 7)
		{
			batsmanTappingStyle = 4;  //4		
		}
		else if (savedStrikerIndex == 5 || savedStrikerIndex == 8)
		{
			batsmanTappingStyle = 1; //1
		}
		else if (savedStrikerIndex == 4 || savedStrikerIndex == 9 || savedStrikerIndex == 0)
		{
			batsmanTappingStyle = 2; //2
		}
		else
		{
			batsmanTappingStyle = 2;  //Manoj30 // 3 and 10
		}
		batsmanTappingStyle = 4; // hardcoded as per vinod
	}

	private void SetStrikerIdleLoop()
	{
		cancelPostAnim();

		if (batsmanTappingStyle == 1)
		{
			strikerScript._playAnimation("1_IdleLoop");
		}
		else if (batsmanTappingStyle == 2)
		{
			strikerScript._playAnimation("2_IdleLoop");
			strikerScript.setAnimationSpeed(Random.Range(0.75f, 0.9f));
		}
		else if (batsmanTappingStyle == 3)
		{
			strikerScript._playAnimation("3_IdleLoop");
			strikerScript.setAnimationSpeed(Random.Range(0.75f, 0.9f));
		}
		else if (batsmanTappingStyle == 4)
		{
			strikerScript._playAnimation("4_IdleLoop");
			strikerScript.setAnimationSpeed(Random.Range(0.75f, 0.9f));
        }
        else
		{
			strikerScript._playAnimation("1_IdleLoop");
		}
	}

	private void SetStrikerPreIdle()
	{
		if (batsmanTappingStyle == 1)
		{
			strikerScript.crossFadeAnimation("1_PreIdle");
		}
		else if (batsmanTappingStyle == 2)
		{
			strikerScript.crossFadeAnimation("2_PreIdle");
		}
		else if (batsmanTappingStyle == 3)
		{
			strikerScript.crossFadeAnimation("3_PreIdle");
		}
		else if (batsmanTappingStyle == 4)
		{			
			strikerScript.crossFadeAnimation("4_PreIdle");
        }
		else
		{
			strikerScript.crossFadeAnimation("PreIdleV2");
		}
	}

	public void SetStrikerTappingIdle()
	{
        if (batsmanTappingStyle == 1)
		{
			strikerScript.crossFadeAnimation("1_TappingIdle");
		}
		else if (batsmanTappingStyle == 2)
		{
			strikerScript.crossFadeAnimation("2_TappingIdle");
		}
		else if (batsmanTappingStyle == 3)
		{
			strikerScript.crossFadeAnimation("3_TappingIdle");
		}
		else if (batsmanTappingStyle == 4)
		{			
			strikerScript.crossFadeAnimation("4_TappingIdle");
		}
		else
		{
			strikerScript.crossFadeAnimation("TappingIdle");
		}
	}

	private void SetStrikerTappingIdlePlayQueued()
	{
		if (currentBatsmanHand == "right")
		{
			batsman.transform.eulerAngles = new Vector3(0, -90, 0);
		}
		else if (currentBatsmanHand == "left")
		{
			batsman.transform.eulerAngles = new Vector3(0, 90, 0);
		}
	}

	private void SetStrikerFocusTapping()
	{
        if (batsmanTappingStyle == 1)
		{
			strikerScript.crossFadeAnimation("1_FocusTapping");
		}
		else if (batsmanTappingStyle == 2)
		{
			strikerScript.crossFadeAnimation("2_FocusTapping");
		}
		else if (batsmanTappingStyle == 3)
		{
			strikerScript.crossFadeAnimation("3_FocusTapping");
		}
		else if (batsmanTappingStyle == 4)
		{
			strikerScript.crossFadeAnimation("4_FocusTapping");
        }
		else
		{
			strikerScript.crossFadeAnimation("FocusTapping");
		}
	}
	public void cancelPostAnim()
	{
		IsPostBatAnim = false;
		CancelInvoke("PlayPostAnim");
	}

	IEnumerator isBowlerBatsmanIdle()
	{
		//int[] batsmanTime = new int[4]{11,10,15,13};
		int[] batsmanTime = new int[4] { 5, 6, 7, 8 };
		int time = Random.Range(0, batsmanTime.Length);
		yield return new WaitForSeconds(batsmanTime[time]);

		if (batsmanTappingStyle == 1)
		{
			strikerScript.crossFadeAnimation("1_IdleLoopVariation");
		}
		else if (batsmanTappingStyle == 2)
		{
			strikerScript.crossFadeAnimation("2_IdleLoopVariation");
		}
		else if (batsmanTappingStyle == 3)
		{
			strikerScript.crossFadeAnimation("3_IdleLoopVariation");
		}
		else if (batsmanTappingStyle == 4)
		{
			strikerScript.crossFadeAnimation("4_IdleLoopVariation");
		}

		if (batsmanTappingStyle != 4)
		{
			StartCoroutine("isBowlerBatsmanIdle");
		}
	}

	IEnumerator isBowlerRunnerIdle()
	{
		int[] Time = new int[4] { 10, 8, 9, 10 };
		int time = Random.Range(0, Time.Length);
		yield return new WaitForSeconds(Time[time]);

		int _ran = Random.Range(1, 4);
        
		runnerScript.setAnimationSpeed(1.0f);
		if (runnerStandingAnimationId == 1)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript.crossFadeAnimation("Runner1_Idle" + _ran);
			}
			else
			{
				runnerScript.crossFadeAnimation("Runner1_Idle" + _ran + "_Left");
			}

		}
		if (runnerStandingAnimationId == 2)
		{
			if (runnerScript.isMirror == false)
			{
				runnerScript.crossFadeAnimation("Runner2_Idle" + _ran);
			}
			else
			{
				runnerScript.crossFadeAnimation("Runner2_Idle" + _ran + "_Left");
			}
		}
		StartCoroutine("isBowlerRunnerIdle");
	}

	public void stopIsBowlerBatsmanIdle()
	{
		StopCoroutine("isBowlerBatsmanIdle");
		StopCoroutine("isBowlerRunnerIdle");
	}


	private Vector3 GetBatColliderSize(bool StartBowling = false)
	{
		float defaultSkill = 30.0f;
		float scaleFactor = 0.25f;
		float scaleValue = (batsmanSkill - bowlerSkill) / defaultSkill * scaleFactor;
		Vector3 batSize = new Vector3(0, 0, 0);
		batSize = new Vector3(1.0f, 1.0f + scaleValue, 1.0f + scaleValue);
		batRadius = 1.1f;

		//batSize = new Vector3(0.0f, 0.0f, 0.0f);
		//batRadius = 0.0f;
		return batSize;
	}

	// This variable (battingTimingMeter) will define the horizontalSpeed & ballFirstPitchDistance (if powerShot == true)
	private bool ismissTimed = false;
	private float missTimedBattingMeter = 0;
	private float battingTimingMeter;
	private bool updateBattingTimingMeterNeedle = false;
	private float btmPerfectValue = 6f;
	private int gudTimingValue = 12;	//15
	float batsmanSkill = 85;
	float batsmanTimingSkill = 45;
	float bowlerSkill = 40;
	private bool edgeCatch = false;
	private bool isBallTouchedTheRope = false;
	private ShotStatus BattingTimingMeterDisplayText;
	void UpdateBattingTimingMeter()
	{
		if (updateBattingTimingMeterNeedle == true)
		{
			battingTimingMeter = ballTransform.position.z / 5.0f * 100; //8.8 
			if (battingTimingMeter < -100.0f)
			{
				battingTimingMeter = -100.0f;
			}
			else if (battingTimingMeter > 100.0f)
			{
				battingTimingMeter = 100.0f;
			}
			//battingTimingNeedle.transform.localPosition = new Vector3(battingTimingMeter, -40, 0);
			//AI Improve
			ismissTimed = false;
			missTimedBattingMeter = (Mathf.Abs(battingTimingMeter));

			int randval = 8;
			if (CONTROLLER.PowerPlay)
			{
				randval = 6;
			}

			//hardcode
			/*#if UNITY_EDITOR
                        battingTimingMeter = btmPerfectValue;     //Hardcode
            #endif*/
			bool isPower = loft;
			if (Mathf.Abs(battingTimingMeter) <= btmPerfectValue)
			{
				//battingTimingNeedleText.text = "[FFFFFF]PERFECT[-]";
				BattingTimingMeterDisplayText = ShotStatus.PERFECT;
				ismissTimed = false;
			}
			else if (battingTimingMeter < -65)
			{
				///battingTimingNeedleText.text = "[FFFFFF]TOO EARLY[-]";
				BattingTimingMeterDisplayText = ShotStatus.TOO_EARLY;

				if (Random.Range(0, 10) > randval && battingBy == "user" && isPower)
				{
					ismissTimed = true;
					battingTimingMeter = -40;
				}
			}
			else if (battingTimingMeter < -30)
			{
				//battingTimingNeedleText.text = "[FFFFFF]EARLY[-]";
				BattingTimingMeterDisplayText = ShotStatus.EARLY;

				if (Random.Range(0, 10) > randval && battingBy == "user" && isPower)
				{
					ismissTimed = true;
					battingTimingMeter = -40;
				}
			}
			else if (battingTimingMeter < -btmPerfectValue)
			{
				//battingTimingNeedleText.text = "[FFFFFF]EARLY - NICE TRY[-]";
				BattingTimingMeterDisplayText = ShotStatus.EARLY_NICETRY;

				if (Random.Range(0, 10) > randval && battingBy == "user" && isPower)
				{
					ismissTimed = true;
					battingTimingMeter = -30;
				}
			}
			else if (battingTimingMeter > 65)
			{
				//battingTimingNeedleText.text = "[FFFFFF]TOO LATE[-]";
				BattingTimingMeterDisplayText = ShotStatus.TOO_LATE;

				if (Random.Range(0, 10) > randval && battingBy == "user" && isPower )
				{
					ismissTimed = true;
					battingTimingMeter = 40;
				}
			}
			else if (battingTimingMeter > 30)
			{
				//battingTimingNeedleText.text = "[FFFFFF]LATE[-]";
				BattingTimingMeterDisplayText = ShotStatus.LATE;

				if (Random.Range(0, 10) > randval && battingBy == "user" && isPower )
				{
					ismissTimed = true;
					battingTimingMeter = 35;
				}
			}
			else
			{
				//battingTimingNeedleText.text = "[FFFFFF]GOOD[-]";
				BattingTimingMeterDisplayText = ShotStatus.GOOD;

				ismissTimed = false;
				if (Random.Range(0, 10) > randval && battingBy == "user"&& isPower)
				{
					ismissTimed = true;
					battingTimingMeter = 30;
				}
			}
			if (battingBy != "user")
			{
				ismissTimed = false;
			}
		}


		//		if(ball.transform.position.z >= -8.0 && ball.transform.position.z <= 0)
		//if (ballRayCastReferenceGOTransform.position.z >= -8.0 && ballRayCastReferenceGOTransform.position.z <= 0)
		if (ballRayCastReferenceGOTransform.position.z >= -4.0 && ballRayCastReferenceGOTransform.position.z <= 4)
		{
			if (CONTROLLER.shotIndicator == 1 && swipeHighlightRenderer.enabled == false)
			{
				SetSwipeHighlightRenderState(true);
			}
			else if (CONTROLLER.shotIndicator == 0 && swipeHighlightRenderer.enabled == true)
			{
				SetSwipeHighlightRenderState(false);
			}
			if (swipeHighlightRenderer.enabled == true)
			{
				swipeHighlight.transform.eulerAngles = new Vector3(swipeHighlight.transform.eulerAngles.x, swipeHighlight.transform.eulerAngles.y, swipeHighlight.transform.eulerAngles.z + 360 * Time.deltaTime);
			}
			if (GameModelScript != null && GameModelScript.CanShowTutorial())
			{
				GameModelScript.ShowShotTutorial(true);
			}
		}
		else
		{
			if (swipeHighlightRenderer.enabled == true)
			{
				SetSwipeHighlightRenderState(false);
			}
			if (GameModelScript != null)
			{
				GameModelScript.ShowShotTutorial(false);//shankar 09April
			}
		}
	}

	private void SetSwipeHighlightRenderState(bool flag)
	{
		if(swipeHighlightRenderer!=null)
			swipeHighlightRenderer.enabled = flag;
	}

	void  updateBatsmanTiming( )
	{
		float incrementVal = 0.007f;
		/*if (CONTROLLER.difficultyMode == DifficultyMode.hard || CONTROLLER.difficultyMode == DifficultyMode.expert)
		{
			incrementVal = 0.0032f;
		}
		else if (CONTROLLER.difficultyMode == DifficultyMode.medium)
		{
			incrementVal = 0.0032f;
		}
		else
		{
			incrementVal = 0.0040f;
		}*/
		float val = batsmanTimingSkill;
		val = (val * incrementVal);
		//timingFiller.transform.localScale = new Vector3(val, 1f, 1);
		float perf = ((val * 10) / 0.5f);//0.7
		if (perf < 2.5f)
		{
			perf = 2.5f;
		}
		float myVal = (Mathf.Ceil(perf));
		btmPerfectValue = (int)myVal;
	}
	#endregion

	public void ResetAll_BatMP()
	{
		action = -2;
		canApplyFriction = true;
		applyBallFriction = false;
		CONTROLLER.SixDistance = 0;

		reflectionFromBlackBoard = false;
		fielderCollectedTheBall = false;
		ballBoundaryReflection = false;
	   //ManojAdded
		radius = 5; //60
		speed = 1.25f;
		mainCameraOnTopDownView = false;
		mainCamera.fieldOfView = 60;
		//ManojAdded
		savedStrikerIndex = CONTROLLER.StrikerIndex;
		GetBatsmanTappingStyleId();
		CamShouldNotFollowBallY = false;
		sixDistanceCamera.enabled = false;
		rightSideCamera.enabled = false;
		leftSideCamera.enabled = false;
		straightCamera.enabled = false;
		sideCameraSelected = false;
		SetSwipeHighlightRenderState(false);
		LateAttempt = false;
		fielderSpeed = 7;
		touchDeviceShotInput = false; // Android || iOS
		ballNoOfBounce = 0;
		ballProjectileAngle = 270;
		ballProjectileHeight = 2.3f; // meters
		horizontalSpeed = 22;//22; // meters per second
		SwitchToHighPoly();
		swingProjectileAngle = 0; // if swing...
		swingValue = 0;
		swingingBall = false;
		playedUltraSlowMotion = false;
		slipShot = false;
		ballToFineLeg = false;
		prevSixDist = 0;
		BallHitTime = 0.0f;
		BallPickTime = 0.0f;
		prevMousePos = Vector2.zero;

		loftBtn.gameObject.SetActive(true);
		loftBtn2.gameObject.SetActive(true);
		isTimeToShowAd = true;
		canSwipeNow = false;
		ballProjectileAnglePerSecondFactor = 1.7f;
		if (GameModel.isGamePaused == false)
		{
			AdIntegrate.instance.SetTimeScale(1f);
		}

		ballPreCatchingDistance = 1.0f; // 1 meter before the bouncing the ball...
		bowlerRunningSpeed = 5; // for program controlled bowler runup animation...
		pauseTheBall = false;
		shotPlayed = ""; // new 04-May-2011, to avoid ball collision even if the batsman not playing the shot...
		ballResult = "";
		canTakeRun = false;
		applyBallFiction = false;
		currentBallNoOfRuns = 0;
		wideBall = false;
		freeHit = false;
		wideBallChecked = false;
		shortestBallPickupDistance = 1000; // init to max distance...
		ballReleased = false;
		ballOverTheFence = false;
		lbwAppeal = false;
		LBW = false;
		ballInline = false;
		throwingFirstBounceDistance = 0;
		canBe4or6 = 6;
		isBallTouchedTheRope = false;
		stump1.GetComponent<Animation>().Play("idle");
		boardCollider.SetActive(false);
		billBoardCollider.SetActive(false);
		stadiumCollider.enabled = false;
		fielder10Action = "";
		
		mainUmpire.GetComponent<Animation>().Play("foldidle");
		mainUmpire.transform.LookAt(new Vector3(groundCenterPoint.transform.position.x, 0, groundCenterPoint.transform.position.z));
		fielder10AnimationComponent.Play("idle");
		fielder10.transform.eulerAngles = new Vector3(fielder10.transform.eulerAngles.x, 0, fielder10.transform.eulerAngles.z);

		wicketKeeperAnimationComponent.Play("idle");
		wicketKeeperBall.GetComponent<Renderer>().enabled = false;
		wicketKeeper.transform.eulerAngles = new Vector3(0f, 180f, 0f);
		wicketKeeperCatchingAnimationSelected = false;

		slipFielderWarmUpAction = false;
		slipFielder2WarmUpAction = false;

		if (CONTROLLER.currentMatchBalls != 0)
		{
			ResetFielders();//shankar 08April
		}
		ballSpinningSpeedInX = Random.Range(-3600, -1800);
		ballSpinningSpeedInZ = Random.Range(-3600, -1800); // for spin bowler
		currentBowlerType = CONTROLLER.bowlerType;//ShankarEdit

		if (currentBowlerType == "fast")
		{
			fielder10.transform.position = fielder10FastInit.transform.position; 
			spinBowlerSkin.GetComponent<Renderer>().enabled = false;
			spinBowlerBall.GetComponent<Renderer>().enabled = false;

			bowler = fastBowler;
			bowlerSkin = fastBowlerSkin;
			bowlerBall = fastBowlerBall;
			ballSpinningSpeedInZ = Random.Range(-500f, 500f);
			swingValue = Random.Range(-2.0f, 2.0f);
			if (currentBatsmanHand == "right")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4RHBFast;
			}
			else if (currentBatsmanHand == "left")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4LHBFast;
			}

			bowlingBounceFactor = 1.0f * pitchFactor;

		}
		else if (currentBowlerType == "spin")
		{
			bowlingBounceFactor = 0.6f * pitchFactor;

			fielder10.transform.position = fielder10SpinInit.transform.position; 
			fastBowlerSkin.GetComponent<Renderer>().enabled = false;
			fastBowlerBall.GetComponent<Renderer>().enabled = false;
			bowler = spinBowler;
			bowlerSkin = spinBowlerSkin;
			bowlerBall = spinBowlerBall;
			if (currentBatsmanHand == "right")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4RHBSpin;
			}
			else if (currentBatsmanHand == "left")
			{
				wicketKeeper.transform.position = wicketKeeperInitPosition4LHBSpin;
			}
		}

		ballTransform.position = ballInitPosition;
		ballTransform.eulerAngles = new Vector3(0f, 2f, 90f);
		ballSkin.transform.eulerAngles = Vector3.zero;

		ShowBall(false);

		strikerScript.resetSpeed(0.0f, 0.0f);
		runnerScript.resetSpeed(0.0f, 0.0f);

		if (currentBatsmanHand == "right")
		{
			batsman.transform.position = RHBatsmanInitPosition;
			batsmanInitXPos = batsman.transform.position.x;
			strikerScript.setMirror(false);
			batCollider = batCollider_Right;
			batColliderHolder = batColliderHolder_Right;
			batColliderComponent = batColliderComponent_Right;
			batColliderHolder_Left.transform.localScale = Vector3.zero;
			//batEdgeGO = rightBatsmanEdgeGO;
			batsman.transform.eulerAngles = new Vector3(batsman.transform.eulerAngles.x, 270, batsman.transform.eulerAngles.z);
			batColliderHolder.transform.localScale = GetBatColliderSize();
		}
		else if (currentBatsmanHand == "left")
		{
			batsman.transform.position = LHBatsmanInitPosition;
			batsmanInitXPos = batsman.transform.position.x;
			strikerScript.setMirror(true);
			batCollider = batCollider_Left;
			batColliderHolder = batColliderHolder_Left;
			batColliderComponent = batColliderComponent_Left;
			batColliderHolder_Right.transform.localScale = Vector3.zero;
			//batEdgeGO = leftBatsmanEdgeGO;
			batsman.transform.eulerAngles = new Vector3(batsman.transform.eulerAngles.x, 90, batsman.transform.eulerAngles.z);
			batColliderHolder.transform.localScale = GetBatColliderSize();
		}
		batsman.transform.localScale = new Vector3(1, batsman.transform.localScale.y, batsman.transform.localScale.z);
		SetStrikerIdleLoop();

		squareLegGlance = false;
		if (currentBowlerHand == "right")
		{
			bowler.transform.localScale = new Vector3(1, bowler.transform.localScale.y, bowler.transform.localScale.z);
			ballOriginGO.transform.position = new Vector3(-0.6f, ballOriginGO.transform.position.y, ballOriginGO.transform.position.z);
			ballTransform.position = new Vector3(-0.6f, ballTransform.position.y, ballTransform.position.z);
		}
		else if (currentBowlerHand == "left")
		{
			bowler.transform.localScale = new Vector3(-1, bowler.transform.localScale.y, bowler.transform.localScale.z);
			ballOriginGO.transform.position = new Vector3(-0.84f, ballOriginGO.transform.position.y, ballOriginGO.transform.position.z);
			ballTransform.position = new Vector3(-0.84f, ballTransform.position.y, ballTransform.position.z);
		}

		bowler.GetComponent<Animation>().Play("BowlerIdle");
		bowlerBall.GetComponent<Renderer>().enabled = true;


		canActivateBowler = false;           //changes
		hideBowlingInterface = false;
		userBowlerCanMoveBowlingSpot = false;
		userBowlingSpotSelected = false;

		ballStatus = "";
		canMakeShot = false;
		batsmanTriggeredShot = false;
		batsmanMadeShot = false;
		batsmanCanMoveLeftRight = false;
		batsmanOnLeftRightMovement = false;
		/*******************Shankar Edit********/
		perfectShot = false;
		DisableTrail();
		/***************************************/
		powerShot = false;
		powerKeyDown = false;
		ballOnboundaryLine = false;

		wicketKeeperIsActive = false;
		wicketKeeperStatus = "";

		stopTheFielders = false;

		runner.transform.position = runnerInitPosition;//25march
		runnerInitPosition = runner.transform.position;//25march

		runner.transform.eulerAngles = new Vector3(runner.transform.eulerAngles.x, 90, runner.transform.eulerAngles.z); // 180

		stickerStatus = "idle";
		nonStickerStatus = "idle";
		strikerScript.resetLayerVal();
		runnerScript.resetLayerVal();
		takingRun = false;
		isRunOut = false;
		boundaryAction = "";
		waitForCommentary = false;


		rightSideCamera.transform.position = new Vector3(rightSideCamera.transform.position.x, rightSideCamera.transform.position.y, 10f);
		leftSideCamera.transform.position = mainCamera.transform.position;
		straightCamera.transform.position = new Vector3(straightCamera.transform.position.x, straightCamera.transform.position.y, -33);

		rightSideCamera.fieldOfView = 50;
		leftSideCamera.fieldOfView = 50;
		straightCamera.fieldOfView = 50;
		cameraToKeeper = false;


		upArrowKeyDown = false;
		downArrowKeyDown = false;
		leftArrowKeyDown = false;
		rightArrowKeyDown = false;

		ShowBowler(true);
		ShowFielder10(false, false);

		updateBattingTimingMeterNeedle = false;
		
		InitCamera();
		mainCamera.enabled = true;
		introCamera.enabled = false;
		FielderExtraActions();

		UpdateShadowsAndPreview();
		ChangePlayerLeftRightTextures();
		SetBowlerSide();
		resetUltraMotionVariables();
		AudioPlayer.instance.CallGarbageCollection();
		ballSkinRigidBody.Sleep();
		cancelPostAnim();
		iTween.Stop(fielder10);	
		fielder10.transform.position = bowler.transform.position;
	}
}
