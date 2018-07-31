
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public float flashWarningDuration = 1.5f;
	public int activeRadius = 5;

	public Vector3 Position { get; private set; }
	public Vector3 Direction { get; private set; }
	bool flashing = false;

	void Awake()
	{
		Position = transform.position;
		Direction = transform.forward;
	}

	public void FlashWarning()
	{
		if (flashing)
			return;

		flashing = true;
		Invoke ("StopFlashingWarning", flashWarningDuration);
	}

	void StopFlashingWarning()
	{
		flashing = false;
	}

#if UNITY_EDITOR

	private void OnDrawGizmosSelected()
	{
		UnityEditor.Handles.color = Color.cyan;
		UnityEditor.Handles.DrawWireDisc (transform.localPosition, transform.up.normalized, activeRadius);
		UnityEditor.Handles.color = Color.magenta;
		UnityEditor.Handles.DrawLine (transform.localPosition, transform.localPosition + transform.forward * activeRadius);
		UnityEditor.Handles.ArrowHandleCap (0, transform.localPosition, transform.rotation, 2, EventType.Repaint);
	}

#endif
}
