using Dominio;
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
    public partial class Detalle : Form
    {
        public Detalle(Articulo articulo )
        {
            InitializeComponent();

            try
            {
                lblId.Text= articulo.id.ToString();
                lblCodigo.Text = articulo.codigo;
                lblNombre.Text = articulo.nombre;
                lblDescripcion.Text= articulo.descripcion;
                lblMarca.Text = articulo.marca.descripcion;
                lblCategoria.Text=articulo.categoria.descripcion;
                lblPrecio.Text= articulo.precio.ToString();

                ptbImg.Load(articulo.UrlImagen);

            }
            catch (Exception ex)
            {

                ptbImg.Load("https://storage.googleapis.com/proudcity/mebanenc/uploads/2021/03/placeholder-image.png");
            }
        }

        private void Detalle_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
