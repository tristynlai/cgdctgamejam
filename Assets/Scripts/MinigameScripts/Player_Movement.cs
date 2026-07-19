using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    public Transform transform;
    public float Speed = 5f;
    public float RotationSpeed = 3f;

    public AudioClip Low, Medium, High;
    public AudioSource AudioSource;

    public GameObject GameOverScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        GameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        //player movement
        if(Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) {
            transform.position += Vector3.right * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,-47), RotationSpeed * Time.deltaTime);
        }

        if(Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) {
            transform.position += Vector3.left * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,47), RotationSpeed * Time.deltaTime);
        }

        if(Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) {
            transform.position += Vector3.up * Speed * Time.deltaTime;
            //AudioSource.pitch = 1.05f;
        }

        if(Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) {
            transform.position += Vector3.down * Speed * Time.deltaTime;
            //AudioSource.pitch = .95f;
        }

        if(transform.rotation.z != 90) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), 10f * Time.deltaTime);
        }

        /*
        if (!(Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) && !(Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed))
        {
            AudioSource.pitch = 1f;
        }*/

        if (AudioSource.clip != Medium){
            AudioSource.clip = Medium;
            //AudioSource.Play();
        }

        //screen bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.75f, 2.75f);
        pos.y = Mathf.Clamp(pos.y, -3f, 3.2f);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Cars")
        {
            Debug.Log("You hit the car smh");
            Time.timeScale = 0f;
            GameOverScreen.SetActive(true);
        }
    }
}
