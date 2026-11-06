using BombonesApp2026.Servicios.DTOs.Caja;
using BombonesApp2026.Servicios.DTOs.Venta;
using BombonesApp2026.Servicios.Intefaces;
using BombonesApp2026.Windows.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace BombonesApp2026.Windows
{
    public partial class frmVentas : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private BindingSource _bindingSource = new BindingSource();

        //para paginar
        private EstadoNavegacion _estado = new(10);

        //para ordenar
        private string campoOrdenar = "FechaVenta";
        private bool esAscendente = true;
        private bool? filtroActivo = null;
        private ServicioMensajes _servicioMensajes = new();

        public frmVentas(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            dgvDatos.DataSource = _bindingSource;
        }
        private void RecargarGrilla()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var ventaServicio = scope.ServiceProvider
                    .GetRequiredService<IVentaServicio>();

                try
                {
                    int paginaSolicitada = _estado.PaginaActual;

                    var resultadoConsulta = ventaServicio
                        .ObtenerPaginado(
                            paginaSolicitada,
                            _estado.RegistrosPorPagina,
                            campoOrdenar,
                            esAscendente,
                            filtroActivo);

                    if (resultadoConsulta.IsFailure)
                    {
                        _servicioMensajes.Errores(resultadoConsulta.Errors);
                        return;
                    }

                    var resultado = resultadoConsulta.Value!;

                    _estado.Actualizar(resultado.CantidadRegistros);

                    if (_estado.PaginaActual != paginaSolicitada)
                    {
                        resultadoConsulta = ventaServicio
                            .ObtenerPaginado(
                                _estado.PaginaActual,
                                _estado.RegistrosPorPagina,
                                campoOrdenar,
                                esAscendente,
                                filtroActivo);

                        if (resultadoConsulta.IsFailure)
                        {
                            _servicioMensajes.Errores(resultadoConsulta.Errors);
                            return;
                        }

                        resultado = resultadoConsulta.Value!;

                        _estado.Actualizar(resultado.CantidadRegistros);
                    }

                    _bindingSource.DataSource = resultado.Items;

                    lblCantidad.Text = _estado.TextoRegistros();
                    lblPaginas.Text = _estado.TextoPaginas();

                    btnPrimero.Enabled = _estado.PuedeIrAnterior();
                    btnAnterior.Enabled = _estado.PuedeIrAnterior();
                    btnSiguiente.Enabled = _estado.PuedeIrSiguiente();
                    btnUltimo.Enabled = _estado.PuedeIrSiguiente();
                }
                catch (Exception ex)
                {
                    _servicioMensajes.Error(ex.Message);
                }
            }
        }

        private void tsbCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            try
            {
                RecargarGrilla();
            }
            catch (Exception ex)
            {

                _servicioMensajes.Error(ex.Message);
            }
        }
        private void tsbNuevo_Click(object sender, EventArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                using (frmVentaAe frm = scope.ServiceProvider
                    .GetRequiredService<frmVentaAe>())
                {
                    frm.Text = "Nueva Venta";
                    frm.ShowDialog();
                    if (frm.DataChanged)
                    {
                        RecargarGrilla();
                    }

                }
            }
        }

        private void btnPrimero_Click(object sender, EventArgs e)
        {
            _estado.PrimeraPagina();
            RecargarGrilla();
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            _estado.PaginaAnterior();
            RecargarGrilla();
        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            _estado.PaginaSiguiente();
            RecargarGrilla();
        }

        private void btnUltimo_Click(object sender, EventArgs e)
        {
            _estado.UltimaPagina();
            RecargarGrilla();
        }

        private void tsbDetalles_Click(object sender, EventArgs e)
        {
            VentaListDto? ventaListDto = _bindingSource
                .ObtenerSeleccionado<VentaListDto>(_servicioMensajes);
            if (ventaListDto is null) return;
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var ventaServicio = scope.ServiceProvider
                            .GetRequiredService<IVentaServicio>();
                    var resultadoConsulta = ventaServicio.ObtenerPorId(ventaListDto.VentaId);
                    if (resultadoConsulta.IsFailure)
                    {
                        _servicioMensajes.Errores(resultadoConsulta.Errors);
                        return;
                    }
                    var ventaDto = resultadoConsulta.Value;
                    if (ventaDto is null) return;
                    using (frmDetalleVenta frm = scope.ServiceProvider
                        .GetRequiredService<frmDetalleVenta>())
                    {
                        frm.Text = "Detalle de la Venta";
                        frm.SetVenta(ventaDto);
                        frm.ShowDialog();
                    }

                }
                catch (Exception ex)
                {

                    _servicioMensajes.Error(ex.Message);
                }
            }
        }
    }
}
