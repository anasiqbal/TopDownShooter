using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	#region Singleton
	private static GameManager instance;
	private GameManager() { }

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<GameManager> ();

			return instance;
		}
	}
	#endregion

	public enum GameState
	{
		IDLE,
		PLAYING,
		PAUSED,
		GAMEOVER
	}

	public Texture2D cursor;

	public MenuManager ref_MenuManager;

	public Player ref_Player;
	public EnemySpawner ref_EnemySpawner;

	public GameState CurrentGameState { get; private set; }

	#region Unity Methods

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);

		SetupCursor ();
	}

	void OnEnable()
	{
		ref_Player.OnDeath += Player_OnDeath;
	}

	void Start()
	{
		CurrentGameState = GameState.IDLE;
		ref_MenuManager.DisplayMainMenu ();
	}

	void OnDisable()
	{
		ref_Player.OnDeath -= Player_OnDeath;
	}

	#endregion

	#region Helper Methods

	void SetupCursor()
	{
		Cursor.SetCursor (cursor, new Vector2(cursor.width * 0.1f, cursor.height * 0.1f), CursorMode.Auto);
	}

	public void StartGame()
	{
		CurrentGameState = GameState.PLAYING;
		ref_MenuManager.DisplayHUDMenu ();

		LevelManager.Instance.Initialize ();
		ref_Player.Initialize ();

		ref_EnemySpawner.Initialize ();
	}

	public void PauseGame()
	{
		CurrentGameState = GameState.PAUSED;
		ref_MenuManager.DisplayPauseMenu ();
	}

	public void ResumeGame()
	{
		CurrentGameState = GameState.PLAYING;
		ref_MenuManager.DisplayHUDMenu ();
	}

	public void GameOver()
	{
		CurrentGameState = GameState.GAMEOVER;
		ref_MenuManager.DisplayGameOverMenu ();
		ref_EnemySpawner.StopSpawner ();
	}

	public void RestartGame()
	{
		ref_Player.Initialize ();
		CurrentGameState = GameState.PLAYING;
		ref_MenuManager.DisplayHUDMenu ();

		ref_EnemySpawner.Initialize ();
	}

	public void EndGame()
	{
		CurrentGameState = GameState.IDLE;
		ref_MenuManager.DisplayMainMenu ();
		ref_EnemySpawner.StopSpawner ();
	}

	#endregion

	#region Events / Delegates
	void Player_OnDeath()
	{
		GameOver ();
	}

	#endregion
}
