using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

    public float upForce;
    public Rigidbody2D MyBody { get; private set; }

    private Text mainText;

    void Awake () {
        MyBody = GetComponent<Rigidbody2D>();
        mainText = GetComponentInChildren<Text>();
	}



    public void Initialize(string text, Vector2 startPosition) {
        SetText(text);
        transform.localPosition = startPosition;
        Bounce();

        Destroy(gameObject, 2f);
    }
	
    public void SetText(string value) {
        mainText.text = value;
    }

    public void Bounce() {

        float errorX = Random.Range(-0.2f, 0.2f);
        float errorY = Random.Range(-0.1f, 0.3f);

        Vector2 errorV = new Vector2(errorX, errorY);

        MyBody.AddForce((Vector2.up + errorV) * upForce);

    }


}
