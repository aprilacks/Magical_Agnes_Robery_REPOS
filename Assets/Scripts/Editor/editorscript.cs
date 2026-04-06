using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;


/* * HOW TO USE:
 * 1. Place this script inside a folder named 'Editor' in your Project window.
 * 2. This script automatically finds 'EnemyScript' components in the Scene.
 * 3. It draws a white arc representing the FOV radius and angle, and red lines 
 * connecting the enemy to any currently detected targets.
 * 4. Ensure 'EnemyScript' is in the same project for this to compile.
 */

[CustomEditor(typeof(EnemyScript))] 
public class editorscript : Editor
{
    void OnSceneGUI()
    {
        EnemyScript fow = (EnemyScript)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.right, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }
}
