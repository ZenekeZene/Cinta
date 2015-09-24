using UnityEngine;
using System.Collections;

public class controlArrastre : MonoBehaviour {
	
	public SpringJoint2D spring;
	private Vector3 screenPoint;
	private bool lanzado = false;
	
	void Awake()
	{
		
		//spring = this.gameObject.GetComponent<SpringJoint2D>(); //"spring" is the SpringJoint2D component that I added to my object
		
		//spring.connectedAnchor = gameObject.transform.position;//i want the anchor position to start at the object's position
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		
	}
	
	
	void OnMouseDown()
	{
		//GetComponent<Rigidbody2D>().isKinematic = false;
		//spring.enabled = true;//I'm reactivating the SpringJoint2D component each time I'm clicking on my object becouse I'm disabling it after I'm releasing the mouse click so it will fly in the direction i was moving my mouse
		
	}
	
	
	void OnMouseDrag()        
	{
		Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 currentPos = Camera.main.ScreenToWorldPoint(currentScreenPoint);
		transform.position = new Vector3(transform.position.x, currentPos.y, transform.position.z);
		/*if (spring.enabled = true) 
		{
			
			Vector2 cursorPosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);//getting cursor position
			
			spring.connectedAnchor = cursorPosition;//the anchor get's cursor's position
			
			
		}*/
	}
	
	
	void OnMouseUp()        
	{
		if (lanzado == false){
			Rigidbody2D rigido = GetComponent<Rigidbody2D>();
			rigido.isKinematic = false;
			rigido.AddForce(new Vector2(0, 200));
			lanzado = true;
		}
		//spring.enabled = false;//disabling the spring component
		
	}
	
}