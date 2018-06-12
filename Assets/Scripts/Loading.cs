using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {

	public GameObject loadingScreen;
	public Slider loadingBar;

	AsyncOperation async;

	public void StartGame() {
		StartCoroutine(StartLoading());
	}

	IEnumerator StartLoading() {
		loadingScreen.SetActive(true);

		async = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
		async.allowSceneActivation = false;

		if (async.isDone == false) {
			loadingBar.value = async.progress;

			if (async.progress <= 0.9f) {
				loadingBar.value = 1;
				async.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
