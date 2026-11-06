using BombonesApp2026.Datos;
using BombonesApp2026.Datos.Interfaces;
using BombonesApp2026.Entidades;
using BombonesApp2026.Servicios.Common;
using BombonesApp2026.Servicios.DTOs.Venta;
using BombonesApp2026.Servicios.Intefaces;
using BombonesApp2026.Servicios.Mapeadores;

namespace BombonesApp2026.Servicios.Servicios
{
    public class VentaServicio : IVentaServicio
    {
        private readonly IVentaRepositorio _ventaRepositorio;
        private readonly IUnitOfWork _unitOfWork;
        public VentaServicio(IVentaRepositorio ventaRepositorio,
            IUnitOfWork unitOfWork)
        {
            _ventaRepositorio = ventaRepositorio;
            _unitOfWork = unitOfWork;
        }
        public Result<int> Agregar(VentaCreateDto ventaDto)
        {
            throw new NotImplementedException();
        }

        public Result<ResultadoPaginacionDto<VentaListDto>> ObtenerPaginado(
            int pagina, int cantidad, string campoOrdenar, bool esAscendente,
            bool? filtro, string? textoBuscar)
        {
            try
            {
                Func<IQueryable<Venta>, IOrderedQueryable<Venta>>? ordenarPor = campoOrdenar switch
                {
                    "VentaId" => q => esAscendente ? q.OrderBy(c => c.VentaId) : q.OrderByDescending(c => c.VentaId),
                    "FechaVenta" => q => esAscendente ? q.OrderBy(c => c.FechaVenta) : q.OrderByDescending(c => c.FechaVenta),
                    _ => q => esAscendente ? q.OrderBy(c => c.FechaVenta) : q.OrderByDescending(c => c.FechaVenta),
                };

                var resultado = _unitOfWork.Ventas.ObtenerPagina(pagina, cantidad, filtro, textoBuscar);
                var listaDto = resultado.lista
                    .Select(b => b.ToListDto()).ToList();

                var resultadoPaginado = new ResultadoPaginacionDto<VentaListDto>()
                {
                    Items = listaDto,
                    CantidadRegistros = resultado.cantidadRegistros,
                    CantidadPorPagina = cantidad,
                    PaginaActual = pagina
                };

                return Result<ResultadoPaginacionDto<VentaListDto>>.Success(resultadoPaginado);
            }
            catch (Exception ex)
            {
                return Result<ResultadoPaginacionDto<VentaListDto>>.Failure($"Error al intentar paginar: {ex.Message}");
            }
        }

        public Result<VentaDetailDto?> ObtenerPorId(int ventaId)
        {
            throw new NotImplementedException();
        }

        public Result<List<VentaListDto>> ObtenerTodos()
        {
            throw new NotImplementedException();
        }
    }
}
