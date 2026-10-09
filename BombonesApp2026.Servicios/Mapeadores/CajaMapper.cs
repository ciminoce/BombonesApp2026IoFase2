using BombonesApp2026.Entidades;
using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.Mapeadores;

namespace CajaesApp2026.Servicios.Mapeadores
{
    public static class CajaMapper
    {
        public static CajaDetailDto ToDetailDto(this Caja caja)
        {
            return new CajaDetailDto
            {
                ProductoId = caja.ProductoId,
                NombreCaja = caja.Nombre,
                Descripcion = caja.Descripcion,
                Precio = caja.Precio,
                Stock = caja.Stock,
                EsSurtida = caja.EsSurtida,
                CantidadBombones = caja.CantidadBombones,
                Activo = caja.Activo,
                Detalles = caja.Detalles.Select(d => d.ToDto()).ToList(),
            };
        }
        public static CajaListDto ToListDto(this Caja caja)
        {
            return new CajaListDto
            {
                ProductoId = caja.ProductoId,
                NombreCaja = caja.Nombre,
                Precio = caja.Precio,
                Stock = caja.Stock,
                EsSurtida = caja.EsSurtida,
                CantidadBombones = caja.CantidadBombones,
                Activo = caja.Activo
            };
        }

        public static Caja ToEntidad(this CajaCreateDto dto)
        {
            return new Caja
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Stock = dto.Stock,
            };
        }
        public static CajaUpdateDto ToUpdateDto(this Caja caja)
        {
            return new CajaUpdateDto
            {
                ProductoId = caja.ProductoId,
                Nombre = caja.Nombre,
                Descripcion = caja.Descripcion,
                Activo = caja.Activo,
                RowVersion = caja.RowVersion
            };
        }
        public static CajaDeleteDto ToDeleteDto(this Caja caja)
        {
            return new CajaDeleteDto
            {
                ProductoId = caja.ProductoId,
                RowVersion = caja.RowVersion,
            };
        }

    }

}
