namespace VRTK.Examples
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class LevelSwitcherButton3 : VRTK_InteractableObject
    {

        public override void StartUsing(VRTK_InteractUse usingObject)
        {
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            AsyncOperation loadState = SceneManager.LoadSceneAsync("Binary-Grip_Pretest_3_Grip-Anchor");

            while (!loadState.isDone)
            {
                yield return null;
            }
        }
    }

}