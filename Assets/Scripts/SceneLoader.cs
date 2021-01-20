using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void MainScene()
    {
        SceneManager.LoadScene(1);
    }
}
