using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[SerializeField] private Transform toFollow;

	private void Update()
	{
		Vector3 newPos = toFollow.position;
		newPos.z = -10f;
		transform.position = newPos;
	}
}
