using Formulario.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Formulario.Model.ModelExt.Mensajeria;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Formulario.Model.ModelExt;

namespace Formulario.Repositories
{
    public interface IFormularioRepository
    {
        public void guardarFormulario(Persona persona, Institucion institucion, Capacitacion capacitacion, Pago pago);
    }

    public class FormularioRepository : IFormularioRepository
    {
        private readonly FormularioContext context;

        public FormularioRepository(FormularioContext context)
        {
            this.context = context;
        }

        public void guardarFormulario(Persona persona, Institucion institucion, Capacitacion capacitacion, Pago pago)
        {
            Datos datos = new Datos();
            datos.nombre = persona.nombre;
            datos.apellido = persona.apellido;
            datos.direccion = persona.direccion;
            datos.telefono = persona.telefono;
            datos.correo = persona.correo;
            datos.documento = persona.documento;
            datos.fechaNacimiento = persona.fechaNacimiento;
            datos.curso = capacitacion.nombre;
            datos.horario = capacitacion.horario;
            datos.formaPago = pago.pago;
            datos.NoContrato = institucion.numero;
            datos.personaAtendio = institucion.personas;
            datos.fechaInscripcion = institucion.fecha;
            context.Add(datos);
            context.SaveChanges();
        }
    }
}
