using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    //Componentes
    private Rigidbody rb;

    //Ajustes de cámara
    public Camera miCamara;
    public float xSens; //Sensitividad horizontal
    public float ySens; //Sensitividad vertical

    //Movimiento jugador
    public float velocidad = 5f;


    //Salto
    public float fuerzaSalto = 20f;
    public Transform deteccionSuelo;
    public float distanciaSuelo = 0.3f;
    public LayerMask mascaraSuelo;
    private bool estaSuelo;

    //Cuestas
    public LayerMask limitadorCuesta;
    public bool puedeSubirCuesta;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        puedeSubirCuesta = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovimientoCamara();
        Movimiento();
        Saltar();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            puedeSubirCuesta = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            puedeSubirCuesta = true;
        }
    }

    private void Saltar()
    {
        estaSuelo = Physics.CheckSphere(deteccionSuelo.position, distanciaSuelo, mascaraSuelo);
        if (estaSuelo && Input.GetKey(KeyCode.Space) && rb.velocity.y <= 0.1f)
        {
            Debug.Log("Salta");
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
        }
    }

    private void Movimiento()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");

        Vector3 moveX = transform.right * inputX * velocidad * Time.deltaTime;
        transform.position += moveX;

        if (puedeSubirCuesta)
        {
            Vector3 moveY = transform.forward * inputY * velocidad * Time.deltaTime;
            transform.position += moveY;
        }
    }

    private void MovimientoCamara()
    {
        float ratonX = Input.GetAxis("Mouse X");
        float ratonY = Input.GetAxis("Mouse Y");

        //Si movemos el ratón horizontalmente
        if (ratonX != 0)
        {
            //Rotar jugador verticalmente
            transform.Rotate(Vector3.up * ratonX * xSens * Time.deltaTime);
        }

        //Si movemos el ratón verticalmente
        if (ratonY != 0)
        {
            //Rotar camara verticalmente
            miCamara.transform.Rotate(Vector3.left * ratonY * ySens * Time.deltaTime);
        }
    }
}
