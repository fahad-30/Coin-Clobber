using UnityEngine;
using UnityEngine.SceneManagement;
public class MainUI : MonoBehaviour
{
  
    public void RefreshScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
