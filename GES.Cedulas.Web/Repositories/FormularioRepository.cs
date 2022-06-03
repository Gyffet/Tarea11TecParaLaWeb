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
        public dynamic getTodas();
        public void modificarFormulario(Persona persona, Institucion institucion, Capacitacion capacitacion, Pago pago, int id);
        public dynamic getPersona(int folio);
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

        public void modificarFormulario(Persona persona, Institucion institucion, Capacitacion capacitacion, Pago pago, int id)
        {
            var modificar = (from registro in context.Datos
                             where registro.pkFormulario.Equals(id)
                             select registro).FirstOrDefault();
            modificar.nombre = persona.nombre;
            modificar.apellido = persona.apellido;
            modificar.direccion = persona.direccion;
            modificar.telefono = persona.telefono;
            modificar.correo = persona.correo;
            modificar.documento = persona.documento;
            modificar.fechaNacimiento = persona.fechaNacimiento;
            modificar.curso = capacitacion.nombre;
            modificar.horario = capacitacion.horario;
            modificar.formaPago = pago.pago;
            modificar.NoContrato = institucion.numero;
            modificar.personaAtendio = institucion.personas;
            modificar.fechaInscripcion = institucion.fecha;
            context.Update(modificar);
            context.SaveChanges();

        }

        public dynamic getTodas()
        {
            dynamic personas = new ExpandoObject();

            personas = (from persona in context.Datos

                                  select persona).ToList();

            return personas;
        }
        public dynamic getPersona(int folio)
        {
            dynamic persona = new ExpandoObject();

            persona = (from registro in context.Datos
                       where registro.pkFormulario.Equals(folio)
                        select registro).FirstOrDefault();

            return persona;
        }

    }
}
