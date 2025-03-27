using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
   public void GoToLoadoutScene()
    {
        SceneManager.LoadScene(1);
    }
}
