using UnityEngine;
using System.Collections;

public class controlObjetos : MonoBehaviour {
	
	private float velocidad;
	private Vector3 posIni, pos;
	private Vector2 tamObjeto;
	public GameObject prefab_objeto, prefab_objeto_vacio;
	private generarObjetos gnrObjetos;
	
	void Start () {
		posIni = Camera.main.ViewportToWorldPoint(Vector3.right);
		gnrObjetos = GetComponent<generarObjetos>();
		pos = dibujarPrimerObjeto();
		dibujarObjetos(pos);
		velocidad = recogerVelocidad();
		//InvokeRepeating("moverObjetos", 1f, 1f);
	}	
	
	void Update () { 
		moverObjetos();
	}
	
	private float recogerVelocidad(){
		Transform pasarelaPadre = transform.parent;
		controlPasarela cntPasarela = pasarelaPadre.GetComponent<controlPasarela>();
		return cntPasarela.velocidad;
	}
	
	private void moverObjetos() {
		Transform objeto;
		for(int i = 0; i<transform.childCount; i++) {
			objeto = transform.GetChild(i);
			//if (objeto.gameObject.GetComponent<controlObjeto>() != null){
				//if (objeto.gameObject.GetComponent<controlObjeto>().libre == false)
					//objeto.position = new Vector3(objeto.position.x + velocidad, objeto.position.y, objeto.position.z);
			//} else {
			if (objeto.gameObject.GetComponent<controlObjeto>() == null)
				objeto.position = new Vector3(objeto.position.x + velocidad, objeto.position.y, objeto.position.z);
			//}
			if (estaFueraDeCamara(objeto)){
				//gnrObjetos.targets.Remove(objeto.gameObject);
				Destroy (objeto.gameObject);
			}
				
				//objeto.position = new Vector3(posIni.x + (tamObjeto.x/2), posIni.y + (tamObjeto.y/2) + 1, 0);;
		}
	}
	
	private Vector3 dibujarPrimerObjeto(){
		Vector3 pos = posIni;
		GameObject celda = Instantiate(prefab_objeto, posIni, Quaternion.identity) as GameObject;
		tamObjeto = celda.GetComponent<SpriteRenderer>().sprite.bounds.size;
		celda.transform.position = new Vector3(posIni.x + (tamObjeto.x/2), transform.position.y, 0);
		pos = celda.transform.position;
		celda.transform.parent = transform;
		return pos;
	}
	
	private void dibujarObjetos(Vector3 pos) {
		GameObject objeto;
		for(int i = 0; i<5; i++){
			objeto = dibujarObjeto();
		}
		//} while((objeto!=null) && (pos.x > Camera.main.ViewportToWorldPoint(Vector3.zero).x));
	}
	
	private GameObject dibujarObjeto(){
		int seDibujaObjeto = Random.Range(0, 2);
		GameObject objeto;
		if (seDibujaObjeto == 0)
			objeto = Instantiate(prefab_objeto, pos, Quaternion.identity) as GameObject;
		 else 
			objeto = Instantiate(prefab_objeto_vacio, pos, Quaternion.identity) as GameObject;
		objeto.transform.position = new Vector3(pos.x - (tamObjeto.x), transform.position.y, 0);
		pos = objeto.transform.position;
		objeto.transform.parent = transform;
		return objeto;
		
	}
	
	private bool estaFueraDeCamara(Transform elemento){
		float desfX = elemento.renderer.bounds.size.x/2;
		Vector3 pos = Camera.main.WorldToViewportPoint(new Vector3(elemento.position.x + desfX, elemento.position.y, 0));
		return (pos.x <= 0);
	}
}
