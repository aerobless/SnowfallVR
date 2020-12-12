using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;

public class PhotonViewParentableObject : MonoBehaviour
{

    public string parentPath_vr = "";
    public string alternativePath = "";

    // Start is called before the first frame update
    void Start()
    {
        ParentObject(parentPath_vr);
    }

    private void ParentObject(string path)
    {
        var photonView = GetComponent<PhotonView>();
        var parent = GameObject.Find(photonView.IsMine ? path : alternativePath);
        
        Assert.IsNotNull(parent, "Photon view " + gameObject.name + " has an invalid parent gameobject path " + path);

        transform.parent = parent.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
}
