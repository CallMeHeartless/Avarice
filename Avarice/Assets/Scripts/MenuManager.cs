using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	public void StartGame ()
	{
    	SceneManager.LoadScene(1);
	}

    public void Controls()
    {
        SceneManager.LoadScene(2);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
	{
        Debug.Log("WE QUIT THE GAME!");
		Application.Quit();
	}

}
