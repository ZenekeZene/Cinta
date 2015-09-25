using UnityEngine;
using System.Collections;
using TouchScript.Gestures;
using TouchScript.Behaviors;
using TouchScript;
using System;
using DG.Tweening;

public class controlObjeto : MonoBehaviour {
	public enum Tipo { Normal, Piedra, Flubber };
	public Tipo tipo;
	public bool libre = false;
	public int peso = 0;
	private int pesoMax = 3;
	private Vector3 ultimaPosArrastre;
	private Tween myTween;
	
	private BoxCollider2D collider;
	//private Vector3 s, c;
	public LayerMask mask;
	
	void Start () {
		collider = GetComponent<BoxCollider2D>();
		transform.localRotation = Quaternion.AngleAxis((float)UnityEngine.Random.Range(-10, 10), new Vector3(0, 0, 1));
		if (GetComponent<TapGesture>() != null)
			GetComponent<TapGesture>().Tapped += manejadorToque;
		if (GetComponent<PanGesture>() != null){
			GetComponent<PanGesture>().Panned += manejadorArrastre;
			GetComponent<PanGesture>().PanCompleted += arrastreCompletoManejador;
		}
		transform.DOMoveX(-20, 50, false).SetEase (Ease.Linear).SetLoops(0).OnComplete(completado);
		if (GetComponent<Transformer2D>() != null)
			GetComponent<Transformer2D>().enabled = false;
	}
	
	private void completado(){
		Destroy (gameObject);
	}
	
	private void manejadorToque(object sender, EventArgs e){
		StartCoroutine("desvanecerse");
		DOTween.Kill(transform, false);
	}
	
	private void manejadorArrastre(object sender, EventArgs e){
		if (libre == false) {
			if (peso == 0){
				ultimaPosArrastre = transform.position;
				libre = true;
				DOTween.Pause(transform);
				GetComponent<Transformer2D>().enabled = true;
			}
		}
	}
	
	private void arrastreCompletoManejador(object sender, EventArgs e){
		Transform otro = encontrarNodoMasCercano(transform, "Objeto");
		if (otro != null){
			if (engordar (otro)){
				reanudar (otro);
				Destroy (gameObject);
				return;
			}
		}
		transform.DOMove(ultimaPosArrastre, 0.1f, false).SetEase (Ease.Linear).OnComplete(reanudar);
	}
	
	public bool engordar(Transform otro){
		controlObjeto cntObjeto = otro.GetComponent<controlObjeto>();
		if (cntObjeto.tipo.Equals(Tipo.Flubber)){
			if (cntObjeto.peso < cntObjeto.pesoMax){
				otro.localScale += new Vector3(1,1,1);
				cntObjeto.peso++;
				if (cntObjeto.peso == 1){
					otro.GetComponent<BoxCollider2D>().enabled = false;
					if (otro.GetComponent<TapGesture>() != null)
						otro.GetComponent<TapGesture>().enabled = false;
					if (otro.GetComponent<PanGesture>() != null)
						otro.GetComponent<PanGesture>().enabled = false;
				}
				return true;
			}
			return false;
		}
		return false;
	}
	
	public Transform encontrarNodoMasCercano(Transform quien, string tipoNodo){
		float nearestDistanceSqr = 10f;
		GameObject[] nodosTorre = GameObject.FindGameObjectsWithTag(tipoNodo);
		Transform nodoMasCercano = null;
		
		// loop through each tagged object, remembering nearest one found
		foreach (GameObject nodo in nodosTorre) {
			Vector3 objectPos = nodo.transform.position;
			float distanceSqr = (objectPos - quien.position).sqrMagnitude;
			
			if ((distanceSqr < nearestDistanceSqr) && (!nodo.transform.Equals(transform))) {
				nodoMasCercano = nodo.transform;
				nearestDistanceSqr = distanceSqr;
			}
		}
		return nodoMasCercano;
	}
	
	private void reanudar(){
		DOTween.Play(transform);
		libre = false;
	}
	
	private void reanudar(Transform otro){
		DOTween.Play(otro);
		libre = false;
	}
	
	private void cambiarColor(){
		int cual = UnityEngine.Random.Range(0, 2);
		SpriteRenderer renderer = transform.GetComponent<SpriteRenderer>();
		renderer.color = (cual==0?Color.green:Color.red);
	}
	
	IEnumerator desvanecerse() {
		for (float f = 1f; f >= -1; f -= 0.05f) {
			Color c = renderer.material.color;
			c.a = f;
			renderer.material.color = c;
			yield return null;
		}
		StopCoroutine("desvanecerse");
		Destroy(gameObject);
	}
}
