using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Formulario.Entities.Login
{
    // NOTA: El código generado puede requerir, como mínimo, .NET Framework 4.5 o .NET Core/Standard 2.0.
    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1", IsNullable = false)]
    public partial class diffgram
    {

        private NewDataSet newDataSetField;

        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute(Namespace = "")]
        public NewDataSet NewDataSet
        {
            get
            {
                return this.newDataSetField;
            }
            set
            {
                this.newDataSetField = value;
            }
        }
    }

    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class NewDataSet
    {

        private NewDataSetDatosGenerales datosGeneralesField;

        private NewDataSetRoles rolesField;

        private NewDataSetÁreas áreasField;

        /// <comentarios/>
        public NewDataSetDatosGenerales DatosGenerales
        {
            get
            {
                return this.datosGeneralesField;
            }
            set
            {
                this.datosGeneralesField = value;
            }
        }

        /// <comentarios/>
        public NewDataSetRoles Roles
        {
            get
            {
                return this.rolesField;
            }
            set
            {
                this.rolesField = value;
            }
        }

        /// <comentarios/>
        public NewDataSetÁreas Áreas
        {
            get
            {
                return this.áreasField;
            }
            set
            {
                this.áreasField = value;
            }
        }
    }

    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class NewDataSetDatosGenerales
    {

        private uint expField;

        private string cve_usuarioField;

        private byte gral_edo_usrField;

        private string nom_catField;

        private string nombre_empField;

        private string paterno_empField;

        private string materno_empField;

        private string nombre_NPMField;

        private string nombre_PMNField;

        private string cve_puestoField;

        private string nom_pueField;

        private byte intentosField;

        private System.DateTime fch_vig_pswdField;

        private string rfc_empField;

        private string curp_empField;

        private string ind_rangoField;

        private string correo_electronicoField;

        private string idField;

        private byte rowOrderField;

        /// <comentarios/>
        public uint exp
        {
            get
            {
                return this.expField;
            }
            set
            {
                this.expField = value;
            }
        }

        /// <comentarios/>
        public string cve_usuario
        {
            get
            {
                return this.cve_usuarioField;
            }
            set
            {
                this.cve_usuarioField = value;
            }
        }

        /// <comentarios/>
        public byte gral_edo_usr
        {
            get
            {
                return this.gral_edo_usrField;
            }
            set
            {
                this.gral_edo_usrField = value;
            }
        }

        /// <comentarios/>
        public string nom_cat
        {
            get
            {
                return this.nom_catField;
            }
            set
            {
                this.nom_catField = value;
            }
        }

        /// <comentarios/>
        public string nombre_emp
        {
            get
            {
                return this.nombre_empField;
            }
            set
            {
                this.nombre_empField = value;
            }
        }

        /// <comentarios/>
        public string paterno_emp
        {
            get
            {
                return this.paterno_empField;
            }
            set
            {
                this.paterno_empField = value;
            }
        }

        /// <comentarios/>
        public string materno_emp
        {
            get
            {
                return this.materno_empField;
            }
            set
            {
                this.materno_empField = value;
            }
        }

        /// <comentarios/>
        public string nombre_NPM
        {
            get
            {
                return this.nombre_NPMField;
            }
            set
            {
                this.nombre_NPMField = value;
            }
        }

        /// <comentarios/>
        public string nombre_PMN
        {
            get
            {
                return this.nombre_PMNField;
            }
            set
            {
                this.nombre_PMNField = value;
            }
        }

        /// <comentarios/>
        public string cve_puesto
        {
            get
            {
                return this.cve_puestoField;
            }
            set
            {
                this.cve_puestoField = value;
            }
        }

        /// <comentarios/>
        public string nom_pue
        {
            get
            {
                return this.nom_pueField;
            }
            set
            {
                this.nom_pueField = value;
            }
        }

        /// <comentarios/>
        public byte intentos
        {
            get
            {
                return this.intentosField;
            }
            set
            {
                this.intentosField = value;
            }
        }

        /// <comentarios/>
        public System.DateTime fch_vig_pswd
        {
            get
            {
                return this.fch_vig_pswdField;
            }
            set
            {
                this.fch_vig_pswdField = value;
            }
        }

        /// <comentarios/>
        public string rfc_emp
        {
            get
            {
                return this.rfc_empField;
            }
            set
            {
                this.rfc_empField = value;
            }
        }

        /// <comentarios/>
        public string curp_emp
        {
            get
            {
                return this.curp_empField;
            }
            set
            {
                this.curp_empField = value;
            }
        }

        /// <comentarios/>
        public string ind_rango
        {
            get
            {
                return this.ind_rangoField;
            }
            set
            {
                this.ind_rangoField = value;
            }
        }

        /// <comentarios/>
        public string correo_electronico
        {
            get
            {
                return this.correo_electronicoField;
            }
            set
            {
                this.correo_electronicoField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:schemas-microsoft-com:xml-msdata")]
        public byte rowOrder
        {
            get
            {
                return this.rowOrderField;
            }
            set
            {
                this.rowOrderField = value;
            }
        }
    }

    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class NewDataSetRoles
    {

        private ushort cve_sistemaField;

        private string siglas_sistField;

        private string nom_sistField;

        private ushort cve_moduloField;

        private string siglas_moduloField;

        private string nom_moduloField;

        private byte rol_rdField;

        private string rol_rd1Field;

        private string nom_rol_rdField;

        private byte rol_wrField;

        private string rol_wr1Field;

        private string nom_rol_wrField;

        private string idField;

        private byte rowOrderField;

        /// <comentarios/>
        public ushort cve_sistema
        {
            get
            {
                return this.cve_sistemaField;
            }
            set
            {
                this.cve_sistemaField = value;
            }
        }

        /// <comentarios/>
        public string siglas_sist
        {
            get
            {
                return this.siglas_sistField;
            }
            set
            {
                this.siglas_sistField = value;
            }
        }

        /// <comentarios/>
        public string nom_sist
        {
            get
            {
                return this.nom_sistField;
            }
            set
            {
                this.nom_sistField = value;
            }
        }

        /// <comentarios/>
        public ushort cve_modulo
        {
            get
            {
                return this.cve_moduloField;
            }
            set
            {
                this.cve_moduloField = value;
            }
        }

        /// <comentarios/>
        public string siglas_modulo
        {
            get
            {
                return this.siglas_moduloField;
            }
            set
            {
                this.siglas_moduloField = value;
            }
        }

        /// <comentarios/>
        public string nom_modulo
        {
            get
            {
                return this.nom_moduloField;
            }
            set
            {
                this.nom_moduloField = value;
            }
        }

        /// <comentarios/>
        public byte rol_rd
        {
            get
            {
                return this.rol_rdField;
            }
            set
            {
                this.rol_rdField = value;
            }
        }

        /// <comentarios/>
        public string rol_rd1
        {
            get
            {
                return this.rol_rd1Field;
            }
            set
            {
                this.rol_rd1Field = value;
            }
        }

        /// <comentarios/>
        public string nom_rol_rd
        {
            get
            {
                return this.nom_rol_rdField;
            }
            set
            {
                this.nom_rol_rdField = value;
            }
        }

        /// <comentarios/>
        public byte rol_wr
        {
            get
            {
                return this.rol_wrField;
            }
            set
            {
                this.rol_wrField = value;
            }
        }

        /// <comentarios/>
        public string rol_wr1
        {
            get
            {
                return this.rol_wr1Field;
            }
            set
            {
                this.rol_wr1Field = value;
            }
        }

        /// <comentarios/>
        public string nom_rol_wr
        {
            get
            {
                return this.nom_rol_wrField;
            }
            set
            {
                this.nom_rol_wrField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:schemas-microsoft-com:xml-msdata")]
        public byte rowOrder
        {
            get
            {
                return this.rowOrderField;
            }
            set
            {
                this.rowOrderField = value;
            }
        }
    }

    /// <comentarios/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class NewDataSetÁreas
    {

        private uint cveAreaField;

        private string cve_adscripcionField;

        private string nom_areaField;

        private byte cve_edoField;

        private string nom_edoField;

        private byte circuitoField;

        private byte cve_cdField;

        private string nom_cdField;

        private string idField;

        private byte rowOrderField;

        /// <comentarios/>
        public uint cveArea
        {
            get
            {
                return this.cveAreaField;
            }
            set
            {
                this.cveAreaField = value;
            }
        }

        /// <comentarios/>
        public string cve_adscripcion
        {
            get
            {
                return this.cve_adscripcionField;
            }
            set
            {
                this.cve_adscripcionField = value;
            }
        }

        /// <comentarios/>
        public string nom_area
        {
            get
            {
                return this.nom_areaField;
            }
            set
            {
                this.nom_areaField = value;
            }
        }

        /// <comentarios/>
        public byte cve_edo
        {
            get
            {
                return this.cve_edoField;
            }
            set
            {
                this.cve_edoField = value;
            }
        }

        /// <comentarios/>
        public string nom_edo
        {
            get
            {
                return this.nom_edoField;
            }
            set
            {
                this.nom_edoField = value;
            }
        }

        /// <comentarios/>
        public byte circuito
        {
            get
            {
                return this.circuitoField;
            }
            set
            {
                this.circuitoField = value;
            }
        }

        /// <comentarios/>
        public byte cve_cd
        {
            get
            {
                return this.cve_cdField;
            }
            set
            {
                this.cve_cdField = value;
            }
        }

        /// <comentarios/>
        public string nom_cd
        {
            get
            {
                return this.nom_cdField;
            }
            set
            {
                this.nom_cdField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:schemas-microsoft-com:xml-diffgram-v1")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form = System.Xml.Schema.XmlSchemaForm.Qualified, Namespace = "urn:schemas-microsoft-com:xml-msdata")]
        public byte rowOrder
        {
            get
            {
                return this.rowOrderField;
            }
            set
            {
                this.rowOrderField = value;
            }
        }
    }
}
