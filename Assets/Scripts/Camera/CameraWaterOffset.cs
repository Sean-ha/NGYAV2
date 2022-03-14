using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWaterOffset : MonoBehaviour
{
	public Material waterBG;
	[Tooltip("Larger value means slower speed")]
	public float offsetMagnitude;

	private void LateUpdate()
	{
		Vector2 offset = transform.position;

		Vector4 currOffset = waterBG.GetVector("_SurfaceFoamTilingAndOffset");
		waterBG.SetVector("_SurfaceFoamTilingAndOffset", new Vector4((-offset.x / offsetMagnitude), (-offset.y / offsetMagnitude), currOffset.z, currOffset.w));
	}
}
