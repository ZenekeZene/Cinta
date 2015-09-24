using UnityEngine;
using System.Collections.Generic;


public class generarObjetos : MonoBehaviour {

	public GameObject prefab_objeto, prefab_objeto_vacio;
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
		int seDibujaObjeto = Random.Range(0, 3);
		GameObject objeto;
		if (seDibujaObjeto < 2)
			objeto = Instantiate(prefab_objeto, transform.position, Quaternion.identity) as GameObject;
		else
			objeto = Instantiate(prefab_objeto_vacio, transform.position, Quaternion.identity) as GameObject;
		Vector2 tam = objeto.GetComponent<SpriteRenderer>().sprite.bounds.size;
		objeto.transform.position = new Vector3(posIni.x + (tam.x/2), transform.position.y, 0);;
		objeto.transform.parent = transform;
	}

	// If a new object enters the trigger, add it to the list of targets
	void OnTriggerEnter2D(Collider2D other){
		if (other.CompareTag("Objeto")) {
			if (other.transform.IsChildOf(transform) == true){
				GameObject go = other.gameObject;
				if (!targets.Contains(go))
					targets.Add(go);
			}
		}
	}
	
	// When an object exits the trigger, remove it from the list
	void OnTriggerExit2D(Collider2D other){
		if (other.CompareTag("Objeto")) {
			if (other.transform.IsChildOf(transform) == true){
				GameObject go = other.gameObject;
				targets.Remove(go);	
			}
		}
	}
}
