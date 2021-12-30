using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AgentVision))]
public class AIVisionEditor : Editor
{
    private void OnSceneGUI()
    {
        AgentVision fov = (AgentVision)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        Handles.color = Color.red;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.nearRadius);

        //сделать цикл и перебрать каждую булеву переменную
        for (int i = 0; i < fov.isSeeSmth.Length; i++)
        {
            if (fov.isSeeSmth[i])
            {
                Handles.color = Color.green;
                //Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
                Handles.DrawLine(fov.transform.position, fov.objectsInRangeChecks[i].transform.position);

            }
        }
            
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
