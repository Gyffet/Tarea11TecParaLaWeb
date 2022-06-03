﻿using Formulario.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Newtonsoft.Json;
//using Formulario.Model.ModelExt.Mensajeria;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;

namespace Formulario.Repositories
{
    public interface IFormularioRepository
    {
        public void guardarFormulario();
    }

    public class FormularioRepository : IFormularioRepository
    {
        private readonly BD_HerramientasDGSGContext context;

        public FormularioRepository(BD_HerramientasDGSGContext context)
        {
            this.context = context;
        }

        public void guardarFormulario()
        {

        }
    }
}