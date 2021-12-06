using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveillanceCamera : MonoBehaviour
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

    [Header("Alert settings")]
    [SerializeField] float alertRadius;
    [SerializeField] LayerMask alertLayerMask;

    private Mesh viewMesh;

    private List<Transform> targetsInRadiusEarlier = new List<Transform>();
    private Vector3 lastKnownPlayerLocation;
    private bool isDisabled = false;

    private ScoreKeeper scoreKeeper;


    // Start is called before the first frame update
    private void Start() {
        isDisabled = FindObjectOfType<LevelManager>().GetCamerasDisabled();
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        fovMeshFilter.mesh = viewMesh;
        StartCoroutine("FindTargetsWithDelay", .2f);
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
        if(!scoreKeeper || scoreKeeper == null){
            Debug.LogWarning("No ScoreKeeper found in scene for " +  gameObject.name);
        }
    }
    // Update is called once per frame
    private void LateUpdate() {
        if(!isDisabled){
            DrawFieldOfView();
        }
    }

    public void DisableCamera(){
        isDisabled = true;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Debug.Log("Camera "+ gameObject.name + " has been set to disabled");
    }

    IEnumerator FindTargetsWithDelay(float delay){
        while(!isDisabled) {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
            ScanTargets();
        }
    }

    private void ScanTargets(){
        //loop through the targets in the field of view and check if there's an incapacitated guard or the player
        if(targetsInFieldOfView.Count > 0){
            PlayerActionController playerActionController;
            NPC nPC;
            foreach(Transform t in targetsInFieldOfView){
                if(t.TryGetComponent<PlayerActionController>(out playerActionController)){
                    if(playerActionController.isCompromisedDisguise || playerActionController.isDoingIllegalAction || (playerActionController.isInRestrictedArea && playerActionController.isCompromisedDisguise)){
                        //Debug.Log("Player has been detected, moving to alerted state");
                        lastKnownPlayerLocation = playerActionController.transform.position;
                        //start alerted state for guards in the vicinity of the camera
                        Debug.Log("Camera spotted player and tries to alert");
                        //TODO subtract points if the player is spotted by the cameras!
                        if(scoreKeeper.GetIsDetectedByCameras() == false){
                            scoreKeeper.SetIsDetectedByCameras(true);
                        }
                        AlertGuards(lastKnownPlayerLocation);
                
                    }
                } else if(t.TryGetComponent<NPC>(out nPC)){
                    if(nPC.isNPCDown == true){
                        //saw a downed NPC
                        //get rid of the NPC in the scene, or mark them as "seen" so that the NPC does not fall into a loop of detecting the same downed NPC
                        Debug.Log("An incapacitated NPC detected, moving to alerted state");
                        AlertGuards(nPC.transform.position);
                    }
                }
            }
        }
    }

    private void AlertGuards(Vector3 lastKnownLocation)
    {
        GuardFSM[] allGuards = FindObjectsOfType<GuardFSM>();
        foreach(GuardFSM g in allGuards){
            if(Vector3.Distance(g.transform.position, lastKnownLocation) <= alertRadius){
                //raycast to the guard in the radius to see if there is a wall in between
                Vector3 dirToGuard = g.transform.position - transform.position;
                Ray ray = new Ray(transform.position, (dirToGuard));
                if(Physics.Raycast(ray, alertRadius, alertLayerMask)){
                    //alert others
                    if(g.activeState != g.alertState){
                        g.gameObject.GetComponent<GuardStateAlerted>().SetIsAlertedByAnother(true);
                        g.gameObject.GetComponent<GuardStateAlerted>().SetLastKnownLocation(lastKnownLocation);
                        g.PushState(g.alertState);
                        g.activeState.EndGuardState();
                    } else {
                        g.gameObject.GetComponent<GuardStateAlerted>().SetIsAlertedByAnother(true);
                        g.gameObject.GetComponent<GuardStateAlerted>().SetLastKnownLocation(lastKnownLocation);
                    }
                    
                }
            }
        }
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
