using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

	public AudioSource laserSound;
	public AudioSource laserSoundReverse;
	
	void Start () {
		StartCoroutine(LaserSound());
	}

	IEnumerator LaserSound() {

		yield return new WaitForSeconds(1.8f);

		while (true) {

			laserSound.Play();

			yield return new WaitForSeconds(6.4f);

			laserSoundReverse.Play();

			yield return new WaitForSeconds(3.6f);
		}
	}
}
