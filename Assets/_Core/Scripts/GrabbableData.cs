using UnityEngine;

public class GrabbableData
{
	public int InstanceID => transform.GetInstanceID();
	public Vector3 Position => transform.position;
	public bool IsActive => transform.gameObject.activeSelf;

	public IGrabbable grabbable;
	public ICombinable combinable = null;
	public IShakable shakable = null;
	public Transform transform;
	public bool canBeCombined = false;

	public GrabbableData (IGrabbable grabbable, Transform transform)
	{
		this.grabbable = grabbable;
		this.transform = transform;

		combinable = transform.GetComponent<ICombinable>();
		canBeCombined = combinable == null ? false : true;
		shakable = transform.GetComponent<IShakable>();
	}
}
