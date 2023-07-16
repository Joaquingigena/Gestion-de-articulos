using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Negocio
{
    public class ArticuloNegocio
    {
        //Listar articulos que estaen guardados en base de datos
        public List<Articulo> listar()
        {
            List<Articulo> lista= new List<Articulo>();
            AccesoDatos datos= new AccesoDatos();
            try
            {
                
                datos.setearConsulta("select A.Id,Codigo, Nombre, A.Descripcion, ImagenUrl, M.Descripcion Marca, C.Descripcion Categoria, Precio, A.IdMarca,A.IdCategoria from ARTICULOS A, MARCAS M, CATEGORIAS C where IdMarca = M.Id and IdCategoria = C.Id");
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();

                    articulo.id = (int)datos.Lector["Id"];

                    if (!(datos.Lector["Codigo"] is DBNull))
                        articulo.codigo = (string)datos.Lector["Codigo"];

                    if (!(datos.Lector["Nombre"] is DBNull))
                        articulo.nombre = (string)datos.Lector["Nombre"];
                    
                    if (!(datos.Lector["Descripcion"] is DBNull))
                        articulo.descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        articulo.UrlImagen = (string)datos.Lector["ImagenUrl"];

                    articulo.marca= new Marca();
                    if (!(datos.Lector["Marca"] is DBNull))
                    {
                        articulo.marca.id = (int)datos.Lector["IdMarca"];
                        articulo.marca.descripcion = (string)datos.Lector["Marca"];
                        
                    }
                    articulo.categoria = new Categoria();
                    if (!(datos.Lector["Categoria"] is DBNull))
                    {
                        articulo.categoria.id = (int)datos.Lector["IdCategoria"];
                        articulo.categoria.descripcion = (string)datos.Lector["Categoria"];
                    }
                    if (!(datos.Lector["Precio"] is DBNull))
                        articulo.precio = (decimal)datos.Lector["Precio"];

                    lista.Add(articulo);

                }


                return lista;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos= new AccesoDatos();

            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdCategoria,IdMarca,Precio,ImagenUrl) values('" + nuevo.codigo+"','"+nuevo.nombre+"','"+nuevo.descripcion+"',"+nuevo.categoria.id+","+nuevo.marca.id+","+nuevo.precio+",'"+nuevo.UrlImagen+"')");
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion();}

        }

        public void Modificar(Articulo articulo)
        {
            AccesoDatos datos= new AccesoDatos();

            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo=@Codigo, Nombre= @Nombre, Descripcion= @Des, ImagenUrl= @Img, IdMarca=@Marca, IdCategoria=@Cat, Precio= @Precio where Id=@Id");
                datos.setearParametro("@Id", articulo.id);
                datos.setearParametro("@Codigo", articulo.codigo);
                datos.setearParametro("@Nombre",articulo.nombre);
                datos.setearParametro("@Des",articulo.descripcion);
                datos.setearParametro("@Img",articulo.UrlImagen);
                datos.setearParametro("@Marca",articulo.marca.id);
                datos.setearParametro("@Cat",articulo.categoria.id);
                datos.setearParametro("@Precio",articulo.precio);

                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion();}

        }

        public void EliminacarFis(int Id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("delete ARTICULOS where Id=@Id");
                datos.setearParametro("Id", Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { datos.cerrarConexion();}
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista= new List<Articulo>();
            AccesoDatos datos= new AccesoDatos();

            try
            {
                string consulta = "select A.Id,Codigo, Nombre, A.Descripcion, ImagenUrl, M.Descripcion Marca, C.Descripcion Categoria, Precio, A.IdMarca,A.IdCategoria from ARTICULOS A, MARCAS M, CATEGORIAS C where IdMarca = M.Id and IdCategoria = C.Id and ";

                switch (campo)
                {
                    case "Precio":
                        switch (criterio)
                        {
                            case "Mayor a: ":
                                consulta += "Precio > " + filtro;
                                break;
                            case "Menor a: ":
                                consulta += "Precio < " + filtro;
                                break;
                            default:
                                consulta += "Precio = " + filtro;
                                break;
                        }

                        break;
                    case "Nombre":
                        switch (criterio)
                        {
                            case "Comienza con: ":
                                consulta += "Nombre like '" + filtro + "%'";
                                break;
                            case "Termina con: ":
                                consulta += "Nombre like '%" + filtro + "' ";
                                break;
                            case "Contiene: ":
                                consulta += "Nombre like '%"+filtro+"%' " ;
                                break;

                        }

                        break;
                    default:
                        switch (criterio)
                        {
                            case "Comienza con: ":
                                consulta += "A.Descripcion like '" + filtro + "%'";
                                break;
                            case "Termina con: ":
                                consulta += "A.Descripcion like '%" + filtro + "' ";
                                break;
                            case "Contiene: ":
                                consulta += "A.Descripcion like '%" + filtro + "%' ";
                                break;
                        }
                        break;
                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo articulo = new Articulo();

                    articulo.id = (int)datos.Lector["Id"];

                    if (!(datos.Lector["Codigo"] is DBNull))
                        articulo.codigo = (string)datos.Lector["Codigo"];

                    if (!(datos.Lector["Nombre"] is DBNull))
                        articulo.nombre = (string)datos.Lector["Nombre"];

                    if (!(datos.Lector["Descripcion"] is DBNull))
                        articulo.descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        articulo.UrlImagen = (string)datos.Lector["ImagenUrl"];

                    articulo.marca = new Marca();
                    if (!(datos.Lector["Marca"] is DBNull))
                    {
                        articulo.marca.id = (int)datos.Lector["IdMarca"];
                        articulo.marca.descripcion = (string)datos.Lector["Marca"];

                    }
                    articulo.categoria = new Categoria();
                    if (!(datos.Lector["Categoria"] is DBNull))
                    {
                        articulo.categoria.id = (int)datos.Lector["IdCategoria"];
                        articulo.categoria.descripcion = (string)datos.Lector["Categoria"];
                    }
                    if (!(datos.Lector["Precio"] is DBNull))
                        articulo.precio = (decimal)datos.Lector["Precio"];

                    lista.Add(articulo);

                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    
}
