using Photon.Pun;
using UnityEngine;
using UnityEngine.Assertions;

namespace SixtyMetersAssets.Scripts.Networking
{
    public class PhotonViewParentableObject : MonoBehaviour
    {

        public string parentPathVr = "";
        public string alternativePath = "";
        public int localRotationZ = 0;

        // Start is called before the first frame update
        void Start()
        {
            ParentObject(parentPathVr);
        }

        private void ParentObject(string path)
        {
            var photonView = GetComponent<PhotonView>();
            var parent = GameObject.Find(photonView.IsMine ? path : alternativePath);
        
            Assert.IsNotNull(parent, "Photon view " + gameObject.name + " has an invalid parent gameobject path " + path);

            transform.parent = parent.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, localRotationZ);
        }
    }
}
