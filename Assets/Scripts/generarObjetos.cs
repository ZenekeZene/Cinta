using UnityEngine;
using System.Collections.Generic;


public class generarObjetos : MonoBehaviour {

	public List<GameObject> objetos;
	private Vector3 posIni;
	private float velocidad, distancia;
	private BoxCollider2D collider;
	// Set up a list to keep track of targets
	public List<GameObject> targets = new List<GameObject>();
	private float tamZorro, posZorro;
	
	void Start () {
		posIni = Camera.main.ViewportToWorldPoint(Vector3.right);
		collider = GetComponent<BoxCollider2D>();
		collider.center = new Vector2(posIni.x, 0);
		Transform zorro = transform.parent.FindChild("Zorro");
		tamZorro = zorro.GetComponent<BoxCollider2D>().bounds.size.x; 
		posZorro = zorro.position.x;
		velocidad = recogerVelocidad();
		distancia = recogerDistancia();
		InvokeRepeating("comprobar", 0f, 2f);
	}

	private void comprobar(){
		//if (targets.Count == 0)
			generar();
	}
	
	private void LateUpdate(){
		moverObjetos();
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
	
	private float recogerVelocidad(){
		controlPasarela cntPasarela = transform.parent.GetComponent<controlPasarela>();
		if (cntPasarela != null){
			return cntPasarela.velocidad;
		}
		return 0f;
	}
	
	public float getVelocidad(){
		return velocidad;
	}
	
	private float recogerDistancia(){
		//Transform nodoFinalCinta = transform.parent.FindChild("NodoFinal");
		//Debug.Log ("nodo position: " + nodoFinalCinta.localPosition.x + ", posIni.x: " + posIni.x);
		float height = 2*Camera.main.orthographicSize;
		float width = height*Camera.main.aspect;
		float desfZorro = (tamZorro)/width;
		//return (width - tamZorro) + 130;
		float dist = collider.center.x + (collider.size.x/2) - posZorro - (tamZorro/2);
		Debug.Log ("Dist: " + dist);
		return dist;
		//return 12;
	}
	
	public float getDistancia(){
		return Mathf.Abs(distancia);
	}
	
	private void moverObjetos() {
		Transform objeto;
		for(int i = 0; i<transform.childCount; i++) {
			objeto = transform.GetChild(i);
			if (estaFueraDeCamara(objeto))
				Destroy (objeto.gameObject);
			objeto.Translate(Vector3.left * Time.deltaTime * velocidad, Space.World);
		}
	}
	
	private bool estaFueraDeCamara(Transform elemento){
		float desfX = elemento.renderer.bounds.size.x/2;
		Vector3 pos = Camera.main.WorldToViewportPoint(new Vector3(elemento.position.x + desfX, elemento.position.y, 0));
		float height = 2*Camera.main.orthographicSize;
		float width = height*Camera.main.aspect;
		float desfZorro = (tamZorro)/width;
		return (pos.x <= (0f + (desfZorro)));
	}
	
}
