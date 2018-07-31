using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform followTarget;

	void LateUpdate()
	{
		if(followTarget != null)
			transform.position = followTarget.position;
	}
}
