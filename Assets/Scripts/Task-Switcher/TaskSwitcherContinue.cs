namespace VRTK.Examples
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class TaskSwitcherContinue : VRTK_InteractableObject
    {

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            AsyncOperation loadState = SceneManager.LoadSceneAsync(currentScene + 1);

            while (!loadState.isDone)
            {
                yield return null;
            }
        }
    }

}