using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;
using PathCreation;
using DG.Tweening;


public class Finish : MonoBehaviour
{

    public Transform[] finishLineWayPoints;
    public Transform[] finishLineFinishPositions;

    public ParticleSystem finishParticle;
    public int playerCount=0;

    public RaceManager raceManager;
    
    private float finishStartSpeed = 64f;
    private float eachCharacterDiffRatio = 5f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            playerCount++;
            raceManager.canSort = false;
            GameManager.Instance.cameraManagement.RaceFinish(new Vector3(4f, 16f, -20f), 0f);


            PathFollower movement = other.GetComponentInParent<PathFollower>();
            movement.playerControl.finishDeviation = playerCount % 2 == 0 ? -.3f : .3f;

            movement.warpEffect.gameObject.SetActive(false);

            //int rank = other.gameObject.transform.parent.GetComponent<DistanceCounter>().rank - 1;
            Vector3 playerFinishPoint = finishLineFinishPositions[playerCount - 1].position;
            float finishDistanceTravelled = movement.pathCreator.path.GetClosestDistanceAlongPath(playerFinishPoint);
            movement.finishDistanceTravelled = finishDistanceTravelled;            
            movement.isfinish = true;

            if(playerCount <= 1) finishParticle.Play();

            movement.MaxSpeed = finishStartSpeed - eachCharacterDiffRatio * playerCount;
            movement.speed = movement.MaxSpeed;

            //Debug.Log("player name = " + other.gameObject.transform.parent.name + " || Speed = " + movement.speed + " || finishPoint = " + playerFinishPoint);

            GameCanvasControl.Instance.gameOverPanelControl.EnablePanel(GameState.LevelCompleted, 3f);
            HapticManager.Instance.PlayHaptic(HapticType.Success);

        }

        if (other.transform.CompareTag("Ai"))
        {
            other.GetComponent<BoxCollider>().enabled = false;
            playerCount++;
            if (playerCount <= 1) finishParticle.Play();

            AIPathFollower movement = other.GetComponentInParent<AIPathFollower>();
            AI ai = movement.GetComponent<AI>();
            ai.finishDeviation = playerCount % 2 == 0 ? -.3f : .3f;

            //int rank = other.gameObject.transform.parent.GetComponent<DistanceCounter>().rank - 1;

            Vector3 playerFinishPoint = finishLineFinishPositions[playerCount - 1].position;
            float finishDistanceTravelled = movement.pathCreator.path.GetClosestDistanceAlongPath(playerFinishPoint);
            movement.finishDistanceTravelled = finishDistanceTravelled;            

            movement.MaxSpeed = finishStartSpeed - eachCharacterDiffRatio * playerCount;
            movement.speed = movement.MaxSpeed;

            //Debug.Log("player name = " + other.gameObject.transform.parent.name + " || Speed = " + movement.speed + " || finishPoint = " + playerFinishPoint);

            movement.isfinish = true;
        }

    }
}