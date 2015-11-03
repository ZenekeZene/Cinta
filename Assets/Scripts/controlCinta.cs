using UnityEngine;
using System.Collections;

public class controlCinta : MonoBehaviour {
	
	private float velocidad;
	public GameObject prefab_celda;
	private Vector3 posIni, pos;
	private Vector2 tamObjeto;
	private Transform celdaMasDerecha;
	private float tamZorro;	
	
	void Start () {
		posIni = Camera.main.ViewportToWorldPoint(Vector3.right);
		tamZorro = transform.parent.FindChild("Zorro").GetComponent<BoxCollider2D>().bounds.size.x; 
		pos = dibujarPrimeraCelda();
		dibujarCeldas(pos);
		velocidad = recogerVelocidad();
	}
	
	void LateUpdate() {
		moverCeldas();
	}
	
	private float recogerVelocidad(){
		Transform pasarelaPadre = transform.parent;
		controlPasarela cntPasarela = pasarelaPadre.GetComponent<controlPasarela>();
		return cntPasarela.velocidad;
	}
	
	private Vector3 dibujarPrimeraCelda(){
		Vector3 pos = posIni;
		GameObject celda = Instantiate(prefab_celda, posIni, Quaternion.identity) as GameObject;
		tamObjeto = celda.GetComponent<SpriteRenderer>().sprite.bounds.size;
		celda.transform.position = new Vector3(posIni.x + (tamObjeto.x/2), transform.position.y, 0);
		celda.transform.parent = transform;
		celdaMasDerecha = celda.transform;
		return celda.transform.position;
	}
	
	private bool dibujarCeldas(Vector3 pos) {
		GameObject celda;
		do {
			celda = Instantiate(prefab_celda, pos, Quaternion.identity) as GameObject;
			celda.transform.position = new Vector3(pos.x - (tamObjeto.x), transform.position.y, 0);
			pos = celda.transform.position;
			celda.transform.parent = transform;
		} while(!estaFueraDeCamara(celda.transform));//while((pos.x > Camera.main.ViewportToWorldPoint(Vector3.zero).x));
		return true;
	}
	
	private void moverCeldas() {
		Transform celda;
		for(int i = 0; i<transform.childCount; i++) {
			celda = transform.GetChild(i);
			if (estaFueraDeCamara(celda)){
				actualizarCeldaMasDerecha();
				celda.position = new Vector3(celdaMasDerecha.position.x + (tamObjeto.x), celda.position.y, celda.position.z);
			}
			celda.Translate(Vector3.left * Time.deltaTime * velocidad);
		}
	}
	
	private void actualizarCeldaMasDerecha(){
		Transform celda;
		for(int i = 0; i<transform.childCount; i++) {
			celda = transform.GetChild(i);
			if (celda.position.x > celdaMasDerecha.position.x)
				celdaMasDerecha = celda;
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
