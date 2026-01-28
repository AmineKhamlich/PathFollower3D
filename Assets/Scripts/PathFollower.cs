using UnityEngine;
using System.Collections;

public class PathFollower : MonoBehaviour
{
    // Variable que controla com de ràpid es mou l'objecte.
    // Com que és 'public', la puc canviar des de Unity.
    public float speed = 3f;

    // EL CAMÍ (Pare): Aquí hi posarem l'objecte que conté tots els punts (els 'Fills') del camí.
    // L'script buscarà dins d'aquest objecte per saber on ha d'anar.
    public Transform pathParent;

    // Aquesta variable serveix per recordar cap a quin punt estem anant ara mateix.
    Transform targetPoint;

    // És el número del punt on anem (0 és el primer, 1 el segon, etc.).
    int index;

    // És l'objecte que el nostre personatge mirarà fixament mentre es mou (com una càmera o un punt d'interès).
    public Transform lookTarget;

    // DIBUIXAR GIZMOS: Això només serveix per a nosaltres, per veure el camí a l'editor de Unity.
    // No afecta el joc real, només dibuixa línies vermelles per saber per on passarà l'objecte.
    void OnDrawGizmos()
    {
        Vector3 from;
        Vector3 to;
        // Fem un bucle per repassar tots els fills (punts) del camí
        for (int a = 0; a < pathParent.childCount; a++)
        {
            // Agafem la posició del punt actual
            from = pathParent.GetChild(a).position;
            
            // Agafem la posició del següent punt.
            // L'operació '%' (mòdul) serveix perquè quan arribem a l'últim punt, el "següent" torni a ser el primer (0).
            // Així es tanca el cercle.
            to = pathParent.GetChild((a + 1) % pathParent.childCount).position;
            
            // Posem el color vermell i dibuixem la línia entre els dos punts
            Gizmos.color = new Color(1, 0, 0);
            Gizmos.DrawLine(from, to);
        }
    }

    // START: Es crida una sola vegada quan comença el joc.
    void Start()
    {
        // Comencem pel punt número 0 (el primer de la llista)
        index = 0;
        
        // Diem que el nostre primer objectiu és el fill número 0 del pare del camí.
        targetPoint = pathParent.GetChild(index);
    }

    // UPDATE: Es crida constantment a cada frame (imatge) del joc.
    void Update()
    {
        // Movem l'objecte des d'on és ara cap al punt objectiu.
        // Multipliquem per 'Time.deltaTime' perquè es mogui suau i a la mateixa velocitat
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        // Mirem la distància entre nosaltres i l'objectiu.
        // Si estem molt a prop (menys de 0.1 unitats), considerem que hem arribat.
        if (Vector3.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            // Passem al següent punt (sumem 1 a l'índex)
            index++;
            
            // Si l'índex és massa gran (més que punts hi ha), torna a començar per 0.
            index %= pathParent.childCount;
            
            // Actualitzem l'objectiu amb el nou punt
            targetPoint = pathParent.GetChild(index);
        }

        // ROTAR: Fem que l'objecte miri cap a l'objectiu de mirada ('lookTarget').
        transform.LookAt(lookTarget);
    }
}