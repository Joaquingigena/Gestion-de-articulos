using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Presentacion
{
    public partial class Principal : Form
    {
        private List<Articulo> ListaArticulos;
        public Principal()
        {
            InitializeComponent();
        }


        private void Principal_Load(object sender, EventArgs e)
        {
            cargar();

            cboCampo.Items.Add("Precio");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripcion");
        }
        
        //Cargar la lista de articulos
        private void cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {
                ListaArticulos = negocio.listar();
                dgvLista.DataSource = ListaArticulos;
                //dgvLista.Columns[""].Visible=false;
                dgvLista.Columns["Descripcion"].Visible = false;
                dgvLista.Columns["UrlImagen"].Visible = false;
                dgvLista.Columns["Id"].Visible = false;
                CargarImagen(ListaArticulos[0].UrlImagen);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void dgvLista_SelectionChanged_1(object sender, EventArgs e)
        {
            Articulo seleccionado;
            if (dgvLista.CurrentRow != null)
            {
                seleccionado = (Articulo)dgvLista.CurrentRow.DataBoundItem;
                CargarImagen(seleccionado.UrlImagen);

            }
        }

        //Funcion generica para cargar imagen
        private void CargarImagen(string imagen)
        {
            try
            {
                pcbImagen.Load(imagen);
            }
            catch (Exception)
            {

                pcbImagen.Load("https://storage.googleapis.com/proudcity/mebanenc/uploads/2021/03/placeholder-image.png");
            }
        }


        //Agreagar un articulo
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            VentanaAgregar ventana= new VentanaAgregar();
            ventana.ShowDialog();
            cargar();
        }


        //Para modificar un articulo
        private void btnModificar_Click(object sender, EventArgs e)
        {
            if( dgvLista.CurrentRow== null)
            {
                MessageBox.Show("Debe seleccionar el articulo que desea modificar. ");
                return;
            }

            Articulo seleccionado;
            seleccionado = (Articulo)dgvLista.CurrentRow.DataBoundItem;

            VentanaAgregar ventana = new VentanaAgregar(seleccionado);
            ventana.ShowDialog();
            cargar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar el articulo que desea eliminar. ");
                return;
            }

            ArticuloNegocio negocio=new ArticuloNegocio();

            try
            {
                DialogResult resultado=  MessageBox.Show("De verdad desea eliminar el articulo)","Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                
                if(resultado == DialogResult.Yes)
                {
                    Articulo seleccionado = (Articulo)dgvLista.CurrentRow.DataBoundItem;
                    negocio.EliminacarFis(seleccionado.id);

                    cargar();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private bool validarFiltro()
        {
            if(cboCampo.SelectedIndex== -1)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar. ");
                return true;
            }
            if(cboCriterio.SelectedIndex== -1)
            {
                MessageBox.Show("Por favor seleccione el criterio para filtrar. ");
                return true;
            }
            if(cboCampo.SelectedItem.ToString() == "Precio")
            {
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    MessageBox.Show("Debes cargar el filtro...");
                    return true;
                }
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Solo numero para filtrar un campo numerico.. ");
                    return true;
                }
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
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                if(validarFiltro())
                {
                    return;
                }
                string campo= cboCampo.SelectedItem.ToString();
                string criterio= cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                
                dgvLista.DataSource= negocio.filtrar(campo, criterio, filtro);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void txbBuscar_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltrada;

            string filtro = txbBuscar.Text;

            if (filtro.Length >= 3)
            {
                listaFiltrada = ListaArticulos.FindAll(Art => Art.nombre.ToUpper().Contains(filtro.ToUpper()) || Art.marca.descripcion.ToUpper().Contains(filtro.ToUpper())) ;
            }
            else
            {
                listaFiltrada = ListaArticulos;
            }

            dgvLista.DataSource = null;
            dgvLista.DataSource = listaFiltrada;
            dgvLista.Columns["Descripcion"].Visible = false;
            dgvLista.Columns["UrlImagen"].Visible = false;
            dgvLista.Columns["Id"].Visible = false;

        }

      

        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcion= cboCampo.SelectedItem.ToString();

            if(opcion == "Precio")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a: ");
                cboCriterio.Items.Add("Menor a: ");
                cboCriterio.Items.Add("Igual a: ");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con: ");
                cboCriterio.Items.Add("Termina con: ");
                cboCriterio.Items.Add("Contiene: ");
            }
        }

        private void btnDetalle_Click(object sender, EventArgs e)
        {
            if (dgvLista.CurrentRow == null)
            {
                MessageBox.Show("Debe seleccionar el articulo que desea ver el detalle. ");
                return;
            }

            Articulo seleccionado;

            try
            {
                seleccionado =(Articulo) dgvLista.CurrentRow.DataBoundItem;

                Detalle ventana = new Detalle(seleccionado);
                ventana.ShowDialog();
                cargar();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
    }
}
