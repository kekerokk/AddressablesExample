using UnityEngine;
using Random = UnityEngine.Random;

public class FunnyObject : MonoBehaviour {
    [SerializeField] Rigidbody _3dRb;
    [SerializeField] float _forcePower, _additionalDirectionPower;

    void OnValidate() {
        if (!_3dRb) TryGetComponent(out _3dRb);
    }

    void OnMouseDown() {
        AddForce();
    }
    
    void AddForce() {
        float dot = Random.Range(0, 1f);
        Vector3 additional = Vector3.Lerp(Vector3.left * _additionalDirectionPower, Vector3.right * _additionalDirectionPower, dot);
        
        _3dRb.linearVelocity = Vector3.up * _forcePower + additional * _forcePower;
        _3dRb.AddTorque(_3dRb.linearVelocity);
    }
}
