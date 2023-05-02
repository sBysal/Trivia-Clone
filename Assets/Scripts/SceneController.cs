using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour    //Controls scene loading jobs.
{
    public void LoadMainScene()     //Loads main scene.
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGameplayScene()  //Loads gameplay scene.
    {
        SceneManager.LoadScene(1);
    }
}
