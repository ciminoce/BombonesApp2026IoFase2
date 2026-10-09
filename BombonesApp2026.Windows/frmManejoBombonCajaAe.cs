using BombonesApp2026.Entidades.Enums;
using BombonesApp2026.Servicios.DTOs.Bombon;
using BombonesApp2026.Servicios.Intefaces;
using BombonesApp2026.Windows.Helpers;

namespace BombonesApp2026.Windows
{
    public partial class frmManejoBombonCajaAe : Form
    {
        private readonly IBombonServicio _bombonServicio;
        private BombonComboDto? _bombonSeleccionado;
        private int _cantidadBombones;
        public frmManejoBombonCajaAe(IBombonServicio bombonServicio)
        {
            InitializeComponent();
            _bombonServicio = bombonServicio;
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CargarDatosComboBombones(cboBombones);
        }

        private void CargarDatosComboBombones(ComboBox cboBombones)
        {
            var respuestaConsulta = _bombonServicio
                .ObtenerDatosCombo(BombonDefault.Seleccione, false);
            if (respuestaConsulta.IsFailure)
            {
                ErrorHelper.MostrarErrores(respuestaConsulta.Errors);
                return;
            }
            var listaCbo = respuestaConsulta.Value!;
            cboBombones.DataSource = listaCbo;
            cboBombones.DisplayMember = "NombreBombon";
            cboBombones.ValueMember = "BombonId";
            cboBombones.SelectedIndex = 0;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidarDatos())
            {
                _cantidadBombones = (int)nudCantidad.Value;
                DialogResult = DialogResult.OK;
            }
        }

        private bool ValidarDatos()
        {
            bool valido = true;
            errorProvider1.Clear();
            if (cboBombones.SelectedIndex == 0)
            {
                valido = false;
                errorProvider1.SetError(cboBombones, "Debe seleccionar un bombón");
            }
            return valido;
        }
        public (BombonComboDto?, int) GetDatos()
        {
            return (_bombonSeleccionado, _cantidadBombones);
        }

        private void cboBombones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboBombones.SelectedIndex == 0) return;
            _bombonSeleccionado = (BombonComboDto)cboBombones.SelectedItem!;
        }
    }
}
