namespace VRTK.Examples
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class TaskSwitcherReload : VRTK_InteractableObject
    {

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            AsyncOperation loadState = SceneManager.LoadSceneAsync(currentScene);

            while (!loadState.isDone)
            {
                yield return null;
            }
        }
    }

}