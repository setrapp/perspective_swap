using UnityEngine;
using System.Collections;

public class CameraFollowCharacter : MonoBehaviour
{
	public CharacterInfo targetCharacter = null;

	void Update() {
		transform.position = new Vector3(targetCharacter.transform.position.x, targetCharacter.transform.position.y, -100);
	}
}
