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

		yield return new WaitForSeconds(1.2f);

		while (true) {

			laserSound.Play();

			yield return new WaitForSeconds(7.4f);

			laserSoundReverse.Play();

			yield return new WaitForSeconds(3.8f);
		}
	}
}
