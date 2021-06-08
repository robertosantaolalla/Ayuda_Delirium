using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo {

    public string nombrePaciente;
    public string fechaNacimientoPaciente;
    public string bytesFotoPaciente;

    public string nombreHospital;
    public string ciudadHospital;
    public string fechaHospitalizacion;
    public string motivoHospitalizacion;
    public string servicioHospitalizacion;
    public int plantaHabitacion;
    public int numeroHabitacion;
    public string nombreMedico;

    public List<Familiar> familiares;

    public UserInfo(string nombrePaciente, string fechaNacimientoPaciente, string bytesFotoPaciente, string nombreHospital, string ciudadHospital, string fechaHospitalizacion, string motivoHospitalizacion,
        string servicioHospitalizacion, int plantaHabitacion, int numeroHabitacion, string nombreMedico ) {

        this.nombrePaciente = nombrePaciente;
        this.fechaNacimientoPaciente = fechaNacimientoPaciente;
        this.bytesFotoPaciente = bytesFotoPaciente;
        this.nombreHospital = nombreHospital;
        this.ciudadHospital = ciudadHospital;
        this.fechaHospitalizacion = fechaHospitalizacion;
        this.motivoHospitalizacion = motivoHospitalizacion;
        this.servicioHospitalizacion = servicioHospitalizacion;
        this.plantaHabitacion = plantaHabitacion;
        this.numeroHabitacion = numeroHabitacion;
        this.nombreMedico = nombreMedico;
        familiares = new List<Familiar>();

    }

    public void addFamiliar (Familiar nuevoFamiliar) {
        if (nuevoFamiliar.familiarPorDefecto) { // comprobamos si había otro por defecto y lo quitamos
            for (int i=0; i<familiares.Count; i++) {
                if (familiares[i].familiarPorDefecto == true)
                    familiares[i].familiarPorDefecto = false;
            }
        }
        familiares.Add(nuevoFamiliar);
    }

    public Familiar getFamiliarPorDefecto() {
        foreach (Familiar familiar in familiares) {
            if (familiar.familiarPorDefecto)
                return familiar;
        }
        return familiares[0];
    }

     
}

[System.Serializable]
public class Familiar {
    public string nombre;
    public string parentesco;
    public int numeroTelefono;
    public bool familiarPorDefecto;
    public string bytesFotoFamiliar;

    public Familiar (string nombre, string parentesco, int numeroTelefono, bool familiarPorDefecto, string bytesFotoFamiliar) {
        this.nombre = nombre;
        this.parentesco = parentesco;
        this.numeroTelefono = numeroTelefono;
        this.familiarPorDefecto = familiarPorDefecto;
        this.bytesFotoFamiliar = bytesFotoFamiliar;
    }
}