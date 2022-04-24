using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTween : MonoBehaviour
{
    public CinemachineFreeLook camera;

    public float targetTopHeight = 4;
    public float targetMiddleHeight = 2.5f;
    public float targetBottomHeight = 0.4f;

    public float targetTopRadius = 3;
    public float targetMiddleRadius = 3;
    public float targetBottomRadius = 3;

    public float targetFov = 40;

    public float speed = 4f;

    private void Update()
    {
        // Update top orbit rig.
        camera.m_Orbits[0].m_Height =
            Mathf.Lerp(camera.m_Orbits[0].m_Height, targetTopHeight, speed * Time.deltaTime);
        camera.m_Orbits[0].m_Radius =
            Mathf.Lerp(camera.m_Orbits[0].m_Radius, targetTopRadius, speed * Time.deltaTime);

        // Update middle orbit rig.
        camera.m_Orbits[1].m_Height =
            Mathf.Lerp(camera.m_Orbits[1].m_Height, targetMiddleHeight, speed * Time.deltaTime);
        camera.m_Orbits[1].m_Radius =
            Mathf.Lerp(camera.m_Orbits[1].m_Radius, targetMiddleRadius, speed * Time.deltaTime);

        // Update bottom orbit rig.
        camera.m_Orbits[2].m_Height =
            Mathf.Lerp(camera.m_Orbits[2].m_Height, targetBottomHeight, speed * Time.deltaTime);
        camera.m_Orbits[2].m_Radius =
            Mathf.Lerp(camera.m_Orbits[2].m_Radius, targetBottomRadius, speed * Time.deltaTime);

        // Update FOV.
        camera.m_Lens.FieldOfView =
            Mathf.Lerp(camera.m_Lens.FieldOfView, targetFov, speed * Time.deltaTime);
    }

    public void SetRigs(float topHeight = -1, float topRadius = -1, float middleHeight = -1,
        float middleRadius = -1, float bottomHeight = -1, float bottomRadius = -1)
    {
        targetTopHeight = topHeight >= 0 ? topHeight : targetTopHeight;
        targetTopRadius = topRadius >= 0 ? topRadius : targetTopRadius;
        targetMiddleHeight = middleHeight >= 0 ? middleHeight : targetMiddleHeight;
        targetMiddleRadius = middleRadius >= 0 ? middleRadius : targetMiddleRadius;
        targetBottomHeight = bottomHeight >= 0 ? bottomHeight : targetBottomHeight;
        targetBottomRadius = bottomRadius >= 0 ? bottomRadius : targetBottomRadius;
    }
}