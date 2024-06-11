using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<GameObject> Paths;
    [HideInInspector]
    public List<Vector3> mainControlPoints;
    public Finish finish;
    public PathCreator path;
    public CameraManagement cameraManagement;
    public AmplifyMotionEffect amplifyMotion;
    //public CinemachineSmoothPath cinemachineSmooth;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        ConstructLine();
    }

    private void OnEnable()
    {
        //RenderSettings.fog = true;
    }

    public void ConstructLine()
    {
        int j = 0;
        for (int pathIndex = 0; pathIndex < Paths.Count; pathIndex++)
        {
            if (Paths[pathIndex].GetComponent<WayPoints>())
            {
                WayPoints wayPoints = Paths[pathIndex].GetComponent<WayPoints>();
                for (int i = 0; i < wayPoints.waypoints.Count; i++)
                {
                    mainControlPoints.Add(wayPoints.waypoints[i].position);
                    //cinemachineSmooth.m_Waypoints.SetValue(wayPoints.waypoints[i].position, j);
                        j++;
                }
            }
        }

        for (int i = 0; i < finish.finishLineWayPoints.Length; i++)
        {
            mainControlPoints.Add(finish.finishLineWayPoints[i].position);
        }

        if (mainControlPoints.Count > 0)
        {
            BezierPath bezierPath = new PathCreation.BezierPath(mainControlPoints.ToArray(), false, PathSpace.xyz);
            path.bezierPath = bezierPath;
        }
    }
}

public static class ConstVars
{
   public static int scoreinc = 10;
}