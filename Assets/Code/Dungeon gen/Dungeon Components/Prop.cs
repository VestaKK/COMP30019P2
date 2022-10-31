using UnityEngine;

public class Prop
{
	public GameObject propObject { get; set; }
	public Vector3 coordinates { get; set; }
	public Quaternion rotation { get; set; }
	public Vector3 bounds;

	public Prop(GameObject propObject, Vector3 coordinates)
	{
		this.propObject = propObject;
		this.coordinates = coordinates;
		this.rotation = propObject.transform.rotation;
		this.bounds = propObject.GetComponent<Renderer>().bounds.size;
	}

	public Prop(GameObject propObject, Vector3 coordinates, Quaternion rotation)
	{
		this.propObject = propObject;
		this.coordinates = coordinates;
		this.rotation = propObject.transform.rotation * rotation;
		this.bounds = propObject.GetComponent<Renderer>().bounds.size;
	}

}