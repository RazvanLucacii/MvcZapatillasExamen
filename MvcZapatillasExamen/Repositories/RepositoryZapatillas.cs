using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcZapatillasExamen.Data;
using MvcZapatillasExamen.Models;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

#region PROCEDIMIENTOS ALMACENADOS

//create procedure SP_REGISTRO_IMAGENES_ZAPATILLAS
//(@posicion int, @zapatilla int, @registros int out)
//as
//select @registros = count(IDIMAGEN) from IMAGENESZAPASPRACTICA
//where IDPRODUCTO=@zapatilla
//select IDIMAGEN, IDPRODUCTO, IMAGEN from 
//    (select cast(
//    ROW_NUMBER() OVER (ORDER BY IMAGEN) as int) AS POSICION
//    , IDIMAGEN, IDPRODUCTO, IMAGEN
//    from IMAGENESZAPASPRACTICA
//    where IDPRODUCTO=@zapatilla) as QUERY
//    where QUERY.POSICION = @posicion
//go

//create procedure SP_INSERT_IMAGENES
//(@idproducto int, @imagen nvarchar(1500))
//as
//	DECLARE @NEXTID int
//	select @NEXTID = Max(IDIMAGEN) + 1 from IMAGENESZAPASPRACTICA
//	insert into IMAGENESZAPASPRACTICA values(@NEXTID, @idproducto, @imagen)
//go

#endregion

namespace MvcZapatillasExamen.Repositories
{
    public class RepositoryZapatillas
    {
        private ZapatillasContext context;

        public RepositoryZapatillas(ZapatillasContext context)
        {
            this.context = context;
        }

        public async Task<List<Zapatilla>> GetZapatillasAsync()
        {
            return await this.context.Zapatillas.ToListAsync();
        }

        public async Task<Zapatilla> FindZapatillaAsync(int idProducto)
        {
            return await this.context.Zapatillas.FirstOrDefaultAsync(x => x.IdProducto == idProducto);
        }

        public async Task<ModelImagenesZapasPaginacion> GetImagenZapatillasAsync(int posicion, int idProducto)
        {
            string sql = "SP_REGISTRO_IMAGENES_ZAPATILLAS @posicion, @zapatilla, @registros out";
            SqlParameter pamPosicion = new SqlParameter("@posicion", posicion);
            SqlParameter pamZapatilla = new SqlParameter("@zapatilla", idProducto);
            SqlParameter pamRegistros = new SqlParameter("@registros", -1);
            pamRegistros.Direction = ParameterDirection.Output;
            var consulta = this.context.ImagenesZapatillas.FromSqlRaw(sql, pamPosicion, pamZapatilla, pamRegistros);
            var datos = await consulta.ToListAsync();
            ImagenZapatilla imagen = datos.FirstOrDefault();
            int registros = (int)pamRegistros.Value;
            return new ModelImagenesZapasPaginacion
            {
                Registros = registros,
                ImagenZapatilla = imagen
            };
        }

        public async Task<List<ImagenZapatilla>> GetImagenesZapatillaAsync(int idProducto)
        {
            var imagenesZapas = this.context.ImagenesZapatillas.Where(x => x.IdProdcuto == idProducto);
            if(imagenesZapas.Count() == null)
            {
                return null;
            }
            else
            {
                return await imagenesZapas.ToListAsync();
            }
        }

        public void InsertarImagenes(int idproducto, List<string> imagen)
        {
            string sql = "SP_INSERT_IMAGENES @idproducto, @imagen";
            SqlParameter pamIdProducto = new SqlParameter("idproducto", idproducto);
            SqlParameter pamImagenes = new SqlParameter("imagen", imagen);
            this.context.Database.ExecuteSqlRaw(sql, pamIdProducto, pamImagenes);
        }
    }

}
