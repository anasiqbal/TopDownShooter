
using UnityEngine;

public class LevelManager : MonoBehaviour
{
	#region Singleton
	private static LevelManager instance;
	private LevelManager () { }

	public static LevelManager Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<LevelManager> ();

			return instance;
		}
	}
	#endregion

	public Level CurrentLevel { get; private set; }

	public void Initialize ()
	{
		if (CurrentLevel == null)
			CurrentLevel = FindObjectOfType<Level> ();
	}
}
