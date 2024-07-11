using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExplosionCube : MonoBehaviour
{
    [SerializeField]
    float pieceSize = 0.2f;

    [SerializeField]
    int piecesInRow = 5;

    [SerializeField]
    float explosionForce = 5;

    [SerializeField]
    float explosionRadius = 5;

    [SerializeField]
    float explosionUpward = 5;

    float piecesPivotDistance;
    Vector3 piecesPivot;

    private void Start() {
        // 조각들의 피봇과 피봇 거리 계산
        piecesPivotDistance = pieceSize * piecesInRow / 2f;
        piecesPivot = Vector3.one * piecesPivotDistance;
    }

    // 폭발
    public void Explode() {
        Debug.Log("Explode");

        // 기존 오브젝트 비활성화
        gameObject.SetActive(false);

        // 조각 생성
        for(int x = 0; x < piecesInRow; x++) {
            for(int y = 0; y < piecesInRow; y++) {
                for(int z = 0; z < piecesInRow; z++) {
                    CreatePieces(x, y, z);
                }
            }
        }

        // 조각들 폭발
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

        foreach(Collider hit in colliders) {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if(rb != null) {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, explosionUpward);
            }
        }
    }

    // 조각 생성
    void CreatePieces(int x, int y, int z) {
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        piece.transform.position = transform.position + new Vector3(pieceSize * x, pieceSize * y, pieceSize * z) - piecesPivot;
        piece.transform.localScale = Vector3.one * pieceSize;

        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = pieceSize;
    }
}
