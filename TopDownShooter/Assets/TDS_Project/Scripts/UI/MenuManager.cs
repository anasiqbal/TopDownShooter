using UnityEngine;

public class MenuManager : MonoBehaviour
{
	[SerializeField] GameObject mainMenu;
	[SerializeField] GameObject hudMenu;
	[SerializeField] GameObject gameOverMenu;
	[SerializeField] GameObject pauseMenu;

	void HideAllMenus()
	{
		mainMenu.SetActive (false);
		hudMenu.SetActive (false);
		gameOverMenu.SetActive (false);
		pauseMenu.SetActive (false);
	}

	public void DisplayMainMenu()
	{
		HideAllMenus ();
		mainMenu.SetActive (true);
	}

	public void DisplayHUDMenu()
	{
		HideAllMenus ();
		hudMenu.SetActive (true);
	}

	public void DisplayGameOverMenu()
	{
		HideAllMenus ();
		gameOverMenu.SetActive (true);
	}

	public void DisplayPauseMenu()
	{
		HideAllMenus ();
		pauseMenu.SetActive (true);
	}
}
