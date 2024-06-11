using UnityEngine;
using DG.Tweening;
using System;

namespace PathCreation.Examples
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private float releaseHardness = 1;
        [SerializeField] private float dragControl = 1;
        [SerializeField] private Transform cameraPlaceHolder;
        [SerializeField] private float leftRightRotation = 9;

        public PathFollower _movements;

        //public Transform WheelFrontLeft, WheelFrontRight, WheelBackLeft, WheelBackRight;
        public GameObject[] characters;
        public Transform PlayerBody;

        public bool permitControl = true;
        public float finishDeviation = 0f;


        float currentMouseX, LastMouseX, difference;
        [HideInInspector]
        public float targetDeviation = 0;
        public bool touched = false;
        float catchUpSpeed = 0f;
        int characterIdx = 0;

        public static Player Instance;
        public event Action OnScoreUp;
        public int score = 0;

        private void Awake()
        {
            Instance = this;
        }


        public void DisallowControl()
        {
            permitControl = false;
            DOVirtual.DelayedCall(.35f, () => permitControl = true);
        }

        public void Start()
        {
            GameCanvasControl.Instance.OnPrevCharacter += OnPrevCharacter;
            GameCanvasControl.Instance.OnNextCharacter += OnNextCharacter;

        }

        void OnPrevCharacter()
        {            
            if (characterIdx > 0 ) characterIdx--;

            Debug.Log("Player:OnPrevCharacter");
            GameObject curCharacter = PlayerBody.GetChild(0).gameObject;
            if (curCharacter != null) GameObject.Destroy(curCharacter);
            GameObject newCharacter = GameObject.Instantiate(characters[characterIdx], transform.position, transform.rotation);
            newCharacter.transform.parent = PlayerBody.transform;
        }

        void OnNextCharacter()
        {
            if (characterIdx < characters.Length - 1 ) characterIdx++;

            Debug.Log("Player:OnPrevCharacter");
            GameObject curCharacter = PlayerBody.GetChild(0).gameObject;
            if (curCharacter != null) GameObject.Destroy(curCharacter);
            GameObject newCharacter = GameObject.Instantiate(characters[characterIdx], transform.position, transform.rotation);
            newCharacter.transform.parent = PlayerBody.transform;
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Player Trigger gameObject = " + other.gameObject.name);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Debug.Log("Player Collision gameObjet = " + collision.gameObject.name);
        }

        void Update()
        {
            if (GameCanvasControl.Instance.gameState == GameState.CharaterSelection) return;

            catchUpSpeed = Mathf.Clamp(_movements.speed/3f, 10f, 15f);

            if (Input.GetMouseButtonDown(0) )
            {
                _movements.isStart = true;
                LastMouseX = Input.mousePosition.x;
                touched = true;

                //Debug.Log("Player: Update: GetMouseButtonDown");

            }
            if (Input.GetMouseButtonUp(0) )
            {
                //Debug.Log("Player: Update: GetMouseButtonDown");
                touched = false;
            }

            if (_movements.isStart && !_movements.isfinish)
            {
                if (touched)
                {
                    currentMouseX = Input.mousePosition.x;
                    difference = currentMouseX - LastMouseX;
                    LastMouseX = currentMouseX;
                }
                else
                {
                    difference = 0f;
                }

                if (permitControl)
                {
                    if (_movements.onPath && touched)
                    {
                        targetRotation = 0f;
                        targetDeviation += difference * dragControl * Time.deltaTime;
                        targetDeviation = Mathf.Clamp(targetDeviation, -.98f, .98f);
                        _movements.deviation = Mathf.Lerp(_movements.deviation, targetDeviation, catchUpSpeed * Time.deltaTime);
                    }
                    else
                    {
                        targetDeviation = Mathf.Lerp(targetDeviation, 0f, Time.deltaTime * releaseHardness);
                        _movements.deviation = targetDeviation;
                    }

                    if (!_movements.onPath)
                    {
                        FlyRotation(difference);
                    }
                }

            }

            if(_movements.isfinish)
            {
                targetDeviation = Mathf.Lerp(targetDeviation, finishDeviation, Time.deltaTime * releaseHardness / 2f);
                _movements.deviation = targetDeviation;
            }
        }

        float targetRotation = 0f;

        void FlyRotation(float _mouseX)
        {
            transform.Rotate(Vector3.up, _mouseX * leftRightRotation * Time.deltaTime, Space.Self);

            if (!_movements.istransation)
            {
                if (_mouseX != 0)
                {
                    targetRotation += _mouseX * releaseHardness * Time.deltaTime;
                }
                else
                {
                    targetRotation = Mathf.Lerp(targetRotation, 0f, releaseHardness * Time.deltaTime);
                }

                targetRotation = Mathf.Clamp(targetRotation, -30f, 30f);
                _movements.PlayerBody.localEulerAngles = new Vector3(_movements.PlayerBody.localEulerAngles.x, _movements.PlayerBody.localEulerAngles.y, -targetRotation);
            }

        }

    }
}