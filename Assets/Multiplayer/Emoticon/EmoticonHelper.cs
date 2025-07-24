using UnityEngine;
using System.Collections;
using UnityEngine .UI;
/// <summary>
/// Helper Class to assign Emoticon values and animate them.
/// </summary>
public class EmoticonHelper : MonoBehaviour
{
	/// <summary>
	/// The sprite to animate.
	/// </summary>
	public RawImage thisSprite;

	#region Local Variables to Animate Sprites
	/// <summary>
	/// The name of the sprite (emoticon type).
	/// </summary>
	string spriteName = string.Empty;
	/// <summary>
	/// The current loop count / frames.
	/// </summary>
	int currentLoopCount = -1;
	/// <summary>
	/// The total loop count / frames.
	/// </summary>
	int totalLoopCount = -1;
	#endregion

	#region Unity Events
	/// <summary>
	/// Start this instance.
	/// </summary>
	IEnumerator Start ()
	{
		yield return new WaitForSeconds (0.2f);
		thisSprite.enabled = false;
	}
	#endregion

	#region Public Methods
	/// <summary>
	/// Playes the animation.
	/// </summary>
	/// <param name="_spriteName">_sprite name.</param>
	/// <param name="_totalLoopCount">_total loop count.</param>
	public void PlayAnimation (string _spriteName, int _totalLoopCount)
	{
		if(ScoreBoardMultiPlayer .instance .EmoticonPanel .activeSelf )
			StartCoroutine (AssignAnimationData (_spriteName, _totalLoopCount));
	}
	#endregion

	#region Start / Stop Animation
	/// <summary>
	/// Assigns the animation data.
	/// </summary>
	/// <returns>The animation data.</returns>
	/// <param name="_spriteName">_sprite name.</param>
	/// <param name="_totalLoopCount">_total loop count.</param>
	private IEnumerator AssignAnimationData (string _spriteName, int _totalLoopCount)
	{
		if (currentLoopCount != -1)
			yield return StartCoroutine ("StopAnimation");
		yield return new WaitForSeconds (0.1f);
		spriteName = _spriteName;//+ "-0";
		totalLoopCount = _totalLoopCount;
		thisSprite.enabled = true;
		StartCoroutine ("StartAnimation");
	}

	/// <summary>
	/// Starts the animation.
	/// </summary>
	/// <returns>The animation.</returns>
	private IEnumerator StartAnimation ()
	{
		for (int i=0; i<5; i++)
		{
//			thisSprite.name = spriteName + (i + 1);
			thisSprite .texture =EmoticonAnimator.Instance .getTexture (spriteName ,(i)); 
			yield return new WaitForSeconds (0.05f);
		}

		if (currentLoopCount < totalLoopCount)
		{
			currentLoopCount ++;
			StartCoroutine ("StartAnimation");
		}
		else
		{
			StartCoroutine ("StopAnimation");
		}
	}

	/// <summary>
	/// Stops the animation.
	/// </summary>
	/// <returns>The animation.</returns>
	public IEnumerator StopAnimation ()
	{
		if (currentLoopCount != -1)
		{
			StopCoroutine ("StartAnimation");
		}
		yield return null;
//		thisSprite.name = "EmptyAlpha";
		thisSprite.texture = EmoticonAnimator.Instance.alphaImage;
		currentLoopCount = -1;
		totalLoopCount = -1;
		thisSprite.enabled = false;
	}
	#endregion
}
