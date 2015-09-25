using UnityEngine;
using System.Collections.Generic;


public class generarObjetos : MonoBehaviour {

	public List<GameObject> objetos;
	private Vector3 posIni;
	
	// Set up a list to keep track of targets
	public List<GameObject> targets = new List<GameObject>();
	
	void Start () {
		posIni = Camera.main.ViewportToWorldPoint(Vector3.right);
		BoxCollider2D collider = GetComponent<BoxCollider2D>();
		collider.center = new Vector2(posIni.x, 0);
		InvokeRepeating("comprobar", 0.1f, 0.1f);
	}

	private void comprobar(){
		if (targets.Count == 0)
			generar();
	}
	
	private void generar() {
		int indexObjeto = Random.Range(0, objetos.Count);
		GameObject objeto = Instantiate(objetos[indexObjeto], transform.position, Quaternion.identity) as GameObject;
		Vector2 tam = objeto.GetComponent<SpriteRenderer>().sprite.bounds.size;
		objeto.transform.position = new Vector3(posIni.x + (tam.x/2), transform.position.y, 0);;
		objeto.transform.parent = transform;
	}

	private void OnTriggerEnter2D(Collider2D other){
		// If a new object enters the trigger, add it to the list of targets
		if (other.CompareTag("Objeto")) {
			if (other.transform.IsChildOf(transform) == true){
				GameObject go = other.gameObject;
				if (!targets.Contains(go))
					targets.Add(go);
			}
		}
	}
	
	private void OnTriggerExit2D(Collider2D other){
		// When an object exits the trigger, remove it from the list
		if (other.CompareTag("Objeto")) {
			if (other.transform.IsChildOf(transform) == true){
				GameObject go = other.gameObject;
				targets.Remove(go);	
			}
		}
	}
}
