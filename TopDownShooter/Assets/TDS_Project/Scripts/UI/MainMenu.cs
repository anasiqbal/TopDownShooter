using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public void OnClick_Start()
	{
		GameManager.Instance.StartGame ();
	}
}
