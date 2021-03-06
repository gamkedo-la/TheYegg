using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("FOV settings")]
    [SerializeField] float viewRadius;
    [SerializeField] [Range(0f, 360f)] float viewAngle;

    [SerializeField] LayerMask detectableTargetMask;
    [SerializeField] LayerMask obstacleMask;

    public List<Transform> targetsInFieldOfView = new List<Transform>();

    [SerializeField] float meshResolution;
    [SerializeField] int edgeResolveIterations;
    [SerializeField] float edgeDistanceThreshold;

    [SerializeField] MeshFilter fovMeshFilter;
    [SerializeField] Material fovMaterial;

    [Header("Interface to GuardFSM")]
    [SerializeField] GuardFSM guardFSM;
    [SerializeField] GuardStateAlerted guardStateAlerted;
    public Vector3 lastKnownPlayerLocation;

    private Mesh viewMesh;

    private List<Transform> targetsInRadiusEarlier = new List<Transform>();

    private ScoreKeeper scoreKeeper;
    private LevelManager levelManager;
    private AlarmSystemSwitch alarmSystemSwitch;

    private void Start() {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        fovMeshFilter.mesh = viewMesh;
        StartCoroutine("FindTargetsWithDelay", .2f);
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(!scoreKeeper || scoreKeeper == null){
            //no scorekeeper found. Scorekeeper should be a dont destroy
            Debug.LogWarning("No ScoreKeeper found in scene by " + gameObject.name);
        }
        levelManager = FindObjectOfType<LevelManager>();
        if(!levelManager || levelManager == null){
            Debug.LogWarning("No LevelManager found in scene by " + gameObject.name);
        }
        alarmSystemSwitch = FindObjectOfType<AlarmSystemSwitch>();
        if(!alarmSystemSwitch || alarmSystemSwitch == null){
            Debug.Log("No AlarmSystemSwitch found in scene by " + gameObject.name + " and that is ok if intentional!");
        }
    }

    public void ReactivateFOV(){
        StartCoroutine("FindTargetsWithDelay", .2f);
    }

    private void LateUpdate() {
        DrawFieldOfView();
    }

    IEnumerator FindTargetsWithDelay(float delay){
        while(true) {
            yield return new WaitForSeconds(delay);
            if(guardFSM.activeState != guardFSM.triggerAlarmState){
                FindVisibleTargets();
                ScanTargets();
            }
            
           
        }
    }

    private void ScanTargets()
    {
        //do not scan if the guard is alerted by another
        //if(!guardStateAlerted.isAlertedByAnother){
            if(targetsInFieldOfView.Count > 0){
                //if there are targets, go through them and check if one of them is either an incapacitated NPC or the player who is exposed/compromised
                PlayerActionController playerActionController;
                NPC nPC;
                foreach(Transform t in targetsInFieldOfView){
                    if(t.TryGetComponent<PlayerActionController>(out playerActionController)){
                        if(playerActionController.isCompromisedDisguise || playerActionController.isDoingIllegalAction || (playerActionController.isInRestrictedArea && playerActionController.isCompromisedDisguise)){
                            lastKnownPlayerLocation = playerActionController.transform.position;
                            //start alerted state
                            guardStateAlerted.SetLastKnownLocation(lastKnownPlayerLocation);
                            if(guardFSM.activeState != guardFSM.alertState){
                                guardFSM.PushState(guardFSM.alertState);
                                guardFSM.activeState.EndGuardState();
                                scoreKeeper.IncreaseTimesDetected();
                                return;
                            }
                            
                        }
                    } else if(t.TryGetComponent<NPC>(out nPC)){
                        if(nPC.isNPCDown == true){
                            //saw a downed NPC
                            //get rid of the NPC in the scene, or mark them as "seen" so that the NPC does not fall into a loop of detecting the same downed NPC
                            guardStateAlerted.SetLastKnownLocation(nPC.transform.position);
                            if(guardFSM.activeState != guardFSM.alertState ){
                                if(guardFSM.activeState != guardFSM.triggerAlarmState){
                                    guardFSM.PushState(guardFSM.alertState);
                                    guardFSM.activeState.EndGuardState();
                                }
                            }

                            if(nPC.hasNPCOutfit == false){
                                //outfit has been removed for the detected NPC, check if player has the same outfit and set outfit as compromised
                                FindObjectOfType<DisguiseHandler>().CompromiseDisguise(nPC.GetDisguiseAnimator());
                            }
                        }
                    }
                }
            }
        //}
    }

    void FindVisibleTargets(){
        Collider[] targetsInView = Physics.OverlapSphere(transform.position, viewRadius, detectableTargetMask);

        List<Transform> tempList = new List<Transform>();
        for (int i = 0; i < targetsInView.Length; i++)
        {
            Transform target = targetsInView[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2){
                //is in view angle
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if(!Physics.Raycast(transform.position, dirToTarget, distanceToTarget, obstacleMask)){
                    //no obstacles in the way, so the target is visible
                    if(!tempList.Contains(target)){
                        tempList.Add(target);
                    } 
                   
                } else {
                    // Debug.Log("there was an obstacle between FOV source and target");
                }
            }
        }

        //handling for removing transforms that are not in the FOV
        targetsInFieldOfView.Clear();
        foreach (Transform t in tempList)
        {
            targetsInFieldOfView.Add(t);
        }
        tempList.Clear();

    }

    void DrawFieldOfView(){
        int rayCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / rayCount;

        List<Vector3> viewPoints = new List<Vector3>();


        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i < rayCount; i++){
            float currentAngle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            //Debug.DrawLine(transform.position, transform.position + DirectionFromAngle(currentAngle, false) * viewRadius, Color.red);
            ViewCastInfo newViewCast = ViewCast(currentAngle);

            if(i > 0){
                //check if old viewcast hit obstacle and the new one didn't, or if the old viewcast didn't hit and the new one did
                bool edgeDistanceThresholdExceeded = Mathf.Abs(oldViewCast.distance - newViewCast.distance) > edgeDistanceThreshold; 
                if(oldViewCast.hasHit != newViewCast.hasHit || (oldViewCast.hasHit && newViewCast.hasHit && edgeDistanceThresholdExceeded)){
                    
                    EdgeInformation edge = FindEdge(oldViewCast, newViewCast);
                    if(edge.pointA != Vector3.zero){
                        viewPoints.Add(edge.pointA);
                    }

                    if(edge.pointB != Vector3.zero){
                        viewPoints.Add(edge.pointB);
                    }
                }
            }
            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++){
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
            
            if(i < vertexCount - 2){
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    private EdgeInformation FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast){
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

       
        for (int i = 0; i < edgeResolveIterations; i++){
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);
            bool edgeDistanceThresholdExceeded = Mathf.Abs(minViewCast.distance - newViewCast.distance) > edgeDistanceThreshold; 
            if(newViewCast.hasHit == minViewCast.hasHit && edgeDistanceThresholdExceeded == false){
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }


        }

        return new EdgeInformation(minPoint, maxPoint);
    }

    public Vector3 DirectionFromAngle(float angleInDegrees, bool angleIsGlobal){
        if(!angleIsGlobal){
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0f, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    private ViewCastInfo ViewCast(float globalAngle){
        Vector3 direction = DirectionFromAngle(globalAngle, true);
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, viewRadius, obstacleMask);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction, out hit, viewRadius, obstacleMask)){
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + direction * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct ViewCastInfo{
        public bool hasHit;
        public Vector3 point;
        public float distance;
        public float angle;

        public ViewCastInfo(bool _hasHit, Vector3 _point, float _distance, float _angle){
            hasHit = _hasHit;
            point = _point;
            distance = _distance;
            angle = _angle;
        }
    }

    public struct EdgeInformation{
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInformation(Vector3 _pointA, Vector3 _pointB){
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
