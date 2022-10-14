using UnityEngine;

public class Prop
{
	public GameObject propObject { get; set; }
	public Vector3 coordinates { get; set; }
	public Vector3 bounds;

	public Prop(GameObject propObject, Vector3 coordinates)
	{
		this.propObject = propObject;
		this.coordinates = coordinates;
		this.bounds = propObject.GetComponent<Renderer>().bounds.size;
	}

}