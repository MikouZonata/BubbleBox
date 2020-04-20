using UnityEngine;

public interface IGrabbable
{
	bool RequestPull (Transform handTransform);
	void Release ();
	void Attach (Transform handTransform);
	void Throw (Vector3 direction, float speed);
}
