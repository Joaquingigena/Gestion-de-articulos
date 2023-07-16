using Dominio;
using Negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Presentacion
{
    public partial class VentanaAgregar : Form
    {
        private Articulo articulo = null;
        public VentanaAgregar()
        {
            InitializeComponent();
            lblSoloNumeros.Visible = false;
            lblValidarCodigo.Visible = false;
            lblValidarNombre.Visible = false;
        }
        public VentanaAgregar( Articulo articulo)
        {
            InitializeComponent();
            this.articulo = articulo;
            Text = "Modificar articulo";
            lblSoloNumeros.Visible= false;
            lblValidarCodigo.Visible=false;
            lblValidarNombre.Visible=false;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            
            ArticuloNegocio negocio= new ArticuloNegocio();

            if (!(soloNumeros(txbPrecio.Text)))
            {
                lblSoloNumeros.Visible = true;
                return ;
            }
            if (string.IsNullOrEmpty(txbPrecio.Text))
            {
                lblSoloNumeros.Visible = true;
                return;
            }

            if (string.IsNullOrEmpty(txbCodigo.Text))
            {
                lblValidarCodigo.Visible = true;
                return ;
            }
            if (string.IsNullOrEmpty(txbNombre.Text))
            {
                lblValidarNombre.Visible = true;
                return;
            }
            try
            {

                if (articulo == null)
                   articulo= new Articulo();    
                articulo.codigo = txbCodigo.Text;
                articulo.nombre= txbNombre.Text;
                articulo.descripcion= txbDescripcion.Text;
                articulo.UrlImagen= txbUrlImagen.Text;
                articulo.categoria = (Categoria)cboCategoria.SelectedItem;
                articulo.marca=(Marca) cboMarca.SelectedItem;
                articulo.precio=decimal.Parse(txbPrecio.Text);

                

                if (articulo.id != 0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("Articulo modificado exitosamente");
                    Close();
                }
                else
                {
                    negocio.Agregar(articulo);
                    MessageBox.Show("Articulo agregado exitosamente");
                    Close();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void VentanaAgregar_Load(object sender, EventArgs e)
        {

            CategoriaNegocio cat=new CategoriaNegocio();
            MarcaNegocio marca=new MarcaNegocio();

            try
            {
                cboCategoria.DataSource = cat.listar();
                cboCategoria.ValueMember = "id";
                cboCategoria.DisplayMember = "Descripcion";

                cboMarca.DataSource= marca.listar();
                cboMarca.ValueMember = "id";
                cboMarca.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txbCodigo.Text = articulo.codigo.ToString();
                    txbNombre.Text = articulo.nombre;
                    txbDescripcion.Text = articulo.descripcion;
                    txbUrlImagen.Text = articulo.UrlImagen;
                    CargarImagen(articulo.UrlImagen);
                    cboCategoria.SelectedValue = articulo.categoria.id;
                    cboMarca.SelectedValue= articulo.marca.id;
                    txbPrecio.Text = articulo.precio.ToString();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void UrlImagen_Leave(object sender, EventArgs e)
        {
            CargarImagen(txbUrlImagen.Text);
        }

        private void CargarImagen(string imagen)
        {
            try
            {
                pcbImagen.Load(imagen);
            }
            catch (Exception)
            {

                pcbImagen.Load("https://i.ytimg.com/vi/R0H91UEbgwQ/maxresdefault.jpg");
            }
        }

        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            OpenFileDialog archivo= new OpenFileDialog();
            archivo.Filter = "jpg|*.jpg;|png|*png ";
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                txbUrlImagen.Text = archivo.FileName;
                CargarImagen(archivo.FileName);
            }
        }

        private bool Validar()
        {
            if (!(soloNumeros(txbPrecio.Text)))
            {
                lblSoloNumeros.Visible = true;
                return true;
            }
            if(string.IsNullOrEmpty(txbPrecio.Text))
            {
                lblPrecio.Visible = true;
                return true;
            }

            if(string.IsNullOrEmpty(txbCodigo.Text))
            {
                lblValidarCodigo.Visible = true;
                return true;
            }
            if(string.IsNullOrEmpty(txbNombre.Text))
            {
                lblValidarNombre.Visible = true;
                return true;
            }


            return false;

        }

        private bool soloNumeros(string cadena)
        {
            foreach (char item in cadena)
            {
                if (!(char.IsNumber(item)))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
