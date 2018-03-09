namespace VRTK.Examples
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelSwitcherButton1 : VRTK_InteractableObject
    {

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            AsyncOperation loadState = SceneManager.LoadSceneAsync("Binary-Grip_Pretest_1_Baseline");

            while (!loadState.isDone)
            {
                yield return null;
            }
        }
    }

}