class GlowEffect extends MonoBehaviour
{
	public static var instance : GlowEffect;
	private var animSpeed : float = 0.2;
	
	protected function Awake ()
	{
		instance = this;
	}
	
	protected function Start ()
	{
		
	}
	
	/*private function TweenThis ()
	{
		var scaleHash : Hashtable = new Hashtable ();
		scaleHash.Add ("scale", new Vector3(0, 0, 0));
		scaleHash.Add ("time", 0);
		scaleHash.Add ("easetype", "spring");
		scaleHash.Add ("oncomplete", "FadeOutCompleted");
		scaleHash.Add ("oncompletetarget", this.gameObject);
			
		iTween.FadeTo (this.gameObject, 0, 0);
		iTween.ScaleTo (this.gameObject, scaleHash);
	}
	
	private function FadeOutCompleted ()
	{
		var scaleHash : Hashtable = new Hashtable ();
		scaleHash.Add ("scale", new Vector3(1, 1, 1));
		scaleHash.Add ("time", animSpeed);
		scaleHash.Add ("easetype", "spring");
		scaleHash.Add ("oncomplete", "HideThis");
		scaleHash.Add ("oncompletetarget", this.gameObject);
			
		iTween.FadeTo (this.gameObject, 0.3, animSpeed);
		iTween.ScaleTo (this.gameObject, scaleHash);
	}
	
	private function HideThis ()
	{
		this.gameObject.transform.position = CONTROLLER.HIDEPOS;
	}*/
}