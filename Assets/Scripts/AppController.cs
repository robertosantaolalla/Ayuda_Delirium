using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class AppController : MonoBehaviour {
    public DataManager dataManager;
    public MessageManager messageManager;
    public ImagePickerCroperHandler imagePicker;

    UserInfo userInfo;
    int numeroFamiliarAbierto;

    // PANELES:
    public GameObject PREREGISTER_PANEL;
    public GameObject REGISTER_PANEL;
    public GameObject REGISTER_PANEL_STEP1;
    public GameObject REGISTER_PANEL_STEP2;
    public GameObject MAIN_MENU_PANEL;
    public GameObject ORIENTAME_PANEL;
    public GameObject MI_FAMILIA_PANEL;
    public GameObject AÑADIR_FAMILIAR_PANEL;
    public GameObject FAMILIAR_PANEL;


    // INPUTFIELDS REGISTRO PACIENTE:
    public InputField nombrePaciente;
    public InputField fechaNacimientoPaciente;
    public string bytesImagenPaciente = "";
    public GameObject textoImagenPacienteSeleccionada;

    // INPUTFIELDS REGISTRO HOSPITAL:
    public InputField nombreHospital;
    public InputField ciudadHospital;
    public InputField fechaHospitalizacion;
    public InputField motivoHospitalizacion;
    public InputField servicioHospitalizacion;
    public InputField plantaHospital;
    public InputField habitacionHospital;
    public InputField nombreMedico;

    // TEXTOS ORIENTAME:
    public Text textoFechaHoy;
    public Text textoNombreYEdad;
    public Text textoNombreHospital;
    public Text textoNumeroPlanta;
    public Text textoNumeroHabitacion;
    public Text textoNombreCiudad;
    public Text textoMotivo;
    public Text diasHospitalizado;
    public Image imagenPaciente;

    // INPUTFIELDS REGISTRO FAMILIAR:
    public InputField nombreFamiliar;
    public InputField parentescoFamiliar;
    public InputField numeroTelefonoFamiliar;
    public string bytesImagenFamiliar = "";
    public Toggle esFamiliarPorDefecto;
    public GameObject textoImagenFamiliarSeleccionada;


    // TEXTOS VER FAMILIAR
    public Text textoNombreFamiliar;
    public Text textoParentescoFamiliar;
    public Text textoNumeroTelefonoFamiliar;
    public Image imagenFamiliar;

    void Start() {
        userInfo = dataManager.getAppData();
        if (userInfo == null) {
            // PRE REGISTER PANEL ACTIVADO POR DEFECTO
            MAIN_MENU_PANEL.SetActive(false);
            PREREGISTER_PANEL.SetActive(true);
        }
        else {
            PREREGISTER_PANEL.SetActive(false);
            MAIN_MENU_PANEL.SetActive(true);
        }
    }


    ////////////////// REGISTRO //////////////////

    public void establecerDatosPaciente() {
        if (nombrePaciente.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el nombre del paciente.");
            return;
        }
        if (!fechaValida(fechaNacimientoPaciente.text)) {
            messageManager.showMessage("Por favor, escriba la fecha de nacimiento del paciente en el formato correcto. (dd/mm/aaaa)");
            return;
        }
        if (bytesImagenPaciente == "") {
            messageManager.showMessage("Por favor, seleccione una imágen del paciente.");
            return;
        }

        // Datos OK:
        REGISTER_PANEL_STEP1.SetActive(false);
        REGISTER_PANEL_STEP2.SetActive(true);
    }

    public void seleccionarImagenPaciente() {
        imagePicker.PickImage(success => {
            messageManager.showMessage(success?"Imagen del paciente seleccionada.":"Imagen del paciente no seleccionada");
            if (success)
                textoImagenPacienteSeleccionada.SetActive(true);
        }, imageBytes => {
            bytesImagenPaciente = imageBytes;
         });
    }

    public void establecerDatosHospital() {
        if (nombreHospital.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el nombre del hospital.");
            return;
        }
        if (ciudadHospital.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba la ciudad del hospital.");
            return;
        }
        if (!fechaValida(fechaHospitalizacion.text)){
            messageManager.showMessage("Por favor, escriba la fecha de hospitalización del paciente en el formato correcto. (dd/mm/aaaa)");
            return;
        }
        if (motivoHospitalizacion.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el motivo de hospitalización.");
            return;
        }
        if (servicioHospitalizacion.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el servicio de hospitalización.");
            return;
        }
        if (plantaHospital.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba la planta en la que se encuentra el paciente.");
            return;
        }
        if (habitacionHospital.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el número de la habitación en la que se encuentra el paciente.");
            return;
        }
        if (nombreMedico.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el nombre del médico del paciente.");
            return;
        }

        userInfo = new UserInfo(nombrePaciente.text, darFormatoFecha(fechaNacimientoPaciente.text), bytesImagenPaciente, nombreHospital.text,
            ciudadHospital.text, darFormatoFecha(fechaHospitalizacion.text), motivoHospitalizacion.text, servicioHospitalizacion.text,
            int.Parse(plantaHospital.text), int.Parse(habitacionHospital.text), nombreMedico.text);
        dataManager.saveAppData(userInfo);
        messageManager.showMessage("Paciente registrado correctamente.");

        REGISTER_PANEL.SetActive(false);
        MAIN_MENU_PANEL.SetActive(true);
    }


    bool fechaValida (string fechaString) {
        try {
            fechaString = darFormatoFecha(fechaString);
            DateTime date = DateTime.ParseExact(fechaString, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return true;
        }
        catch {
            return false;
        }

    }

    string darFormatoFecha (string fechaString) {
        string[] digitos = fechaString.Split('/');
        if (digitos.Length == 3) {
            if (digitos[0].Length == 1)
                digitos[0] = "0" + digitos[0];
            if (digitos[1].Length == 1)
                digitos[1] = "0" + digitos[1];
            fechaString = digitos[0] + "/" + digitos[1] + "/" + digitos[2];
        }
        return fechaString;
    }



    ////////////////// PANEL ORIENTAME //////////////////

    public void openOrientamePanel() {
        DateTime ahora = DateTime.Now;
        DateTime fechaNacimiento = DateTime.ParseExact(userInfo.fechaNacimientoPaciente, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        DateTime fechaHospitalizado = DateTime.ParseExact(userInfo.fechaHospitalizacion, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        TimeSpan edad = ahora - fechaNacimiento;
        TimeSpan tiempoHospitalizado = ahora - fechaHospitalizado;
        imagenPaciente.sprite = leerImagen(userInfo.bytesFotoPaciente);

        textoFechaHoy.text = "Hoy es día <b>" + ahora.Day + "/" + ahora.Month + "</b> del año <b>" + ahora.Year + "</b> y son las <b>" + ahora.Hour + ":" + ahora.Minute + "</b> horas";
        textoNombreYEdad.text = "Mi nombre es <b>" + userInfo.nombrePaciente + "</b> y tengo <b>" + (int)(edad.Days / 365.2425) + "</b> años";
        textoNombreHospital.text = "Estoy hospitalizado en <b>" + userInfo.nombreHospital + "</b>";
        textoNumeroPlanta.text = "En la planta número <b>" + userInfo.plantaHabitacion + "</b>";
        textoNumeroHabitacion.text = "Y en la habitación número <b>" + userInfo.numeroHabitacion + "</b>";
        textoNombreCiudad.text = "En la ciudad de <b>" + userInfo.ciudadHospital + "</b>";
        textoMotivo.text = "Por el motivo de <b>" + userInfo.motivoHospitalizacion + "</b>";
        diasHospitalizado.text = "Y llevo <b>" + (int) tiempoHospitalizado.TotalDays + "</b> días hospitalizado";

    }


    Sprite leerImagen(string base64Text) {
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(Convert.FromBase64String(base64Text));
        Sprite sprite = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
        return sprite;
    }

    ////////////////// PANEL AYUDA //////////////////

    public void pedirAyuda() {
        if (userInfo.familiares.Count == 0) {
            messageManager.showMessage("Error. No tienes ningún familiar registrado.");
        }
        else {
            Familiar familiarDeAyuda = userInfo.getFamiliarPorDefecto();
            marcarNumeroEnTelefono(familiarDeAyuda.numeroTelefono);
        }
    }



    ////////////////// MI FAMILIA //////////////////

    public void openMiFamiliaPanel() {
        // Borro los familiares que hubiera cargados
        foreach (Transform t in scrollContainer.transform) 
            Destroy(t.gameObject);

        for (int i = 0; i<userInfo.familiares.Count; i++){
            GameObject familiarObject = Instantiate(familiarItem, scrollContainer);
            familiarObject.transform.Find("Text").GetComponent<Text>().text = userInfo.familiares[i].nombre;
            string indiceElemento = i.ToString();
            familiarObject.GetComponent<Button>().onClick.AddListener(() => { verFamiliar(indiceElemento); });

        }

        GameObject botonNuevoFamiliar = Instantiate(nuevoFamiliarItem, scrollContainer);
        botonNuevoFamiliar.GetComponent<Button>().onClick.AddListener(() => { abrirPanelNuevoFamiliar(); });

    }
    public GameObject familiarItem;
    public GameObject nuevoFamiliarItem;
    public Transform scrollContainer;

    public void establecerNuevoFamiliar() {
        if (nombreFamiliar.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el nombre del familiar.");
            return;
        }
        if (parentescoFamiliar.text.Length == 0) {
            messageManager.showMessage("Por favor, escriba el parentesco del familiar.");
            return;
        }
        if (numeroTelefonoFamiliar.text.Length != 9) {
            messageManager.showMessage("Por favor, escriba el número de teléfono del familiar. (9 dígitos)");
            return;
        }
        if (bytesImagenFamiliar == "") {
            messageManager.showMessage("Por favor, seleccione una imágen del familiar.");
            return;
        }

        // DATOS OK, lo registramos y borramos los textos por si quieren poner más familiares
        userInfo.addFamiliar(new Familiar(nombreFamiliar.text, parentescoFamiliar.text, int.Parse(numeroTelefonoFamiliar.text), esFamiliarPorDefecto.isOn, bytesImagenFamiliar));
        dataManager.saveAppData(userInfo);
        Debug.Log("Familiares: " + userInfo.familiares.Count);
        messageManager.showMessage("Familiar registrado correctamente.");

        nombreFamiliar.text = "";
        parentescoFamiliar.text = "";
        numeroTelefonoFamiliar.text = "";
        bytesImagenFamiliar = "";
        textoImagenFamiliarSeleccionada.SetActive(false);
        esFamiliarPorDefecto.isOn = false;

        MI_FAMILIA_PANEL.SetActive(true);
        AÑADIR_FAMILIAR_PANEL.SetActive(false);
        openMiFamiliaPanel();
    }

    public void seleccionarImagenFamiliar() {
        imagePicker.PickImage(success => {
            messageManager.showMessage(success?"Imagen del familiar seleccionada.":"Imagen del familiar no seleccionada");
            if (success)
                textoImagenFamiliarSeleccionada.SetActive(true);
        }, imageBytes => {
            bytesImagenFamiliar = imageBytes;
         });
    }
    public void abrirPanelNuevoFamiliar() {
        MI_FAMILIA_PANEL.SetActive(false);
        AÑADIR_FAMILIAR_PANEL.SetActive(true);
    }

    public void verFamiliar (string indice) {
        Familiar familiarAbierto = userInfo.familiares[int.Parse(indice)];
        textoNombreFamiliar.text = familiarAbierto.nombre;
        textoParentescoFamiliar.text = "Es mi " + familiarAbierto.parentesco;
        textoNumeroTelefonoFamiliar.text = "Su teléfono es " + familiarAbierto.numeroTelefono;
        imagenFamiliar.sprite = leerImagen(familiarAbierto.bytesFotoFamiliar);

        numeroFamiliarAbierto = familiarAbierto.numeroTelefono;
        FAMILIAR_PANEL.SetActive(true);
        MI_FAMILIA_PANEL.SetActive(false);
    }

    public void llamarAlFamiliar() {
        marcarNumeroEnTelefono(numeroFamiliarAbierto);
    }
    void marcarNumeroEnTelefono (int numero) {
        Application.OpenURL("tel://[+34" + numero + "]");
    }
}
