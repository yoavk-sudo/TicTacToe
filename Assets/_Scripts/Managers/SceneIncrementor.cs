using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneIncrementor : MonoBehaviour
{
    private int _numberOfScenes = 0;
    private int _indexOfSceneToLoad = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void TransferToNextScene()
    {
        SetIndexToNextScene();
        SceneManager.LoadScene(_indexOfSceneToLoad);
    }

    private void SetIndexToNextScene()
    {
        _numberOfScenes = SceneManager.sceneCountInBuildSettings;
        _indexOfSceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (_indexOfSceneToLoad > _numberOfScenes - 1)
        {
            _indexOfSceneToLoad = 0;
        }
    }
}
