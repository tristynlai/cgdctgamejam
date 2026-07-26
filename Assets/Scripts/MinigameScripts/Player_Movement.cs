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
    public AudioSource BikeSkid;

    private Vector3 StartPosition;
    private Quaternion StartRotation;

    public float PitchSmoothTime = 0.15f;
    private float TargetPitch = 1f;
    private float PitchVelocity;
    private bool WasTurning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        StartPosition = transform.position;
        StartRotation = transform.rotation;
        //GameOverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update() {


        if (Time.timeScale == 0f) {
            AudioSource.Pause();
            return;
        } else {
            if (AudioSource.clip != Medium) {
                AudioSource.clip = Medium;
            }
            if (!AudioSource.isPlaying) {
                AudioSource.Play();
            }
        }

        //player movement
        if(Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) {
            transform.position += Vector3.right * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,-47), RotationSpeed * Time.deltaTime);
        }

        if(Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed) {
            transform.position += Vector3.left * Speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,47), RotationSpeed * Time.deltaTime);
        }

        bool IsTurning = (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed) || (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed);

        if (IsTurning && !WasTurning) {
            BikeSkid.Play();
        }
        WasTurning = IsTurning;

        if(Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed) {
            transform.position += Vector3.up * Speed * Time.deltaTime;
            TargetPitch = 1.05f;
        }

        if(Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) {
            transform.position += Vector3.down * Speed * Time.deltaTime;
            TargetPitch = .95f;
        }

        if(transform.rotation.z != 90) {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), 10f * Time.deltaTime);
        }

        
        if (!(Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed) && !(Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)) {
            TargetPitch = 1f;
        }
        AudioSource.pitch = Mathf.SmoothDamp(AudioSource.pitch, TargetPitch, ref PitchVelocity, PitchSmoothTime);

        //screen bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2.75f, 2.75f);
        pos.y = Mathf.Clamp(pos.y, -3f, 3.2f);
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Cars") {
            Debug.Log("You hit the car smh");
            AudioSource.Stop();
            Game_Controller.Instance.GameOver();
        }
    }

    public void ResetPlayer() {
        transform.position = StartPosition;
        transform.rotation = StartRotation;
        TargetPitch = 1f;
        PitchVelocity = 0f;
        AudioSource.pitch = 1f;
        WasTurning = false;

        if (AudioSource != null && !AudioSource.isPlaying){
            AudioSource.clip = Medium;
            AudioSource.Play();
        }
    }
}
