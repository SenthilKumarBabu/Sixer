﻿namespace CodeStage.AntiCheat.Detectors
{
	using Common;

	using System;
	using UnityEngine;

#if UNITY_5_4_OR_NEWER
	using UnityEngine.SceneManagement;
#endif

	/// <summary>
	/// Detects cheating of any Obscured type (except \link ObscuredTypes.ObscuredPrefs ObscuredPrefs\endlink, it has own detection features) used in project.
	/// </summary>
	/// It allows cheaters to find desired (fake) values in memory and change them, keeping original values secure.<br/>
	/// It's like a cheese in the mouse trap - cheater tries to change some obscured value and get caught on it.
	/// 
	/// Just add it to any GameObject as usual or through the "GameObject > Create Other > Code Stage > Anti-Cheat Toolkit" 
	/// menu to get started.<br/>
	/// You can use detector completely from inspector without writing any code except the actual reaction on cheating.
	/// 
	/// Avoid using detectors from code at the Awake phase.
	[AddComponentMenu(MenuPath + ComponentName)]
	[HelpURL(ACTkConstants.DocsRootUrl + "class_code_stage_1_1_anti_cheat_1_1_detectors_1_1_obscured_cheating_detector.html")]
	public class ObscuredCheatingDetector : ActDetectorBase
	{
		internal const string ComponentName = "Obscured Cheating Detector";
		internal const string FinalLogPrefix = ACTkConstants.LogPrefix + ComponentName + ": ";

		private static int instancesInScene;

		#region public fields

		/// <summary>
		/// Max allowed difference between encrypted and fake values in \link ObscuredTypes.ObscuredDouble ObscuredDouble\endlink. Increase in case of false positives.
		/// </summary>
		[Tooltip("Max allowed difference between encrypted and fake values in ObscuredDouble. Increase in case of false positives.")]
		public double doubleEpsilon = 0.0001d;

		/// <summary>
		/// Max allowed difference between encrypted and fake values in \link ObscuredTypes.ObscuredFloat ObscuredFloat\endlink. Increase in case of false positives.
		/// </summary>
		[Tooltip("Max allowed difference between encrypted and fake values in ObscuredFloat. Increase in case of false positives.")]
		public float floatEpsilon = 0.0001f;

		/// <summary>
		/// Max allowed difference between encrypted and fake values in \link ObscuredTypes.ObscuredVector2 ObscuredVector2\endlink. Increase in case of false positives.
		/// </summary>
		[Tooltip("Max allowed difference between encrypted and fake values in ObscuredVector2. Increase in case of false positives.")]
		public float vector2Epsilon = 0.1f;

		/// <summary>
		/// Max allowed difference between encrypted and fake values in \link ObscuredTypes.ObscuredVector3 ObscuredVector3\endlink. Increase in case of false positives.
		/// </summary>
		[Tooltip("Max allowed difference between encrypted and fake values in ObscuredVector3. Increase in case of false positives.")]
		public float vector3Epsilon = 0.1f;

		/// <summary>
		/// Max allowed difference between encrypted and fake values in \link ObscuredTypes.ObscuredQuaternion ObscuredQuaternion\endlink. Increase in case of false positives.
		/// </summary>
		[Tooltip("Max allowed difference between encrypted and fake values in ObscuredQuaternion. Increase in case of false positives.")]
		public float quaternionEpsilon = 0.1f;
		#endregion

		#region public static methods
		/// <summary>
		/// Creates new instance of the detector at scene if it doesn't exists. Make sure to call NOT from Awake phase.
		/// </summary>
		/// <returns>New or existing instance of the detector.</returns>
		public static ObscuredCheatingDetector AddToSceneOrGetExisting()
		{
			return GetOrCreateInstance;
		}

		/// <summary>
		/// Starts all Obscured types cheating detection for detector you have in scene.
		/// </summary>
		/// Make sure you have properly configured detector in scene with #autoStart disabled before using this method.
		public static void StartDetection()
		{
			if (Instance != null)
			{
				Instance.StartDetectionInternal(null);
			}
			else
			{
				Debug.LogError(FinalLogPrefix + "can't be started since it doesn't exists in scene or not yet initialized!");
			}
		}

		/// <summary>
		/// Starts all Obscured types cheating detection with specified callback.
		/// </summary>
		/// If you have detector in scene make sure it has empty Detection Event.<br/>
		/// Creates a new detector instance if it doesn't exists in scene.
		/// <param name="callback">Method to call after detection.</param>
		public static void StartDetection(Action callback)
		{
			GetOrCreateInstance.StartDetectionInternal(callback);
		}

		/// <summary>
		/// Stops detector. Detector's component remains in the scene. Use Dispose() to completely remove detector.
		/// </summary>
		public static void StopDetection()
		{
			if (Instance != null) Instance.StopDetectionInternal();
		}

		/// <summary>
		/// Stops and completely disposes detector component.
		/// </summary>
		/// On dispose Detector follows 2 rules:
		/// - if Game Object's name is "Anti-Cheat Toolkit Detectors": it will be automatically 
		/// destroyed if no other Detectors left attached regardless of any other components or children;<br/>
		/// - if Game Object's name is NOT "Anti-Cheat Toolkit Detectors": it will be automatically destroyed only
		/// if it has neither other components nor children attached;
		public static void Dispose()
		{
			if (Instance != null) Instance.DisposeInternal();
		}
		#endregion

		#region static instance
		/// <summary>
		/// Allows reaching public properties from code.
		/// Can be null if detector does not exist in scene or if accessed at Awake phase.
		/// </summary>
		public static ObscuredCheatingDetector Instance { get; private set; }

		private static ObscuredCheatingDetector GetOrCreateInstance
		{
			get
			{
			    if (Instance != null) return Instance;

			    if (detectorsContainer == null)
			    {
			        detectorsContainer = new GameObject(ContainerName);
			    }
			    Instance = detectorsContainer.AddComponent<ObscuredCheatingDetector>();
			    return Instance;
			}
		}
		#endregion

		internal static bool ExistsAndIsRunning
		{
			get
			{
				//object.Equals(Instance, null); 
                return (object)Instance != null && Instance.IsRunning;
			}
		}

		private ObscuredCheatingDetector() {} // prevents direct instantiation

		#region unity messages
#if ACTK_EXCLUDE_OBFUSCATION
		[System.Reflection.Obfuscation(Exclude = true)]
#endif
		private void Awake()
		{
			instancesInScene++;
			if (Init(Instance, ComponentName))
			{
				Instance = this;
			}

#if UNITY_5_4_OR_NEWER
			SceneManager.sceneLoaded += OnLevelWasLoadedNew;
#endif
		}

#if ACTK_EXCLUDE_OBFUSCATION
		[System.Reflection.Obfuscation(Exclude = true)]
#endif
		protected override void OnDestroy()
		{
			base.OnDestroy();
			instancesInScene--;
		}

#if UNITY_5_4_OR_NEWER
		private void OnLevelWasLoadedNew(Scene scene, LoadSceneMode mode)
		{
			OnLevelLoadedCallback();
		}
#else
#if ACTK_EXCLUDE_OBFUSCATION
		[System.Reflection.Obfuscation(Exclude = true)]
#endif
		private void OnLevelWasLoaded(int level)
		{
			OnLevelLoadedCallback();
		}
#endif

		private void OnLevelLoadedCallback()
		{
			if (instancesInScene < 2)
			{
				if (!keepAlive)
				{
					DisposeInternal();
				}
			}
			else
			{
				if (!keepAlive && Instance != this)
				{
					DisposeInternal();
				}
			}
		}
		#endregion

		private void StartDetectionInternal(Action callback)
		{
			if (isRunning)
			{
				Debug.LogWarning(FinalLogPrefix + "already running!", this);
				return;
			}

			if (!enabled)
			{
				Debug.LogWarning(FinalLogPrefix + "disabled but StartDetection still called from somewhere (see stack trace for this message)!", this);
				return;
			}

			if (callback != null && detectionEventHasListener)
			{
				Debug.LogWarning(FinalLogPrefix + "has properly configured Detection Event in the inspector, but still get started with Action callback. Both Action and Detection Event will be called on detection. Are you sure you wish to do this?", this);
			}

			if (callback == null && !detectionEventHasListener)
			{
				Debug.LogWarning(FinalLogPrefix + "was started without any callbacks. Please configure Detection Event in the inspector, or pass the callback Action to the StartDetection method.", this);
				enabled = false;
				return;
			}

			CheatDetected += callback;
			started = true;
			isRunning = true;
		}

		protected override void StartDetectionAutomatically()
		{
			StartDetectionInternal(null);
		}

		protected override void DisposeInternal()
		{
			base.DisposeInternal();
			if (Instance == this) Instance = null;
		}
    }
}