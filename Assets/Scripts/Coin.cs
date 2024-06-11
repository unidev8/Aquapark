using PathCreation.Examples;
using System;
using System.Net.Sockets;
using UnityEngine;
using static ToonyColorsPro.ShaderGenerator.Enums;

public class Coin : MonoBehaviour
{
    //public event Action<int> OnScoreUp;
    //public static Coin instance;
    private bool isHited = false;
    private float flySpeed = 100f;

    private void Awake()
    {
        //Player.Instance.OnScoreUp += OnScoreUp;
        //if (instance == null)
            //instance = this;
    }

    private void Update()
    {
        if (isHited)
        {
            transform.Translate(new Vector3(flySpeed * Time.deltaTime, flySpeed * Time.deltaTime, flySpeed * Time.deltaTime * 0.2f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Ai")
        {
            //Player.Instance.OnScoreUp?.Invoke();
            if (other.gameObject.tag == "Player")
                other.gameObject.transform.parent.GetComponent<Player>().score += ConstVars.scoreinc;
            else
                other.gameObject.transform.parent.GetComponent<AI>().score += ConstVars.scoreinc;
            isHited = true;
            //Debug.Log("Coin Trigger Score = " + Player.Instance.score);
            GameObject.Destroy(this.gameObject, 1f);
        }
    }

}
